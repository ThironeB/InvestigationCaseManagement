using InvestigationCaseManagement.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InvestigationCaseManagement.Data.Utilities;

namespace InvestigationCaseManagement.Pages
{
    public class DetalleInvestigadorModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        // Constructor de la clase DetalleInvestigadorModel
        public DetalleInvestigadorModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<Caso> Casos { get; set; } = new(); // Lista de casos asociados al investigador

        public string Investigador {  get; set; } // Nombre del investigador

        public IActionResult OnGet(string id)
        {
            // Obtener los casos asociados al investigador
            Casos = _context.Casos
                .Where(c => c.InvestigadorId == id && c.Estado != EstadoCaso.Asignado.ToString())
                .Include(c => c.Investigador)
                .ToList();

            if (Casos == null)
            {
                return NotFound();
            }

            Investigador = _userManager.FindByIdAsync(id).Result.UserName; // Obtener el nombre del investigador

            return Page();
        }
    }
}
