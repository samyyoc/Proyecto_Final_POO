using System.ComponentModel.DataAnnotations;

namespace LoginMVC.Models
{
    // Modelo que representa a un usuario de la tabla Usuarios en SQL Server
    // y también se utiliza para recibir y validar los datos del formulario de inicio de sesión
    public class Usuario
    {
        // Identificador único del usuario (corresponde a la columna Id en SQL Server)
        public int Id { get; set; }

        // Nombre del usuario/integrante; es opcional al enviar el login porque se obtiene de la base de datos
        public string? Nombre { get; set; }

        // Correo electrónico del usuario; es obligatorio y debe tener formato de correo válido 
        [Required(ErrorMessage = "Por favor ingresa tu correo.")]
        [EmailAddress(ErrorMessage = "Ingresa un formato de correo válido.")]
        public string Correo { get; set; } = string.Empty;

        // Contraseña del usuario; es obligatoria y se trata como campo de contraseña en la interfaz 
        [Required(ErrorMessage = "Por favor ingresa tu contraseña.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}