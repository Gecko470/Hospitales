using Hospitales.Clases;
using Hospitales.Filters;
using Hospitales.Helpers;
using Hospitales.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace Hospitales.Controllers
{
    [ServiceFilter(typeof(Seguridad))]
    public class TipoUsuarioPaginaController : Controller
    {
        private readonly BDHospitalContext context;
        private readonly IReporting reporting;
        private static List<TipoUsuarioPaginaCLS> listaTipoUsuarioPagCLS;

        public TipoUsuarioPaginaController(BDHospitalContext context, IReporting reporting)
        {
            this.context = context;
            this.reporting = reporting;
        }
        public async Task<IActionResult> Index()
        {
            await tiposUsuario();
            await paginas();

            return View();
        }

        public async Task<List<TipoUsuarioPaginaCLS>> Listar(int iidTipoUsuario)
        {
            List<TipoUsuarioPaginaCLS> list = new List<TipoUsuarioPaginaCLS>();
            try
            {
                if (iidTipoUsuario == 0)
                {
                    list = await (from tipUsuPag in context.TipoUsuarioPaginas
                                  join tipoUsuario in context.TipoUsuarios
                                  on tipUsuPag.Iidtipousuario equals tipoUsuario.Iidtipousuario
                                  join pagina in context.Paginas
                                  on tipUsuPag.Iidpagina equals pagina.Iidpagina
                                  where tipUsuPag.Bhabilitado == 1
                                  select new TipoUsuarioPaginaCLS()
                                  {
                                      Iidtipousuariopagina = tipUsuPag.Iidtipousuariopagina,
                                      NombreTipoUsuario = tipoUsuario.Nombre,
                                      NombrePagina = pagina.Mensaje
                                  }).ToListAsync();
                }
                else
                {
                    list = await (from tipUsuPag in context.TipoUsuarioPaginas
                                  join tipoUsuario in context.TipoUsuarios
                                  on tipUsuPag.Iidtipousuario equals tipoUsuario.Iidtipousuario
                                  join pagina in context.Paginas
                                  on tipUsuPag.Iidpagina equals pagina.Iidpagina
                                  where tipUsuPag.Bhabilitado == 1 && tipUsuPag.Iidtipousuario == iidTipoUsuario
                                  select new TipoUsuarioPaginaCLS()
                                  {
                                      Iidtipousuariopagina = tipUsuPag.Iidtipousuariopagina,
                                      NombreTipoUsuario = tipoUsuario.Nombre,
                                      NombrePagina = pagina.Mensaje
                                  }).ToListAsync();
                }
            }
            catch { }

            listaTipoUsuarioPagCLS = list;
            return list;
        }

        [HttpPost]
        public async Task<string> Guardar(TipoUsuarioPaginaCLS tipoUsuarioPaginaCLS, int[] idBotones)
        {
            string nombreVista = tipoUsuarioPaginaCLS.Iidtipousuariopagina == 0 ? "Create" : "Edit";
            bool existeNombre = false;
            bool existeDescripcion = false;
            string resp = "";

            try
            {
                using (var trasnsaccion = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {

                    //if (tipoUsuarioPaginaCLS.Iidtipousuariopagina == 0 && ModelState.IsValid)
                    //{
                    //    existeNombre = await context.TipoUsuarioPaginas.AnyAsync(x => x..Trim().ToUpper() == oTipoUsuarioCLS.Nombre.Trim().ToUpper());
                    //    existeDescripcion = await context.TipoUsuarioPaginas.AnyAsync(x => x.Descripcion.Trim().ToUpper() == oTipoUsuarioCLS.Descripcion.Trim().ToUpper());
                    //}
                    //else if (tipoUsuarioPaginaCLS.Iidtipousuario != 0 && ModelState.IsValid)
                    //{
                    //    existeNombre = await context.TipoUsuarios.AnyAsync(x => x.Nombre.Trim().ToUpper() == oTipoUsuarioCLS.Nombre.Trim().ToUpper() && x.Iidtipousuario != oTipoUsuarioCLS.Iidtipousuario);
                    //    existeDescripcion = await context.TipoUsuarios.AnyAsync(x => x.Descripcion.Trim().ToUpper() == oTipoUsuarioCLS.Descripcion.Trim().ToUpper() && x.Iidtipousuario != oTipoUsuarioCLS.Iidtipousuario);
                    //}

                    if (!ModelState.IsValid)
                    {
                        var errores = (from state in ModelState.Values
                                       from error in state.Errors
                                       select error.ErrorMessage).ToList();

                        resp += "<ul class = 'list-group'>";

                        if (existeNombre) resp += "<li class = 'list-group-item text-danger'>Ya existe en la BD ese Tipo de Usuario..</li>";
                        if (existeDescripcion) resp += "<li class = 'list-group-item text-danger'>Ya existe esa misma descripción en la BD..</li>";

                        foreach (var item in errores)
                        {
                            resp += $"<li class = 'list-group-item text-danger'>{item}</li>";
                        }
                        resp += "</ul>";
                    }
                    else
                    {
                        if (tipoUsuarioPaginaCLS.Iidtipousuariopagina == 0)
                        {
                            TipoUsuarioPagina tipoUsuarioPag = new TipoUsuarioPagina();

                            tipoUsuarioPag.Iidtipousuario = tipoUsuarioPaginaCLS.Iidtipousuario;
                            tipoUsuarioPag.Iidpagina = tipoUsuarioPaginaCLS.Iidpagina;
                            tipoUsuarioPag.Bhabilitado = 1;

                            context.TipoUsuarioPaginas.Add(tipoUsuarioPag);
                            await context.SaveChangesAsync();

                            trasnsaccion.Complete();

                            resp = "1";

                        }
                        else
                        {
                            TipoUsuarioPagina tipoUsuarioPag = await context.TipoUsuarioPaginas.FirstOrDefaultAsync(x => x.Iidtipousuariopagina == tipoUsuarioPaginaCLS.Iidtipousuariopagina);

                            tipoUsuarioPag.Iidtipousuario = tipoUsuarioPaginaCLS.Iidtipousuario;
                            tipoUsuarioPag.Iidpagina = tipoUsuarioPaginaCLS.Iidpagina;

                            List<TipoUsuarioPaginaBoton> list = await context.TipoUsuarioPaginaBotons.Where(x => x.Iidtipousuariopagina == tipoUsuarioPaginaCLS.Iidtipousuariopagina).ToListAsync();

                            if (list != null)
                            {
                                foreach (var item in list)
                                {
                                    item.Bhabilitado = 0;
                                }
                            }

                            foreach (var item in idBotones)
                            {
                                var existe = await context.TipoUsuarioPaginaBotons.AnyAsync(x => x.Iidtipousuariopagina == tipoUsuarioPaginaCLS.Iidtipousuariopagina && x.Iidboton == item);
                                if (!existe)
                                {
                                    TipoUsuarioPaginaBoton tipoUsuarioPaginaBtn = new TipoUsuarioPaginaBoton();

                                    tipoUsuarioPaginaBtn.Iidtipousuariopagina = tipoUsuarioPaginaCLS.Iidtipousuariopagina;
                                    tipoUsuarioPaginaBtn.Iidboton = item;
                                    tipoUsuarioPaginaBtn.Bhabilitado = 1;

                                    context.TipoUsuarioPaginaBotons.Add(tipoUsuarioPaginaBtn);
                                }
                                else
                                {
                                    TipoUsuarioPaginaBoton tipoUsuarioPaginaBtn = await context.TipoUsuarioPaginaBotons.FirstOrDefaultAsync(x => x.Iidtipousuariopagina == tipoUsuarioPaginaCLS.Iidtipousuariopagina && x.Iidboton == item);
                                    tipoUsuarioPaginaBtn.Bhabilitado = 1;
                                }

                            }

                            await context.SaveChangesAsync();

                            trasnsaccion.Complete();

                            resp = "1";
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                return resp;
            }

            return resp;
        }


        public async Task<TipoUsuarioPaginaCLS> Edit(int id)
        {
            TipoUsuarioPaginaCLS tipoUsuarioPaginaCLS = new TipoUsuarioPaginaCLS();
            TipoUsuarioPagina tipoUsuarioPagina = await context.TipoUsuarioPaginas.FirstOrDefaultAsync(x => x.Iidtipousuariopagina == id);

            tipoUsuarioPaginaCLS.Iidtipousuariopagina = tipoUsuarioPagina.Iidtipousuariopagina;
            tipoUsuarioPaginaCLS.Iidtipousuario = tipoUsuarioPagina.Iidtipousuario;
            tipoUsuarioPaginaCLS.Iidpagina = tipoUsuarioPagina.Iidpagina;

            return tipoUsuarioPaginaCLS;
        }

        public async Task<List<BotonCLS>> listarBotones()
        {
            List<BotonCLS> list = new List<BotonCLS>();

            list = await (from boton in context.Botons
                          where boton.Bhabilitado == 1
                          select new BotonCLS()
                          {
                              Iidboton = boton.Iidboton,
                              Nombre = boton.Nombre

                          }).ToListAsync();

            return list;
        }

        public async Task<List<BotonCLS>> ObtenerBotones(int id)
        {
            List<BotonCLS> list = await (from tipoUsuarioPagBtn in context.TipoUsuarioPaginaBotons
                                         where tipoUsuarioPagBtn.Bhabilitado == 1 && tipoUsuarioPagBtn.Iidtipousuariopagina == id
                                         select new BotonCLS()
                                         {
                                             Iidboton = (int)tipoUsuarioPagBtn.Iidboton

                                         }).ToListAsync();

            return list;
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

        public async Task paginas()
        {
            List<SelectListItem> listaPaginas = new List<SelectListItem>();

            listaPaginas = await context.Paginas.Where(x => x.Bhabilitado == 1).Select(x => new SelectListItem()
            {
                Text = x.Mensaje.ToString(),
                Value = x.Iidpagina.ToString()

            }).ToListAsync();

            listaPaginas.Insert(0, new SelectListItem()
            {
                Text = "Seleccione..",
                Value = ""
            });

            ViewBag.listaPaginas = listaPaginas;
        }

        public FileResult Descargar(string[] nombrePropiedades, string tipo)
        {
            if (tipo == "excel")
            {
                byte[] file = reporting.Excel(listaTipoUsuarioPagCLS, nombrePropiedades);

                return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }
            else if (tipo == "pdf")
            {
                byte[] file = reporting.Pdf("TiposUsuarioPaginas", nombrePropiedades, listaTipoUsuarioPagCLS);
                return File(file, "application/pdf");
            }
            else if (tipo == "word")
            {
                byte[] file = reporting.Word("TiposUsuarioPaginas", nombrePropiedades, listaTipoUsuarioPagCLS);
                return File(file, "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
            }
            return null;
        }

        public string DescargarPDF(string[] nombrePropiedades)
        {
            byte[] file = reporting.Pdf("TiposUsuarioPaginas", nombrePropiedades, listaTipoUsuarioPagCLS);
            string cadena = Convert.ToBase64String(file);
            cadena = "data:application/pdf;base64," + cadena;

            return cadena;
        }
    }
}
