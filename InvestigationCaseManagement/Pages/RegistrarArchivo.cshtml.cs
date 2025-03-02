using InvestigationCaseManagement.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InvestigationCaseManagement.Data.Utilities;

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
        public Archivo Archivo { get; set; } = new Archivo(); // Archivo a registrar

        public async Task OnGetAsync()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            RemoveModelState(); // Eliminar el estado del modelo para casos que no apliquen

            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            await RegistrarArchivo(Archivo); // Registrar el archivo

            return Page();
        }

        public void RemoveModelState()
        {
            ModelState.Remove("Archivo.Estado");
            ModelState.Remove("Archivo.Investigador");
            ModelState.Remove("Archivo.InvestigadorId");
        }

        /// <summary>
        /// La función "RegistrarArchivo" registra de forma asíncrona un archivo, asigna el usuario actual como
        /// el investigador, establece el estado del archivo como "Abierto" y lo guarda en la base de datos.
        /// </summary>
        /// <param name="Archivo">Archivo es una clase que representa un archivo o documento en este contexto. Contiene
        /// propiedades como InvestigadorId, Investigador, Estado, FechaCreacion, etc. El método RegistrarArchivo es
        /// responsable de registrar un nuevo archivo en el sistema.</param>
        public async Task RegistrarArchivo(Archivo archivo)
        {
            var usuarioActual = await _userManager.GetUserAsync(User);
            UsuarioActualId = usuarioActual?.Id;
            UsuarioActualText = usuarioActual?.UserName;
            archivo.InvestigadorId = UsuarioActualId;
            archivo.Investigador = await _userManager.FindByIdAsync(UsuarioActualId);
            archivo.Estado = EstadoCaso.Abierto.ToString();
            archivo.FechaCreacion = DateTime.Now;

            _context.Archivos.Add(archivo); // Agregar el archivo a la base de datos
            await _context.SaveChangesAsync();
            ViewData["MostrarPopup"] = true;
        }
    }
}
