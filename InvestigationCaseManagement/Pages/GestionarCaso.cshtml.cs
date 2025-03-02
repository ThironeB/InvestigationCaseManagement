using InvestigationCaseManagement.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InvestigationCaseManagement.Data.Utilities;

namespace InvestigationCaseManagement.Pages
{
    public class GestionarCasoModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public GestionarCasoModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<SelectListItem> Investigadores { get; set; } // Lista de investigadores
        public string UsuarioActualId { get; set; } // ID del usuario actual
        public string UsuarioActualText { get; set; } // Nombre del usuario actual

        [BindProperty]
        public Caso Caso { get; set; } = new Caso();

        [BindProperty(SupportsGet = true)]
        public string Modo { get; set; }  // "Editar" o "Cerrar"

        public async Task<IActionResult> OnGetAsync(int id, string modo)
        {
            if (modo != "Editar" && modo != "Cerrar" && modo != "ReAbrir")
            {
                return BadRequest("Modo inv�lido");
            }

            Modo = modo;
            Caso = await _context.Casos.FindAsync(id);
            if (Caso == null)
            {
                return NotFound();
            }

            if (modo == "ReAbrir")
            {
                await ReAbrirCaso(Caso);
                return RedirectToPage("ListaCasos");
            }

            HttpContext.Session.SetString("previousState", Caso.Estado);
            if (User.IsInRole("Administrador"))
            {
                // Obtener la lista de investigadores (usuarios con rol "Investigador")
                Investigadores = await _context.Users
                    .Where(u => _context.UserRoles.Any(ur => ur.UserId == u.Id && ur.RoleId == RoleIdentifier.Investigador.ToString()))
                    .Select(u => new SelectListItem
                    {
                        Value = u.Id,
                        Text = u.UserName
                    })
                    .ToListAsync();
            }
            else
            {
                // Si el usuario es un investigador, asignar su ID al caso
                Caso.InvestigadorId = UsuarioActualId;
            }

            ViewData["EsSoloLectura"] = (Modo == "Cerrar");
            ViewData["EsReAbierto"] = Caso.Estado == EstadoCaso.ReAbierto.ToString();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            RemoveModelState(); // Eliminar el estado del modelo para casos que no apliquen
            byte[]? previousState;
            var pState = HttpContext.Session.TryGetValue("previousState", out previousState); // Obtener el estado anterior del caso
            Caso.Estado = previousState != null ? System.Text.Encoding.UTF8.GetString(previousState) : "";

            if (Caso.Estado == "")
            {
                return NotFound();
            }

            if (Modo == "Cerrar")
            {
                RemoveModelStateCloseAction(); // Eliminar el estado del modelo para casos que no apliquen
            }

            if (!ModelState.IsValid)
            {
                await OnGetAsync(Caso.Id, Modo);
                return Page();
            }

            if (Modo == "Cerrar")
            {
                if (string.IsNullOrWhiteSpace(Caso.Conclusiones) || string.IsNullOrWhiteSpace(Caso.Observaciones))
                {
                    ModelState.AddModelError(string.Empty, "Todos los campos obligatorios deben estar llenos para cerrar el caso.");
                    await OnGetAsync(Caso.Id, Modo);
                    return Page();
                }

                Caso.Estado = EstadoCaso.Cerrado.ToString();
            }

            var investigador = await _userManager.FindByIdAsync(Caso.InvestigadorId);
            if (investigador != null)
            {
                Caso.Investigador = investigador;
            }
            else
            {
                await OnGetAsync(Caso.Id, Modo);
                return Page();
            }

            if ((Caso.Estado == EstadoCaso.Asignado.ToString() || Caso.Estado == EstadoCaso.Abierto.ToString()) && Modo == "Editar")  // Si el caso esta en estado "Abierto" o "Asignado" y el modo es "Editar"
            {
                Caso.Estado = "En Seguimiento"; 
            }

            Caso.Conclusiones = Caso.Conclusiones ?? string.Empty;
            Caso.Observaciones = Caso.Observaciones ?? string.Empty;
            Caso.Soporte = Caso.Soporte ?? string.Empty;
            Caso.UltimaActualizacion = DateTime.Now.Date;

            var casoEnBd = await _context.Casos
                    .AsNoTracking() // No rastrear para evitar conflictos
                    .FirstOrDefaultAsync(c => c.Id == Caso.Id);

            if (casoEnBd != null)
            {
                if (Caso.Estado == EstadoCaso.ReAbierto.ToString())
                {
                    _context.Attach(Caso);
                    _context.Entry(Caso).Property(a => a.Soporte).IsModified = true;
                    _context.Entry(Caso).Property(a => a.UltimaActualizacion).IsModified = true;
                    _context.Entry(Caso).OriginalValues.SetValues(casoEnBd);

                    await _context.SaveChangesAsync();
                }
                else if ((System.Text.Encoding.UTF8.GetString(previousState) == EstadoCaso.ReAbierto.ToString() || System.Text.Encoding.UTF8.GetString(previousState) == EstadoCaso.Asignado.ToString() || System.Text.Encoding.UTF8.GetString(previousState) == "En Seguimiento") && Caso.Estado == EstadoCaso.Cerrado.ToString()) // Si el caso estaba en estado "ReAbierto", "Asignado" o "En Seguimiento" y se desea cerrar
                {
                    _context.Attach(Caso);
                    _context.Entry(Caso).Property(a => a.Observaciones).IsModified = true;
                    _context.Entry(Caso).Property(a => a.Conclusiones).IsModified = true;
                    _context.Entry(Caso).Property(a => a.UltimaActualizacion).IsModified = true;
                    _context.Entry(Caso).OriginalValues.SetValues(casoEnBd);
                    await _context.SaveChangesAsync();
                }
                else if (Caso.Estado == "En Seguimiento")
                {
                    _context.Attach(Caso).State = EntityState.Modified;
                    _context.Entry(Caso).OriginalValues.SetValues(casoEnBd);
                    await _context.SaveChangesAsync();
                }
            }
            
            HttpContext.Session.Remove("previusState");
            ViewData["MostrarPopup"] = true;

            await OnGetAsync(Caso.Id, Modo);
            return Page();
        }

        public void RemoveModelStateCloseAction()
        {
            ModelState.Remove("Caso.Soporte");
        }
        
        public void RemoveModelState()
        {
            ModelState.Remove("Caso.Estado");
            ModelState.Remove("Caso.Investigador");

            if (Modo == "Editar")
            {
                ModelState.Remove("Caso.Conclusiones");
                ModelState.Remove("Caso.Observaciones");
                ModelState.Remove("Caso.Soporte");
            }
        }

        /// <summary>
        /// La funcion ReAbrirCaso actualiza el estado de un caso a "ReAbierto" en la base de datos.
        /// </summary>
        /// <param name="Caso">Caso es una entidad que representa un caso en la aplicación. El método
        /// ReAbrirCaso toma una instancia de esta clase como parámetro y actualiza el Estado del caso a
        /// "ReAbierto" en la base de datos.</param>
        public async Task ReAbrirCaso(Caso caso)
        {
            caso.Estado = EstadoCaso.ReAbierto.ToString();
            _context.Attach(caso);
            _context.Entry(caso).Property(a => a.Estado).IsModified = true;
            await _context.SaveChangesAsync();
        }
    }
}
