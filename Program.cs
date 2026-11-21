// Inicializa el constructor de la aplicación web con la configuración y servicios predeterminados
var builder = WebApplication.CreateBuilder(args);

// Agrega los servicios necesarios para trabajar con el patrón Modelo-Vista-Controlador (MVC)
builder.Services.AddControllersWithViews();

// Habilita el servicio de sesiones en memoria para mantener los datos del usuario autenticado
builder.Services.AddSession(); 

// Construye la instancia de la aplicación web con los servicios registrados
var app = builder.Build();

// Configura el pipeline de manejo de solicitudes HTTP según el entorno
if (!app.Environment.IsDevelopment())
{
    // En producción, redirige los errores no controlados a una vista de error amigable
    app.UseExceptionHandler("/Home/Error");
    // Aplica seguridad estricta de transporte HTTP (HSTS) por defecto (30 días)
    app.UseHsts();
}

// Redirige automáticamente las peticiones HTTP hacia HTTPS
app.UseHttpsRedirection();

// Habilita el enrutamiento para dirigir las peticiones a los controladores correspondientes
app.UseRouting();

// Activa el middleware de sesión para leer y guardar variables de sesión en cada petición
app.UseSession();

// Habilita la validación de autorización de acceso
app.UseAuthorization();

// Mapea los archivos estáticos optimizados (CSS, JavaScript, Bootstrap, imágenes)
app.MapStaticAssets();

// Define la ruta por defecto de la aplicación: inicia en HomeController -> acción Index
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// Inicia la ejecución de la aplicación web y queda escuchando peticiones
app.Run();
