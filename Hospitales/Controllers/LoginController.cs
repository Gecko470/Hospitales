using Hospitales.Clases;
using Hospitales.Helpers;
using Hospitales.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Transactions;

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

        public async Task<IActionResult> Registro()
        {
            await LlenarSexo();

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

                List<PaginaCLS> listaBotonesPag = await (from tipoUsuPagBtn in context.TipoUsuarioPaginaBotons
                                                         join tipoUsuPag in context.TipoUsuarioPaginas
                                                         on tipoUsuPagBtn.Iidtipousuariopagina equals tipoUsuPag.Iidtipousuariopagina
                                                         join pagina in context.Paginas
                                                         on tipoUsuPag.Iidpagina equals pagina.Iidpagina
                                                         where tipoUsuPagBtn.Bhabilitado == 1 && tipoUsuPag.Iidtipousuario == usuario.Iidtipousuario
                                                         select new PaginaCLS()
                                                         {
                                                             Iidpagina = (int)tipoUsuPag.Iidpagina,
                                                             iidBoton = (int)tipoUsuPagBtn.Iidboton,
                                                             Controlador = pagina.Controlador

                                                         }).ToListAsync();

                Listas.ListaBotonesPag = listaBotonesPag;
            }

            return resp;
        }

        [HttpPost]
        public async Task<string> Guardar(RegistroCLS oRegistroCLS)
        {
            string resp = "";
            bool existe = false;
            bool existeEmail = false;
            bool existeUsuario = false;

            try
            {
                if (oRegistroCLS.Iidpersona == 0 && ModelState.IsValid)
                {
                    existe = await context.Personas.AnyAsync(x => x.Nombre.ToUpper().Trim() == oRegistroCLS.Nombre.ToUpper().Trim() && x.Appaterno.ToUpper().Trim() == oRegistroCLS.Appaterno.ToUpper().Trim() && x.Apmaterno.ToUpper().Trim() == oRegistroCLS.Apmaterno.ToUpper().Trim());
                    existeEmail = await context.Personas.AnyAsync(x => x.Email.ToUpper().Trim() == oRegistroCLS.Email.ToUpper().Trim());
                    existeUsuario = await context.Usuarios.AnyAsync(x => x.Nombreusuario.ToUpper().Trim() == oRegistroCLS.Nombreusuario.ToUpper().Trim());
                }


                if (!ModelState.IsValid || existe || existeEmail || existeUsuario)
                {
                    var errores = (from state in ModelState.Values
                                   from error in state.Errors
                                   select error.ErrorMessage).ToList();

                    resp += "<ul class = 'list-group'>";

                    if (existe) resp += "<li class = 'list-group-item text-danger'>Esa Persona ya existe en la BD..</li>";
                    if (existeEmail) resp += "<li class = 'list-group-item text-danger'>Ese Email ya existe en la BD..</li>";
                    if (existeUsuario) resp += "<li class = 'list-group-item text-danger'>Ese Nombre de Usuario ya existe en la BD..</li>";

                    foreach (var item in errores)
                    {
                        resp += $"<li class = 'list-group-item text-danger'>{item}</li>";
                    }
                    resp += "</ul>";

                    await LlenarSexo();
                }
                else
                {
                    using (var transaccion = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        string format = "dd-MM-yyyy";

                        Persona persona = new Persona();

                        persona.Nombre = oRegistroCLS.Nombre;
                        persona.Appaterno = oRegistroCLS.Appaterno;
                        persona.Apmaterno = oRegistroCLS.Apmaterno;
                        persona.Email = oRegistroCLS.Email;
                        persona.Direccion = oRegistroCLS.Direccion;
                        persona.Telefonofijo = oRegistroCLS.Telefonofijo;
                        persona.Telefonocelular = oRegistroCLS.Telefonocelular;
                        persona.Fechanacimiento = DateTime.ParseExact(oRegistroCLS.FechaNtoString, format, CultureInfo.InvariantCulture);
                        persona.Iidsexo = oRegistroCLS.Iidsexo;
                        persona.Foto = oRegistroCLS.Foto;
                        persona.Bdoctor = 0;
                        persona.Bpaciente = 0;
                        persona.Btieneusuario = 1;
                        persona.Bhabilitado = 1;

                        context.Personas.Add(persona);
                        await context.SaveChangesAsync();

                        Usuario usuario = new Usuario();
                        usuario.Nombreusuario = oRegistroCLS.Nombreusuario;
                        usuario.Contraseña = Encriptar.EncriptarPass(oRegistroCLS.Contraseña);
                        usuario.Iidtipousuario = 3;
                        usuario.Iidpersona = persona.Iidpersona;
                        usuario.Bhabilitado = 1;

                        context.Usuarios.Add(usuario);

                        await context.SaveChangesAsync();
                        transaccion.Complete();
                    }

                    resp = "1";
                }
            }
            catch (Exception ex)
            {
                await LlenarSexo();
                return ex.Message;
            }
            return resp;
        }

        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Remove("user");
            return RedirectToAction("Index");
        }

        public async Task LlenarSexo()
        {
            List<SelectListItem> list = new List<SelectListItem>();

            list = await (from sexo in context.Sexos
                          where sexo.Bhabilitado == 1
                          select new SelectListItem()
                          {
                              Text = sexo.Nombre,
                              Value = sexo.Iidsexo.ToString()

                          }).ToListAsync();

            list.Insert(0, new SelectListItem()
            {
                Text = "Seleccione..",
                Value = ""
            });

            ViewBag.Sexos = list;
        }
    }
}
