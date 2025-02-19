using InvestigationCaseManagement.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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

        public List<SelectListItem> Investigadores { get; set; }
        public string UsuarioActualId { get; set; }
        public string UsuarioActualText { get; set; }

        [BindProperty]
        public Caso Caso { get; set; } = new Caso();

        [BindProperty(SupportsGet = true)]
        public string Modo { get; set; }  // "Editar" o "Cerrar"

        public async Task<IActionResult> OnGetAsync(int id, string modo)
        {
            if (modo != "Editar" && modo != "Cerrar" && modo != "ReAbrir")
            {
                return BadRequest("Modo inválido");
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
                    .Where(u => _context.UserRoles.Any(ur => ur.UserId == u.Id && ur.RoleId == "68a24bd8-6f5d-4951-9e41-45b232780e1a"))
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
            ViewData["EsReAbierto"] = Caso.Estado == "ReAbierto";

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            RemoveModelState();
            byte[]? previousState;
            var pState = HttpContext.Session.TryGetValue("previousState", out previousState);
            Caso.Estado = previousState != null ? System.Text.Encoding.UTF8.GetString(previousState) : "";

            if (Caso.Estado == "")
            {
                return NotFound();
            }

            if (Modo == "Cerrar")
            {
                RemoveModelStateCloseAction();
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

                Caso.Estado = "Cerrado";
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

            if (Caso.Estado == "Asignado" && Modo == "Editar") 
            {
                Caso.Estado = "En Seguimiento";
            }

            Caso.Conclusiones = string.Empty;
            Caso.Observaciones = string.Empty;
            Caso.Soporte = Caso.Soporte ?? string.Empty;
            Caso.UltimaActualizacion = DateTime.Now.Date;
            _context.Attach(Caso).State = EntityState.Modified;
            await _context.SaveChangesAsync();

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

        public async Task ReAbrirCaso(Caso caso)
        {
            caso.Estado = "ReAbierto";
            _context.Attach(caso).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
