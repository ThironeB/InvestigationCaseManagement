namespace InvestigationCaseManagement.Data
{
    public class Empresa
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;

        public static List<Empresa> ObtenerEmpresa()
        {
            return new List<Empresa> {
                new Empresa { Id = 1, Nombre = "Empresa1" },
                new Empresa { Id = 2, Nombre = "Empresa2" },
                new Empresa { Id = 3, Nombre = "Empresa3" },
                new Empresa { Id = 4, Nombre = "Empresa4" },
                new Empresa { Id = 5, Nombre = "Empresa5" },
                new Empresa { Id = 6, Nombre = "Empresa6" },
            };
        }
    }
}
