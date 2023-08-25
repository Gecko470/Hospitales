using Hospitales.Clases;
using Hospitales.Filters;
using Hospitales.Helpers;
using Hospitales.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hospitales.Controllers
{
    [ServiceFilter(typeof(Seguridad))]
    public class SedeController : Controller
    {
        private readonly BDHospitalContext context;
        private readonly IReporting reporting;
        private static List<SedeCLS> listaSedes;

        public SedeController(BDHospitalContext context, IReporting reporting)
        {
            this.context = context;
            this.reporting = reporting;
        }

        public async Task<IActionResult> Index(string nombre)
        {
            List<SedeCLS> list = new List<SedeCLS>();

            if (string.IsNullOrEmpty(nombre))
            {
                list = await (from sede in context.Sedes
                              where sede.Bhabilitado == 1
                              select new SedeCLS()
                              {
                                  Iidsede = sede.Iidsede,
                                  Nombre = sede.Nombre,
                                  Direccion = sede.Direccion

                              }).ToListAsync();

                ViewBag.Nombre = "";
            }
            else
            {
                list = await (from sede in context.Sedes
                              where sede.Bhabilitado == 1 && sede.Nombre.Contains(nombre)
                              select new SedeCLS()
                              {
                                  Iidsede = sede.Iidsede,
                                  Nombre = sede.Nombre,
                                  Direccion = sede.Direccion

                              }).ToListAsync();

                ViewBag.Nombre = nombre;
            }

            listaSedes = list;
            return View(list);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<string> Guardar(SedeCLS sedeCLS)
        {
            string nombreVista = sedeCLS.Iidsede == 0 ? "Create" : "Edit";
            string resp = "";
            try
            {
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
                    if (sedeCLS.Iidsede == 0)
                    {
                        Sede sede = new Sede();

                        sede.Nombre = sedeCLS.Nombre;
                        sede.Direccion = sedeCLS.Direccion;
                        sede.Bhabilitado = 1;

                        context.Sedes.Add(sede);
                        await context.SaveChangesAsync();
                        resp = "1";
                    }
                    else
                    {
                        Sede sede = await context.Sedes.FirstOrDefaultAsync(x => x.Iidsede == sedeCLS.Iidsede);

                        sede.Nombre = sedeCLS.Nombre;
                        sede.Direccion = sedeCLS.Direccion;

                        await context.SaveChangesAsync();

                        resp = "1";
                    }
                }

            }
            catch (Exception ex)
            {
                return resp;
            }

            return resp;
        }

        public async Task<SedeCLS> Edit(int id)
        {
            SedeCLS sedeCLS = new SedeCLS();

            Sede sede = await context.Sedes.FirstOrDefaultAsync(x => x.Iidsede == id);

            sedeCLS.Iidsede = sede.Iidsede;
            sedeCLS.Nombre = sede.Nombre;
            sedeCLS.Direccion = sede.Direccion;

            return sedeCLS;
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int idEliminar)
        {
            try
            {
                Sede sede = await context.Sedes.FirstOrDefaultAsync(x => x.Iidsede == idEliminar);

                sede.Bhabilitado = 0;
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

            return RedirectToAction("Index");
        }

        public FileResult Descargar(string[] nombrePropiedades, string tipo)
        {
            if (tipo == "excel")
            {
                byte[] file = reporting.Excel(listaSedes, nombrePropiedades);

                return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }
            else if (tipo == "pdf")
            {
                byte[] file = reporting.Pdf("Sedes", nombrePropiedades, listaSedes);
                return File(file, "application/pdf");
            }
            else if (tipo == "word")
            {
                byte[] file = reporting.Word("Sedes", nombrePropiedades, listaSedes);
                return File(file, "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
            }
            return null;
        }

        public string DescargarPDF(string[] nombrePropiedades)
        {
            byte[] file = reporting.Pdf("Sedes", nombrePropiedades, listaSedes);
            string cadena = Convert.ToBase64String(file);
            cadena = "data:application/pdf;base64," + cadena;

            return cadena;
        }
    }
}
