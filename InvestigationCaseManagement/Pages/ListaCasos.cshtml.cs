using InvestigationCaseManagement.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InvestigationCaseManagement.Pages
{
    public class ListaCasosModel(ApplicationDbContext context, UserManager<IdentityUser> userManager) : PageModel
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<IdentityUser> _userManager = userManager;

        public List<Caso> Casos { get; set; }
        public async Task OnGetAsync()
        {
            var usuarioActual = await _userManager.GetUserAsync(User);
            var rolesUsuario = await _userManager.GetRolesAsync(usuarioActual);
            // Obtener todos los casos con la información del investigador asignado
            if (rolesUsuario.Contains("Administrador"))
            {
                // Si el usuario es Administrador, ver todos los casos
                Casos = await _context.Casos
                    .Include(c => c.Investigador) // Cargar la relación con el investigador
                    .OrderBy(c => c.NumeroExpediente) // Ordenar por número de expediente
                    .ToListAsync();
            }
            else if (rolesUsuario.Contains("Investigador"))
            {
                // Si el usuario es Investigador, ver solo los casos asignados a él
                Casos = await _context.Casos
                    .Include(c => c.Investigador) // Cargar la relación con el investigador
                    .Where(c => c.InvestigadorId == usuarioActual.Id) // Filtrar por el ID del investigador
                    .OrderBy(c => c.NumeroExpediente) // Ordenar por número de expediente
                    .ToListAsync();
            }
        }
    }
}
