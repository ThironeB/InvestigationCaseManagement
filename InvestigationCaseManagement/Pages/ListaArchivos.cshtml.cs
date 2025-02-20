using InvestigationCaseManagement.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InvestigationCaseManagement.Pages
{
    public class ListaArchivosModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public ListaArchivosModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<Archivo> Archivos { get; set; }
        public async Task OnGetAsync()
        {
            var usuarioActual = await _userManager.GetUserAsync(User);
            var rolesUsuario = await _userManager.GetRolesAsync(usuarioActual);
            // Obtener todos los casos con la información del investigador asignado
            //if (rolesUsuario.Contains("Administrador"))
            //{
            // Si el usuario es Administrador, ver todos los casos
            Archivos = await _context.Archivos
                    .Include(c => c.Investigador) // Cargar la relación con el investigador
                    .OrderBy(c => c.Id) // Ordenar por id
                    .ToListAsync();
            //}
            //else if (rolesUsuario.Contains("Investigador"))
            //{
            //    // Si el usuario es Investigador, ver solo los casos asignados a él
            //    Casos = await _context.Casos
            //        .Include(c => c.Investigador) // Cargar la relación con el investigador
            //        .Where(c => c.InvestigadorId == usuarioActual.Id) // Filtrar por el ID del investigador
            //        .OrderBy(c => c.Id) // Ordenar por id
            //        .ToListAsync();
            //}
        }
    }
}
