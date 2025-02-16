using InvestigationCaseManagement.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class RegistroCasosModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public RegistroCasosModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [BindProperty]
    public Caso Caso { get; set; } = new Caso();

    public List<SelectListItem> Investigadores { get; set; }
    public string UsuarioActualId { get; set; }
    public string UsuarioActualText { get; set; }

    public async Task OnGetAsync()
    {
        var usuarioActual = await _userManager.GetUserAsync(User);
        UsuarioActualId = usuarioActual?.Id;
        UsuarioActualText = usuarioActual?.UserName;

        if (User.IsInRole("Administrador"))
        {
            // Obtener la lista de investigadores (usuarios con rol "Investigador")
            Investigadores = await _context.Users
                .Where(u => _context.UserRoles.Any(ur => ur.UserId == u.Id && ur.RoleId == "68a24bd8-6f5d-4951-9e41-45b232780e1a"))
                .Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = u.UserName
                })
                .ToListAsync();
        }
        else
        {
            // Si el usuario es un investigador, asignar su ID al caso
            Caso.InvestigadorId = UsuarioActualId;
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        ModelState.Remove("Caso.Estado");
        ModelState.Remove("Caso.Investigador");
        // Asignar el estado del caso
        Caso.Estado = User.IsInRole("Administrador") ? "Asignado" : "Abierto";

        var investigador = await _userManager.FindByIdAsync(Caso.InvestigadorId);

        if (investigador != null)
        {
            Caso.Investigador = investigador;
        }
        else
        {
            return Page();
        }


        if (!ModelState.IsValid)
        {
            await OnGetAsync();
            return Page();
        }

        _context.Casos.Add(Caso);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}