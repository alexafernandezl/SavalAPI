using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SavalAPI.Models
{
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
        [Column(TypeName = "decimal(5,2)")]
        public decimal Altura { get; set; } // En metros

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal Peso { get; set; } // Se calculará automáticamente antes de guardar

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal IMC { get; set; } // Se calculará automáticamente antes de guardar

        [Required]
        [MaxLength(50)]
        public string Sexo { get; set; } // Masculino, Femenino, Otro

        [MaxLength(255)]
        public string? Ubicacion { get; set; } // Provincia, cantón, distrito (opcional)
    }
}
