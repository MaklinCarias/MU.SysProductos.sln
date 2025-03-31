using MU.SysProductos.DAL;
using MU.SysProductos.EN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MU.SysProductos.BL
{
    public class ProductoBL
    {
        private readonly ProductoDAL _productoDAL;

        public ProductoBL(ProductoDAL productoDAL)
        {
            _productoDAL = productoDAL;
        }

        public async Task<int> CrearAsync(Producto pProducto)
        {
            return await _productoDAL.CrearAsync(pProducto);
        }

        public async Task<int> ModificarAsync(Producto pProducto)
        {
            return await _productoDAL.ModificarAsync(pProducto);
        }

        public async Task<int> EliminarAsync(Producto pProducto)
        {
            return await _productoDAL.EliminarAsync(pProducto);
        }

        public async Task<Producto> ObtenerPorIdAsync(Producto pProducto)
        {
            return await _productoDAL.ObtenerPorIdAsync(pProducto);
        }

        public async Task<List<Producto>> ObtenerTodosAsync()
        {
            return await _productoDAL.ObtenerTodosAsync();
        }
        public Task AgregarTodosAsync(List<Producto> pProductos)
        {

            return _productoDAL.AgregarTodosAsync(pProductos);
        }
    }
}
