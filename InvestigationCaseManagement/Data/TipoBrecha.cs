namespace InvestigationCaseManagement.Data
{
    public class TipoBrecha
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = string.Empty;

        public static List<TipoBrecha> ObtenerTipos()
        {
            return new List<TipoBrecha> {
                new TipoBrecha { Id = 1, Tipo = "Brecha1" },
                new TipoBrecha { Id = 2, Tipo = "Brecha2" },
            };
        }
    }
}
