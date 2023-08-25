using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Hospitales.Clases
{
    public class DoctorCLS
    {
        //LISTAR

        [Display(Name = "Id")]
        [DisplayName("Id")]
        public int Iiddoctor { get; set; }
        [Display(Name = "Nombre")]
        [DisplayName("Nombre")]
        public string NombreCompleto { get; set; }
        [Display(Name = "Sede")]
        [DisplayName("Sede")]
        public string Sede { get; set; }
        [Display(Name = "Especialidad")]
        [DisplayName("Especialidad")]
        public string Especialidad { get; set; }

        //AGREGAR

        [Required(ErrorMessage = "El campo {0} es obligatorio..")]
        [Display(Name = "Sede")]
        public int? Iidsede { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio..")]
        [Display(Name = "Especialidad")]
        public int? Iidespecialidad { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio..")]
        [Display(Name = "Sueldo")]
        public decimal? Sueldo { get; set; }
        [Display(Name = "Fecha contrato")]
        public DateTime? Fechacontrato { get; set; }
        [Display(Name = "Persona")]
        public int? Iidpersona { get; set; }
        public int? Bhabilitado { get; set; }
        [Required(ErrorMessage = "El campo Fecha Contrato es obligatorio..")]
        public string FechaContratoString { get; set; }
    }
}
