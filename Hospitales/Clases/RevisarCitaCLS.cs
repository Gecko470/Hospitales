using System.ComponentModel.DataAnnotations;

namespace Hospitales.Clases
{
    public class RevisarCitaCLS
    {
        [Display(Name = "Doctor")]
        [Required(ErrorMessage = "El campo {0} es obligatorio..")]
        public int? iidDoctor { get; set; }
        [Display(Name = "Fecha")]
        [Required(ErrorMessage = "El campo {0} es obligatorio..")]
        public DateTime fechaCita { get; set; }
        [Display(Name = "Precio")]
        [Required(ErrorMessage = "El campo {0} es obligatorio..")]
        public string precioCita { get; set; }
        public int? iidCita { get; set; }
    }
}
