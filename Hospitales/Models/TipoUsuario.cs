using System;
using System.Collections.Generic;

namespace Hospitales.Models
{
    public partial class TipoUsuario
    {
        public TipoUsuario()
        {
            TipoUsuarioPaginas = new HashSet<TipoUsuarioPagina>();
            Usuarios = new HashSet<Usuario>();
        }

        public int Iidtipousuario { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public int? Bhabilitado { get; set; }

        public virtual ICollection<TipoUsuarioPagina> TipoUsuarioPaginas { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
