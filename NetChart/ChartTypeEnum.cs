using System;

namespace NetChart
{
    /// <summary>
    /// Define los posibles formatos graficos
    /// </summary>
    public enum ChartTypeEnum
    {
        Debug = 0,
        Bar = 1, //grafico de barras
        Line = 2, //grafico de fiebre o de lineas
        Scatter = 3, //grafico de dispersion
        Bubble = 4, //grafico de burbuja
        Temperature = 5, //gráfico de temperatura
        Pie = 6, //grafico de sector o tarta
        Radar = 7  //grafico radar
            //si meto el grafico de burbujas o el de temperatura me hara falta una tercera propiedad
            //GRAFICO DE ROSA?
    }
}
