using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using InvestigationCaseManagement.Data;
using System.Text.Json.Nodes;

namespace InvestigationCaseManagement.Pages
{
    public class DetalleAuditoriaModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetalleAuditoriaModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Auditoria Auditoria { get; set; }
        public string DatosDeserializados { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Auditoria = await _context.Auditorias.FirstOrDefaultAsync(a => a.Id == id);

            if (Auditoria == null)
            {
                return NotFound();
            }

            try
            {
                var jsonObject = JsonNode.Parse(Auditoria.DatosEntidad);

                // Verificar si existe la propiedad "Investigador" en el JSON
                if (jsonObject is JsonObject obj && obj.ContainsKey("Investigador"))
                {
                    var investigador = obj["Investigador"]?.AsObject();
                    if (investigador != null)
                    {
                        // Conservar solo Id y UserName, eliminando el resto de propiedades
                        var investigadorFiltrado = new JsonObject
                        {
                            ["Id"] = investigador["Id"]?.GetValue<string>(),  // Extrae el valor como string
                            ["UserName"] = investigador["UserName"]?.GetValue<string>()
                        };

                        obj["Investigador"] = investigadorFiltrado; // Reemplazar el objeto Investigador en el JSON
                    }
                }

                DatosDeserializados = jsonObject?.ToJsonString(new JsonSerializerOptions { WriteIndented = true }) ?? "Error al procesar los datos.";
            }
            catch
            {
                DatosDeserializados = "Error al deserializar los datos.";
            }

            return Page();
        }
    }
}
