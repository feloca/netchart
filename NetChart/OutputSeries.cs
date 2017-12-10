using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChart
{
    public class OutputSeries //<T> where T : class
    {
        /// <summary>
        /// Obtiene o establece el identificador de la serie
        /// </summary>
        public object Descriptor { get; set; }

        //public object VariableDatum { get; set; }

        //public object DimensionDatum { get; set; }

        //public List<T> Data { get; set; }

        //public OutputSerie()
        //{
        //    Data = new List<T>();
        //}

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
