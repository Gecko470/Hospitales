using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Hospitales.Clases
{
    public class SedeCLS
    {
        [Display(Name = "Id")]
        [DisplayName("ID")]
        public int Iidsede { get; set; }
        [Display(Name = "Nombre")]
        [DisplayName("NOMBRE")]
        [Required(ErrorMessage = "El campo {0} es obligatorio..")]
        public string Nombre { get; set; }
        [Display(Name = "Dirección")]
        [DisplayName("DIRECCIÓN")]
        [Required(ErrorMessage = "El campo {0} es obligatorio..")]
        public string Direccion { get; set; }
    }
}
