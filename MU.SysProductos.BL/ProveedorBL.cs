using MU.SysProductos.DAL;
using MU.SysProductos.EN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MU.SysProductos.BL
{
    public class ProveedorBL
    {
        readonly ProveedorDAL proveedorDAL;
        public ProveedorBL(ProveedorDAL pProveedorDAL)
        {
            proveedorDAL = pProveedorDAL;
        }

        public async Task<int> CrearAsync(Proveedor pProveedor)
        {
            return await proveedorDAL.CrearAsync(pProveedor);
        }

        public async Task<int> ModificarAsync(Proveedor pProveedor)
        {
            return await proveedorDAL.ModificarAsync(pProveedor);
        }

        public async Task<int> EliminarAsync(Proveedor pProveedor)
        {
            return await proveedorDAL.EliminarAsync(pProveedor);
        }

        public async Task<Proveedor> ObtenerPorIdAsync(Proveedor pProveedor)
        {
            return await proveedorDAL.ObtenerPorIdAsync(pProveedor);
        }

        public async Task<List<Proveedor>> ObtenerTodosAsync()
        {
            return await proveedorDAL.ObtenerTodosAsync();
        }
        public Task AgregarTodosAsync(List<Proveedor> pProveedores)
        {

            return proveedorDAL.AgregarTodosAsync(pProveedores);
        }
    }
}
