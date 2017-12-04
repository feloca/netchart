using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChart
{
    public class DataComparer: IComparer
    {
        public DataComparer()
        {

        }

        public Type PropertyType
        {
            get;set;
        }

        poner algo para indicar si es ascendente o descendente

        public int Compare(object x, object y)
        {
            throw new NotImplementedException();
        }

    }
}
