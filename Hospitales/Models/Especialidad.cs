using System;
using System.Collections.Generic;

namespace Hospitales.Models
{
    public partial class Especialidad
    {
        public Especialidad()
        {
            Doctors = new HashSet<Doctor>();
        }

        public int Iidespecialidad { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public int? Bhabilitado { get; set; }

        public virtual ICollection<Doctor> Doctors { get; set; }
    }
}
