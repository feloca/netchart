using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChart
{
    public class NetChartException : ApplicationException
    {
        public NetChartException() : base()
        {

        }

        public NetChartException(string message) : base(message)
        {

        }

        public NetChartException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
