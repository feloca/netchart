using System;

namespace NetChart
{
    public class Message
    {
        public static string ErrorInvalidPropertyName = "La propiedad [{0}] no está incluida en el tipo [{1}].";

        public static string ErrorConfigurationNoData = "La propiedad [Data] no está asignada.";

        public static string ErrorConfigurationPropertyNameNull = "La propiedad [PropertyName] no está definida.";

        public static string ErrorConfigurationInvalidAggregation = "No se pueden agregar simultaneamente la variable y la dimensión.";

        public static string ErrorConfigurationStringTypeInvalidAggregation = "La variables nominales u ordinales no admiten agregaciones de tipo media, suma, máximo y mínimo.";

        public static string ErrorConfigurationAggregationWithoutGroup = "La propiedad [PropertyAggregation] define una función agregada pero no se ha indicado el campo de agrupación [SecondPropertyName].";

        public static string ErrorConfigurationInvalidDisplayType = "La propiedad [DisplayType] cuyo tipo original es cadena no puede representarse mediante un formato numerico de tipo discreto o continuo";
    }
}
