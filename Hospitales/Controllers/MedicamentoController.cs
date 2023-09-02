using Hospitales.Clases;
using Hospitales.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using Hospitales.Helpers;
using Hospitales.Filters;

namespace Hospitales.Controllers
{
    [ServiceFilter(typeof(Seguridad))]
    public class MedicamentoController : Controller
    {
        private readonly BDHospitalContext context;
        private readonly IReporting reporting;
        private static List<MedicamentoCLS> listaMedicamentos;

        public MedicamentoController(BDHospitalContext context, IReporting reporting)
        {
            this.context = context;
            this.reporting = reporting;
        }


        public async Task<IActionResult> Index()
        {
            string controlador = ControllerContext.ActionDescriptor.ControllerName;
            List<PaginaCLS> listaBotonesPag = Listas.listarBotones(controlador);
            ViewBag.Botones = listaBotonesPag.Select(x => x.iidBoton).ToList();

            await LlenarFormaFarmac();
            return View();
        }

        public async Task<List<MedicamentoCLS>> Listar(int IidFF)
        {
            await LlenarFormaFarmac();

            List<MedicamentoCLS> list = new List<MedicamentoCLS>();

            if (IidFF == 0)
            {
                list = await (from medicamento in context.Medicamentos
                              join formaFarmaceutica in context.FormaFarmaceuticas
                              on medicamento.Iidformafarmaceutica equals formaFarmaceutica.Iidformafarmaceutica
                              where medicamento.Bhabilitado == 1
                              select new MedicamentoCLS()
                              {
                                  Iidmedicamento = medicamento.Iidmedicamento,
                                  Nombre = medicamento.Nombre,
                                  Precio = medicamento.Precio,
                                  Stock = (int)medicamento.Stock,
                                  NombreFormaFarmaceutica = formaFarmaceutica.Nombre

                              }).ToListAsync();

                listaMedicamentos = list;
            }
            else
            {
                list = await (from medicamento in context.Medicamentos
                              join formaFarmaceutica in context.FormaFarmaceuticas
                              on medicamento.Iidformafarmaceutica equals formaFarmaceutica.Iidformafarmaceutica
                              where medicamento.Bhabilitado == 1 && medicamento.Iidformafarmaceutica == IidFF
                              select new MedicamentoCLS()
                              {
                                  Iidmedicamento = medicamento.Iidmedicamento,
                                  Nombre = medicamento.Nombre,
                                  Precio = medicamento.Precio,
                                  Stock = (int)medicamento.Stock,
                                  NombreFormaFarmaceutica = formaFarmaceutica.Nombre

                              }).ToListAsync();

                listaMedicamentos = list;
            }

            return list;
        }

        public async Task<IActionResult> Create()
        {
            await LlenarFormaFarmac();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Guardar(MedicamentoCLS oMedicamentoCLS)
        {
            string nombreVista = oMedicamentoCLS.Iidmedicamento == 0 ? "Create" : "Edit";
            try
            {
                if (!ModelState.IsValid)
                {
                    await LlenarFormaFarmac();
                    return View(nombreVista, oMedicamentoCLS);
                }
                else
                {
                    if (oMedicamentoCLS.Iidmedicamento == 0)
                    {
                        Medicamento medicamento = new Medicamento();

                        medicamento.Nombre = oMedicamentoCLS.Nombre;
                        medicamento.Concentracion = oMedicamentoCLS.Concentracion;
                        medicamento.Precio = oMedicamentoCLS.Precio;
                        medicamento.Stock = oMedicamentoCLS.Stock;
                        medicamento.Presentacion = oMedicamentoCLS.Presentacion;
                        medicamento.Iidformafarmaceutica = oMedicamentoCLS.IidFormaFarmaceutica;
                        medicamento.Bhabilitado = 1;

                        context.Medicamentos.Add(medicamento);

                        await context.SaveChangesAsync();
                    }
                    else
                    {
                        Medicamento medicamento = await context.Medicamentos.FirstAsync(x => x.Iidmedicamento == oMedicamentoCLS.Iidmedicamento);

                        medicamento.Nombre = oMedicamentoCLS.Nombre;
                        medicamento.Concentracion = oMedicamentoCLS.Concentracion;
                        medicamento.Precio = oMedicamentoCLS.Precio;
                        medicamento.Stock = oMedicamentoCLS.Stock;
                        medicamento.Presentacion = oMedicamentoCLS.Presentacion;
                        medicamento.Iidformafarmaceutica = oMedicamentoCLS.IidFormaFarmaceutica;

                        await context.SaveChangesAsync();
                    }

                }
            }
            catch (Exception ex)
            {
                return View(nombreVista, oMedicamentoCLS);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            await LlenarFormaFarmac();

            MedicamentoCLS oMedicamentoCLS = new MedicamentoCLS();

            Medicamento medicamento = await context.Medicamentos.FirstAsync(x => x.Iidmedicamento == id);

            oMedicamentoCLS.Nombre = medicamento.Nombre;
            oMedicamentoCLS.Concentracion = medicamento.Concentracion;
            oMedicamentoCLS.Precio = medicamento.Precio;
            oMedicamentoCLS.Stock = (int)medicamento.Stock;
            oMedicamentoCLS.Presentacion = medicamento.Presentacion;
            oMedicamentoCLS.IidFormaFarmaceutica = medicamento.Iidformafarmaceutica;

            return View(oMedicamentoCLS);
        }

        [HttpPost]
        public async Task<int> Delete(int idEliminar)
        {
            int resp = 0;
            try
            {
                Medicamento medicamento = await context.Medicamentos.FirstAsync(x => x.Iidmedicamento == idEliminar);

                context.Medicamentos.Remove(medicamento);
                await context.SaveChangesAsync();

                resp = 1;
            }
            catch (Exception ex)
            {
                resp = 0;
            }

            return resp;
        }

        private async Task LlenarFormaFarmac()
        {
            List<SelectListItem> list = new List<SelectListItem>();

            list = await (from formaFarma in context.FormaFarmaceuticas
                          where formaFarma.Bhabilitado == 1
                          select new SelectListItem()
                          {
                              Text = formaFarma.Nombre,
                              Value = formaFarma.Iidformafarmaceutica.ToString()

                          }).ToListAsync();

            list.Insert(0, new SelectListItem()
            {
                Text = "Seleccione..",
                Value = ""
            });

            ViewBag.listaFF = list;
        }

        public FileResult Descargar(string[] nombrePropiedades, string tipo)
        {
            if (tipo == "excel")
            {
                byte[] file = reporting.Excel(listaMedicamentos, nombrePropiedades);

                return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }
            else if (tipo == "pdf")
            {
                byte[] file = reporting.Pdf("Medicamentos", nombrePropiedades, listaMedicamentos);
                return File(file, "application/pdf");
            }
            else if (tipo == "word")
            {
                byte[] file = reporting.Word("Medicamentos", nombrePropiedades, listaMedicamentos);
                return File(file, "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
            }
            return null;
        }


        public string DescargarPDF(string[] nombrePropiedades)
        {
            byte[] file = reporting.Pdf("Medicamentos", nombrePropiedades, listaMedicamentos);
            string cadena = Convert.ToBase64String(file);
            cadena = "data:application/pdf;base64," + cadena;

            return cadena;
        }
    }
}
