using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SavalAPI.Models
{
    public class Pregunta
    {
        [Key]
        public int IdPregunta { get; set; }

        [Required]
        [MaxLength(50)]
        public string TipoPregunta { get; set; }

        [Required]
        [MaxLength(500)]
        public string TextoPregunta { get; set; }

        public ICollection<FormularioPregunta>? Formularios { get; set; } // Opcional
        public ICollection<OpcionRespuesta>? Opciones { get; set; } // Opcional
    }
}
