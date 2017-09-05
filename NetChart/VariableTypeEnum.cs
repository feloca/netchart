using System;

namespace NetChart
{
    /// <summary>
    /// Representa los posibles tipos o clasificaciones de variable a hora de representar un grafico, no se incluye el
    /// tipo ordinal ya que este se esteblecera indicando que se desea ordenar por la propiedad.
    /// </summary>
    public enum VariableTypeEnum
    {
        //discreta, continua, ordinal, nominal
        Discrete = 0,
        Continuous = 1,
        //Ordinal = 2,
        Nominal = 2
    }
}
