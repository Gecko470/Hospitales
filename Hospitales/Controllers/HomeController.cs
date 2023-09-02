using Hospitales.Clases;
using Hospitales.Filters;
using Hospitales.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Hospitales.Controllers
{
    [ServiceFilter(typeof(Seguridad))]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BDHospitalContext context;

        public HomeController(ILogger<HomeController> logger, BDHospitalContext context)
        {
            _logger = logger;
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            int iidUsuario = int.Parse(HttpContext.Session.GetString("user"));

            Usuario usuario = await context.Usuarios.FirstOrDefaultAsync(x => x.Iidusuario == iidUsuario);
            Persona persona = await context.Personas.FirstOrDefaultAsync(x => x.Iidpersona == usuario.Iidpersona);
            TipoUsuario tipoUsuario = await context.TipoUsuarios.FirstOrDefaultAsync(x => x.Iidtipousuario == usuario.Iidtipousuario);

            RegistroCLS oRegistroCLS = new RegistroCLS();
            oRegistroCLS.Nombreusuario = usuario.Nombreusuario;
            oRegistroCLS.Nombre = persona.Nombre;
            oRegistroCLS.Appaterno = persona.Appaterno;
            oRegistroCLS.Apmaterno = persona.Apmaterno;
            oRegistroCLS.Email = persona.Email;
            oRegistroCLS.Direccion = persona.Direccion;
            oRegistroCLS.Telefonocelular = persona.Telefonocelular;
            oRegistroCLS.Telefonofijo = persona.Telefonofijo;
            oRegistroCLS.FechaNtoString = string.Format("{0:dd-MM-yyyy}", persona.Fechanacimiento);
            oRegistroCLS.Foto = persona.Foto;
            oRegistroCLS.nombreTipoUsuario = tipoUsuario.Nombre;

            return View(oRegistroCLS);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}