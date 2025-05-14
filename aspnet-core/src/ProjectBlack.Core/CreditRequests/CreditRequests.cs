using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;

namespace ProjectBlack.CreditRequests
{
    public enum CreditStatus { Pendiente, Aprobado, Rechazado }

    public class CreditRequest : FullAuditedEntity<long>
    {
        public decimal MontoSolicitado { get; set; }
        public int PlazoMeses { get; set; }
        public decimal IngresoMensual { get; set; }
        public int AntiguedadLaboral { get; set; }
        public CreditStatus Estado { get; set; }
        public long? UserId { get; set; } // Para saber quién la creó
    }
}
