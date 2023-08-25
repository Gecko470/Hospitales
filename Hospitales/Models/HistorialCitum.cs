using System;
using System.Collections.Generic;

namespace Hospitales.Models
{
    public partial class HistorialCitum
    {
        public int Iidhistorialcita { get; set; }
        public int? Iidcita { get; set; }
        public int? Iidestado { get; set; }
        public int? Iidusuario { get; set; }
        public DateTime? Dfecha { get; set; }
        public string? Vobservacion { get; set; }

        public virtual Citum? IidcitaNavigation { get; set; }
        public virtual EstadoCitum? IidestadoNavigation { get; set; }
        public virtual Usuario? IidusuarioNavigation { get; set; }
    }
}
