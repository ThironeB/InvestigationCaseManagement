namespace InvestigationCaseManagement.Data
{
    public class ProcesoRealizado
    {
        public int Id { get; set; }
        public string Proceso { get; set; } = string.Empty;

        public static List<ProcesoRealizado> ObtenerProcesos()
        {
            return new List<ProcesoRealizado> {
                new ProcesoRealizado { Id = 1, Proceso = "Proceso1" },
                new ProcesoRealizado { Id = 2, Proceso = "Proceso2" }
            };
        }
    }
}
