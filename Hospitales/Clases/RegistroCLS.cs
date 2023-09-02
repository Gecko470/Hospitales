using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Hospitales.Clases
{
    public class RegistroCLS
    {
        //PERSONA

        [Display(Name = "Id")]
        public int Iidpersona { get; set; }
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo {0} es obligatorio..")]
        public string Nombre { get; set; }
        [Display(Name = "Ap.Paterno")]
        [Required(ErrorMessage = "El campo {0} es obligatorio..")]
        public string Appaterno { get; set; }
        [Display(Name = "Ap.Materno")]
        [Required(ErrorMessage = "El campo {0} es obligatorio..")]
        public string Apmaterno { get; set; }
        [Display(Name = "Email")]
        [Required(ErrorMessage = "El campo {0} es obligatorio..")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Ingresa un email válido..")]
        public string Email { get; set; }
        [Display(Name = "Dirección")]
        [Required(ErrorMessage = "El campo {0} es obligatorio..")]
        public string Direccion { get; set; }
        [Display(Name = "Telef.Fijo")]
        [Required(ErrorMessage = "El campo {0} es obligatorio..")]
        [StringLength(10, ErrorMessage = "El campo {0} debe tener un máx. de {1} caracteres..")]
        public string Telefonofijo { get; set; }
        [Display(Name = "Telef.Móvil")]
        [Required(ErrorMessage = "El campo {0} es obligatorio..")]
        [StringLength(10, ErrorMessage = "El campo {0} debe tener un máx. de {1} caracteres..")]
        public string Telefonocelular { get; set; }
        [Display(Name = "Fecha Nto.")]

        [DataType(DataType.Date, ErrorMessage = "El formato no es correcto..")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Fechanacimiento { get; set; }
        [Display(Name = "Sexo")]
        [Required(ErrorMessage = "El campo {0} es obligatorio..")]
        public int? Iidsexo { get; set; }
        [Display(Name = "Fecha Nto.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio..")]
        public string FechaNtoString { get; set; }
        public string Foto { get; set; }

        //USUARIO

        [Required(ErrorMessage = "El campo {0} es obligatorio..")]
        [DisplayName("Nombre Usuario")]
        public string Nombreusuario { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio..")]
        [DisplayName("Password")]
        public string Contraseña { get; set; }
        [DisplayName("Tipo Usuario")]
        public string nombreTipoUsuario { get; set; }
    }
}
