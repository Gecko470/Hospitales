using Hospitales.Clases;
using Hospitales.Helpers;
using Hospitales.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hospitales.Controllers
{
    public class LoginController : Controller
    {
        private readonly BDHospitalContext context;

        public LoginController(BDHospitalContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Registro()
        {
            return View();
        }

        public async Task<string> Login(string user, string pass)
        {
            string resp = "";
            var passCifrada = Encriptar.EncriptarPass(pass);
            Usuario usuario = await context.Usuarios.FirstOrDefaultAsync(x => x.Nombreusuario == user && x.Contraseña == passCifrada);

            if (usuario != null)
            {
                resp = "1";
                HttpContext.Session.SetString("user", usuario.Iidusuario.ToString());

                List<PaginaCLS> listaPaginas = await (from tipoUsuarioPag in context.TipoUsuarioPaginas
                                                      join pagina in context.Paginas
                                                      on tipoUsuarioPag.Iidpagina equals pagina.Iidpagina
                                                      where tipoUsuarioPag.Bhabilitado == 1 && tipoUsuarioPag.Iidtipousuario == usuario.Iidtipousuario
                                                      select new PaginaCLS()
                                                      {
                                                          Mensaje = pagina.Mensaje,
                                                          Accion = pagina.Accion,
                                                          Controlador = pagina.Controlador,

                                                      }).ToListAsync();

               Listas.ListaPaginasMenu = listaPaginas;
            }

            return resp;
        }

        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Remove("user");
            return RedirectToAction("Index");
        }
    }
}
