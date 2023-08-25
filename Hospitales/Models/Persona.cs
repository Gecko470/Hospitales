using System;
using System.Collections.Generic;

namespace Hospitales.Models
{
    public partial class Persona
    {
        public Persona()
        {
            Cita = new HashSet<Citum>();
            Doctors = new HashSet<Doctor>();
            Usuarios = new HashSet<Usuario>();
        }

        public int Iidpersona { get; set; }
        public string? Nombre { get; set; }
        public string? Appaterno { get; set; }
        public string? Apmaterno { get; set; }
        public string? Email { get; set; }
        public string? Direccion { get; set; }
        public string? Telefonofijo { get; set; }
        public string? Telefonocelular { get; set; }
        public DateTime? Fechanacimiento { get; set; }
        public int? Iidsexo { get; set; }
        public int? Bdoctor { get; set; }
        public int? Bpaciente { get; set; }
        public int? Bhabilitado { get; set; }
        public string? Foto { get; set; }
        public int? Btieneusuario { get; set; }
        public int? Iidusuario { get; set; }

        public virtual Sexo? IidsexoNavigation { get; set; }
        public virtual Usuario? IidusuarioNavigation { get; set; }
        public virtual ICollection<Citum> Cita { get; set; }
        public virtual ICollection<Doctor> Doctors { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
