using System;
using System.Collections.Generic;

namespace Hospitales.Models
{
    public partial class Medicamento
    {
        public Medicamento()
        {
            CitaMedicamentos = new HashSet<CitaMedicamento>();
        }

        public int Iidmedicamento { get; set; }
        public string? Nombre { get; set; }
        public string? Concentracion { get; set; }
        public int? Iidformafarmaceutica { get; set; }
        public decimal? Precio { get; set; }
        public int? Stock { get; set; }
        public string? Presentacion { get; set; }
        public int? Bhabilitado { get; set; }

        public virtual FormaFarmaceutica? IidformafarmaceuticaNavigation { get; set; }
        public virtual ICollection<CitaMedicamento> CitaMedicamentos { get; set; }
    }
}
