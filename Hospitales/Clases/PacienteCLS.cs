using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Hospitales.Clases
{
    public class PacienteCLS
    {
        //LISTAR

        [Display(Name = "Id")]
        [DisplayName("Id")]
        public int Iidpaciente { get; set; }
        [Display(Name = "Nombre")]
        [DisplayName("Nombre")]
        public string NombreCompleto { get; set; }
        [Display(Name = "Grupo Sanguíneo")]
        [DisplayName("Grupo Sanguíneo")]
        public string GrupoSanguineo { get; set; }
        [Display(Name = "Alergias")]
        [DisplayName("Alergias")]
        public string Alergias { get; set; }

        //AGREGAR

        [Required(ErrorMessage = "El campo {0} es obligatorio..")]
        [Display(Name = "Grupo Sanguíneo")]
        public int? Iidtiposangre { get; set; }
        [Display(Name = "Enfermedades crónicas")]
        public string Enfermedadescronicas { get; set; }
        [Display(Name = "Vacunas")]
        public string Cuadrovacunas { get; set; }
        [Display(Name = "Antecedentes quirúrgicos")]
        public string Antecedentesquirurgicos { get; set; }
        [Display(Name = "Persona")]
        public int? Iidpersona { get; set; }
        public int? Bhabilitado { get; set; }
    }
}
