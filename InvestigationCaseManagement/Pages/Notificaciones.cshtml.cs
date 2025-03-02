using InvestigationCaseManagement.Data;
using InvestigationCaseManagement.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InvestigationCaseManagement.Pages
{
    public class NotificacionesModel : PageModel
    {
        private readonly NotificationService _notificationService;
        private readonly UserManager<IdentityUser> _userManager;

        public NotificacionesModel(NotificationService notificationService, UserManager<IdentityUser> userManager)
        {
            _notificationService = notificationService;
            _userManager = userManager;
        }

        public List<Notificacion> Notificaciones { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SortColumn { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool SortAscending { get; set; } = true;


        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var notificaciones = await _notificationService.ObtenerNotificacionesNoLeidasAsync(user.Id); // Obtener las notificaciones no leidas

                // Ordenar las notificaciones segun la columna seleccionada en la interfaz
                Notificaciones = SortColumn switch 
                {
                    "Mensaje" => SortAscending ? notificaciones.OrderBy(n => n.Mensaje).ToList() : notificaciones.OrderByDescending(n => n.Mensaje).ToList(),
                    "FechaCreacion" => SortAscending ? notificaciones.OrderBy(n => n.FechaCreacion).ToList() : notificaciones.OrderByDescending(n => n.FechaCreacion).ToList(),
                    _ => notificaciones.OrderByDescending(n => n.FechaCreacion).ToList(),
                };
            }
        }

        /// <summary>
        /// Esta funcion marca de forma asincrona una notificacion como leida y luego redirige a la pagina.
        /// </summary>
        /// <param name="id">El parametro `id` en el metodo `OnPostMarcarComoLeidaAsync` se usa para
        /// identificar la notificacion especifica que necesita ser marcada como leida. 
        /// <returns>
        /// El metodo esta retornando una redireccion a la misma pagina.
        /// </returns>
        public async Task<IActionResult> OnPostMarcarComoLeidaAsync(int id)
        {
            await _notificationService.MarcarComoLeidaAsync(id);
            return RedirectToPage();
        }
    }
}
