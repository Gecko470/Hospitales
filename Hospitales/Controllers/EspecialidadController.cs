using Hospitales.Clases;
using Hospitales.Filters;
using Hospitales.Helpers;
using Hospitales.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Hospitales.Controllers
{
    [ServiceFilter(typeof(Seguridad))]
    public class EspecialidadController : Controller
    {
        private readonly BDHospitalContext context;
        private readonly IReporting reporting;
        private static List<EspecialidadCLS> listaEspicialidades;

        public EspecialidadController(BDHospitalContext context, IReporting reporting)
        {
            this.context = context;
            this.reporting = reporting;
        }
        public IActionResult Index(string nombre)
        {
            //List<EspecialidadCLS> lista = new List<EspecialidadCLS>();

            //lista = await (from especialidad in context.Especialidads
            //               where especialidad.Bhabilitado == 1
            //               select new EspecialidadCLS()
            //               {
            //                   Iidespecialidad = especialidad.Iidespecialidad,
            //                   Nombre = especialidad.Nombre,
            //                   Descripcion = especialidad.Descripcion
            //               }).ToListAsync();


            //listaEspicialidades = lista;
            return View();
        }

        public async Task<List<EspecialidadCLS>> Listar()
        {
            List<EspecialidadCLS> lista = new List<EspecialidadCLS>();

            lista = await (from especialidad in context.Especialidads
                           where especialidad.Bhabilitado == 1
                           select new EspecialidadCLS()
                           {
                               Iidespecialidad = especialidad.Iidespecialidad,
                               Nombre = especialidad.Nombre,
                               Descripcion = especialidad.Descripcion
                           }).ToListAsync();


            listaEspicialidades = lista;
            return lista;
        }

        public async Task<List<EspecialidadCLS>> Filtrar(string nombre)
        {
            List<EspecialidadCLS> lista = new List<EspecialidadCLS>();

            if (!string.IsNullOrEmpty(nombre))
            {
                lista = await (from especialidad in context.Especialidads
                               where especialidad.Bhabilitado == 1 && especialidad.Nombre.Contains(nombre)
                               select new EspecialidadCLS()
                               {
                                   Iidespecialidad = especialidad.Iidespecialidad,
                                   Nombre = especialidad.Nombre,
                                   Descripcion = especialidad.Descripcion
                               }).ToListAsync(); ;
            }
            else
            {
                lista = await (from especialidad in context.Especialidads
                               where especialidad.Bhabilitado == 1
                               select new EspecialidadCLS()
                               {
                                   Iidespecialidad = especialidad.Iidespecialidad,
                                   Nombre = especialidad.Nombre,
                                   Descripcion = especialidad.Descripcion
                               }).ToListAsync();
            }

            listaEspicialidades = lista;
            return lista;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Guardar(EspecialidadCLS oEspecilidadCLS)
        {
            string nombreVista = oEspecilidadCLS.Iidespecialidad == 0 ? "Create" : "Edit";
            bool existe = false;
            try
            {
                if (oEspecilidadCLS.Iidespecialidad == 0)
                {
                    existe = await context.Especialidads.AnyAsync(x => x.Nombre.ToUpper() == oEspecilidadCLS.Nombre.ToUpper());
                }

                if (!ModelState.IsValid || existe)
                {
                    if (existe) oEspecilidadCLS.Mensaje = "Esa especialidad ya existe en la BD..";
                    return View(nombreVista, oEspecilidadCLS);
                }
                else
                {
                    if (oEspecilidadCLS.Iidespecialidad == 0)
                    {
                        Especialidad especialidad = new Especialidad();
                        especialidad.Nombre = oEspecilidadCLS.Nombre;
                        especialidad.Descripcion = oEspecilidadCLS.Descripcion;
                        especialidad.Bhabilitado = 1;

                        context.Add(especialidad);
                        await context.SaveChangesAsync();
                    }
                    else
                    {
                        existe = await context.Especialidads.AnyAsync(x => x.Nombre.ToUpper().Trim() == oEspecilidadCLS.Nombre.ToUpper().Trim() && x.Iidespecialidad != oEspecilidadCLS.Iidespecialidad);

                        if (existe)
                        {
                            if (existe) oEspecilidadCLS.Mensaje = "Esa especialidad ya existe en la BD..";
                            return View(nombreVista, oEspecilidadCLS);
                        }

                        Especialidad especialidad = await context.Especialidads.FirstAsync(x => x.Iidespecialidad == oEspecilidadCLS.Iidespecialidad);

                        especialidad.Nombre = oEspecilidadCLS.Nombre;
                        especialidad.Descripcion = oEspecilidadCLS.Descripcion;

                        await context.SaveChangesAsync();
                    }

                }
            }
            catch (Exception ex)
            {

                return View(nombreVista, oEspecilidadCLS);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            EspecialidadCLS oEspecialidadCLS = new EspecialidadCLS();

            try
            {
                Especialidad especialidad = await context.Especialidads.FirstAsync(x => x.Iidespecialidad == id);


                oEspecialidadCLS.Iidespecialidad = especialidad.Iidespecialidad;
                oEspecialidadCLS.Nombre = especialidad.Nombre;
                oEspecialidadCLS.Descripcion = especialidad.Descripcion;
            }
            catch (Exception ex)
            {
                return View();
            }

            return View(oEspecialidadCLS);
        }

        [HttpPost]
        public async Task<int> Delete(int idEliminar)
        {
            int resp = 0;
            try
            {
                Especialidad especialidad = await context.Especialidads.FirstAsync(x => x.Iidespecialidad == idEliminar);

                especialidad.Bhabilitado = 0;
                await context.SaveChangesAsync();

                resp = 1;
            }
            catch (Exception ex)
            {
                resp = 0;
            }

            return resp;
        }

        public FileResult Descargar(string[] nombrePropiedades, string tipo)
        {
            if (tipo == "excel")
            {
                byte[] file = reporting.Excel(listaEspicialidades, nombrePropiedades);

                return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }
            else if (tipo == "pdf")
            {
                byte[] file = reporting.Pdf("Especialidades", nombrePropiedades, listaEspicialidades);
                return File(file, "application/pdf");
            }
            else if (tipo == "word")
            {
                byte[] file = reporting.Word("Especialidades", nombrePropiedades, listaEspicialidades);
                return File(file, "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
            }
            return null;
        }


        public string DescargarPDF(string[] nombrePropiedades)
        {
            byte[] file = reporting.Pdf("Especialidades", nombrePropiedades, listaEspicialidades);
            string cadena = Convert.ToBase64String(file);
            cadena = "data:application/pdf;base64," + cadena;

            return cadena;
        }
    }
}

