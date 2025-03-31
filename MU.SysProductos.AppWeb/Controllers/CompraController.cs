using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MU.SysProductos.BL;
using MU.SysProductos.EN;
using static MU.SysProductos.EN.Compra;

namespace MU.SysProductos.AppWeb.Controllers
{
    public class CompraController : Controller
    {
        readonly ProveedorBL proveedorBL;
        readonly CompraBL compraBL;
        readonly ProductoBL productoBL;

        public CompraController(ProveedorBL pProveedorBL, CompraBL pCompraBL, ProductoBL pProductoBL)
        {
            proveedorBL = pProveedorBL;
            compraBL = pCompraBL;
            productoBL = pProductoBL;
        }
        // GET: CompraController
        public async Task<IActionResult> Index(byte? estado)
        {
            var compras = await compraBL.ObtenerPorEstadoAsync(estado ?? 0);

            var estados = new List<SelectListItem>
    {
        new SelectListItem { Value = "", Text = "Todos" },
        new SelectListItem { Value = "1", Text = "Activa" },
        new SelectListItem { Value = "2", Text = "Anulada" }
    };

            ViewBag.Estados = new SelectList(estados, "Value", "Text", estado?.ToString());

            return View(compras);
        }

        // GET: CompraController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CompraController/Create  
        public async Task<IActionResult> Create()
        {
            ViewBag.Proveedores = new SelectList(await proveedorBL.ObtenerTodosAsync(), "Id", "Nombre");
            ViewBag.Productos = await productoBL.ObtenerTodosAsync();
            return View();
        }

        // POST: CompraController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Compra compra)
        {
            try
            {
                compra.Estado = (byte)EnumEstadoCompra.Activa;
                compra.FechaCompra = DateTime.Now;
                await compraBL.CrearAsync(compra);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CompraController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CompraController/Edit/5
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

        // GET: CompraController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CompraController/Delete/5
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
            var compra = await compraBL.ObtenerPorIdAsync(id);
            if (compra == null)
            {
                return NotFound();
            }

            await compraBL.AnularAsync(id);

            return RedirectToAction("Index");
        }
    }
}
