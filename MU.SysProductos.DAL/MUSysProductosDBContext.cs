using Microsoft.EntityFrameworkCore;
using MU.SysProductos.EN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MU.SysProductos.DAL
{
    public class MUSysProductosDBContext: DbContext
    {
        public MUSysProductosDBContext(DbContextOptions<MUSysProductosDBContext> options): base(options) 
        {

        }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Compra> Compras {  get; set; }
        public DbSet<DetalleCompra> DetalleCompras { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<DetalleVenta> DetalleVentas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DetalleCompra>()
                .HasOne(d => d.Compra)
                .WithMany(c => c.DetalleCompras)
                .HasForeignKey(d => d.IdCompra);

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DetalleVenta>()
           .HasOne(d => d.Venta)
           .WithMany(c => c.DetalleVentas)
           .HasForeignKey(d => d.IdVenta);

            base.OnModelCreating(modelBuilder);
        }
    }
}
