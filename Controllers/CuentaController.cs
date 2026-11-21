using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using LoginMVC.Models;

namespace LoginMVC.Controllers
{
    // Controlador encargado de gestionar el inicio de sesión y cierre de sesión de los usuarios
    public class CuentaController : Controller
    {
        // Permite leer la cadena de conexión definida en appsettings.json
        private readonly IConfiguration _configuration;

        // Constructor que inyecta la configuración de la aplicación
        public CuentaController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: /Cuenta/Login
        // Muestra el formulario de inicio de sesión
        [HttpGet]
        public IActionResult Login()
        {
            // Si ya existe un usuario autenticado en sesión, lo redirige directamente al panel principal
            if (HttpContext.Session.GetString("UsuarioNombre") != null)
            {
                return RedirectToAction("Index", "Home");
            }

            // Si no hay sesión activa, muestra la vista del formulario de login
            return View();
        }

        // POST: /Cuenta/Login
        // Procesa los datos enviados por el formulario y valida las credenciales en SQL Server
        [HttpPost]
        public IActionResult Login(Usuario model)
        {
            // Verifica que los campos obligatorios y el formato del modelo sean válidos
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Obtiene la cadena de conexión "DefaultConnection" desde appsettings.json
            string connectionString = _configuration.GetConnectionString("DefaultConnection")!;

            // Crea y abre la conexión con SQL Server (se cierra automáticamente al salir del bloque using)
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Consulta parametrizada para buscar un usuario que coincida con el correo y contraseña ingresados
                string query = "SELECT Id, Nombre, Correo FROM Usuarios WHERE Correo = @Correo AND Password = @Password";
                SqlCommand cmd = new SqlCommand(query, conn);

                // Asigna los valores de forma segura mediante parámetros para prevenir inyección SQL
                cmd.Parameters.AddWithValue("@Correo", model.Correo);
                cmd.Parameters.AddWithValue("@Password", model.Password);

                conn.Open();

                // Ejecuta la consulta y lee el resultado si existe coincidencia
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Guarda los datos del usuario autenticado en la sesión HTTP
                        HttpContext.Session.SetInt32("UsuarioId", Convert.ToInt32(reader["Id"]));
                        HttpContext.Session.SetString("UsuarioNombre", reader["Nombre"].ToString()!);
                        HttpContext.Session.SetString("UsuarioCorreo", reader["Correo"].ToString()!);

                        // Redirige a la pantalla principal / dashboard
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            // Si no se encontró ningún usuario con esas credenciales, envía un mensaje de error a la vista
            ViewBag.Error = "Correo o contraseña incorrectos. Verifica tus credenciales.";
            return View(model);
        }

        // GET: /Cuenta/Logout
        // Elimina los datos de la sesión actual y devuelve al usuario a la pantalla de Login
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Cuenta");
        }
    }
}