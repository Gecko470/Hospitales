using System.ComponentModel.DataAnnotations;

namespace Hospitales.Clases
{
    public class HistorialCitaCLS
    {
        [Display(Name = "Id")]
        public int Iidhistorialcita { get; set; }
        [Display(Name = "Id Cita")]
        public int Iidcita { get; set; }
        [Display(Name = "Estado")]
        public string nombreEstado { get; set; }
        [Display(Name = "Usuario")]
        public string nombreUsuario { get; set; }
        [Display(Name = "Fecha")]
        public string Dfecha { get; set; }
    }
}
