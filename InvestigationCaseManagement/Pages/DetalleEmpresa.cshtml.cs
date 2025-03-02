using InvestigationCaseManagement.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InvestigationCaseManagement.Pages
{
    public class DetalleEmpresaModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        // Constructor de la clase DetalleEmpresaModel
        public DetalleEmpresaModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Empresa? Empresa { get; set; } // Empresa a mostrar
        public List<Caso> Casos { get; set; } = new(); // Lista de casos asociados a la empresa

        public async Task<IActionResult> OnGet(int id)
        {
            // Buscar la empresa en la lista estatica
            Empresa = Empresa.ObtenerEmpresa().FirstOrDefault(e => e.Id == id);
            if (Empresa == null)
            {
                return NotFound();
            }

            // Obtener los casos asociados a la empresa
            Casos = _context.Casos
                .Where(c => c.EmpresaId == id)
                .Include(c => c.Investigador)
                .ToList();

            return Page();
        }
    }
}
