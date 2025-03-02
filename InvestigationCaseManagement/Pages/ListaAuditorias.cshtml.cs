using InvestigationCaseManagement.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InvestigationCaseManagement.Data.Utilities;

namespace InvestigationCaseManagement.Pages
{
    public class ListaAuditoriasModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public ListaAuditoriasModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<Auditoria> Logs { get; set; } // Lista de auditorias
        public List<SelectListItem> Investigadores { get; set; } = new List<SelectListItem>(); // Lista de investigadores

        [BindProperty(SupportsGet = true)]
        public string SelectedInvestigador { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SelectedEntidad { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SelectedAccion { get; set; }
        public string UsuarioActualId { get; set; }
        public string UsuarioActualText { get; set; }

        public async Task OnGetAsync()
        {
            var usuarioActual = await _userManager.GetUserAsync(User);
            UsuarioActualId = usuarioActual?.Id;
            UsuarioActualText = usuarioActual?.UserName;

            if (await _userManager.IsInRoleAsync(usuarioActual, "Administrador"))
            {
                // Si el usuario es un administrador, mostrar todos los investigadores
                Investigadores = await _context.Users
                    .Select(u => new SelectListItem
                    {
                        Value = u.Id,
                        Text = u.UserName
                    })
                    .ToListAsync();
            }
            else if (await _userManager.IsInRoleAsync(usuarioActual, "Investigador"))
            {
                // Si el usuario es un investigador, solo mostrar su propio usuario
                Investigadores.Add(new SelectListItem
                {
                    Value = UsuarioActualId,
                    Text = UsuarioActualText,
                    Selected = true
                });
                SelectedInvestigador = UsuarioActualText;
            }

            // Construir la consulta base
            IQueryable<Auditoria> query = _context.Auditorias.OrderByDescending(a => a.Fecha);

            // Aplicar los filtros
            if (!string.IsNullOrEmpty(SelectedInvestigador))
            {
                query = query.Where(a => a.Usuario == SelectedInvestigador);
            }

            if (!string.IsNullOrEmpty(SelectedEntidad))
            {
                query = query.Where(a => a.Entidad == SelectedEntidad);
            }

            if (!string.IsNullOrEmpty(SelectedAccion))
            {
                query = query.Where(a => a.Accion == SelectedAccion);
            }

            // Ejecutar la consulta y obtener las auditorias filtradas
            Logs = await query.ToListAsync(); 
        }
    }
}
