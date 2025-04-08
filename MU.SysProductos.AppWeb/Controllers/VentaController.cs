using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MU.SysProductos.BL;
using MU.SysProductos.DAL;
using MU.SysProductos.EN;
using MU.SysProductos.EN.Filtros;
using OfficeOpenXml;
using Rotativa.AspNetCore;
using static MU.SysProductos.EN.Compra;
using static MU.SysProductos.EN.Filtros.VentaFiltros;
using static MU.SysProductos.EN.Venta;

namespace MU.SysProductos.AppWeb.Controllers
{
    public class VentaController : Controller
    {
        readonly ClienteBL clienteBL;
        readonly VentaBL ventaBL;
        readonly ProductoBL productoBL;

        public VentaController(ClienteBL pClienteBL, VentaBL pVentaBL, ProductoBL pProductoBL)
        {
            clienteBL = pClienteBL;
            ventaBL = pVentaBL;
            productoBL = pProductoBL;
        }
        // GET: VentaController
        public async Task<IActionResult> Index(byte? estado)
        {
            var ventas = await ventaBL.ObtenerPorEstadoAsync(estado ?? 0);

            var estados = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "Todos" },
                new SelectListItem { Value = "1", Text = "Activa" },
                new SelectListItem { Value = "2", Text = "Anulada" }
            };

            ViewBag.Estados = new SelectList(estados, "Value", "Text", estado?.ToString());

            return View(ventas);
        }

        // GET: VentaController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: VentaController/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Clientes = new SelectList(await clienteBL.ObtenerTodosAsync(), "Id", "Nombre");
            ViewBag.Productos = await productoBL.ObtenerTodosAsync();
            return View();
        }

        // POST: VentaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Venta venta)
        {
            try
            {
                venta.Estado = (byte)EnumEstadoVenta.Activa;
                venta.FechaVenta = DateTime.Now;
                await ventaBL.CrearAsync(venta);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: VentaController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: VentaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: VentaController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: VentaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public async Task<IActionResult> Anular(int id)
        {
            var venta = await ventaBL.ObtenerPorIdAsync(id);
            if (venta == null)
            {
                return NotFound();
            }

            await ventaBL.AnularAsync(id);

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> ReporteVentasExcel(List<Venta> ventas)
        {
            using (var package = new ExcelPackage())
            {
                var hojaExcel = package.Workbook.Worksheets.Add("Reporte Ventas");

                // Encabezados  
                hojaExcel.Cells["A1"].Value = "Fecha de Venta";
                hojaExcel.Cells["B1"].Value = "Cliente";
                hojaExcel.Cells["C1"].Value = "Producto";
                hojaExcel.Cells["D1"].Value = "Cantidad";
                hojaExcel.Cells["E1"].Value = "Subtotal";
                hojaExcel.Cells["F1"].Value = "Total de la Venta";

                int row = 2;
                int totalCantidad = 0;
                decimal totalSubtotal = 0;
                decimal totalGeneral = 0;

                foreach (var venta in ventas)
                {
                    foreach (var detalle in venta.DetalleVentas)
                    {
                        hojaExcel.Cells[row, 1].Value = venta.FechaVenta.ToString("yyyy-MM-dd");
                        hojaExcel.Cells[row, 2].Value = venta.Cliente?.Nombre ?? "N/A";
                        hojaExcel.Cells[row, 3].Value = detalle.Producto?.Nombre ?? "N/A";
                        hojaExcel.Cells[row, 4].Value = detalle.Cantidad;
                        hojaExcel.Cells[row, 5].Value = detalle.SubTotal;
                        hojaExcel.Cells[row, 6].Value = venta.Total;

                        // Acumular totales  
                        totalCantidad += detalle.Cantidad;
                        totalSubtotal += detalle.SubTotal;
                        totalGeneral += venta.Total;

                        row++;
                    }
                }

                // Fila de totales  
                hojaExcel.Cells[row, 3].Value = "Totales";
                hojaExcel.Cells[row, 4].Value = totalCantidad;
                hojaExcel.Cells[row, 5].Value = totalSubtotal;
                hojaExcel.Cells[row, 6].Value = totalGeneral;

                // Negrita para la fila de totales  
                hojaExcel.Cells[row, 3, row, 6].Style.Font.Bold = true;

                hojaExcel.Cells["A:F"].AutoFitColumns();

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteVentasExcel.xlsx");
            }
        }
        [HttpGet]
        public async Task<IActionResult> DescargarReporte(VentaFiltros filtro)
        {
            var ventas = await ventaBL.ObtenerReporteVentasAsync(filtro);

            if (filtro.TipoReporte == (byte)EnumTipoReporte.PDF)
            {
                return new ViewAsPdf("rpVentas", ventas);
            }
            else if (filtro.TipoReporte == (byte)EnumTipoReporte.Excel)
            {
                return await ReporteVentasExcel(ventas);
            }

            return BadRequest("Formato no válido");
        }
        [HttpGet]
        public IActionResult ReporteVentas()
        {
            return View();
        }
    }
}
