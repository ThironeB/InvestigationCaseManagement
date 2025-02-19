using InvestigationCaseManagement.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InvestigationCaseManagement.Pages
{
    public class DetalleInvestigadorModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DetalleInvestigadorModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<Caso> Casos { get; set; } = new();

        public string Investigador {  get; set; }

        public IActionResult OnGet(string id)
        {
            // Obtener los casos asociados a la empresa
            Casos = _context.Casos
                .Where(c => c.InvestigadorId == id && c.Estado != "Asignado")
                .Include(c => c.Investigador)
                .ToList();

            if (Casos == null)
            {
                return NotFound();
            }

            Investigador = _userManager.FindByIdAsync(id).Result.UserName;

            return Page();
        }
    }
}
