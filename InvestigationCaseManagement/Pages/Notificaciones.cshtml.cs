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
                var notificaciones = await _notificationService.ObtenerNotificacionesNoLeidasAsync(user.Id);

                Notificaciones = SortColumn switch
                {
                    "Mensaje" => SortAscending ? notificaciones.OrderBy(n => n.Mensaje).ToList() : notificaciones.OrderByDescending(n => n.Mensaje).ToList(),
                    "FechaCreacion" => SortAscending ? notificaciones.OrderBy(n => n.FechaCreacion).ToList() : notificaciones.OrderByDescending(n => n.FechaCreacion).ToList(),
                    _ => notificaciones.OrderByDescending(n => n.FechaCreacion).ToList(),
                };
            }
        }

        public async Task<IActionResult> OnPostMarcarComoLeidaAsync(int id)
        {
            await _notificationService.MarcarComoLeidaAsync(id);
            return RedirectToPage();
        }
    }
}
