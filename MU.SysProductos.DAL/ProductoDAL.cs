using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics.Internal;
using MU.SysProductos.EN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MU.SysProductos.DAL
{
    public class ProductoDAL
    {
        readonly MUSysProductosDBContext _dbContext;
        public ProductoDAL(MUSysProductosDBContext context)
        {
            _dbContext = context;
        }
            // Método para crear un producto  
            public async Task<int> CrearAsync(Producto pProducto)
            {
                Producto producto  = new Producto() // ConvierteProductoEN a la entidad  
                {
                    Nombre = pProducto.Nombre,
                    Precio = pProducto.Precio,
                    CantidadDisponible = pProducto.CantidadDisponible,
                    FechaCreacion = pProducto.FechaCreacion
                };

                _dbContext.Add(producto); // Agrega el producto al DbContext  
                return await _dbContext.SaveChangesAsync(); // Guarda los cambios  
            }

            // Método para modificar un producto  
            public async Task<int> ModificarAsync(Producto pProducto)
            {
                var producto = await _dbContext.Productos.FirstOrDefaultAsync(p => p.Id == pProducto.Id);
                if (producto != null)
                {
                    // Actualiza las propiedades  
                    producto.Nombre = pProducto.Nombre;
                    producto.Precio = pProducto.Precio;
                    producto.CantidadDisponible = pProducto.CantidadDisponible;
                    producto.FechaCreacion = pProducto.FechaCreacion;

                    // Actualiza el producto en el DbContext  
                    _dbContext.Productos.Update(producto);
                    return await _dbContext.SaveChangesAsync();
                }
                else
                    return 0; // Producto no encontrado
        }

            // Método para eliminar un producto  
            public async Task<int> EliminarAsync(Producto pProducto)
            {
                var producto = _dbContext.Productos.FirstOrDefault(p => p.Id == pProducto.Id);
                if (producto != null)
                {
                    _dbContext.Productos.Remove(producto);
                    return await _dbContext.SaveChangesAsync();
                }
                else
                    return 0; // Producto no encontrado  
            }

            // Método para obtener un producto por ID  
            public async Task<Producto> ObtenerPorIdAsync(Producto pProducto)
            {
                var producto = await _dbContext.Productos.FirstOrDefaultAsync(p => p.Id == pProducto.Id);
                if (producto != null && producto.Id != 0)
                {
                    return new Producto
                    {
                        Id = producto.Id,
                        Nombre = producto.Nombre,
                        Precio = producto.Precio,
                        CantidadDisponible = producto.CantidadDisponible,
                        FechaCreacion = producto.FechaCreacion
                    };
                }
                else
                    return new Producto();
            }

            // Método para obtener todos los productos  
            public async Task<List<Producto>> ObtenerTodosAsync()
            {
                var productos = await _dbContext.Productos.ToListAsync();
                if (productos != null && productos.Count > 0)
                {
                    var list = new List<Producto>();
                    productos.ForEach(p => list.Add(new Producto
                    {
                        Id = p.Id,
                        Nombre = p.Nombre,
                        Precio = p.Precio,
                        CantidadDisponible = p.CantidadDisponible,
                        FechaCreacion = p.FechaCreacion
                    }));
                    return list;
                }
                else
                    return new List<Producto>();
            }
            public async Task AgregarTodosAsync(List<Producto> productos)
            {
                await _dbContext.Productos.AddRangeAsync(productos);
                await _dbContext.SaveChangesAsync();
            }
      
    }
}
