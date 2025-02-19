namespace InvestigationCaseManagement.Data
{
    public class SubTipoFicha
    {
        public int Id { get; set; }
        public string sSubTipoFicha { get; set; } = string.Empty;

        public static List<SubTipoFicha> ObtenerSubTipoFicha()
        {
            return new List<SubTipoFicha> {
                new SubTipoFicha { Id = 1, sSubTipoFicha = "SubTipoFicha1" },
                new SubTipoFicha { Id = 2, sSubTipoFicha = "SubTipoFicha2" },
                new SubTipoFicha { Id = 3, sSubTipoFicha = "SubTipoFicha3" }
            };
        }
    }
}
