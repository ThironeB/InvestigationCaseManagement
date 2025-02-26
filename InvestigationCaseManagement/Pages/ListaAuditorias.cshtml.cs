using InvestigationCaseManagement.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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

        public List<Auditoria> Logs { get; set; }
        public List<SelectListItem> Investigadores { get; set; } = new List<SelectListItem>();

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
            //var usuarioActual = await _userManager.GetUserAsync(User);
            //UsuarioActualId = usuarioActual?.Id;
            //UsuarioActualText = usuarioActual?.UserName;

            //if (User.IsInRole("Administrador"))
            //{
            //    // Obtener la lista de investigadores (usuarios con rol "Investigador")
            //    Investigadores = await _context.Users
            //        .Where(u => _context.UserRoles.Any(ur => ur.UserId == u.Id && ur.RoleId == "68a24bd8-6f5d-4951-9e41-45b232780e1a"))
            //        .Select(u => new SelectListItem
            //        {
            //            Value = u.Id,
            //            Text = u.UserName
            //        })
            //        .ToListAsync();
            //}
            //else
            //{
            //    // Si el usuario es un investigador, asignar su ID al caso
            //    //Caso.InvestigadorId = UsuarioActualId;
            //}

            //Logs = _context.Auditorias
            //.OrderByDescending(a => a.Fecha)
            //.ToList();

            Investigadores = await _context.Users
                    //.Where(u => _context.UserRoles.Any(ur => ur.UserId == u.Id && ur.RoleId == "68a24bd8-6f5d-4951-9e41-45b232780e1a"))
                    .Select(u => new SelectListItem
                    {
                        Value = u.Id,
                        Text = u.UserName
                    })
                    .ToListAsync();// Trae los investigadores

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

            // Ejecutar la consulta y obtener los resultados
            Logs = await query.ToListAsync();
        }
    }
}
