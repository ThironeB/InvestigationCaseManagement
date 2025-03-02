using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace InvestigationCaseManagement.Data
{
    public class Caso
    {
        public int Id { get; set; } // Identificador único del caso

        [Required]
        public string NumeroExpediente { get; set; } // Número de expediente

        public string InvestigadorId { get; set; } // FK para el investigador asignado

        public IdentityUser Investigador { get; set; } // Relación con el investigador

        [Required]
        public DateTime FechaInicio { get; set; } = DateTime.Now.Date; // Fecha actual por defecto

        [Required]
        public int DuracionDias { get; set; } // Duración en días

        [Required]
        public string TipoCaso { get; set; } // Tipo de caso 

        [Required]
        public string MovilAfectado { get; set; } // Móvil afectado

        [Required]
        public string ObjetivoAgraviado { get; set; } // Objetivo/Agraviado

        [Required]
        public string Incidencia { get; set; } // Incidencia

        [Required]
        public string ModusOperandi { get; set; } // Modus Operandi

        [Required]
        public string AreaApoyo { get; set; } // Área de apoyo

        [Required]
        public string OrigenCaso { get; set; } // Origen del caso

        [Required]
        public string DetallesFraude { get; set; } // Detalles de fraude

        [Required]
        public string Actuaciones { get; set; } // Actuaciones realizadas

        [Required]
        public string Conclusiones { get; set; } = string.Empty; // Conclusiones

        [Required]
        public string Observaciones { get; set; } = string.Empty;// Observaciones

        [Required]
        public string Soporte { get; set; } = string.Empty;// Soporte
        public string Estado { get; set; } // Estado del caso (Abierto, Asignado, Cerrado, ReAbierto)

        public DateTime UltimaActualizacion { get; set; } // Fecha de la última actualización

        [Required]
        public string PersonasInvolucradas { get; set; } // Personas involucradas

        [Required]
        [Precision(18,2)]
        public Decimal MontoExpuesto { get; set; } // Monto expuesto

        [Required]
        public int TipoProyectoId { get; set; } // FK para el tipo de proyecto
        public TipoProyecto TipoProyecto => TipoProyecto.ObtenerTipos().FirstOrDefault(t => t.Id == TipoProyectoId) ?? new TipoProyecto();

        [Required]
        public int TipoBrechaId { get; set; } // FK para el tipo de brecha
        public TipoBrecha TipoBrecha => TipoBrecha.ObtenerTipos().FirstOrDefault(t => t.Id == TipoBrechaId) ?? new TipoBrecha();

        [Required]
        public int ProcesoCorregidoId { get; set; } // FK para el proceso corregido
        public ProcesoCorregido ProcesoCorregido => ProcesoCorregido.ObtenerProcesos().FirstOrDefault(t => t.Id == ProcesoCorregidoId) ?? new ProcesoCorregido();

        [Required]
        public int ProcesoRealizadoId { get; set; } // FK para el proceso realizado
        public ProcesoRealizado ProcesoRealizado => ProcesoRealizado.ObtenerProcesos().FirstOrDefault(t => t.Id == ProcesoRealizadoId) ?? new ProcesoRealizado();

        [Required]
        public int EmpresaId { get; set; } // FK para la empresa
        public Empresa Empresa => Empresa.ObtenerEmpresa().FirstOrDefault(t => t.Id == EmpresaId) ?? new Empresa();

        [Required]
        public int SubTipoFichaId { get; set; } // FK para el subtipo de ficha
        public SubTipoFicha SubTipoFicha => SubTipoFicha.ObtenerSubTipoFicha().FirstOrDefault(t => t.Id == SubTipoFichaId) ?? new SubTipoFicha();

        [Required]
        public int TipoIrregularidadId { get; set; } // FK para el tipo de irregularidad
        public TipoIrregularidad TipoIrregularidad => TipoIrregularidad.ObtenerTipoIrregularidad().FirstOrDefault(t => t.Id == TipoIrregularidadId) ?? new TipoIrregularidad();

        [Required]
        public int SubTipoIrregularidadId { get; set; } // FK para el subtipo de irregularidad
        public SubTipoIrregularidad SubTipoIrregularidad => SubTipoIrregularidad.ObtenerSubTipoIrregularidad().FirstOrDefault(t => t.Id == SubTipoIrregularidadId) ?? new SubTipoIrregularidad();

        [Required]
        public int ProcedenciaCasoId { get; set; } // FK para la procedencia del caso
        public ProcedenciaCaso ProcedenciaCaso => ProcedenciaCaso.ObtenerProcedenciaCaso().FirstOrDefault(t => t.Id == ProcedenciaCasoId) ?? new ProcedenciaCaso();

        public bool NecesitaAtencion() // Casos que necesitan atencion. Tienen +1 dias sin atencion.
        {
            return (DateTime.Now - (UltimaActualizacion != DateTime.MinValue ? UltimaActualizacion : FechaInicio)).TotalDays > 1;
        }

        public string TiempoSinAtencionFormateado() // Formatear el tiempo sin atencion
        {
            var tiempoSinAtencion = DateTime.Now - (UltimaActualizacion != DateTime.MinValue ? UltimaActualizacion : FechaInicio);
            int dias = tiempoSinAtencion.Days;
            int horas = tiempoSinAtencion.Hours;
            int minutos = tiempoSinAtencion.Minutes;

            return $"{dias} Días, {horas} Horas y {minutos} Minutos";
        }
    }
}
