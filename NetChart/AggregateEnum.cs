using System;

namespace NetChart
{
    /// <summary>
    /// Define las posibles funciones agregadas aplicables a los datos de entrada de la gráfica
    /// </summary>
    public enum AggregateEnum
    {
        NoAggregate = 0,
        Sum = 1,
        Average = 2,
        Count = 3,
        Maximum = 4,
        Minimum = 5        
    }
}
