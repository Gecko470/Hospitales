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
    public class DoctorController : Controller
    {
        private readonly BDHospitalContext context;
        private readonly IReporting reporting;
        private static List<DoctorCLS> listaDoctores;

        public DoctorController(BDHospitalContext context, IReporting reporting)
        {
            this.context = context;
            this.reporting = reporting;
        }
        public async Task<IActionResult> Index()
        {
            string controlador = ControllerContext.ActionDescriptor.ControllerName;
            List<PaginaCLS> listaBotonesPag = Listas.listarBotones(controlador);
            ViewBag.Botones = listaBotonesPag.Select(x => x.iidBoton).ToList();

            await personas();
            await sedes();
            await especialidades();

            return View();
        }

        public async Task<List<DoctorCLS>> Listar()
        {

            List<DoctorCLS> list = new List<DoctorCLS>();

            list = await (from doctor in context.Doctors
                          join persona in context.Personas
                          on doctor.Iidpersona equals persona.Iidpersona
                          join especialidad in context.Especialidads
                          on doctor.Iidespecialidad equals especialidad.Iidespecialidad
                          join sede in context.Sedes
                          on doctor.Iidsede equals sede.Iidsede
                          where doctor.Bhabilitado == 1
                          select new DoctorCLS()
                          {
                              Iiddoctor = doctor.Iiddoctor,
                              NombreCompleto = persona.Nombre + " " + persona.Appaterno + " " + persona.Apmaterno,
                              Especialidad = especialidad.Nombre,
                              Sede = sede.Nombre

                          }).ToListAsync();

            listaDoctores = list;
            return list;
        }

        public async Task<List<DoctorCLS>> Filtrar(string nombre)
        {

            List<DoctorCLS> list = new List<DoctorCLS>();

            if (string.IsNullOrEmpty(nombre))
            {
                list = await (from doctor in context.Doctors
                              join persona in context.Personas
                              on doctor.Iidpersona equals persona.Iidpersona
                              join especialidad in context.Especialidads
                              on doctor.Iidespecialidad equals especialidad.Iidespecialidad
                              join sede in context.Sedes
                              on doctor.Iidsede equals sede.Iidsede
                              where doctor.Bhabilitado == 1
                              select new DoctorCLS()
                              {
                                  Iiddoctor = doctor.Iiddoctor,
                                  NombreCompleto = persona.Nombre + " " + persona.Appaterno + " " + persona.Apmaterno,
                                  Especialidad = especialidad.Nombre,
                                  Sede = sede.Nombre

                              }).ToListAsync();
            }
            else
            {
                list = await (from doctor in context.Doctors
                              join persona in context.Personas
                              on doctor.Iidpersona equals persona.Iidpersona
                              join especialidad in context.Especialidads
                              on doctor.Iidespecialidad equals especialidad.Iidespecialidad
                              join sede in context.Sedes
                              on doctor.Iidsede equals sede.Iidsede
                              where doctor.Bhabilitado == 1 && (persona.Nombre + " " + persona.Appaterno + " " + persona.Apmaterno).Contains(nombre)
                              select new DoctorCLS()
                              {
                                  Iiddoctor = doctor.Iiddoctor,
                                  NombreCompleto = persona.Nombre + " " + persona.Appaterno + " " + persona.Apmaterno,
                                  Especialidad = especialidad.Nombre,
                                  Sede = sede.Nombre

                              }).ToListAsync();
            }


            listaDoctores = list;
            return list;
        }

        [HttpPost]
        public async Task<string> Guardar(DoctorCLS oDoctorCLS)
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
                        string format = "dd-MM-yyyy";

                        if (oDoctorCLS.Iiddoctor == 0)
                        {
                            Doctor doctor = new Doctor();

                            doctor.Iidsede = oDoctorCLS.Iidsede;
                            doctor.Iidespecialidad = oDoctorCLS.Iidespecialidad;
                            doctor.Sueldo = oDoctorCLS.Sueldo;
                            doctor.Fechacontrato = DateTime.ParseExact(oDoctorCLS.FechaContratoString, format, CultureInfo.InvariantCulture);
                            doctor.Iidpersona = oDoctorCLS.Iidpersona;
                            doctor.Bhabilitado = 1;

                            context.Add(doctor);
                            await context.SaveChangesAsync();

                            Persona persona = await context.Personas.FirstOrDefaultAsync(x => x.Iidpersona == oDoctorCLS.Iidpersona);
                            persona.Bdoctor = 1;

                            await context.SaveChangesAsync();

                            transaccion.Complete();

                            resp = "1";
                        }
                        else
                        {
                            Doctor doctor = await context.Doctors.FirstOrDefaultAsync(x => x.Iiddoctor == oDoctorCLS.Iiddoctor);

                            doctor.Iidsede = oDoctorCLS.Iidsede;
                            doctor.Iidespecialidad = oDoctorCLS.Iidespecialidad;
                            doctor.Sueldo = oDoctorCLS.Sueldo;
                            doctor.Fechacontrato = DateTime.ParseExact(oDoctorCLS.FechaContratoString, format, CultureInfo.InvariantCulture);

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

        public async Task<DoctorCLS> Edit(int id)
        {
            DoctorCLS doctorCLS = new DoctorCLS();
            Doctor doctor = await context.Doctors.FirstOrDefaultAsync(x => x.Iiddoctor == id);

            doctorCLS.Iiddoctor = doctor.Iiddoctor;
            doctorCLS.Iidsede = doctor.Iidsede;
            doctorCLS.Iidespecialidad = doctor.Iidespecialidad;
            doctorCLS.Sueldo = doctor.Sueldo;
            doctorCLS.FechaContratoString = string.Format("{0:dd-MM-yyyy}", doctor.Fechacontrato);
            doctorCLS.Iidpersona = doctor.Iidpersona;

            return doctorCLS;
        }

        public async Task<string> Delete(int idEliminar)
        {
            string resp = "";

            try
            {
                Doctor doctor = await context.Doctors.FirstOrDefaultAsync(x => x.Iiddoctor == idEliminar);
                doctor.Bhabilitado = 0;

                await context.SaveChangesAsync();

                resp = "1";
            }
            catch (Exception ex)
            {
                resp = ex.Message;
            }

            return resp;
        }

        public async Task personas()
        {
            List<SelectListItem> listaPersonas = new List<SelectListItem>();

            listaPersonas = await context.Personas.Where(x => x.Bhabilitado == 1 && x.Bdoctor == 0).OrderBy(x => x.Appaterno).Select(x => new SelectListItem()
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

        public async Task sedes()
        {
            List<SelectListItem> listaSedes = new List<SelectListItem>();

            listaSedes = await context.Sedes.Where(x => x.Bhabilitado == 1).Select(x => new SelectListItem()
            {
                Text = x.Nombre.ToString(),
                Value = x.Iidsede.ToString()

            }).ToListAsync();

            listaSedes.Insert(0, new SelectListItem()
            {
                Text = "Seleccione..",
                Value = ""
            });

            ViewBag.listaSedes = listaSedes;
        }

        public async Task especialidades()
        {
            List<SelectListItem> listaEspecialidades = new List<SelectListItem>();

            listaEspecialidades = await context.Especialidads.Where(x => x.Bhabilitado == 1).Select(x => new SelectListItem()
            {
                Text = x.Nombre.ToString(),
                Value = x.Iidespecialidad.ToString()

            }).ToListAsync();

            listaEspecialidades.Insert(0, new SelectListItem()
            {
                Text = "Seleccione..",
                Value = ""
            });

            ViewBag.listaEspecialidades = listaEspecialidades;
        }

        public FileResult Descargar(string[] nombrePropiedades, string tipo)
        {
            if (tipo == "excel")
            {
                byte[] file = reporting.Excel(listaDoctores, nombrePropiedades);

                return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }
            else if (tipo == "pdf")
            {
                byte[] file = reporting.Pdf("Doctores", nombrePropiedades, listaDoctores);
                return File(file, "application/pdf");
            }
            else if (tipo == "word")
            {
                byte[] file = reporting.Word("Doctores", nombrePropiedades, listaDoctores);
                return File(file, "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
            }
            return null;
        }


        public string DescargarPDF(string[] nombrePropiedades)
        {
            byte[] file = reporting.Pdf("Doctores", nombrePropiedades, listaDoctores);
            string cadena = Convert.ToBase64String(file);
            cadena = "data:application/pdf;base64," + cadena;

            return cadena;
        }
    }
}
