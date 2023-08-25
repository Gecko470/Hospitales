using System.ComponentModel.DataAnnotations;

namespace Hospitales.Clases
{
    public class TipoUsuarioCLS
    {
        [Display(Name = "Id")]
        public int Iidtipousuario { get; set; }
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo {0} es obligatorio..")]
        public string Nombre { get; set; }
        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "El campo {0} es obligatorio..")]
        public string Descripcion { get; set; }

        //ADICIONAMOS
        public string ErrorNombre { get; set; }
        public string ErrorDescripcion { get; set; }
    }
}
