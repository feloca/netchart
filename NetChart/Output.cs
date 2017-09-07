using System;

namespace NetChart
{
    /// <summary>
    /// Resultado o respuesta enviada al navegador, debera ser formateada a tipo JSON, los cambios implicar
    /// reescribir el javascript que genera los gráficos.
    /// </summary>
    public class Output
    {
        /// <summary>
        /// Obtiene o establece el tipo de gráfico a representar
        /// </summary>
        public string ChartType { get; set; }

        /// <summary>
        /// Obtiene o establece las opciones gráficas recomendadas para los datos especificados, unicamente disponible para ChartType.Debug
        /// </summary>
        public string[] Suggestions { get; set; }

        //incluir propiedades de presentacion, usar una subclase para agruparlo

        //public object[] ComputedPropertyData { get; set; }

        //public object[] ComputedSecondPropertyData { get; set; }

        /// <summary>
        /// Obtiene o establece los datos procesados del eje y
        /// </summary>
        public object[] VariableData { get; set; }

        /// <summary>
        /// Obtiene o establece los datos procesados del eje x
        /// </summary>
        public object[] DimensionData { get; set; }

        /// <summary>
        /// Obtine o establece los datos procesados del eje z
        /// </summary>
        public object[] ZVariableData { get; set; }
    }
}
