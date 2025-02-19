namespace InvestigationCaseManagement.Data
{
    public class SubTipoIrregularidad
    {
        public int Id { get; set; }
        public string SubTipo { get; set; } = string.Empty;

        public static List<SubTipoIrregularidad> ObtenerSubTipoIrregularidad()
        {
            return new List<SubTipoIrregularidad> {
                new SubTipoIrregularidad { Id = 1, SubTipo = "SubTipoIrregularidad1" },
                new SubTipoIrregularidad { Id = 2, SubTipo = "SubTipoIrregularidad2" }
            };
        }
    }
}
