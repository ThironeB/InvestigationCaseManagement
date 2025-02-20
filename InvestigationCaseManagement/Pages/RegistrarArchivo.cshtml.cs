using InvestigationCaseManagement.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InvestigationCaseManagement.Pages
{
    public class RegistrarArchivoModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public string UsuarioActualId { get; set; }
        public string UsuarioActualText { get; set; }

        public RegistrarArchivoModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Archivo Archivo { get; set; } = new Archivo();

        public async Task OnGetAsync()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            RemoveModelState();

            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            await RegistrarArchivo(Archivo);

            return Page();
        }

        public void RemoveModelState()
        {
            ModelState.Remove("Archivo.Estado");
            ModelState.Remove("Archivo.Investigador");
            ModelState.Remove("Archivo.InvestigadorId");
        }

        public async Task RegistrarArchivo(Archivo archivo)
        {
            var usuarioActual = await _userManager.GetUserAsync(User);
            UsuarioActualId = usuarioActual?.Id;
            UsuarioActualText = usuarioActual?.UserName;
            archivo.InvestigadorId = UsuarioActualId;
            archivo.Estado = "Abierto";
            archivo.FechaCreacion = DateTime.Now;

            _context.Archivos.Add(archivo);
            await _context.SaveChangesAsync();
            ViewData["MostrarPopup"] = true;
        }
    }
}
