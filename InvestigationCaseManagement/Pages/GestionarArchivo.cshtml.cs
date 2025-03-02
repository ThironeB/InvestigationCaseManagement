using System.Text.Json;
using InvestigationCaseManagement.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace InvestigationCaseManagement.Pages
{
    public class GestionarArchivoModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        // Constructor de la clase GestionarArchivoModel
        public GestionarArchivoModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Archivo Archivo { get; set; } = new Archivo(); // Archivo a gestionar

        public async Task<IActionResult> OnGetAsync(int id) // Método para obtener el archivo por ID
        {
            Archivo = await _context.Archivos.FindAsync(id); // Buscar el archivo por ID
            if (Archivo == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            RemoveModelState(); // Eliminar el estado del modelo para casos que no apliquen

            if (!ModelState.IsValid)
            {
                await OnGetAsync(Archivo.Id);
                return Page();
            }

            var archivoEnBd = await _context.Archivos // Obtener el archivo de la base de datos
            .AsNoTracking() // Evita rastreo para no afectar la entidad original
            .FirstOrDefaultAsync(a => a.Id == Archivo.Id);

            Archivo = JsonSerializer.Deserialize<Archivo>(JsonSerializer.Serialize(archivoEnBd));

            // Verificar si el archivo existe en la base de datos y actualizar su estado y fecha de cierre.
            if (archivoEnBd != null)
            {
                Archivo.Estado = "Negado";
                Archivo.FechaCierre = DateTime.Now.Date;

                _context.Attach(Archivo);
                _context.Entry(Archivo).Property(a => a.Estado).IsModified = true;
                _context.Entry(Archivo).Property(a => a.FechaCierre).IsModified = true;
                _context.Entry(Archivo).OriginalValues.SetValues(archivoEnBd);

                await _context.SaveChangesAsync();
            }

            ViewData["MostrarPopup"] = true;

            await OnGetAsync(Archivo.Id);
            return Page();
        }

        public void RemoveModelState()
        {
            ModelState.Remove("Archivo.Estado");
            ModelState.Remove("Archivo.Investigador");
        }
    }
}
