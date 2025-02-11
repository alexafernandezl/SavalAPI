using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Encuestado
{
    [Key]
    [Required]
    [MaxLength(50)]
    public string Identificacion { get; set; }

    [Required]
    [MaxLength(50)]
    public string TipoIdentificacion { get; set; }

    [Required]
    [MaxLength(255)]
    public string NombreCompleto { get; set; }

    [Required]
    public DateTime FechaNacimiento { get; set; }

    [Required]
    [Column(TypeName = "decimal(5,2)")]
    public decimal Altura { get; set; } // En metros

    [Required]
    [Column(TypeName = "decimal(5,2)")]
    public decimal Peso { get; set; } // En kilogramos

    [Required] //  
    [Column(TypeName = "decimal(5,2)")]
    public decimal IMC { get; set; } // Índice de Masa Corporal

    [Required]
    [MaxLength(50)]
    public string Sexo { get; set; }

    [MaxLength(255)]
    public string? Ubicacion { get; set; }
}
