using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MU.SysProductos.BL;
using MU.SysProductos.EN;

namespace MU.SysProductos.AppWeb.Controllers
{
    public class ClienteController : Controller
    {
        readonly ClienteBL _clienteBL;
        public ClienteController(ClienteBL pClienteBL)
        {
            _clienteBL = pClienteBL;
        }
        // GET: ClienteController
        public async Task<ActionResult> Index()
        {
            var clientes = await _clienteBL.ObtenerTodosAsync();
            return View(clientes);
        }

        // GET: ClienteController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ClienteController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ClienteController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Cliente pCliente)
        {
            try
            {
                var result = await _clienteBL.CrearAsync(pCliente);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ClienteController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var cliente = await _clienteBL.ObtenerPorIdAsync(new Cliente { Id = id });
            return View(cliente);
        }
        // POST: ClienteController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Cliente pCliente)
        {
            try
            {
                var result = await _clienteBL.ModificarAsync(pCliente);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ClienteController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var cliente = await _clienteBL.ObtenerPorIdAsync(new Cliente { Id = id });
            return View(cliente);
        }

        // POST: ClienteController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteCliente(int id)
        {
            try
            {
                var result = await _clienteBL.EliminarAsync(new Cliente { Id = id });
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
