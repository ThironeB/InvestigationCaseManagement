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

        public GestionarArchivoModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Archivo Archivo { get; set; } = new Archivo();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Archivo = await _context.Archivos.FindAsync(id);
            if (Archivo == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            RemoveModelState();

            if (!ModelState.IsValid)
            {
                await OnGetAsync(Archivo.Id);
                return Page();
            }

            //Archivo.Estado = "Negado";
            //Archivo.FechaCierre = DateTime.Now.Date;

            var archivoEnBd = await _context.Archivos
            .AsNoTracking() // Evita rastreo para no afectar la entidad original
            .FirstOrDefaultAsync(a => a.Id == Archivo.Id);

            //Archivo = archivoEnBd;

            Archivo = JsonSerializer.Deserialize<Archivo>(JsonSerializer.Serialize(archivoEnBd));

            if (archivoEnBd != null)
            {
                Archivo.Estado = "Negado";
                Archivo.FechaCierre = DateTime.Now.Date;

                _context.Attach(Archivo);
                _context.Entry(Archivo).Property(a => a.Estado).IsModified = true;
                _context.Entry(Archivo).Property(a => a.FechaCierre).IsModified = true;

                // Asegurar que la auditoría registre los valores originales correctamente
                _context.Entry(Archivo).OriginalValues.SetValues(archivoEnBd);

                await _context.SaveChangesAsync();
            }

            //_context.Attach(Archivo);
            //_context.Entry(Archivo).Property(a => a.Estado).IsModified = true;
            //_context.Entry(Archivo).Property(a => a.FechaCierre).IsModified = true;
            //await _context.SaveChangesAsync();

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
