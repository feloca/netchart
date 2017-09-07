using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NetChart
{
    internal class DataHelper
    {
        //funcion -> sacar el minimo valor de una lista de una propiedad indicada (o expresion)
        //funcion -> sacar el maximo valor de una lista de una propiedad indicada (o expresion)

        //calcular regla, es decir, dado un minimo y un maximo y un salto devolver un array con los valores generados
        public void Test()
        {
            Func<int,bool> miFuncion = null;

            List<int> lista = new List<int>();
            lista.AddRange(new int[] { 4, 5, 6 });

            var result = lista.Where(x => x == 5);
          
            lista.Where(miFuncion);

            IQueryable<int> fq = null;

            //fq.
            //lista

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <remarks>
        /// Podrían ser los miembros permitiendo de esta manera emplear los campos
        /// pero no me mola
        /// </remarks>
        public static PropertyInfo[] GetProperties(Type type)
        {
            var result = type.GetProperties();
            return result;            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static PropertyInfo GetProperty(Type type, string propertyName)
        {
            var result = type.GetProperties().FirstOrDefault(x => x.Name.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase));
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<string> GetPropertyNames(Type type)
        {            
            var result = new List<string>();
            var properties = GetProperties(type);
            foreach(var property in properties)
            {
                result.Add(property.Name);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        /// <remarks>
        /// Deberia devolver un enumerado indicando si es 
        /// continuo, discreo, categorico, ordinal (o combinación de ellos)
        /// ESTE NO LE TENGO CLARO, IGUAL SUGERIRLO Y DEJAR QUE EL USUARIO LO 
        /// MANIPULE
        /// </remarks>
        public static object GetPropertyInnerType(Type type, string propertyName)
        {
            var properties = GetProperties(type);
            var result = properties.FirstOrDefault(x => x.Name.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase));
            //Se podria validar que no sea null, pero no deberia de llamarse desde un sitio no controlado
            return result.PropertyType;
        }

        /// <summary>
        /// Devuelve el tipo de dato gráfico asociado a una propiedad
        /// </summary>
        /// <param name="type"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static VariableTypeEnum GetPropertyDisplayType(Type type, string propertyName)
        {
            //sacar la propiedad y el tipo, si son tipos basicos directo, 
            //si es un enumerado ordinal
            //si es un objeto? => usar ordinal y to string?
            throw new NotImplementedException();
        }


        public static object CalculateAggregateSum<T>(string propertyName, IEnumerable<T> data)
        {
            object result = null;
            var property = GetProperty(typeof(T), propertyName);
            switch (property.PropertyType.Name)
            {
                case "Float":
                    result = data.Sum(x => (float)property.GetValue(x));
                    break;
                case "Decimal":
                    result = data.Sum(x => (decimal)property.GetValue(x));
                    break;
                case "Int32":
                    result = data.Sum(x => (int)property.GetValue(x));
                    break;
                case "Long":
                    result = data.Sum(x => (long)property.GetValue(x));
                    break;
                default:
                    var asd = property.PropertyType.Name;
                    throw new NotSupportedException();
            }

            return result;
        }


        public static object CalculateAggregateAverage<T>(string propertyName, IEnumerable<T> data)
        {
            object result = null;
            var property = GetProperty(typeof(T), propertyName);
            switch (property.PropertyType.Name)
            {
                case "Float":
                    data.Average(x => (float)property.GetValue(x));
                    break;
                case "Decimal":
                    data.Average(x => (decimal)property.GetValue(x));
                    break;
                case "Int32":
                    result = data.Average(x => (int)property.GetValue(x));
                    break;
                case "Long":
                    data.Average(x => (long)property.GetValue(x));
                    break;
                default:
                    var asd = property.PropertyType.Name;
                    throw new NotSupportedException();
            }

            return result;
        }

        public static object CalculateAggregateCount<T>(string propertyName, IEnumerable<T> data)
        {
            object result = null;
            var property = GetProperty(typeof(T), propertyName);
            switch (property.PropertyType.Name)
            {
                case "Float":
                case "Decimal":
                case "Int32":
                case "Long":
                    result = data.Count();
                    break;
                default:
                    var asd = property.PropertyType.Name;
                    throw new NotSupportedException();
            }

            return result;
        }

        public static object CalculateAggregateMaximum<T>(string propertyName, IEnumerable<T> data)
        {
            object result = null;
            var property = GetProperty(typeof(T), propertyName);
            switch (property.PropertyType.Name)
            {
                case "Float":
                    data.Max(x => (float)property.GetValue(x));
                    break;
                case "Decimal":
                    data.Max(x => (decimal)property.GetValue(x));
                    break;
                case "Int32":
                    result = data.Max(x => (int)property.GetValue(x));
                    break;
                case "Long":
                    result = data.Max(x => (long)property.GetValue(x));
                    break;
                default:
                    var asd = property.PropertyType.Name;
                    throw new NotSupportedException();
            }

            return result;
        }

        public static object CalculateAggregateMinimum<T>(string propertyName, IEnumerable<T> data)
        {
            object result = null;
            var property = GetProperty(typeof(T), propertyName);
            switch (property.PropertyType.Name)
            {
                case "Float":
                    result = data.Min(x => (float)property.GetValue(x));
                    break;
                case "Decimal":
                    result = data.Min(x => (decimal)property.GetValue(x));
                    break;
                case "Int32":
                    result = data.Min(x => (int)property.GetValue(x));
                    break;
                case "Long":
                    result = data.Min(x => (long)property.GetValue(x));
                    break;
                default:
                    var asd = property.PropertyType.Name;
                    throw new NotSupportedException();
            }

            return result;
        }

        /// <summary>
        /// Calcula el valor de la funcion agregada indicada para un conjunto de datos
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName"></param>
        /// <param name="aggregateType"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static object CalculateAggregate<T>(string propertyName, AggregateEnum aggregateType, IEnumerable<T> data)
        {
            object result = null;
            switch (aggregateType)
            {
                case AggregateEnum.Sum:
                    result = DataHelper.CalculateAggregateSum<T>(propertyName, data);
                    break;
                case AggregateEnum.Average:
                    result = DataHelper.CalculateAggregateAverage<T>(propertyName, data);
                    break;
                case AggregateEnum.Count:
                    result = DataHelper.CalculateAggregateCount<T>(propertyName, data);
                    break;
                case AggregateEnum.Maximum:
                    result = DataHelper.CalculateAggregateMaximum<T>(propertyName, data);
                    break;
                case AggregateEnum.Minimum:
                    result = DataHelper.CalculateAggregateMinimum<T>(propertyName, data);
                    break;
                default:
                    throw new NotSupportedException();
            }

            return result;
        }

        /// <summary>
        /// Obiene una lista con todos los valores distintos de una propiedad incluidos en la coleccion
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static List<object> GetPropertyValues<T>(string propertyName, IEnumerable<T> data)
        {
            List<object> results = new List<object>();
            var property = GetProperty(typeof(T), propertyName);

            switch (property.PropertyType.Name)
            {
                case "Float":                    
                    data.Select(x => (float)property.GetValue(x)).Distinct().ToList().ForEach(y => results.Add(y));
                    break;
                case "Decimal":
                    data.Select(x => (decimal)property.GetValue(x)).Distinct().ToList().ForEach(y => results.Add(y));
                    break;
                case "Int32":
                    data.Select(x => (int)property.GetValue(x)).Distinct().ToList().ForEach(y => results.Add(y));
                    break;
                case "Long":
                    data.Select(x => (long)property.GetValue(x)).Distinct().ToList().ForEach(y => results.Add(y));
                    break;
                default:
                    var asd = property.PropertyType.Name;
                    throw new NotSupportedException();
            }

            return results;
        }

        /// <summary>
        /// Obtiene las filas pertenecientes a una agrupación dados los criterios de agrupacion de propiedad y valor
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="groupProperty"></param>
        /// <param name="groupPropertyValue"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static List<T> GetGroupRows<T>(string groupProperty, object groupPropertyValue, IEnumerable<T> data)
        {
            var property = GetProperty(typeof(T), groupProperty);
            return data.Where(x => property.GetValue(x) == groupPropertyValue).ToList();
        }
    }
}
