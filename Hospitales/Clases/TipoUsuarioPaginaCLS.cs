using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Hospitales.Clases
{
    public class TipoUsuarioPaginaCLS
    {
        //LISTAR
         [Display(Name = "Tipo Usuario")]
         [DisplayName("Tipo Usuario")]
        public string NombreTipoUsuario { get; set; }
         [Display(Name = "Página")]
         [DisplayName("Página")]
        public string NombrePagina { get; set; }

        //INSERTAR
        [Display(Name = "Id")]
        [DisplayName("ID")]
        public int Iidtipousuariopagina { get; set; }
        public int? Iidtipousuario { get; set; }
        public int? Iidpagina { get; set; }
        public int? Bhabilitado { get; set; }
        public int? Iidvista { get; set; }
    }
}
