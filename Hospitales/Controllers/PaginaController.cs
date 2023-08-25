using Hospitales.Clases;
using Hospitales.Filters;
using Hospitales.Helpers;
using Hospitales.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hospitales.Controllers
{
    [ServiceFilter(typeof(Seguridad))]
    public class PaginaController : Controller
    {
        private readonly BDHospitalContext context;
        private readonly IReporting reporting;
        private static List<PaginaCLS> listaPaginas;

        public PaginaController(BDHospitalContext context, IReporting reporting)
        {
            this.context = context;
            this.reporting = reporting;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<List<PaginaCLS>> Listar(string mensaje)
        {
            List<PaginaCLS> list = new List<PaginaCLS>();

            if (string.IsNullOrEmpty(mensaje))
            {
                list = await (from pagina in context.Paginas
                              where pagina.Bhabilitado == 1
                              select new PaginaCLS()
                              {
                                  Iidpagina = pagina.Iidpagina,
                                  Accion = pagina.Accion,
                                  Controlador = pagina.Controlador,
                                  Mensaje = pagina.Mensaje

                              }).ToListAsync();

                listaPaginas = list;

                //list = await context.Paginas.Where(x => x.Bhabilitado == 1).OrderByDescending(x => x.Iidpagina).Select(x => new PaginaCLS()
                //{
                //    Iidpagina = x.Iidpagina,
                //    Accion = x.Accion,
                //    Controlador = x.Controlador,
                //    Mensaje = x.Mensaje

                //}).ToListAsync();
            }
            else
            {
                list = await (from pagina in context.Paginas
                              where pagina.Bhabilitado == 1 && pagina.Mensaje.Contains(mensaje)
                              select new PaginaCLS()
                              {
                                  Iidpagina = pagina.Iidpagina,
                                  Accion = pagina.Accion,
                                  Controlador = pagina.Controlador,
                                  Mensaje = pagina.Mensaje

                              }).ToListAsync();

                listaPaginas = list;
            }

            return list;
        }

        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> Edit(int id)
        {
            PaginaCLS paginaCLS = new PaginaCLS();

            try
            {
                Pagina pagina = await context.Paginas.FirstOrDefaultAsync(x => x.Iidpagina == id);

                paginaCLS.Iidpagina = pagina.Iidpagina;
                paginaCLS.Mensaje = pagina.Mensaje;
                paginaCLS.Accion = pagina.Accion;
                paginaCLS.Controlador = pagina.Controlador;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

            return View(paginaCLS);
        }

        [HttpPost]
        public async Task<IActionResult> Guardar(PaginaCLS oPaginaCLS)
        {
            bool existe = false;
            string nombreVista = oPaginaCLS.Iidpagina == 0 ? "Create" : "Edit";
            try
            {
                if (oPaginaCLS.Iidpagina == 0)
                {
                    existe = await context.Paginas.AnyAsync(x => x.Mensaje.Trim().ToUpper() == oPaginaCLS.Mensaje.Trim().ToUpper());
                }
                else
                {
                    existe = await context.Paginas.AnyAsync(x => x.Mensaje.Trim().ToUpper() == oPaginaCLS.Mensaje.Trim().ToUpper() && x.Iidpagina != oPaginaCLS.Iidpagina);
                }

                if (!ModelState.IsValid || existe)
                {
                    if (existe) oPaginaCLS.ErrorMensaje = "Ya existe ese link en la BD..";
                    return View(nombreVista, oPaginaCLS);
                }
                else
                {
                    if (oPaginaCLS.Iidpagina == 0)
                    {
                        Pagina pagina = new Pagina();

                        pagina.Mensaje = oPaginaCLS.Mensaje;
                        pagina.Accion = oPaginaCLS.Accion;
                        pagina.Controlador = oPaginaCLS.Controlador;
                        pagina.Bhabilitado = 1;

                        context.Paginas.Add(pagina);
                        await context.SaveChangesAsync();
                    }
                    else
                    {
                        Pagina pagina = await context.Paginas.FirstOrDefaultAsync(x => x.Iidpagina == oPaginaCLS.Iidpagina);

                        pagina.Mensaje = oPaginaCLS.Mensaje;
                        pagina.Accion = oPaginaCLS.Accion;
                        pagina.Controlador = oPaginaCLS.Controlador;

                        await context.SaveChangesAsync();
                    }

                }
            }
            catch (Exception ex)
            {
                return View(nombreVista, oPaginaCLS);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<int> Delete(int idEliminar)
        {
            int resp = 0;
            try
            {
                Pagina pagina = await context.Paginas.FirstOrDefaultAsync(x => x.Iidpagina == idEliminar);

                context.Paginas.Remove(pagina);
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
                byte[] file = reporting.Excel(listaPaginas, nombrePropiedades);

                return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }
            else if (tipo == "pdf")
            {
                byte[] file = reporting.Pdf("Páginas", nombrePropiedades, listaPaginas);
                return File(file, "application/pdf");
            }
            else if (tipo == "word")
            {
                byte[] file = reporting.Word("Páginas", nombrePropiedades, listaPaginas);
                return File(file, "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
            }
            return null;
        }


        public string DescargarPDF(string[] nombrePropiedades)
        {
            byte[] file = reporting.Pdf("Páginas", nombrePropiedades, listaPaginas);
            string cadena = Convert.ToBase64String(file);
            cadena = "data:application/pdf;base64," + cadena;

            return cadena;
        }
    }
}
