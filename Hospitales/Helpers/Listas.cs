using Hospitales.Clases;
using Hospitales.Models;
using Microsoft.EntityFrameworkCore;

namespace Hospitales.Helpers
{
    public class Listas
    {

        public static List<PaginaCLS> ListaPaginasMenu { get; set; } = new List<PaginaCLS>();
        public static List<PaginaCLS> ListaBotonesPag { get; set; } = new List<PaginaCLS>();


        public static List<PaginaCLS> listarBotones(string controlador)
        {
            List<PaginaCLS> listaPagBtn = new List<PaginaCLS>();
            listaPagBtn = ListaBotonesPag.Where(x => x.Controlador.ToUpper() == controlador.ToUpper()).ToList();

            return listaPagBtn;
        }

        public static async Task<int> TipoVista(string nombrePagina, int idUsuario)
        {
            int resultado = 0;
            using (var bd = new BDHospitalContext())
            {
                resultado = await (from tipoUsuarioPag in bd.TipoUsuarioPaginas
                                   join tipoUsuario in bd.TipoUsuarios
                                   on tipoUsuarioPag.Iidtipousuario equals tipoUsuario.Iidtipousuario
                                   join pagina in bd.Paginas
                                   on tipoUsuarioPag.Iidpagina equals pagina.Iidpagina
                                   join usuario in bd.Usuarios
                                   on tipoUsuario.Iidtipousuario equals usuario.Iidtipousuario
                                   where usuario.Iidusuario == idUsuario && pagina.Mensaje == nombrePagina && usuario.Bhabilitado == 1
                                   select tipoUsuarioPag.Iidvista).FirstOrDefaultAsync();
            }

            return resultado;
        }
    }
}
