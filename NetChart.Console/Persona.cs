using System;

namespace NetChart.Console
{
    public class Persona
    {
        public int PersonaId { get; set; }

        /// <summary>
        /// Discreta 15, 23, etc
        /// </summary>
        public int Edad { get; set; }

        /// <summary>
        /// Discreta 174, 185, 205, etc
        /// </summary>
        public int Altura { get; set; }               

        /// <summary>
        /// Variable continua 65.6, 87.5, etc
        /// </summary>
        public float Peso { get; set; }

        /// <summary>
        /// Variable continua 8.5, 12.4
        /// </summary>
        public float Tension { get; set; }

        /// <summary>
        /// Nominal
        /// </summary>
        public string Nacionalidad { get; set; }

        /// <summary>
        /// Nominal medico, secretario, estudiante, repartidor
        /// </summary>
        public string Ocupacion { get; set; }

        /// <summary>
        /// Ordinal, primaria, secundaria, fp, diplomado, doctor
        /// </summary>
        public string Estudios { get; set; }

        /// <summary>
        /// Ordinal, bajos, medios, altos, muy_altos
        /// </summary>
        public string Ingresos { get; set; }
    }
}
