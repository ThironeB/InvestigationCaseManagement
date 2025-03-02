using InvestigationCaseManagement.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InvestigationCaseManagement.Data.Utilities;

namespace InvestigationCaseManagement.Pages
{
    public class ReportesModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ReportesModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<EmpresaReporte> Empresas { get; set; } = new();
        public List<InvestigadorReporte> Investigadores { get; set; } = new();

        public async Task OnGetAsync()
        {
            // Obtener lista de empresas estaticas
            var empresasLista = Empresa.ObtenerEmpresa();

            // Contar los casos por empresa
            Empresas = empresasLista.Select(e => new EmpresaReporte
            {
                Id = e.Id,
                Nombre = e.Nombre,
                CasosRegistrados = _context.Casos.Count(c => c.EmpresaId == e.Id)
            }).ToList();

            // Obtener la lista de investigadores y contar los casos atendidos
            var investigadoresIds = _context.Casos
                .Select(c => c.InvestigadorId)  // Obtener todos los InvestigadorIds desde los casos
                .Distinct()
                .ToList();

            // Obtener la informacion de los investigadores
            Investigadores = new List<InvestigadorReporte>();
            foreach (var investigadorId in investigadoresIds)
            {
                var investigador = await _userManager.FindByIdAsync(investigadorId);

                if (investigador != null)
                {
                    var casosAtendidos = _context.Casos.Count(c => c.InvestigadorId == investigadorId && c.Estado != EstadoCaso.Asignado.ToString());

                    Investigadores.Add(new InvestigadorReporte
                    {
                        Id = investigador.Id,
                        Nombre = investigador.UserName,
                        CasosAtendidos = casosAtendidos
                    });
                }
            }
        }
    }
}
