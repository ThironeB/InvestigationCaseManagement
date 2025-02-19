namespace InvestigationCaseManagement.Data
{
    public class TipoProyecto
    {
        public int Id { get; set; }
        public string Tipo {  get; set; } = string.Empty;

        public static List<TipoProyecto> ObtenerTipos()
        {
            return new List<TipoProyecto> {
                new TipoProyecto { Id = 1, Tipo = "Proyecto1" },
                new TipoProyecto { Id = 2, Tipo = "Proyecto2" },
                new TipoProyecto { Id = 3, Tipo = "Proyecto3" },
                new TipoProyecto { Id = 4, Tipo = "Proyecto4" }
            };
        }
    }
}
