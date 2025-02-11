using SavalAPI.Models;
using System.ComponentModel.DataAnnotations;

public class Usuario
{
    public int IdUsuario { get; set; }

    [Required]
    [EmailAddress]
    public string Correo { get; set; }

    [Required]
    public string Contraseña { get; set; }

    [Required]
    public int IdRol { get; set; } // Llave foránea al rol

    [Required]
    public Rol Rol { get; set; } // Relación obligatoria con el rol
}
