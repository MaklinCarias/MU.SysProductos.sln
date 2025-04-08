using Microsoft.EntityFrameworkCore;
using MU.SysProductos.EN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MU.SysProductos.DAL
{
    public class ClienteDAL
    {
        readonly MUSysProductosDBContext dbContext;
        public ClienteDAL(MUSysProductosDBContext sysClientesDB)
        {
            dbContext = sysClientesDB;
        }
        public async Task<int> CrearAsync(Cliente pCliente)
        {
            Cliente cliente = new Cliente()
            {
                Nombre = pCliente.Nombre,
                Apellido = pCliente.Apellido,
                DUI = pCliente.DUI,
                Telefono = pCliente.Telefono,
            };
            dbContext.Clientes.Add(cliente);
            return await dbContext.SaveChangesAsync();
        }
        public async Task<int> EliminarAsync(Cliente pCliente)
        {
            var cliente = await dbContext.Clientes.FirstOrDefaultAsync(s => s.Id == pCliente.Id);
            if (cliente != null && cliente.Id != 0)
            {
                dbContext.Clientes.Remove(cliente);
                return await dbContext.SaveChangesAsync();
            }
            else
                return 0;
        }
        public async Task<int> ModificarAsync(Cliente pCliente)
        {
            var cliente = await dbContext.Clientes.FirstOrDefaultAsync(s => s.Id == pCliente.Id);
            if (cliente != null && cliente.Id != 0)
            {
                cliente.Nombre = pCliente.Nombre;
                cliente.Apellido = pCliente.Apellido;
                cliente.DUI = pCliente.DUI;
                cliente.Telefono = pCliente.Telefono;

                dbContext.Update(cliente);
                return await dbContext.SaveChangesAsync();
            }
            else
                return 0;
        }
        public async Task<Cliente> ObtenerPorIdAsync(Cliente pCliente)
        {
            var cliente = await dbContext.Clientes.FirstOrDefaultAsync(s => s.Id == pCliente.Id);
            if (cliente != null && cliente.Id != 0)
            {
                return new Cliente
                {
                    Id = cliente.Id,
                    Nombre = cliente.Nombre,
                    Apellido = cliente.Apellido,
                    DUI = cliente.DUI,
                    Telefono = cliente.Telefono,
                };
            }
            else
                return new Cliente();
        }
        public async Task<List<Cliente>> ObtenerTodosAsync()
        {
            var cliente = await dbContext.Clientes.ToListAsync();
            if (cliente != null && cliente.Count > 0)
            {
                var list = new List<Cliente>();
                cliente.ForEach(p => list.Add(new Cliente
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Apellido = p.Apellido,
                    DUI = p.DUI,
                    Telefono = p.Telefono,
                }));
                return list;
            }
            else
                return new List<Cliente>();
        }
    }
}
