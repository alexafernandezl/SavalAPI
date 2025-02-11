namespace SavalAPI.Models
{
    public class ReglaOpcion
    {
        public int IdRegla { get; set; }
        public int IdOpcion { get; set; }
        public OpcionRespuesta Opcion { get; set; }

        public int? IdRecomendacion { get; set; }
        public Recomendacion Recomendacion { get; set; }

        public int? IdFactorRiesgo { get; set; }
        public FactorRiesgo FactorRiesgo { get; set; }

        public string Condicion { get; set; }
    }
}
