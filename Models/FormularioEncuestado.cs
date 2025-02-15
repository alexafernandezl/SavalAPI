using SavalAPI.Models;
using System;
using System.ComponentModel.DataAnnotations;

public class FormularioEncuestado
{
    
    [Required]
    public int IdFormulario { get; set; } // FK - Formulario

    [Required]
    [MaxLength(50)]
    public string IdEncuestado { get; set; } // FK - Encuestado

    [Required]
    public DateTime Fecha { get; set; } // Fecha de respuesta

    [MaxLength(255)]
    public string? Ubicacion { get; set; } // Ubicación opcional

    public decimal Altura { get; set; }
    public decimal Peso { get; set; }
    public decimal IMC { get; set; } // Calculado a partir de Altura y Peso
    public bool Habilitado { get; set; }

    // Relaciones
    public Encuestado? Encuestado { get; set; }
    public Formulario? Formulario { get; set; }
}
