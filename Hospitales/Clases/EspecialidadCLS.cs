using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Hospitales.Clases
{
    public class EspecialidadCLS
    {
        [Display(Name = "Id")]
        [DisplayName("ID")]
        public int Iidespecialidad { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio..")]
        [Display(Name = "Nombre")]
        [DisplayName("Nombre")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio..")]
        [Display(Name = "Descripción")]
        [DisplayName("DESCRIPCIÓN")]
        public string Descripcion { get; set; }

        //ADICIONAMOS
        public string Mensaje { get; set; } = "";
    }
}
