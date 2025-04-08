﻿using Microsoft.EntityFrameworkCore;
using MU.SysProductos.EN;
using MU.SysProductos.EN.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MU.SysProductos.DAL
{
    public class CompraDAL
    {
        readonly MUSysProductosDBContext dbContext;

        public CompraDAL(MUSysProductosDBContext sysProductosDB)
        {
            dbContext = sysProductosDB;
        }

        public async Task<int> CrearAsync(Compra pCompra)
        {
            // Agregar la compra con sus detalles  
            dbContext.Compras.Add(pCompra);
            int result = await dbContext.SaveChangesAsync();
            if (result > 0)
            {
                // Actualizar stock de productos  
                foreach (var detalle in pCompra.DetalleCompras)
                {
                    var producto = await dbContext.Productos.FirstOrDefaultAsync(p => p.Id == detalle.IdProducto);
                    if (producto != null)
                    {
                        producto.CantidadDisponible += detalle.Cantidad;
                    }
                }
            }
            return await dbContext.SaveChangesAsync();
        }
        public async Task<int> AnularAsync(int idCompra)
        {
            var compra = await dbContext.Compras
                .Include(c => c.DetalleCompras)
                .FirstOrDefaultAsync(c => c.Id == idCompra);

            if (compra != null && compra.Estado != (byte)Compra.EnumEstadoCompra.Anulada)
            {
                // Marcar la compra como anulada  
                compra.Estado = (byte)Compra.EnumEstadoCompra.Anulada;

                // Restar la cantidad de los productos comprados  
                foreach (var detalle in compra.DetalleCompras)
                {
                    var producto = await dbContext.Productos.FirstOrDefaultAsync(p => p.Id == detalle.IdProducto);
                    if (producto != null)
                    {
                        producto.CantidadDisponible -= detalle.Cantidad;
                    }
                }

                return await dbContext.SaveChangesAsync();
            }
            return 0; // Si ya estaba anulada, no hacer nada  
        }

        public async Task<Compra> ObtenerPorIdAsync(int idCompra)
        {
            var compra = await dbContext.Compras
                .Include(c => c.DetalleCompras).Include(c => c.Proveedor)
                .FirstOrDefaultAsync(c => c.Id == idCompra);

            return compra ?? new Compra();
        }

        public async Task<List<Compra>> ObtenerTodosAsync()
        {
            var compras = await dbContext.Compras
                .Include(c => c.DetalleCompras)
                .Include(c => c.Proveedor).ToListAsync();
            return compras ?? new List<Compra>();
        }
        public async Task<List<Compra>> ObtenerPorEstadoAsync(byte estado)
        {
            var comprasQuery = dbContext.Compras.AsQueryable();

            if (estado != 0)
            {
                comprasQuery = comprasQuery.Where(c => c.Estado == estado);
            }

            comprasQuery = comprasQuery
                .Include(c => c.DetalleCompras)
                .Include(c => c.Proveedor);

            var compras = await comprasQuery.ToListAsync();

            return compras ?? new List<Compra>();
        }
        public async Task<List<Compra>> ObtenerReporteComprasAsync(CompraFiltros filtros)
        {
            var comprasQuery = dbContext.Compras
                .Include(c => c.DetalleCompras)
                    .ThenInclude(dc => dc.Producto)
                .Include(c => c.Proveedor)
                .AsQueryable();

            if (filtros.FechaInicio.HasValue)
            {
                DateTime FechaInicio = filtros.FechaInicio.Value.Date; // Eliminar la hora, dejar solo la fecha  
                comprasQuery = comprasQuery.Where(c => c.FechaCompra >= FechaInicio);
            }

            if (filtros.FechaFin.HasValue)
            {
                DateTime FechaFin = filtros
                    .FechaFin.Value.Date.AddDays(1).AddSeconds(-1); // Incluir hasta el final del día  
                comprasQuery = comprasQuery.Where(c => c.FechaCompra <= FechaFin);
            }

            return await comprasQuery.ToListAsync();
        }
    }
}
