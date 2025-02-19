namespace InvestigationCaseManagement.Data
{
    public class TipoIrregularidad
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = string.Empty;

        public static List<TipoIrregularidad> ObtenerTipoIrregularidad()
        {
            return new List<TipoIrregularidad> {
                new TipoIrregularidad { Id = 1, Tipo = "TipoIrregularidad1" },
                new TipoIrregularidad { Id = 2, Tipo = "TipoIrregularidad2" },
                new TipoIrregularidad { Id = 3, Tipo = "TipoIrregularidad3" }
            };
        }
    }
}
