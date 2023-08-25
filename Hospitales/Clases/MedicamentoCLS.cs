using System.ComponentModel.DataAnnotations;

namespace Hospitales.Clases
{
    public class MedicamentoCLS
    {
        [Display(Name = "Id")]
        public int Iidmedicamento { get; set; }
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo {0} es obligatorio..")]
        public string Nombre { get; set; }
        [Display(Name = "Concentración")]
        public string Concentracion { get; set; }
        [Display(Name = "Precio")]
        [Required(ErrorMessage = "El campo {0} es obligatorio..")]
        public decimal? Precio { get; set; }
        [Display(Name = "Stock")]
        [Required(ErrorMessage = "El campo {0} es obligatorio..")]
        [Range(1, 500, ErrorMessage = "El campo {0} debe estar entre {1} y {2}")]
        public int Stock { get; set; }
        [Display(Name = "Presentación")]
        public string Presentacion { get; set; }
        [Display(Name = "Forma Farmacéutica")]
        public string NombreFormaFarmaceutica { get; set; }
        [Display(Name = "Forma Farmacéutica")]
        [Required(ErrorMessage = "El campo {0} es obligatorio..")]
        public int? IidFormaFarmaceutica { get; set; }
    }
}
