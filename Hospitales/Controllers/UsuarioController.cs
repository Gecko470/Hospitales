using Hospitales.Clases;
using Hospitales.Filters;
using Hospitales.Helpers;
using Hospitales.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Transactions;

namespace Hospitales.Controllers
{
    [ServiceFilter(typeof(Seguridad))]
    public class UsuarioController : Controller
    {
        private readonly BDHospitalContext context;
        private readonly IReporting reporting;
        private static List<UsuarioCLS> listaUsuarios;

        public UsuarioController(BDHospitalContext context, IReporting reporting)
        {
            this.context = context;
            this.reporting = reporting;
        }

        public async Task<IActionResult> Index()
        {
            await tiposUsuario();
            await personas();

            return View();
        }

        public async Task<List<UsuarioCLS>> Listar()
        {

            List<UsuarioCLS> list = new List<UsuarioCLS>();

            list = await (from usuario in context.Usuarios
                          join persona in context.Personas
                          on usuario.Iidpersona equals persona.Iidpersona
                          join tipoUsuario in context.TipoUsuarios
                          on usuario.Iidtipousuario equals tipoUsuario.Iidtipousuario
                          where usuario.Bhabilitado == 1
                          select new UsuarioCLS()
                          {
                              Iidusuario = usuario.Iidusuario,
                              NombreCompleto = persona.Nombre + " " + persona.Appaterno + " " + persona.Apmaterno,
                              Usuario = usuario.Nombreusuario,
                              TipoUsuario = tipoUsuario.Nombre

                          }).ToListAsync();

            listaUsuarios = list;
            return list;
        }

        public async Task<List<UsuarioCLS>> Filtrar(string nombre)
        {

            List<UsuarioCLS> list = new List<UsuarioCLS>();

            if (string.IsNullOrEmpty(nombre))
            {
                list = await (from usuario in context.Usuarios
                              join persona in context.Personas
                              on usuario.Iidpersona equals persona.Iidpersona
                              join tipoUsuario in context.TipoUsuarios
                              on usuario.Iidtipousuario equals tipoUsuario.Iidtipousuario
                              where usuario.Bhabilitado == 1
                              select new UsuarioCLS()
                              {
                                  Iidusuario = usuario.Iidusuario,
                                  NombreCompleto = persona.Nombre + " " + persona.Appaterno + " " + persona.Apmaterno,
                                  Usuario = usuario.Nombreusuario,
                                  TipoUsuario = tipoUsuario.Nombre

                              }).ToListAsync();
            }
            else
            {
                list = await (from usuario in context.Usuarios
                              join persona in context.Personas
                              on usuario.Iidpersona equals persona.Iidpersona
                              join tipoUsuario in context.TipoUsuarios
                              on usuario.Iidtipousuario equals tipoUsuario.Iidtipousuario
                              where usuario.Bhabilitado == 1 && (persona.Nombre + " " + persona.Appaterno + " " + persona.Apmaterno).Contains(nombre)
                              select new UsuarioCLS()
                              {
                                  Iidusuario = usuario.Iidusuario,
                                  NombreCompleto = persona.Nombre + " " + persona.Appaterno + " " + persona.Apmaterno,
                                  Usuario = usuario.Nombreusuario,
                                  TipoUsuario = tipoUsuario.Nombre

                              }).ToListAsync();
            }


            listaUsuarios = list;
            return list;
        }

        [HttpPost]
        public async Task<string> Guardar(UsuarioCLS oUsuarioCLS)
        {
            string resp = "";

            if (!ModelState.IsValid)
            {
                var errores = (from state in ModelState.Values
                               from error in state.Errors
                               select error.ErrorMessage).ToList();

                resp += "<ul class = 'list-group'>";

                foreach (var item in errores)
                {
                    resp += $"<li class = 'list-group-item text-danger'>{item}</li>";
                }
                resp += "</ul>";

            }
            else
            {
                try
                {
                    using (var transaccion = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {

                        if (oUsuarioCLS.Iidusuario == 0)
                        {
                            Usuario usuario = new Usuario();

                            usuario.Iidtipousuario = oUsuarioCLS.Iidtipousuario;
                            usuario.Nombreusuario = oUsuarioCLS.Nombreusuario;

                            usuario.Contraseña = Encriptar.EncriptarPass(oUsuarioCLS.Contraseña);
                            usuario.Iidpersona = oUsuarioCLS.Iidpersona;
                            usuario.Bhabilitado = 1;

                            context.Add(usuario);
                            await context.SaveChangesAsync();

                            Persona persona = await context.Personas.FirstOrDefaultAsync(x => x.Iidpersona == oUsuarioCLS.Iidpersona);
                            persona.Btieneusuario = 1;

                            await context.SaveChangesAsync();

                            transaccion.Complete();

                            resp = "1";
                        }
                        else
                        {
                            Usuario usuario = await context.Usuarios.FirstOrDefaultAsync(x => x.Iidusuario == oUsuarioCLS.Iidusuario);

                            usuario.Iidtipousuario = oUsuarioCLS.Iidtipousuario;
                            usuario.Nombreusuario = oUsuarioCLS.Nombreusuario;

                            await context.SaveChangesAsync();
                            transaccion.Complete();

                            resp = "1";
                        }
                    }
                }
                catch (Exception ex)
                {
                    resp = ex.Message;
                }

            }

            return resp;
        }

        public async Task<UsuarioCLS> Edit(int id)
        {
            UsuarioCLS usuarioCLS = new UsuarioCLS();
            Usuario usuario = await context.Usuarios.FirstOrDefaultAsync(x => x.Iidusuario == id);

            usuarioCLS.Iidusuario = usuario.Iidusuario;
            usuarioCLS.Iidtipousuario = usuario.Iidtipousuario;
            usuarioCLS.Nombreusuario = usuario.Nombreusuario;
            usuarioCLS.Contraseña = usuario.Contraseña;
            usuarioCLS.Iidpersona = usuario.Iidpersona;

            return usuarioCLS;
        }

        public async Task<string> Delete(int idEliminar)
        {
            string resp = "";

            try
            {
                Usuario usuario = await context.Usuarios.FirstOrDefaultAsync(x => x.Iidusuario == idEliminar);
                usuario.Bhabilitado = 0;

                await context.SaveChangesAsync();

                resp = "1";
            }
            catch (Exception ex)
            {
                resp = ex.Message;
            }

            return resp;
        }

        public async Task tiposUsuario()
        {
            List<SelectListItem> listaTiposUsuario = new List<SelectListItem>();

            listaTiposUsuario = await context.TipoUsuarios.Where(x => x.Bhabilitado == 1).Select(x => new SelectListItem()
            {
                Text = x.Nombre.ToString(),
                Value = x.Iidtipousuario.ToString()

            }).ToListAsync();

            listaTiposUsuario.Insert(0, new SelectListItem()
            {
                Text = "Seleccione..",
                Value = ""
            });

            ViewBag.listaTiposUsuario = listaTiposUsuario;
        }

        public async Task personas()
        {
            List<SelectListItem> listaPersonas = new List<SelectListItem>();

            listaPersonas = await context.Personas.Where(x => x.Bhabilitado == 1 && x.Btieneusuario == 0).OrderBy(x => x.Appaterno).Select(x => new SelectListItem()
            {
                Text = x.Nombre + " " + x.Appaterno + " " + x.Apmaterno,
                Value = x.Iidpersona.ToString()

            }).ToListAsync();

            listaPersonas.Insert(0, new SelectListItem()
            {
                Text = "Seleccione..",
                Value = ""
            });

            ViewBag.listaPersonas = listaPersonas;
        }


        public FileResult Descargar(string[] nombrePropiedades, string tipo)
        {
            if (tipo == "excel")
            {
                byte[] file = reporting.Excel(listaUsuarios, nombrePropiedades);

                return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }
            else if (tipo == "pdf")
            {
                byte[] file = reporting.Pdf("Usuarios", nombrePropiedades, listaUsuarios);
                return File(file, "application/pdf");
            }
            else if (tipo == "word")
            {
                byte[] file = reporting.Word("Usuarios", nombrePropiedades, listaUsuarios);
                return File(file, "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
            }
            return null;
        }


        public string DescargarPDF(string[] nombrePropiedades)
        {
            byte[] file = reporting.Pdf("Usuarios", nombrePropiedades, listaUsuarios);
            string cadena = Convert.ToBase64String(file);
            cadena = "data:application/pdf;base64," + cadena;

            return cadena;
        }
    }
}
