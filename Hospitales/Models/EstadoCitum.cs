using System;
using System.Collections.Generic;

namespace Hospitales.Models
{
    public partial class EstadoCitum
    {
        public EstadoCitum()
        {
            Cita = new HashSet<Citum>();
            HistorialCita = new HashSet<HistorialCitum>();
        }

        public int Iidestado { get; set; }
        public string? Vnombre { get; set; }
        public string? Vdescripcion { get; set; }
        public int? Bhabilitado { get; set; }

        public virtual ICollection<Citum> Cita { get; set; }
        public virtual ICollection<HistorialCitum> HistorialCita { get; set; }
    }
}
