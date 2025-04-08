
using Microsoft.EntityFrameworkCore;
using MU.SysProductos.BL;
using MU.SysProductos.DAL;
using OfficeOpenXml;
var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
// Agrega el contexto de base de datos SysLoginDBContext al contenedor de servicios de la aplicación.  
builder.Services.AddDbContext<MUSysProductosDBContext>(options =>
{
    // Obtiene la cadena de conexión desde el archivo de configuración (appsettings.json).  
    var conexionString = builder.Configuration.GetConnectionString("Conn");
    // Configura Entity Framework Core para usar MySQL como proveedor de base de datos,  
    // detectando automáticamente la versión del servidor.  
    options.UseMySql(conexionString, ServerVersion.AutoDetect(conexionString));
});
builder.Services.AddScoped<ProductoDAL>();
builder.Services.AddScoped<ProductoBL>();
builder.Services.AddScoped<ProveedorDAL>();
builder.Services.AddScoped<ProveedorBL>();
builder.Services.AddScoped<CompraDAL>();
builder.Services.AddScoped<CompraBL>();
builder.Services.AddScoped<ClienteDAL>();
builder.Services.AddScoped<ClienteBL>();
builder.Services.AddScoped<VentaDAL>();
builder.Services.AddScoped<VentaBL>();



builder.Services.AddControllersWithViews();

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
IWebHostEnvironment env = app.Environment;
Rotativa.AspNetCore.RotativaConfiguration.Setup(env.WebRootPath, "../wwwroot/Rotativa");

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
