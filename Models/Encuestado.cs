using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

public class Encuestado
{
    [Key]
    [Required]
    [MaxLength(50)]
    public string Identificacion { get; set; } // PK

    [Required]
    [MaxLength(50)]
    public string TipoIdentificacion { get; set; } // Cédula, Pasaporte

    [Required]
    [MaxLength(255)]
    public string NombreCompleto { get; set; }

    [Required]
    public DateTime FechaNacimiento { get; set; }

    [Required]
    [MaxLength(50)]
    public string Sexo { get; set; } // Masculino, Femenino, Otro

    public bool Habilitado { get; set; } = true; // Activo por defecto

    // Relación con FormularioEncuestado (Muchos a Muchos)dotnet ef migrations add RemoveIMCFromEncuestados

    [JsonIgnore]
    public ICollection<FormularioEncuestado>? FormulariosEncuestados { get; set; }
}
