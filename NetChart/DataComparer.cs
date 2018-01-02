using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChart
{
    public class DataComparer : IComparer
    {
        public DataComparer()
        {

        }

        /// <summary>
        /// Obtiene o establece el tipo del comparador
        /// </summary>
        public Type PropertyType
        {
            get; set;
        }

        /// <summary>
        /// Obtiene o establece la ordenacion descendente, por defecto ascendente
        /// </summary>
        public bool Descending
        {
            get; set;
        }

        public int Compare(object x, object y)
        {
            object compareX = x;
            object compareY = y;

            //invierto los objetos si ordenamos descendentemente
            if (this.Descending == true)
            {
                compareX = y;
                compareY = x;
            }

            //gestionamos los nulos
            if (compareX == null || compareY == null)
            {
                if (compareX == null && compareY == null)
                {
                    return 0;
                }
                if(compareX == null)
                {
                    return 1;
                }
                return -1;
            }

            //gestionamos los ordinales (enumerados de momento)
            if (PropertyType.IsEnum)
            {
                return (compareX.ToString()).CompareTo(compareY.ToString());
            }

            //todo: habria que ver si permitimos datos de clase y gestionarlo aquí

            //gestionamos los tipos básicos
            switch (PropertyType.Name)
            {
                case "Boolean":
                    return ((bool)compareX).CompareTo((bool)compareY);
                case "Int16":
                    return ((short)compareX).CompareTo((short)compareY);
                case "Int32":
                    return ((int)compareX).CompareTo((int)compareY);
                case "Int64":
                    return ((long)compareX).CompareTo((long)compareY);
                case "Single":
                    return ((float)compareX).CompareTo((float)compareY);
                case "Decimal":
                    return ((decimal)compareX).CompareTo((decimal)compareY);
                case "Double":
                    return ((double)compareX).CompareTo((double)compareY);
                case "String":
                    return ((string)compareX).CompareTo((string)compareY);
                default:
                    //TODO: no estan definidos los unsigned
                    //TODO: escapa del ambito inicial del trabajo gestionar variables con clases
                    throw new NotSupportedException();
            }

        }

    }
}
