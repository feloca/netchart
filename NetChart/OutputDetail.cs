using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChart
{
    /// <summary>
    /// Esta clase ayuda a agrupar las filas asociada a variable y dimensión especificas
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OutputDetail<T> where T:class
    {
        public object VariableDatum { get; set; }

        public object DimensionDatum { get; set; }

        public List<T> Data { get; set; }

        public OutputDetail()
        {
            Data = new List<T>();
        }
    }
}
