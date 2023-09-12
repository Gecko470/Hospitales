using Hospitales.Models;
using System.ComponentModel.DataAnnotations;

namespace Hospitales.Clases
{
    public class CitaCLS
    {
        //AGREGAR
        [Display(Name = "Id")]
        public int Iidcita { get; set; }
        [Display(Name = "Persona")]
        [Required(ErrorMessage = "El campo {0} es obligatorio..")]
        public int Iidpersona { get; set; }
        [Display(Name = "Sede")]
        [Required(ErrorMessage = "El campo {0} es obligatorio..")]
        public int Iidsede { get; set; }
        [Display(Name = "Fecha Cita")]
        [Required(ErrorMessage = "El campo {0} es obligatorio..")]
        public string Dfechacita { get; set; }
        [Display(Name = "Desc.Enfermedad")]
        [Required(ErrorMessage = "El campo {0} es obligatorio..")]
        public string Descripcionenfermedad { get; set; }

        //LISTAR
        [Display(Name = "Nombre Persona")]
        public string nombrePersona { get; set; }
        [Display(Name = "Nombre Usuario")]
        public string nombreUsuario { get; set; }
        [Display(Name = "Estado")]
        public string nombreEstado { get; set; }
        public int iidEstadoCita { get; set; }
    }
}
