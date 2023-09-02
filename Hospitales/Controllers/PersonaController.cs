using Hospitales.Clases;
using Hospitales.Filters;
using Hospitales.Helpers;
using Hospitales.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Hospitales.Controllers
{
    [ServiceFilter(typeof(Seguridad))]
    public class PersonaController : Controller
    {
        private readonly BDHospitalContext context;
        private readonly IReporting reporting;
        private static List<PersonaCLS> listaPersonas;

        public PersonaController(BDHospitalContext context, IReporting reporting)
        {
            this.context = context;
            this.reporting = reporting;
        }
        public async Task<IActionResult> Index()
        {
            string controlador = ControllerContext.ActionDescriptor.ControllerName;
            List<PaginaCLS> listaBotonesPag = Listas.listarBotones(controlador);
            ViewBag.Botones = listaBotonesPag.Select(x => x.iidBoton).ToList();

            await LlenarSexo();

            return View();
        }

        public async Task<List<PersonaCLS>> Listar()
        {
            await LlenarSexo();
            List<PersonaCLS> list = new List<PersonaCLS>();

            int idUsuario = int.Parse(HttpContext.Session.GetString("user"));
            int idVista = await Listas.TipoVista("Persona", idUsuario);

            if (idVista == 1)
            {
                list = await (from persona in context.Personas
                              join sexo in context.Sexos on persona.Iidsexo equals sexo.Iidsexo
                              where persona.Bhabilitado == 1
                              select new PersonaCLS()
                              {
                                  Iidpersona = persona.Iidpersona,
                                  NombreCompleto = persona.Nombre + " " + persona.Appaterno + " " + persona.Apmaterno,
                                  Email = persona.Email == null ? "" : persona.Email,
                                  NombreSexo = sexo.Nombre,
                                  FechaNtoString = string.Format("{0:dd-MM-yyyy}", persona.Fechanacimiento)
                              }).ToListAsync();
            }
            else
            {
                list = await (from persona in context.Personas
                              join sexo in context.Sexos on persona.Iidsexo equals sexo.Iidsexo
                              where persona.Bhabilitado == 1 && persona.Iidusuario == idUsuario
                              select new PersonaCLS()
                              {
                                  Iidpersona = persona.Iidpersona,
                                  NombreCompleto = persona.Nombre + " " + persona.Appaterno + " " + persona.Apmaterno,
                                  Email = persona.Email == null ? "" : persona.Email,
                                  NombreSexo = sexo.Nombre,
                                  FechaNtoString = string.Format("{0:dd-MM-yyyy}", persona.Fechanacimiento)
                              }).ToListAsync();
            }

            listaPersonas = list;
            return list;
        }

        public async Task<List<PersonaCLS>> Filtrar(int Iidsexo)
        {
            await LlenarSexo();

            int idUsuario = int.Parse(HttpContext.Session.GetString("user"));
            int idVista = await Listas.TipoVista("Persona", idUsuario);

            List<PersonaCLS> list = new List<PersonaCLS>();

            if (Iidsexo == 0)
            {
                if (idVista == 1)
                {
                    list = await (from persona in context.Personas
                                  join sexo in context.Sexos on persona.Iidsexo equals sexo.Iidsexo
                                  where persona.Bhabilitado == 1
                                  select new PersonaCLS()
                                  {
                                      Iidpersona = persona.Iidpersona,
                                      NombreCompleto = persona.Nombre + " " + persona.Appaterno + " " + persona.Apmaterno,
                                      Email = persona.Email == null ? "" : persona.Email,
                                      NombreSexo = sexo.Nombre,
                                      FechaNtoString = string.Format("{0:dd-MM-yyyy}", persona.Fechanacimiento)
                                  }).ToListAsync();

                    //list = await context.Personas.Include("Sexo").Where(x => x.Bhabilitado == 1).OrderByDescending(x => x.Iidpersona).Select(x => new PersonaCLS()
                    //{
                    //    Iidpersona = x.Iidpersona,
                    //    NombreCompleto = x.Nombre + " " + x.Appaterno + " " + x.Apmaterno,
                    //    Email = x.Email,
                    //    NombreSexo = x.IidsexoNavigation.Nombre,
                    //    FechaNtoString = x.Fechanacimiento.Value.ToShortDateString()

                    //}).ToListAsync();
                }
                else
                {
                    list = await (from persona in context.Personas
                                  join sexo in context.Sexos on persona.Iidsexo equals sexo.Iidsexo
                                  where persona.Bhabilitado == 1 && persona.Iidusuario == idUsuario
                                  select new PersonaCLS()
                                  {
                                      Iidpersona = persona.Iidpersona,
                                      NombreCompleto = persona.Nombre + " " + persona.Appaterno + " " + persona.Apmaterno,
                                      Email = persona.Email == null ? "" : persona.Email,
                                      NombreSexo = sexo.Nombre,
                                      FechaNtoString = string.Format("{0:dd-MM-yyyy}", persona.Fechanacimiento)
                                  }).ToListAsync();
                }

            }
            else
            {
                if (idVista == 1)
                {
                    list = await (from persona in context.Personas
                                  join sexo in context.Sexos on persona.Iidsexo equals sexo.Iidsexo
                                  where persona.Bhabilitado == 1 && persona.Iidsexo == Iidsexo
                                  select new PersonaCLS()
                                  {
                                      Iidpersona = persona.Iidpersona,
                                      NombreCompleto = persona.Nombre + " " + persona.Appaterno + " " + persona.Apmaterno,
                                      Email = persona.Email == null ? "" : persona.Email,
                                      NombreSexo = sexo.Nombre,
                                      FechaNtoString = persona.Fechanacimiento.Value.ToString("dd-MM-yyyy")
                                  }).ToListAsync();
                }
                else
                {
                    list = await (from persona in context.Personas
                                  join sexo in context.Sexos on persona.Iidsexo equals sexo.Iidsexo
                                  where persona.Bhabilitado == 1 && persona.Iidsexo == Iidsexo && persona.Iidusuario == idUsuario
                                  select new PersonaCLS()
                                  {
                                      Iidpersona = persona.Iidpersona,
                                      NombreCompleto = persona.Nombre + " " + persona.Appaterno + " " + persona.Apmaterno,
                                      Email = persona.Email == null ? "" : persona.Email,
                                      NombreSexo = sexo.Nombre,
                                      FechaNtoString = persona.Fechanacimiento.Value.ToString("dd-MM-yyyy")
                                  }).ToListAsync();
                }
            }

            listaPersonas = list;
            return list;
        }

        public async Task<IActionResult> Create()
        {
            await LlenarSexo();
            return View();
        }

        public async Task<PersonaCLS> Edit(int id)
        {
            await LlenarSexo();
            PersonaCLS personaCLS = new PersonaCLS();

            try
            {

                Persona persona = await context.Personas.FirstOrDefaultAsync(x => x.Iidpersona == id);
                personaCLS.Iidpersona = persona.Iidpersona;
                personaCLS.Nombre = persona.Nombre;
                personaCLS.Appaterno = persona.Appaterno;
                personaCLS.Apmaterno = persona.Apmaterno;
                personaCLS.Email = persona.Email;
                personaCLS.Direccion = persona.Direccion;
                personaCLS.Telefonofijo = persona.Telefonofijo;
                personaCLS.Telefonocelular = persona.Telefonocelular;
                personaCLS.FechaNtoString = persona.Fechanacimiento.Value.ToString("dd-MM-yyyy");
                personaCLS.Iidsexo = persona.Iidsexo;
                personaCLS.Foto = persona.Foto;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

            return personaCLS;
        }

        [HttpPost]
        public async Task<string> Guardar(PersonaCLS oPersonaCLS)
        {
            string resp = "";
            bool existe = false;
            bool existeEmail = false;
            string nombreVista = oPersonaCLS.Iidpersona == 0 ? "Create" : "Edit";
            try
            {
                if (oPersonaCLS.Iidpersona == 0 && ModelState.IsValid)
                {
                    existe = await context.Personas.AnyAsync(x => x.Nombre.ToUpper().Trim() == oPersonaCLS.Nombre.ToUpper().Trim() && x.Appaterno.ToUpper().Trim() == oPersonaCLS.Appaterno.ToUpper().Trim() && x.Apmaterno.ToUpper().Trim() == oPersonaCLS.Apmaterno.ToUpper().Trim());
                    existeEmail = await context.Personas.AnyAsync(x => x.Email.ToUpper().Trim() == oPersonaCLS.Email.ToUpper().Trim());
                }
                else if (oPersonaCLS.Iidpersona != 0 && ModelState.IsValid)
                {
                    existe = await context.Personas.AnyAsync(x => x.Nombre.ToUpper().Trim() == oPersonaCLS.Nombre.ToUpper().Trim() && x.Appaterno.ToUpper().Trim() == oPersonaCLS.Appaterno.ToUpper().Trim() && x.Apmaterno.ToUpper().Trim() == oPersonaCLS.Apmaterno.ToUpper().Trim() && x.Iidpersona != oPersonaCLS.Iidpersona);
                    existeEmail = await context.Personas.AnyAsync(x => x.Email.ToUpper().Trim() == oPersonaCLS.Email.ToUpper().Trim() && x.Iidpersona != oPersonaCLS.Iidpersona);
                }


                if (!ModelState.IsValid || existe || existeEmail)
                {
                    var errores = (from state in ModelState.Values
                                   from error in state.Errors
                                   select error.ErrorMessage).ToList();

                    resp += "<ul class = 'list-group'>";

                    if (existe) resp += "<li class = 'list-group-item text-danger'>Esa Persona ya existe en la BD..</li>";
                    if (existeEmail) resp += "<li class = 'list-group-item text-danger'>Ese Email ya existe en la BD..</li>";

                    foreach (var item in errores)
                    {
                        resp += $"<li class = 'list-group-item text-danger'>{item}</li>";
                    }
                    resp += "</ul>";

                    await LlenarSexo();
                }
                else
                {
                    string format = "dd-MM-yyyy";

                    if (oPersonaCLS.Iidpersona == 0)
                    {
                        Persona persona = new Persona();

                        persona.Nombre = oPersonaCLS.Nombre;
                        persona.Appaterno = oPersonaCLS.Appaterno;
                        persona.Apmaterno = oPersonaCLS.Apmaterno;
                        persona.Email = oPersonaCLS.Email;
                        persona.Direccion = oPersonaCLS.Direccion;
                        persona.Telefonofijo = oPersonaCLS.Telefonofijo;
                        persona.Telefonocelular = oPersonaCLS.Telefonocelular;
                        persona.Fechanacimiento = DateTime.ParseExact(oPersonaCLS.FechaNtoString, format, CultureInfo.InvariantCulture);
                        persona.Iidsexo = oPersonaCLS.Iidsexo;
                        persona.Foto = oPersonaCLS.Foto;
                        persona.Iidusuario = int.Parse(HttpContext.Session.GetString("user"));
                        persona.Bdoctor = 0;
                        persona.Bpaciente = 0;
                        persona.Btieneusuario = 0;
                        persona.Bhabilitado = 1;

                        context.Personas.Add(persona);
                        await context.SaveChangesAsync();

                        resp = "1";
                    }
                    else
                    {

                        Persona persona = await context.Personas.FirstOrDefaultAsync(x => x.Iidpersona == oPersonaCLS.Iidpersona);

                        persona.Nombre = oPersonaCLS.Nombre;
                        persona.Appaterno = oPersonaCLS.Appaterno;
                        persona.Apmaterno = oPersonaCLS.Apmaterno;
                        persona.Email = oPersonaCLS.Email;
                        persona.Direccion = oPersonaCLS.Direccion;
                        persona.Telefonofijo = oPersonaCLS.Telefonofijo;
                        persona.Telefonocelular = oPersonaCLS.Telefonocelular;
                        persona.Fechanacimiento = DateTime.ParseExact(oPersonaCLS.FechaNtoString, format, CultureInfo.InvariantCulture);
                        persona.Iidsexo = oPersonaCLS.Iidsexo;
                        persona.Foto = oPersonaCLS.Foto;

                        await context.SaveChangesAsync();

                        resp = "1";
                    }

                }
            }
            catch (Exception ex)
            {
                await LlenarSexo();
                return ex.Message;
            }
            return resp;
        }

        [HttpPost]
        public async Task<int> Delete(int idEliminar)
        {
            int resp = 0;
            try
            {
                Persona persona = await context.Personas.FirstAsync(x => x.Iidpersona == idEliminar);

                persona.Bhabilitado = 0;
                await context.SaveChangesAsync();

                resp = 1;
            }
            catch (Exception ex)
            {
                resp = 0;
            }

            return resp;
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

        public FileResult Descargar(string[] nombrePropiedades, string tipo)
        {
            if (tipo == "excel")
            {
                byte[] file = reporting.Excel(listaPersonas, nombrePropiedades);

                return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }
            else if (tipo == "pdf")
            {
                byte[] file = reporting.Pdf("Personas", nombrePropiedades, listaPersonas);
                return File(file, "application/pdf");
            }
            else if (tipo == "word")
            {
                byte[] file = reporting.Word("Personas", nombrePropiedades, listaPersonas);
                return File(file, "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
            }
            return null;
        }

        public string DescargarPDF(string[] nombrePropiedades)
        {
            byte[] file = reporting.Pdf("Personas", nombrePropiedades, listaPersonas);
            string cadena = Convert.ToBase64String(file);
            cadena = "data:application/pdf;base64," + cadena;

            return cadena;
        }
    }
}
