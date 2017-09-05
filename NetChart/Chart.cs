using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace NetChart
{
    //public class NetChart<Tx, Ty, Tz> where Tx : class where Ty : class where Tz : class
    public class Chart<T> where T : class
    {
        public Chart()
        {
            this._dataProperty = new Property<T>();
            this._zDataProperty = new Property<T>();
        }

        public Chart(string configuration) : this()
        {
            throw new NotImplementedException();
            //la variable configuracion tiene formato json
            //aqui hay que enchufar los parametros de configuracion 
            //OJO, CREO QUE no hay que preocuparse por las validaciones porque al generar se valida
        }

        ///entiendo que con la propiedad x deberia de inferir la propiedad y a partir de sus maximos y minimos
        ///al enchufar las propiedades de datos permitir tipar (incluir opcion sin tipo), las listas de datos object?, y validar que coincidan con el tipo del dato
        ///1, 2 o 3 series de datos
        ///Tipo -> enumerado, tipo de grafico seleccionado
        ///ListaX, ListaY, ListZ
        ///PropiedadX, PropiedadY, PropiedadZ

        ///ModoDeveloper 
        ///SUGERENCIAS[]
        ///


        //temas de presentacion
        //titulo
        //titulo eje x, eje y (incluir propiedad de configuración)      

        //private T _type = null;
        private string _propertyName;
        private AggregateEnum _propertyAggregation;
        private string _secondPropertyName;        
        private ChartTypeEnum _chartType;

        //AQUI VA EL FORMATO NUEVO
        private Property<T> _dataProperty;
        private Property<T> _zDataProperty;



        private Type WorkType
        {
            get
            {
                return typeof(T);
            }
        }

        private List<string> PropertyNames
        {
            get
            {
                return DataHelper.GetPropertyNames(this.WorkType);
            }
        }

        ///<remarks>
        ///Esto habra que validarlo, y almacenarlo en una variable privada
        /// </remarks>
        public List<T> Data { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <remarks>
        ///// Si la propiedad no existe en T lanzar excepcion
        ///// </remarks>
        //public string PropertyName
        //{
        //    get { return this._propertyName; }
        //    set
        //    {

        //        if (string.IsNullOrEmpty(this._propertyName) || !this._propertyName.Equals(value.ToLower()))
        //        {
        //            //validar que pone una propiedad existente
        //            if (DataHelper.GetProperty(WorkType, value) == null)
        //            {
        //                throw new NetChartException(string.Format(Message.ErrorInvalidPropertyName, value, WorkType.Name));
        //            }
        //            this._propertyName = value;
        //        }
        //    }
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <remarks>
        ///// Si distinto de NOAGGREGATE, crear automaticamente una agrupacion de datos
        ///// y establecer el campo SecondDataProperty con el campo de agrupación
        ///// </remarks>
        //public AggregateEnum PropertyAggregation
        //{
        //    get { return this._propertyAggregation; }
        //    set
        //    {
        //        if (this._propertyAggregation != value)
        //        {
        //            this._propertyAggregation = value;
        //        }
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// Si la propiedad no existe en T lanzar una excepcion
        /// </remarks>
        public string SecondPropertyName
        {
            get
            {
                return this._secondPropertyName;
            }
            set
            {
                if (string.IsNullOrEmpty(this._secondPropertyName) || !this._secondPropertyName.Equals(value, StringComparison.InvariantCultureIgnoreCase))
                {
                    //permitimos cadena vacia por si queremos que indice o valores de x sean el orden de los datos
                    if (string.IsNullOrEmpty(value))
                    {
                        this._secondPropertyName = value;
                        return;
                    }
                    //validar que pone una propiedad existente
                    if (DataHelper.GetProperty(WorkType, value) == null)
                    {
                        throw new NetChartException(string.Format(Message.ErrorInvalidPropertyName, value, WorkType.Name));
                    }
                    this._secondPropertyName = value;
                }
            }
        }

        public Property<T> Variable
        {
            get
            {
                return this._dataProperty;
            }
        }

        public  Property<T> ZVariable
        {
            get
            {
                return this._zDataProperty;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// establece el tipo de grafica
        /// </remarks>
        public ChartTypeEnum ChartType
        {
            get
            {
                return _chartType;
            }
            set
            {
                if (this._chartType != value)
                {
                    this._chartType = value;
                }
            }
        }

        /// <summary>
        /// Esto deberia devolver el json con la configuracion y los datos, la configuracion deberia de estar en una propiedad bajo demanda
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 1 - Validar datos
        /// 2 - Generar fichero de configuración
        /// 3 - Generar datos
        /// 4 - Incluir propiedades de presentación?
        /// 5 - devolver json compuesto
        /// </remarks>
        public string Generate()
        {
            //definir una clase para agrupar los datos.
            this.ValidateConfiguration();
            var output = new Output();
            output.ChartType = this.ChartType.ToString();

            if (this.ChartType == ChartTypeEnum.Debug)
            {
                //TODO: meter sugerencias
                output.Suggestions = this.GetSuggestions();
            }

            this.AddOutputData(output);
            string result = (new JavaScriptSerializer()).Serialize(output);
            //var asd1 = ComputedPropertyData;
            //var asd2 = ComputedSecondPropertyData;
            return result;
        }

        /// <summary>
        /// Valida que la configuración establecida es correcta para poder generar un gráfico
        /// </summary>
        private void ValidateConfiguration()
        {
            //TODO: no tengo claro que sea necesario validar la existencia de datos para poder decir si la configuración es correcta
            if (this.Data == null)
            {
                throw new NetChartException(Message.ErrorConfigurationNoData);
            }

            if (string.IsNullOrEmpty(this.Variable.Name))
            {
                throw new NetChartException(Message.ErrorConfigurationPropertyNameNull);
            }
            //validar la propiedad secundaria y la agregacion
            if (this.Variable.Aggregation != AggregateEnum.NoAggregate)
            {
                if (string.IsNullOrEmpty(this.SecondPropertyName))
                {
                    throw new NetChartException(Message.ErrorConfigurationAggregationWithoutGroup);
                }
                //HAY QUE VALIDAR EN LA AGRUPACION SI LAS PROPIEDADES TIENEN UN TIPO VALIDO, por ejemplo, si sumo, que sean numeros
            }

        }

        /// <summary>
        /// 
        /// </summary>
        private void AddOutputData(Output output)
        {
            //object[] mainData = null, secondaryData = null;
            Dictionary<object, object> processedData = new Dictionary<object, object>();
            Queue<object> keyOrder = new Queue<object>();

            if (this.Variable.Aggregation != AggregateEnum.NoAggregate)
            {
                //var dataProperty = DataHelper.GetProperty(WorkType, this.Variable.Name);
                var groupProperty = DataHelper.GetProperty(WorkType, this.SecondPropertyName);

                var groups = this.Data.GroupBy(x => groupProperty.GetValue(x));

                object groupData = null;
                foreach (var group in groups)
                {
                    keyOrder.Enqueue(group.Key);
                    switch (this.Variable.Aggregation)
                    {
                        case AggregateEnum.Sum:
                            groupData = DataHelper.CalculateAggregateSum<T>(this.Variable.Name, group);
                            break;
                        case AggregateEnum.Average:
                            groupData = DataHelper.CalculateAggregateAverage<T>(this.Variable.Name, group);
                            break;
                        case AggregateEnum.Count:
                            groupData = DataHelper.CalculateAggregateCount<T>(this.Variable.Name, group);
                            break;
                        case AggregateEnum.Maximum:
                            groupData = DataHelper.CalculateAggregateMaximum<T>(this.Variable.Name, group);
                            break;
                        case AggregateEnum.Minimum:
                            groupData = DataHelper.CalculateAggregateMinimum<T>(this.Variable.Name, group);
                            break;
                        default:
                            throw new NotSupportedException();
                    }
                    processedData.Add(group.Key, groupData);
                }
            }
            else
            {
                //Aqui va el caso de no ser agregados y tambien existe la posibilidad de que no se haya definido la segunda propieddad
            }

            //TODO: SI METEMOS ORDEN VA AQUI, REORDENAR el objeto de keyOrder y pista            
            output.ComputedPropertyData = new object[keyOrder.Count];
            output.ComputedSecondPropertyData = new object[keyOrder.Count];
            int position = 0;
            object key = null;
            while (keyOrder.Count != 0)
            {
                key = keyOrder.Dequeue();
                output.ComputedSecondPropertyData[position] = key;
                output.ComputedPropertyData[position++] = processedData[key];
            }
        }

        /// <summary>
        /// Obtine las sugerencias recomendadas para los datos especificados
        /// </summary>
        /// <returns></returns>
        private string[] GetSuggestions()
        {
            var results = new List<string>();

            //encuentra la primera propiedad y su tipo
            //encuentra la segunda propiedad y su tipo
            //encuentra la tercera propiedad y su tipo

            //mirar si existe agregacion

            //OJO, las variables cuantitaticas discretas pueden tratarse como continuas,
            //contemplar la posibilidad de poner un comentario al usuario

            //cualitativas -> ordinal y nominal

            //para los agregados -> recomendar barras
            //para no agragados: -si tipo int o long -> recomendar barras, aunque podria usar linea
            //                   -si tipo float o decimal -> recomendar linea


            //TODO: Hacer un arbol con las propiedades y meterlo en el documento del TFM

            //VariableTypeEnum mainDisplayType = DataHelper.GetPropertyDisplayType(WorkType, this.PropertyName);
            VariableTypeEnum mainDisplayType = this.Variable.DisplayType;
            VariableTypeEnum secondDisplayType = VariableTypeEnum.Discrete;
            //Si no esta definida toma el valor de la posicion => entero => discreto
            if (!string.IsNullOrEmpty(this.SecondPropertyName))
            {
                secondDisplayType = DataHelper.GetPropertyDisplayType(WorkType, this.SecondPropertyName);
            }
            bool useSecondProp = !string.IsNullOrEmpty(this.SecondPropertyName);

            //hacer lo mismo para la variable z

            bool useZProp = false;

            //TODO: hacer un if para una variable, otro if para 2 variables y un if para las 3 variables
            //caso 1, solo usamos la variable principal
            if (useSecondProp == false && useZProp == false)
            {

            }

            //caso 2, usamos la variable principal y la secundaria
            if (useSecondProp == true && useZProp == false)
            {

            }

            //caso 3, usamos todas las variables, principal, secundaria y z
            if (useSecondProp == true && useZProp == true)
            {

            }
            //asdasd

            if (this.Variable.Aggregation != AggregateEnum.NoAggregate)
            {
                if (mainDisplayType == VariableTypeEnum.Discrete)
                {
                    results.Add(ChartTypeEnum.Bar.ToString());
                }
                if (mainDisplayType == VariableTypeEnum.Continuous)
                {
                    results.Add(ChartTypeEnum.Line.ToString());
                }
                if (mainDisplayType == VariableTypeEnum.Discrete && secondDisplayType == VariableTypeEnum.Discrete)
                {
                    results.Add(ChartTypeEnum.Scatter.ToString());
                }

                ////si mainDisplayType es discreta o continua, y no existe segunda, meter grafico de tarta
                //if(mainDisplayType == VariableTypeEnum.Nominal && useSecondProp == false)
                //{
                //    //meter grafico tarta
                //}
                //if (mainDisplayType == VariableTypeEnum.Continuous && useSecondProp == false)
                //{
                //    //meter gráfico tarta
                //}

                //if(mainDisplayType == VariableTypeEnum.Continuous && useSecondProp == true)
                //{
                //    if
                //}

                //hacer un if para la variable z


            }
            else
            {
                //por ser agregado automaticamente el tipo es discreto
                mainDisplayType = VariableTypeEnum.Discrete;

                //hacer un if para la variable z
                results.Add(ChartTypeEnum.Bar.ToString());
            }

            return results.ToArray();
        }

    }
}
