using System;
using System.Collections.Generic;

namespace Hospitales.Models
{
    public partial class FormaFarmaceutica
    {
        public FormaFarmaceutica()
        {
            Medicamentos = new HashSet<Medicamento>();
        }

        public int Iidformafarmaceutica { get; set; }
        public string? Nombre { get; set; }
        public int? Bhabilitado { get; set; }

        public virtual ICollection<Medicamento> Medicamentos { get; set; }
    }
}
