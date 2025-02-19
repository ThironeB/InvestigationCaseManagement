namespace InvestigationCaseManagement.Data
{
    public class ProcedenciaCaso
    {
        public int Id { get; set; }
        public string Procedencia { get; set; } = string.Empty;

        public static List<ProcedenciaCaso> ObtenerProcedenciaCaso()
        {
            return new List<ProcedenciaCaso> {
                new ProcedenciaCaso { Id = 1, Procedencia = "ProcedenciaCaso1" },
                new ProcedenciaCaso { Id = 2, Procedencia = "ProcedenciaCaso2" }
            };
        }
    }
}
