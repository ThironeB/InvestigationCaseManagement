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
using InvestigationCaseManagement.Data.Utilities;

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
                .Where(u => _context.UserRoles.Any(ur => ur.UserId == u.Id && ur.RoleId == RoleIdentifier.Investigador.ToString()))
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
        RemoveModelState(); // Remover los estados de los campos que no se deben validar

        var investigador = await _userManager.FindByIdAsync(Caso.InvestigadorId); // Obtener el investigador seleccionado
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

        Caso.Conclusiones = string.Empty;
        Caso.Observaciones = string.Empty;
        Caso.Soporte = string.Empty;

        await RegistrarCaso(Caso); // Registrar el caso

        return Page();
    }
    public void RemoveModelState()
    {
        ModelState.Remove("Caso.Estado");
        ModelState.Remove("Caso.Investigador");
        ModelState.Remove("Caso.Conclusiones");
        ModelState.Remove("Caso.Observaciones");
        ModelState.Remove("Caso.Soporte");
    }

    /// <summary>
    /// La función "RegistrarCaso" registra de forma asíncrona un caso, establece su estado en función del rol del usuario,
    /// lo agrega a la base de datos y muestra un mensaje emergente.
    /// </summary>
    /// <param name="Caso">Caso es un objeto que representa un caso en la aplicacion. La propiedad
    /// Estado se establece en función del rol del usuario.</param>
    public async Task RegistrarCaso(Caso caso)
    {
        caso.Estado = User.IsInRole("Administrador") ? EstadoCaso.Asignado.ToString() : EstadoCaso.Abierto.ToString();

        _context.Casos.Add(Caso);
        await _context.SaveChangesAsync();
        ViewData["MostrarPopup"] = true;
    }
}