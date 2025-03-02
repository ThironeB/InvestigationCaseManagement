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

        public List<Archivo> Archivos { get; set; } // Lista de archivos
        public async Task OnGetAsync()
        {
            var usuarioActual = await _userManager.GetUserAsync(User);
            var rolesUsuario = await _userManager.GetRolesAsync(usuarioActual);
            // Obtener todos los casos con la informacion del investigador asignado
            // Si el usuario es Administrador, ver todos los casos
            Archivos = await _context.Archivos
                    .Include(c => c.Investigador) // Cargar la relacion con el investigador
                    .OrderBy(c => c.Id) // Ordenar por id
                    .ToListAsync();
        }
    }
}
