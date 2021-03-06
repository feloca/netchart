﻿using System;

namespace NetChart
{
    /// <summary>
    /// Define los posibles formatos graficos
    /// </summary>
    public enum ChartTypeEnum
    {
        Debug = 0,
        Histogram = 1, //grafico de barras
        Line = 2, //grafico de fiebre o de lineas
        Scatter = 3, //grafico de dispersion
        Bubble = 4, //grafico de burbuja
        Temperature = 5, //gráfico de temperatura
        Pie = 6, //grafico de sector o tarta
        Radar = 7,  //grafico radar
        Area3D = 8,
        Waterfall = 9,
        AttachedColumnPercentage = 10,
        AttachedColumn = 11,
        OverlapAreaPercentage = 12,
        OverlapArea = 13,
        MultipleColumn = 14,
        MultipleLine = 15,
        MultipleBar = 16
    }
}
