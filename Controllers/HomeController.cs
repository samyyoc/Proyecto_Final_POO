using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LoginMVC.Models;

namespace LoginMVC.Controllers
{
    // Controlador principal que gestiona las vistas protegidas y generales de la aplicación
    public class HomeController : Controller
    {
        // GET: /Home/Index
        // Muestra la pantalla principal (Dashboard) únicamente si el usuario tiene una sesión activa
        public IActionResult Index()
        {
            // Recupera el nombre del usuario almacenado en la sesión HTTP
            string? nombreUsuario = HttpContext.Session.GetString("UsuarioNombre");

            // Control de acceso: si no hay sesión activa, redirige al formulario de Login
            if (string.IsNullOrEmpty(nombreUsuario))
            {
                return RedirectToAction("Login", "Cuenta");
            }

            // Envía el nombre y el correo del usuario autenticado hacia la vista mediante ViewBag
            ViewBag.NombreUsuario = nombreUsuario;
            ViewBag.CorreoUsuario = HttpContext.Session.GetString("UsuarioCorreo");

            // Muestra la vista principal de bienvenida
            return View();
        }

        // GET: /Home/Privacy
        // Muestra la vista informativa de privacidad incluida en la plantilla base
        public IActionResult Privacy()
        {
            return View();
        }

        // GET: /Home/Error
        // Muestra la pantalla de error sin almacenar caché cuando ocurre una excepción
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}