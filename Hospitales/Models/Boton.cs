using System;
using System.Collections.Generic;

namespace Hospitales.Models
{
    public partial class Boton
    {
        public Boton()
        {
            TipoUsuarioPaginaBotons = new HashSet<TipoUsuarioPaginaBoton>();
        }

        public int Iidboton { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public int? Bhabilitado { get; set; }

        public virtual ICollection<TipoUsuarioPaginaBoton> TipoUsuarioPaginaBotons { get; set; }
    }
}
