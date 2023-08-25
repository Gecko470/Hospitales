using System.ComponentModel.DataAnnotations;

namespace Hospitales.Clases
{
    public class PaginaCLS
    {
        [Display(Name = "Id")]
        public int Iidpagina { get; set; }
        [Display(Name = "Mensaje")]
        [Required(ErrorMessage = "El campo {0} es obligatorio..")]
        public string Mensaje { get; set; }
        [Display(Name = "Acción")]
        [Required(ErrorMessage = "El campo {0} es obligatorio..")]
        public string Accion { get; set; }
        [Display(Name = "Controlador")]
        [Required(ErrorMessage = "El campo {0} es obligatorio..")]
        public string Controlador { get; set; }

        //ADICIONAMOS
        public string ErrorMensaje { get; set; }
    }
}
