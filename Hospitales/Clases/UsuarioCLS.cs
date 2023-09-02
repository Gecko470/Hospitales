using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Hospitales.Clases
{
    public class UsuarioCLS
    {
        //LISTAR

        [Display(Name = "Id")]
        [DisplayName("Id")]
        public int Iidusuario { get; set; }
        [Display(Name = "Nombre")]
        [DisplayName("Nombre")]
        public string NombreCompleto { get; set; }
        [Display(Name = "Nombre Usuario")]
        [DisplayName("Nombre Usuario")]
        public string Usuario { get; set; }
        [Display(Name = "Tipo de Usuario")]
        [DisplayName("Tipo de Usuario")]
        public string TipoUsuario { get; set; }

        //AGREGAR

        [Required(ErrorMessage = "El campo {0} es obligatorio..")]
        [DisplayName("Tipo de Usuario")]
        public int Iidtipousuario { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio..")]
        [DisplayName("Nombre Usuario")]
        public string Nombreusuario { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio..")]
        [DisplayName("Password")]
        public string Contraseña { get; set; }
        public int? Bhabilitado { get; set; }
        [DisplayName("Persona")]
        public int? Iidpersona { get; set; }
    }
}
