using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChart
{
    public class OutputDisplay
    {
        /// <summary>
        /// Obtiene o establece el titulo del gráfico
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Obtiene o establece el tipos de representacion visual de la variable, util para representar los ejes
        /// </summary>
        public int VariableDisplayType { get; set; }

        /// <summary>
        /// Obtiene o establece el tipo de representacion visual de la dimension, util para representar los ejes
        /// </summary>
        public int DimensionDisplayType { get; set; }

        /// <summary>
        /// Obtiene o establece el tipo de representacion visual de la variable z.
        /// </summary>
        public int ZVariableDisplayType { get; set; }
    }
}
