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
    public class CitaController : Controller
    {
        private readonly BDHospitalContext context;

        public CitaController(BDHospitalContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            string controlador = ControllerContext.ActionDescriptor.ControllerName;
            List<PaginaCLS> listaBotonesPag = Listas.listarBotones(controlador);
            ViewBag.Botones = listaBotonesPag.Select(x => x.iidBoton).ToList();

            int idUsuario = int.Parse(HttpContext.Session.GetString("user"));
            int idVista = await Listas.TipoVista("Cita", idUsuario);
            ViewBag.iidVista = idVista;

            await sedes();
            await personas();
            await estadoCitas();
            await doctores();

            return View();
        }

        public async Task<List<CitaCLS>> Listar(int nEstado)
        {
            int idUsuario = int.Parse(HttpContext.Session.GetString("user"));
            int idVista = await Listas.TipoVista("Cita", idUsuario);

            List<CitaCLS> list = new List<CitaCLS>();

            if (idVista == 1)
            {
                if (nEstado == 0)
                {
                    list = await (from cita in context.Cita
                                  join persona in context.Personas
                                  on cita.Iidpersona equals persona.Iidpersona
                                  join usuario in context.Usuarios
                                  on cita.Iidusuario equals usuario.Iidusuario
                                  join estado in context.EstadoCita
                                  on cita.Iidestadocita equals estado.Iidestado
                                  where cita.Bhabilitado == 1
                                  select new CitaCLS()
                                  {
                                      Iidcita = cita.Iidcita,
                                      nombrePersona = persona.Nombre + " " + persona.Appaterno + " " + persona.Apmaterno,
                                      Dfechacita = string.Format("{0:dd-MM-yyyy}", cita.Dfechacita),
                                      nombreUsuario = usuario.Nombreusuario,
                                      nombreEstado = estado.Vnombre,
                                      iidEstadoCita = (int)cita.Iidestadocita

                                  }).ToListAsync();
                }
                else
                {
                    list = await (from cita in context.Cita
                                  join persona in context.Personas
                                  on cita.Iidpersona equals persona.Iidpersona
                                  join usuario in context.Usuarios
                                  on cita.Iidusuario equals usuario.Iidusuario
                                  join estado in context.EstadoCita
                                  on cita.Iidestadocita equals estado.Iidestado
                                  where cita.Bhabilitado == 1 && estado.Iidestado == nEstado
                                  select new CitaCLS()
                                  {
                                      Iidcita = cita.Iidcita,
                                      nombrePersona = persona.Nombre + " " + persona.Appaterno + " " + persona.Apmaterno,
                                      Dfechacita = string.Format("{0:dd-MM-yyyy}", cita.Dfechacita),
                                      nombreUsuario = usuario.Nombreusuario,
                                      nombreEstado = estado.Vnombre,
                                      iidEstadoCita = (int)cita.Iidestadocita

                                  }).ToListAsync();
                }

            }
            else
            {
                if (nEstado == 0)
                {
                    list = await (from cita in context.Cita
                                  join persona in context.Personas
                                  on cita.Iidpersona equals persona.Iidpersona
                                  join usuario in context.Usuarios
                                  on cita.Iidusuario equals usuario.Iidusuario
                                  join estado in context.EstadoCita
                                  on cita.Iidestadocita equals estado.Iidestado
                                  where cita.Bhabilitado == 1 && cita.Iidusuario == idUsuario
                                  select new CitaCLS()
                                  {
                                      Iidcita = cita.Iidcita,
                                      nombrePersona = persona.Nombre + " " + persona.Appaterno + " " + persona.Apmaterno,
                                      Dfechacita = string.Format("{0:dd-MM-yyyy}", cita.Dfechacita),
                                      nombreUsuario = usuario.Nombreusuario,
                                      nombreEstado = estado.Vnombre,
                                      iidEstadoCita = (int)cita.Iidestadocita

                                  }).ToListAsync();
                }
                else
                {
                    list = await (from cita in context.Cita
                                  join persona in context.Personas
                                  on cita.Iidpersona equals persona.Iidpersona
                                  join usuario in context.Usuarios
                                  on cita.Iidusuario equals usuario.Iidusuario
                                  join estado in context.EstadoCita
                                  on cita.Iidestadocita equals estado.Iidestado
                                  where cita.Bhabilitado == 1 && cita.Iidusuario == idUsuario && estado.Iidestado == nEstado
                                  select new CitaCLS()
                                  {
                                      Iidcita = cita.Iidcita,
                                      nombrePersona = persona.Nombre + " " + persona.Appaterno + " " + persona.Apmaterno,
                                      Dfechacita = string.Format("{0:dd-MM-yyyy}", cita.Dfechacita),
                                      nombreUsuario = usuario.Nombreusuario,
                                      nombreEstado = estado.Vnombre,
                                      iidEstadoCita = (int)cita.Iidestadocita

                                  }).ToListAsync();
                }

            }

            return list;
        }

        public async Task<List<HistorialCitaCLS>> ListarHistorial(int iidCita)
        {
            List<HistorialCitaCLS> list = new List<HistorialCitaCLS>();

            list = await (from historialCita in context.HistorialCita
                          join cita in context.Cita
                          on historialCita.Iidcita equals cita.Iidcita
                          join estado in context.EstadoCita
                          on historialCita.Iidestado equals estado.Iidestado
                          join usuario in context.Usuarios
                          on historialCita.Iidusuario equals usuario.Iidusuario
                          join persona in context.Personas
                          on cita.Iidpersona equals persona.Iidpersona
                          where historialCita.Iidcita == iidCita
                          select new HistorialCitaCLS()
                          {
                              Iidhistorialcita = historialCita.Iidhistorialcita,
                              Iidcita = (int)historialCita.Iidcita,
                              nombreEstado = estado.Vnombre,
                              nombreUsuario = usuario.Nombreusuario,
                              Dfecha = string.Format("{0:dd-MM-yyyy}", historialCita.Dfecha)

                          }).ToListAsync();

            return list;
        }

        public async Task<string> Guardar(CitaCLS oCitaCLS)
        {
            string resp = "";
            string format = "dd-MM-yyyy";
            int iidUsuario = int.Parse(HttpContext.Session.GetString("user"));

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
                        if (oCitaCLS.Iidcita == 0)
                        {
                            Citum cita = new Citum();
                            cita.Iidpersona = oCitaCLS.Iidpersona;
                            cita.Iidsede = oCitaCLS.Iidsede;
                            cita.Dfechacita = DateTime.ParseExact(oCitaCLS.Dfechacita, format, CultureInfo.InvariantCulture);
                            cita.Descripcionenfermedad = oCitaCLS.Descripcionenfermedad;
                            cita.Iidusuario = iidUsuario;
                            cita.Iidestadocita = 1;
                            cita.Bhabilitado = 1;

                            context.Cita.Add(cita);
                            await context.SaveChangesAsync();

                            HistorialCitum oHistorialCita = new HistorialCitum();
                            oHistorialCita.Iidcita = cita.Iidcita;
                            oHistorialCita.Iidusuario = iidUsuario;
                            oHistorialCita.Dfecha = DateTime.Now;
                            oHistorialCita.Iidestado = 1;

                            context.HistorialCita.Add(oHistorialCita);
                            await context.SaveChangesAsync();
                        }
                        else
                        {
                            Citum cita = await context.Cita.FirstOrDefaultAsync(x => x.Iidcita == oCitaCLS.Iidcita);
                            cita.Iidpersona = oCitaCLS.Iidpersona;
                            cita.Iidsede = oCitaCLS.Iidsede;
                            cita.Dfechacita = DateTime.ParseExact(oCitaCLS.Dfechacita, format, CultureInfo.InvariantCulture);
                            cita.Descripcionenfermedad = oCitaCLS.Descripcionenfermedad;

                            await context.SaveChangesAsync();
                        }

                        transaccion.Complete();
                    }
                    resp = "1";
                }
                catch (Exception ex)
                {
                    resp = ex.Message;
                }
            }
            return resp;
        }

        public async Task<CitaCLS> Edit(int id)
        {
            CitaCLS citaCLS = new CitaCLS();

            Citum cita = await context.Cita.FirstOrDefaultAsync(x => x.Iidcita == id);

            citaCLS.Iidcita = cita.Iidcita;
            citaCLS.Iidpersona = (int)cita.Iidpersona;
            citaCLS.Iidsede = (int)cita.Iidsede;
            citaCLS.Dfechacita = string.Format("{0:dd-MM-yyyy}", cita.Dfechacita);
            citaCLS.Descripcionenfermedad = cita.Descripcionenfermedad;

            return citaCLS;
        }

        [HttpPost]
        public async Task<string> CambiarEstado(int idEstado, int idCita, string motivo)
        {
            int iidUsuario = int.Parse(HttpContext.Session.GetString("user"));
            string resp = "";

            try
            {
                using (var transaccion = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    Citum cita = await context.Cita.FirstOrDefaultAsync(x => x.Iidcita == idCita);
                    cita.Iidestadocita = idEstado;

                    await context.SaveChangesAsync();

                    HistorialCitum oHistorialCita = new HistorialCitum();
                    oHistorialCita.Iidcita = cita.Iidcita;
                    oHistorialCita.Iidusuario = iidUsuario;
                    oHistorialCita.Dfecha = DateTime.Now;
                    oHistorialCita.Iidestado = idEstado;
                    oHistorialCita.Vobservacion = string.IsNullOrEmpty(motivo) ? "" : motivo;

                    context.HistorialCita.Add(oHistorialCita);
                    await context.SaveChangesAsync();

                    transaccion.Complete();

                    resp = "1";
                }
            }
            catch (Exception ex)
            {
                resp = "";
            }

            return resp;
        }

        public async Task<string> Eliminar(int iidCita)
        {
            string resp = "";
            try
            {
                Citum cita = await context.Cita.FirstOrDefaultAsync(x => x.Iidcita == iidCita);
                cita.Bhabilitado = 0;

                await context.SaveChangesAsync();

                resp = "1";
            }
            catch (Exception ex)
            {
                resp = "";
            }

            return resp;
        }

        [HttpPost]
        public async Task<string> Revisar(RevisarCitaCLS oRevisarCitaCLS)
        {
            string resp = "";
            string format = "dd-MM-yyyy";
            int iidUsuario = int.Parse(HttpContext.Session.GetString("user"));

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
                        Citum cita = await context.Cita.FirstOrDefaultAsync(x => x.Iidcita == oRevisarCitaCLS.iidCita);
                        cita.Iiddoctorasignado = oRevisarCitaCLS.iidDoctor;
                        cita.Dfechacita = oRevisarCitaCLS.fechaCita;
                        cita.Precioatencion = decimal.Parse(oRevisarCitaCLS.precioCita);
                        cita.Iidestadocita = 7;

                        await context.SaveChangesAsync();

                        HistorialCitum oHistorialCita = new HistorialCitum();
                        oHistorialCita.Iidcita = oRevisarCitaCLS.iidCita;
                        oHistorialCita.Iidusuario = iidUsuario;
                        oHistorialCita.Dfecha = DateTime.Now;
                        oHistorialCita.Iidestado = 7;

                        context.Add(oHistorialCita);
                        await context.SaveChangesAsync();


                        transaccion.Complete();
                    }
                    resp = "1";
                }
                catch (Exception ex)
                {
                    resp = ex.Message;
                }
            }
            return resp;
        }

        public async Task doctores()
        {
            int idUsuario = int.Parse(HttpContext.Session.GetString("user"));
            int idVista = await Listas.TipoVista("Cita", idUsuario);

            List<SelectListItem> listaDoctores = new List<SelectListItem>();

            if (idVista == 1)
            {
                listaDoctores = await (from doctor in context.Doctors
                                       join persona in context.Personas
                                       on doctor.Iidpersona equals persona.Iidpersona
                                       where doctor.Bhabilitado == 1
                                       select new SelectListItem()
                                       {
                                           Value = doctor.Iiddoctor.ToString(),
                                           Text = persona.Nombre + " " + persona.Appaterno + " " + persona.Apmaterno,

                                       }).ToListAsync();
            }
            else
            {
                Usuario usuario = await context.Usuarios.FirstOrDefaultAsync(x => x.Iidusuario == idUsuario);

                listaDoctores = await (from doctor in context.Doctors
                                       join persona in context.Personas
                                       on doctor.Iidpersona equals persona.Iidpersona
                                       where doctor.Bhabilitado == 1 && (persona.Iidusuario == usuario.Iidusuario || persona.Iidpersona == usuario.Iidpersona)
                                       select new SelectListItem()
                                       {
                                           Value = doctor.Iiddoctor.ToString(),
                                           Text = persona.Nombre + " " + persona.Appaterno + " " + persona.Apmaterno,

                                       }).ToListAsync();
            }


            listaDoctores.Insert(0, new SelectListItem()
            {
                Text = "Seleccione..",
                Value = ""
            });

            ViewBag.listaDoctores = listaDoctores;
        }

        public async Task personas()
        {
            int idUsuario = int.Parse(HttpContext.Session.GetString("user"));
            int idVista = await Listas.TipoVista("Cita", idUsuario);

            List<SelectListItem> listaPersonas = new List<SelectListItem>();

            if (idVista == 1)
            {
                listaPersonas = await context.Personas.Where(x => x.Bhabilitado == 1 && x.Bdoctor == 0).OrderBy(x => x.Appaterno).Select(x => new SelectListItem()
                {
                    Text = x.Nombre + " " + x.Appaterno + " " + x.Apmaterno,
                    Value = x.Iidpersona.ToString()

                }).ToListAsync();
            }
            else
            {
                Usuario usuario = await context.Usuarios.FirstOrDefaultAsync(x => x.Iidusuario == idUsuario);

                listaPersonas = await context.Personas.Where(x => x.Bhabilitado == 1 && x.Bdoctor == 0 && (x.Iidusuario == idUsuario || x.Iidpersona == usuario.Iidpersona)).OrderBy(x => x.Appaterno).Select(x => new SelectListItem()
                {
                    Text = x.Nombre + " " + x.Appaterno + " " + x.Apmaterno,
                    Value = x.Iidpersona.ToString()

                }).ToListAsync();
            }


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

        public async Task estadoCitas()
        {
            List<SelectListItem> listaEstadoCitas = new List<SelectListItem>();

            listaEstadoCitas = await context.EstadoCita.Where(x => x.Bhabilitado == 1).Select(x => new SelectListItem()
            {
                Text = x.Vnombre.ToString(),
                Value = x.Iidestado.ToString()

            }).ToListAsync();

            listaEstadoCitas.Insert(0, new SelectListItem()
            {
                Text = "Seleccione..",
                Value = ""
            });

            ViewBag.listaEstadoCitas = listaEstadoCitas;
        }
    }
}
