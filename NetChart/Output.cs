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
        public int ChartType { get; set; }

        /// <summary>
        /// Obtiene o establece las opciones gráficas recomendadas para los datos especificados, unicamente disponible para ChartType.Debug
        /// </summary>
        public int[] Suggestions { get; set; }

        /// <summary>
        /// Obtiene o establece el objeto display el cual alberga informacion relativa a la visualizacion del gráfico
        /// </summary>
        public OutputDisplay Display { get; set; }

        /// <summary>
        /// Obtiene o establece información de depuración de la propiedad "variable", unicamente disponible para ChartType.Debug
        /// </summary>
        public string VariableInfo { get; set; }

        /// <summary>
        /// Obtiene o establece información de depuración de la propiedad "dimension", unicamente disponible para ChartType.Debug
        /// </summary>
        public string DimensionInfo { get; set; }

        /// <summary>
        /// Obtiene o establece información de depuración de la propiedad "zvariable", unicamente disponible para ChartType.Debug
        /// </summary>
        public string ZVariableInfo { get; set; }

        /// <summary>
        /// Obtiene o establece las series de datos a representar, al menos existira una
        /// </summary>
        public OutputSeries[] Series { get; set; }

        /// <summary>
        /// Obtiene o establece todas las dimensiones definidas en las distintas series de un 
        /// gráfico, solamente aplica a graficos con series que empleen la dimensión o en el modo
        /// de depuracion siempre que se seleccione una propiedad de serie.
        /// </summary>
        public object[] SeriesDimensions { get; set; }
    }
}
