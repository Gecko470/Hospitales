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
    public class PacienteController : Controller
    {
        private readonly BDHospitalContext context;
        private readonly IReporting reporting;
        private static List<PacienteCLS> listaPacientes;

        public PacienteController(BDHospitalContext context, IReporting reporting)
        {
            this.context = context;
            this.reporting = reporting;
        }
        public async Task<IActionResult> Index()
        {
            await personas();
            await tiposSangre();

            return View();
        }

        public async Task<List<PacienteCLS>> Listar()
        {

            List<PacienteCLS> list = new List<PacienteCLS>();

            list = await (from paciente in context.Pacientes
                          join persona in context.Personas
                          on paciente.Iidpersona equals persona.Iidpersona
                          join tipoSangre in context.TipoSangres
                          on paciente.Iidtiposangre equals tipoSangre.Iidtiposangre
                          where paciente.Bhabilitado == 1
                          select new PacienteCLS()
                          {
                              Iidpaciente = paciente.Iidpaciente,
                              NombreCompleto = persona.Nombre + " " + persona.Appaterno + " " + persona.Apmaterno,
                              GrupoSanguineo = tipoSangre.Nombre,
                              Alergias = paciente.Alergias == null ? "" : paciente.Alergias

                          }).ToListAsync();

            listaPacientes = list;
            return list;
        }

        public async Task<List<PacienteCLS>> Filtrar(string nombre)
        {

            List<PacienteCLS> list = new List<PacienteCLS>();

            if (string.IsNullOrEmpty(nombre))
            {
                list = await (from paciente in context.Pacientes
                              join persona in context.Personas
                              on paciente.Iidpersona equals persona.Iidpersona
                              join tipoSangre in context.TipoSangres
                              on paciente.Iidtiposangre equals tipoSangre.Iidtiposangre
                              where paciente.Bhabilitado == 1
                              select new PacienteCLS()
                              {
                                  Iidpaciente = paciente.Iidpaciente,
                                  NombreCompleto = persona.Nombre + " " + persona.Appaterno + " " + persona.Apmaterno,
                                  GrupoSanguineo = tipoSangre.Nombre,
                                  Alergias = paciente.Alergias

                              }).ToListAsync();
            }
            else
            {
                list = await (from paciente in context.Pacientes
                              join persona in context.Personas
                              on paciente.Iidpersona equals persona.Iidpersona
                              join tipoSangre in context.TipoSangres
                              on paciente.Iidtiposangre equals tipoSangre.Iidtiposangre
                              where paciente.Bhabilitado == 1 && (persona.Nombre + " " + persona.Appaterno + " " + persona.Apmaterno).Contains(nombre)
                              select new PacienteCLS()
                              {
                                  Iidpaciente = paciente.Iidpaciente,
                                  NombreCompleto = persona.Nombre + " " + persona.Appaterno + " " + persona.Apmaterno,
                                  GrupoSanguineo = tipoSangre.Nombre,
                                  Alergias = paciente.Alergias

                              }).ToListAsync();
            }


            listaPacientes = list;
            return list;
        }

        [HttpPost]
        public async Task<string> Guardar(PacienteCLS oPacienteCLS)
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
                        if (oPacienteCLS.Iidpaciente == 0)
                        {
                            Paciente paciente = new Paciente();

                            paciente.Iidtiposangre = oPacienteCLS.Iidtiposangre;
                            paciente.Alergias = oPacienteCLS.Alergias;
                            paciente.Enfermedadescronicas = oPacienteCLS.Enfermedadescronicas;
                            paciente.Cuadrovacunas = oPacienteCLS.Cuadrovacunas;
                            paciente.Antecedentesquirurgicos = oPacienteCLS.Antecedentesquirurgicos;
                            paciente.Iidpersona = oPacienteCLS.Iidpersona;
                            paciente.Bhabilitado = 1;

                            context.Add(paciente);
                            await context.SaveChangesAsync();

                            Persona persona = await context.Personas.FirstOrDefaultAsync(x => x.Iidpersona == oPacienteCLS.Iidpersona);
                            persona.Bpaciente = 1;

                            await context.SaveChangesAsync();

                            transaccion.Complete();

                            resp = "1";
                        }
                        else
                        {
                            Paciente paciente = await context.Pacientes.FirstOrDefaultAsync(x => x.Iidpaciente == oPacienteCLS.Iidpaciente);

                            paciente.Iidtiposangre = oPacienteCLS.Iidtiposangre;
                            paciente.Alergias = oPacienteCLS.Alergias;
                            paciente.Enfermedadescronicas = oPacienteCLS.Enfermedadescronicas;
                            paciente.Cuadrovacunas = oPacienteCLS.Cuadrovacunas;
                            paciente.Antecedentesquirurgicos = oPacienteCLS.Antecedentesquirurgicos;

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

        public async Task<PacienteCLS> Edit(int id)
        {
            PacienteCLS pacienteCLS = new PacienteCLS();
            Paciente paciente = await context.Pacientes.FirstOrDefaultAsync(x => x.Iidpaciente == id);

            pacienteCLS.Iidpaciente = paciente.Iidpaciente;
            pacienteCLS.Iidtiposangre = paciente.Iidtiposangre;
            pacienteCLS.Alergias = paciente.Alergias;
            pacienteCLS.Enfermedadescronicas = paciente.Enfermedadescronicas;
            pacienteCLS.Cuadrovacunas = paciente.Cuadrovacunas;
            pacienteCLS.Antecedentesquirurgicos = paciente.Antecedentesquirurgicos;
            pacienteCLS.Iidpersona = paciente.Iidpersona;

            return pacienteCLS;
        }

        public async Task<string> Delete(int idEliminar)
        {
            string resp = "";

            try
            {
                Paciente paciente = await context.Pacientes.FirstOrDefaultAsync(x => x.Iidpaciente == idEliminar);
                paciente.Bhabilitado = 0;

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

            listaPersonas = await context.Personas.Where(x => x.Bhabilitado == 1 && x.Bpaciente == 0).OrderBy(x => x.Appaterno).Select(x => new SelectListItem()
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

        public async Task tiposSangre()
        {
            List<SelectListItem> listaTiposSangre = new List<SelectListItem>();

            listaTiposSangre = await context.TipoSangres.Where(x => x.Bhabilitado == 1).Select(x => new SelectListItem()
            {
                Text = x.Nombre.ToString(),
                Value = x.Iidtiposangre.ToString()

            }).ToListAsync();

            listaTiposSangre.Insert(0, new SelectListItem()
            {
                Text = "Seleccione..",
                Value = ""
            });

            ViewBag.listaTiposSangre = listaTiposSangre;
        }

        public FileResult Descargar(string[] nombrePropiedades, string tipo)
        {
            if (tipo == "excel")
            {
                byte[] file = reporting.Excel(listaPacientes, nombrePropiedades);

                return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }
            else if (tipo == "pdf")
            {
                byte[] file = reporting.Pdf("Pacientes", nombrePropiedades, listaPacientes);
                return File(file, "application/pdf");
            }
            else if (tipo == "word")
            {
                byte[] file = reporting.Word("Pacientes", nombrePropiedades, listaPacientes);
                return File(file, "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
            }
            return null;
        }


        public string DescargarPDF(string[] nombrePropiedades)
        {
            byte[] file = reporting.Pdf("Pacientes", nombrePropiedades, listaPacientes);
            string cadena = Convert.ToBase64String(file);
            cadena = "data:application/pdf;base64," + cadena;

            return cadena;
        }
    }
}
