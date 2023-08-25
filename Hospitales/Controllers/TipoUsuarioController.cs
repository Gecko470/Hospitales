using Hospitales.Clases;
using Hospitales.Filters;
using Hospitales.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace Hospitales.Controllers
{
    [ServiceFilter(typeof(Seguridad))]
    public class TipoUsuarioController : Controller
    {
        private readonly BDHospitalContext context;

        public TipoUsuarioController(BDHospitalContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index(TipoUsuarioCLS otipoUsuarioCLS)
        {
            List<TipoUsuarioCLS> list = new List<TipoUsuarioCLS>();

            list = await (from usuario in context.TipoUsuarios
                          where usuario.Bhabilitado == 1
                          select new TipoUsuarioCLS()
                          {
                              Iidtipousuario = usuario.Iidtipousuario,
                              Nombre = usuario.Nombre,
                              Descripcion = usuario.Descripcion

                          }).ToListAsync();

            if (otipoUsuarioCLS.Iidtipousuario == 0 && otipoUsuarioCLS.Nombre == null && otipoUsuarioCLS.Descripcion == null)
            {
                ViewBag.Iidtipousuario = 0;
                ViewBag.Nombre = "";
                ViewBag.Descripcion = "";
            }
            else
            {
                if (otipoUsuarioCLS.Iidtipousuario != 0)
                {
                    list = list.Where(x => x.Iidtipousuario == otipoUsuarioCLS.Iidtipousuario).ToList();
                }
                if (otipoUsuarioCLS.Nombre != null)
                {
                    list = list.Where(x => x.Nombre.Contains(otipoUsuarioCLS.Nombre)).ToList();
                }
                if (otipoUsuarioCLS.Descripcion != null)
                {
                    list = list.Where(x => x.Descripcion.Contains(otipoUsuarioCLS.Descripcion)).ToList();
                }

                ViewBag.Iidtipousuario = otipoUsuarioCLS.Iidtipousuario;
                ViewBag.Nombre = otipoUsuarioCLS.Nombre;
                ViewBag.Descripcion = otipoUsuarioCLS.Descripcion;
            }


            return View(list);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<string> Guardar(TipoUsuarioCLS oTipoUsuarioCLS, int[] idPaginas)
        {
            string nombreVista = oTipoUsuarioCLS.Iidtipousuario == 0 ? "Create" : "Edit";
            bool existeNombre = false;
            bool existeDescripcion = false;
            string resp = "";

            try
            {
                using (var trasnsaccion = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {

                    if (oTipoUsuarioCLS.Iidtipousuario == 0 && ModelState.IsValid)
                    {
                        existeNombre = await context.TipoUsuarios.AnyAsync(x => x.Nombre.Trim().ToUpper() == oTipoUsuarioCLS.Nombre.Trim().ToUpper());
                        existeDescripcion = await context.TipoUsuarios.AnyAsync(x => x.Descripcion.Trim().ToUpper() == oTipoUsuarioCLS.Descripcion.Trim().ToUpper());
                    }
                    else if (oTipoUsuarioCLS.Iidtipousuario != 0 && ModelState.IsValid)
                    {
                        existeNombre = await context.TipoUsuarios.AnyAsync(x => x.Nombre.Trim().ToUpper() == oTipoUsuarioCLS.Nombre.Trim().ToUpper() && x.Iidtipousuario != oTipoUsuarioCLS.Iidtipousuario);
                        existeDescripcion = await context.TipoUsuarios.AnyAsync(x => x.Descripcion.Trim().ToUpper() == oTipoUsuarioCLS.Descripcion.Trim().ToUpper() && x.Iidtipousuario != oTipoUsuarioCLS.Iidtipousuario);
                    }

                    if (!ModelState.IsValid || existeNombre || existeDescripcion)
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
                        if (oTipoUsuarioCLS.Iidtipousuario == 0)
                        {
                            TipoUsuario tipoUsuario = new TipoUsuario();

                            tipoUsuario.Nombre = oTipoUsuarioCLS.Nombre;
                            tipoUsuario.Descripcion = oTipoUsuarioCLS.Descripcion;
                            tipoUsuario.Bhabilitado = 1;

                            context.TipoUsuarios.Add(tipoUsuario);
                            await context.SaveChangesAsync();

                            trasnsaccion.Complete();

                            resp = "1";

                        }
                        else
                        {
                            TipoUsuario tipoUsuario = await context.TipoUsuarios.FirstOrDefaultAsync(x => x.Iidtipousuario == oTipoUsuarioCLS.Iidtipousuario);

                            tipoUsuario.Nombre = oTipoUsuarioCLS.Nombre;
                            tipoUsuario.Descripcion = oTipoUsuarioCLS.Descripcion;

                            List<TipoUsuarioPagina> list = await context.TipoUsuarioPaginas.Where(x => x.Iidtipousuario == oTipoUsuarioCLS.Iidtipousuario).ToListAsync();

                            if (list != null)
                            {
                                foreach (var item in list)
                                {
                                    item.Bhabilitado = 0;
                                }
                            }

                            foreach (var item in idPaginas)
                            {
                                var existe = await context.TipoUsuarioPaginas.AnyAsync(x => x.Iidtipousuario == oTipoUsuarioCLS.Iidtipousuario && x.Iidpagina == item);
                                if (!existe)
                                {
                                    TipoUsuarioPagina tipoUsuarioPagina = new TipoUsuarioPagina();

                                    tipoUsuarioPagina.Iidtipousuario = oTipoUsuarioCLS.Iidtipousuario;
                                    tipoUsuarioPagina.Iidpagina = item;
                                    tipoUsuarioPagina.Bhabilitado = 1;

                                    context.TipoUsuarioPaginas.Add(tipoUsuarioPagina);
                                }
                                else
                                {
                                    TipoUsuarioPagina tipoUsuarioPagina = await context.TipoUsuarioPaginas.FirstOrDefaultAsync(x => x.Iidtipousuario == oTipoUsuarioCLS.Iidtipousuario && x.Iidpagina == item);
                                    tipoUsuarioPagina.Bhabilitado = 1;
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

        public async Task<IActionResult> Edit(int id)
        {
            TipoUsuarioCLS tipoUsuarioCLS = new TipoUsuarioCLS();
            TipoUsuario tipoUsuario = await context.TipoUsuarios.FirstOrDefaultAsync(x => x.Iidtipousuario == id);

            tipoUsuarioCLS.Iidtipousuario = tipoUsuario.Iidtipousuario;
            tipoUsuarioCLS.Nombre = tipoUsuario.Nombre;
            tipoUsuarioCLS.Descripcion = tipoUsuario.Descripcion;

            return View(tipoUsuarioCLS);
        }

        public async Task<List<PaginaCLS>> ObtenerPaginas(int id)
        {
            List<PaginaCLS> list = await (from tipoUsuarioPag in context.TipoUsuarioPaginas
                                          where tipoUsuarioPag.Bhabilitado == 1 && tipoUsuarioPag.Iidtipousuario == id
                                          select new PaginaCLS()
                                          {
                                              Iidpagina = (int)tipoUsuarioPag.Iidpagina

                                          }).ToListAsync();

            return list;
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int idEliminar)
        {
            try
            {
                TipoUsuario tipoUsuario = await context.TipoUsuarios.FirstAsync(x => x.Iidtipousuario == idEliminar);

                context.TipoUsuarios.Remove(tipoUsuario);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}
