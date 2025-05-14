using System.ComponentModel.DataAnnotations;
namespace ProjectBlack.CreditRequests.Dtos
{
    public class CreateCreditRequestDto
    {
        [Required]
        public decimal MontoSolicitado { get; set; }
        public int PlazoMeses { get; set; }
        public decimal IngresoMensual { get; set; }
        public int AntiguedadLaboral { get; set; }
    }
}
