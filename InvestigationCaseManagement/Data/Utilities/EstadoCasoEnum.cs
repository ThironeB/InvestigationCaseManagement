using System.Runtime.Serialization;

namespace InvestigationCaseManagement.Data.Utilities
{
    public enum EstadoCaso
    {
        [EnumMember(Value = "Abierto")]
        Abierto,

        [EnumMember(Value = "Asignado")]
        Asignado,

        [EnumMember(Value = "Cerrado")]
        Cerrado,

        [EnumMember(Value = "ReAbierto")]
        ReAbierto
    }
}
