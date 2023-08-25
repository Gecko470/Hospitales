using System;
using System.Collections.Generic;

namespace Hospitales.Models
{
    public partial class Paciente
    {
        public int Iidpaciente { get; set; }
        public int? Iidtiposangre { get; set; }
        public string? Alergias { get; set; }
        public string? Enfermedadescronicas { get; set; }
        public string? Cuadrovacunas { get; set; }
        public string? Antecedentesquirurgicos { get; set; }
        public int? Iidpersona { get; set; }
        public int? Bhabilitado { get; set; }

        public virtual TipoSangre? IidtiposangreNavigation { get; set; }
    }
}
