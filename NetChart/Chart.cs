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
            this._dimensionProperty = new Property<T>();
            this._zDataProperty = new Property<T>();
        }

        public Chart(string configuration) : this()
        {
            throw new NotImplementedException();
            //la variable configuracion tiene formato json
            //aqui hay que enchufar los parametros de configuracion 
            //OJO, CREO QUE no hay que preocuparse por las validaciones porque al generar se valida
        }

        //temas de presentacion
        //titulo
        //titulo eje x, eje y (incluir propiedad de configuración)      

        //private string _propertyName;
        //private AggregateEnum _propertyAggregation;
        //private string _secondPropertyName;
        private ChartTypeEnum _chartType;

        //AQUI VA EL FORMATO NUEVO
        private Property<T> _dataProperty;
        private Property<T> _dimensionProperty;
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


        /// <summary>
        /// Obtiene o establece el nombre de la variable de dimension (variable eje x)
        /// </summary>
        /// <remarks>
        /// Si la propiedad no existe en T lanzar una excepcion
        /// </remarks>
        public string DimensionPropertyName
        {
            get
            {
                return this._dimensionProperty.Name;
            }
            set
            {
                this._dimensionProperty.Name = value;
            }
        }

        /// <summary>
        /// Obtiene o establece el nombre de la propiedad que actua como variable principal
        /// </summary>
        /// <remarks>
        /// Accesos para facilitar la configuración de las propiedades, el usuario por defecto solo tendria que seleccionar el nombre de la propiedad
        /// </remarks>
        public string VariablePropertyName
        {
            get
            {
                return this.VariableProperty.Name;
            }
            set
            {
                this.VariableProperty.Name = value;
            }
        }

        /// <summary>
        /// Obtiene o establece el nombre de la propiedad que actua como variable z
        /// </summary>
        /// <remarks>
        /// Accesos para facilitar la configuración de las propiedades, el usuario por defecto solo tendria que seleccionar el nombre de la propiedad
        /// </remarks>
        public string ZVariablePropertyName
        {
            get
            {
                return this.ZVariableProperty.Name;
            }
            set
            {
                this.ZVariableProperty.Name = value;
            }
        }

        /// <summary>
        /// Obtiene la configuracion de la variable principal (variable eje y)
        /// </summary>
        public Property<T> VariableProperty
        {
            get
            {
                return this._dataProperty;
            }
        }

        /// <summary>
        /// Obtiene la configuracion de la dimension (variable eje x)
        /// </summary>
        public Property<T> DimensionProperty
        {
            get
            {
                return this._dimensionProperty;
            }
        }

        /// <summary>
        /// Obtiene la configuracion de la variable z (variable interseccion ejes x e y)
        /// </summary>
        public Property<T> ZVariableProperty
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
            this.ValidateConfiguration();

            //podria dejar que la lista de datos fuera null y crear un coleccion vacía pero de momento
            //creo que seria datos == null y tipografico == debug cuando deberia de lanzar esta excepcion
            //TODO: mirar el comentario anterior, tener un grafico por defecto con "no datos"
            if (this.Data == null)
            {
                throw new NetChartException(Message.ErrorConfigurationNoData);
            }
            var output = new Output();
            output.ChartType = this.ChartType.ToString();

            if (this.ChartType == ChartTypeEnum.Debug)
            {
                //TODO: meter sugerencias
                output.Suggestions = this.GetSuggestions();
            }

            //TODO: pendiente gestionar las 3 variables posible, de manera similar al caso de las sugerencias, hay que meterlo tambien en el tipo output
            this.AddOutputData(output);
            string result = (new JavaScriptSerializer()).Serialize(output);
            return result;
        }

        /// <summary>
        /// Valida que la configuración establecida es correcta para poder generar un gráfico
        /// </summary>
        /// <remarks>
        /// 1 - Validar que se haya indicado datos
        /// 2 - Validar que se haya definido la variable principal
        /// 3 - Validar si si existe agregacion en variable o en dimension, que solo exista en una de ellas
        /// 4 - Validar que si existe agregacion en variable que la dimension este definida
        /// 5 - Validar que las agregaciones realizadas sobre nominales u ordinales no sean de suma ni de media
        /// NOTA: linq no permite agregar suma o media sobre tipos cadena
        /// </remarks>
        private void ValidateConfiguration()
        {
            //TODO: no tengo claro que sea necesario validar la existencia de datos para poder decir si la configuración es correcta
            //if (this.Data == null)
            //{
            //    throw new NetChartException(Message.ErrorConfigurationNoData);
            //}

            if (!this.VariableProperty.IsDefined)
            {
                throw new NetChartException(Message.ErrorConfigurationPropertyNameNull);
            }

            if (this.VariableProperty.IsDefined && this.DimensionProperty.IsDefined
                && this.ZVariableProperty.Aggregation != AggregateEnum.NoAggregate
                && this.DimensionProperty.Aggregation != AggregateEnum.NoAggregate)
            {
                throw new NetChartException(Message.ErrorConfigurationInvalidAggregation);
            }

            //validar la propiedad secundaria y la agregacion
            if (this.VariableProperty.Aggregation != AggregateEnum.NoAggregate && !this.DimensionProperty.IsDefined)
            {
                throw new NetChartException(Message.ErrorConfigurationAggregationWithoutGroup);
            }

            //valido que el tipo de la variable soporte la agregacion especificada
            if (this.VariableProperty.DisplayType != VariableTypeEnum.Discrete
                && this.VariableProperty.DisplayType != VariableTypeEnum.Continuous)
            {
                if (this.VariableProperty.Aggregation == AggregateEnum.Sum
                    || this.VariableProperty.Aggregation == AggregateEnum.Average)
                {
                    throw new NetChartException(Message.ErrorConfigurationStringTypeInvalidAggregation);
                }
            }

            //valido que el tipo de la dimension soporte la agregacion especificada
            if (this.DimensionProperty.DisplayType != VariableTypeEnum.Discrete
                && this.DimensionProperty.DisplayType != VariableTypeEnum.Continuous)
            {
                if (this.DimensionProperty.Aggregation == AggregateEnum.Sum
                    || this.DimensionProperty.Aggregation == AggregateEnum.Average)
                {
                    throw new NetChartException(Message.ErrorConfigurationStringTypeInvalidAggregation);
                }
            }

            //validamos la z si esta definida
            if (this.ZVariableProperty.IsDefined 
                && this.ZVariableProperty.DisplayType != VariableTypeEnum.Discrete
                && this.ZVariableProperty.DisplayType != VariableTypeEnum.Continuous)
            {
                if (this.ZVariableProperty.Aggregation == AggregateEnum.Sum
                    || this.ZVariableProperty.Aggregation == AggregateEnum.Average)
                {
                    throw new NetChartException(Message.ErrorConfigurationStringTypeInvalidAggregation);
                }
            }
        }

        /// <summary>
        /// Genera un modelo de datos formateados según la configuración indicada, estos datos son empleados
        /// por la parte javascript para dibujar el gráfico
        /// </summary>
        /// <param name="output"></param>
        /// <remarks>
        /// a) con solo variable principal si se agrupa no hacer nada (agregacion con un unico dato, luego siempre obtenemos el mismo dato)
        /// b) si existe segunda variable y agregacion en la principal, perfecto, caso facil agrupar por los distintos valores de la variable secundaria
        /// c) caso de z, 
	    ///   c1) si variable principal no agregada, => ¿creo que debe estar agregada siempre? => si no secundaria, poner el valor de z directamente, si secundaria hacer un grupo que cumpla x e y y hacer el agregado de z
        ///   c2) si variable principal agregada -> buscar todos los elementos de esa agregacion, la sera el valor de la agregacion de la var principal y la z sera el nuevo agregado de z
        ///
        /// </remarks>
        private void AddOutputData(Output output)
        {
            //var variableData = new List<OutputDetail<T>>();
            var details = new List<OutputDetail<T>>();
            var zDetails = new List<OutputDetail<T>>();

            //TODO: este if se puede refactorizar, los dos else internos son iguales
            if (this.VariableProperty.Aggregation != AggregateEnum.NoAggregate)
            {
                //if (!string.IsNullOrEmpty(this.DimensionPropertyName))
                if(this.DimensionProperty.IsDefined)
                {
                    //no es nula la dimension, para cada valor de dimension hacer un grupo, en dimension poner la llave del 
                    //grupo, y en variable el valor del agregado. Poner todos los elementos del grupo en la propiedad outputdetail.DATA
                    var dimensionValues = DataHelper.GetPropertyValues<T>(this.DimensionPropertyName, this.Data);
                    for (int i = 0; i < dimensionValues.Count; ++i)
                    {
                        var groupRows = DataHelper.GetGroupRows<T>(this.DimensionPropertyName, dimensionValues[i], this.Data);
                        var agregateValue = DataHelper.CalculateAggregate<T>(this.VariableProperty.Name, this.VariableProperty.Aggregation, groupRows);
                        details.Add(new OutputDetail<T>()
                        {
                            VariableDatum = agregateValue,
                            DimensionDatum = dimensionValues[i],
                            Data = groupRows
                        });
                    }
                }
                else
                {
                    //es nula la dimension, en dimension 0, 1, 2, 3, etc.. en variable el valor de variable, y poner fila en data
                    var propertyInfo = DataHelper.GetProperty(this.WorkType, this.VariableProperty.Name);
                    for (int i = 0; i < this.Data.Count; ++i)
                    {
                        details.Add(new OutputDetail<T>()
                        {
                            VariableDatum = propertyInfo.GetValue(this.Data[i]),
                            DimensionDatum = i,
                            Data = new List<T>() { this.Data[i] }
                        });
                    }
                }
            }
            else
            {
                //caso de no agregacion

                //if (!string.IsNullOrEmpty(this.DimensionPropertyName))
                if(this.DimensionProperty.IsDefined)
                {
                    //no es nula la dimension, hace falta sacar los valores de dimension y de variable, 
                    //Poner todos los elementos del grupo en la propiedad outputdetail.DATA
                    var propertyInfo = DataHelper.GetProperty(this.WorkType, this.VariableProperty.Name);
                    var dimensionPropertyInfo = DataHelper.GetProperty(this.WorkType, this.DimensionPropertyName);
                    for (int i = 0; i < this.Data.Count; ++i)
                    {
                        details.Add(new OutputDetail<T>()
                        {
                            VariableDatum = propertyInfo.GetValue(this.Data[i]),
                            DimensionDatum = dimensionPropertyInfo.GetValue(this.Data[i]),
                            Data = new List<T>() { this.Data[i] }
                        });
                    }

                }
                else
                {
                    //es nula la dimension, luego la dimension es 0, 1, 2, 3, etc.. usar el orden de los datos
                    //Poner la fila en DATA
                    var propertyInfo = DataHelper.GetProperty(this.WorkType, this.VariableProperty.Name);
                    for (int i = 0; i < this.Data.Count; ++i)
                    {
                        details.Add(new OutputDetail<T>()
                        {
                            VariableDatum = propertyInfo.GetValue(this.Data[i]),
                            DimensionDatum = i,
                            Data = new List<T>() { this.Data[i] }
                        });
                    }
                }
            }

            //si z distinto de null
            //trabajamos sobre variableData
            //si agregado para cada x e y, hacer el agregado
            //si no agregado, poner el valor de la variable z del primer elemento de la coleccion outputdetail.DATA
            if (this.ZVariableProperty.IsDefined)
            {
                if (this.ZVariableProperty.Aggregation != AggregateEnum.NoAggregate)
                {
                    //caso de agregacion en z
                    //poner en z el valor de la agregacion de la coleccion de datos
                    output.VariableData = new object[details.Count];
                    output.DimensionData = new object[details.Count];
                    output.ZVariableData = new object[details.Count];

                    for (int i = 0; i < details.Count; ++i)
                    {
                        output.VariableData[i] = details[i].VariableDatum;
                        output.DimensionData[i] = details[i].DimensionDatum;
                        output.ZVariableData[i] = DataHelper.CalculateAggregate<T>(this.ZVariableProperty.Name, this.ZVariableProperty.Aggregation, details[i].Data);
                    }

                }
                else
                {
                    //caso de no agregacion en z
                    //poner el valor de z del primer elemento de la coleccion de datos
                    var zPropertyInfo = DataHelper.GetProperty(this.WorkType, this.VariableProperty.Name);

                    output.VariableData = new object[details.Count];
                    output.DimensionData = new object[details.Count];
                    output.ZVariableData = new object[details.Count];

                    for (int i = 0; i < details.Count; ++i)
                    {
                        output.VariableData[i] = details[i].VariableDatum;
                        output.DimensionData[i] = details[i].DimensionDatum;
                        output.ZVariableData[i] = zPropertyInfo.GetValue(details[i].Data.First());
                    }

                }
            }
            else
            {
                //caso, no hay z         
                output.VariableData = new object[details.Count];
                output.DimensionData = new object[details.Count];
                output.ZVariableData = null;
                for (int i = 0; i < details.Count; ++i)
                {
                    output.VariableData[i] = details[i].VariableDatum;
                    output.DimensionData[i] = details[i].DimensionDatum;
                }
            }

        }

        /// <summary>
        /// Obtine las sugerencias recomendadas para los datos especificados
        /// </summary>
        /// <returns></returns>
        private string[] GetSuggestions()
        {
            var results = new List<string>();
            //OJO, las variables cuantitaticas discretas pueden tratarse como continuas,
            //contemplar la posibilidad de poner un comentario al usuario

            //cualitativas -> ordinal y nominal

            //para los agregados -> recomendar barras
            //para no agragados: -si tipo int o long -> recomendar barras, aunque podria usar linea
            //                   -si tipo float o decimal -> recomendar linea

            //TODO: Hacer un arbol con las propiedades y meterlo en el documento del TFM
            VariableTypeEnum dimensionDisplayType = this.DimensionProperty.DisplayType;

            if (this.ZVariableProperty.IsDefined)
            {
                //creo que en este caso no importa si la z es agregada o no, aquí va un valor discreto de z
                switch (this.ZVariableProperty.DisplayType)
                {
                    case VariableTypeEnum.Continuous:
                        results.Add(ChartTypeEnum.Bubble.ToString());
                        results.Add(ChartTypeEnum.Temperature.ToString());
                        break;
                    case VariableTypeEnum.Discrete:
                        results.Add(ChartTypeEnum.Bubble.ToString());
                        results.Add(ChartTypeEnum.Temperature.ToString());
                        break;
                    case VariableTypeEnum.Nominal:
                        //TODO: No se que poner aqui, creo deberiamos de poner una etiqueta, ¿usar una gráfico de burbujas?
                        //results.Add(ChartTypeEnum.Bubble.ToString());
                        throw new NotImplementedException();
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }
            else
            {
                switch (this.VariableProperty.DisplayType)
                {
                    case VariableTypeEnum.Continuous:
                        switch (this.DimensionProperty.DisplayType)
                        {
                            case VariableTypeEnum.Continuous:
                                results.Add(ChartTypeEnum.Line.ToString());
                                break;
                            case VariableTypeEnum.Discrete:
                                results.Add(ChartTypeEnum.Bar.ToString());
                                results.Add(ChartTypeEnum.Pie.ToString());
                                results.Add(ChartTypeEnum.Radar.ToString());
                                break;
                            case VariableTypeEnum.Nominal:
                                results.Add(ChartTypeEnum.Bar.ToString());
                                results.Add(ChartTypeEnum.Pie.ToString());
                                results.Add(ChartTypeEnum.Radar.ToString());
                                break;
                            default:
                                throw new NotSupportedException();
                        }
                        break;
                    case VariableTypeEnum.Discrete:
                        switch (this.DimensionProperty.DisplayType)
                        {
                            case VariableTypeEnum.Continuous:
                                results.Add(ChartTypeEnum.Line.ToString());
                                results.Add(ChartTypeEnum.Scatter.ToString()); //este no lo tengo claro
                                break;
                            case VariableTypeEnum.Discrete:
                                results.Add(ChartTypeEnum.Scatter.ToString());
                                break;
                            case VariableTypeEnum.Nominal:
                                results.Add(ChartTypeEnum.Bar.ToString());
                                results.Add(ChartTypeEnum.Pie.ToString());
                                results.Add(ChartTypeEnum.Radar.ToString());
                                break;
                            default:
                                throw new NotSupportedException();
                        }
                        break;
                    case VariableTypeEnum.Nominal:
                        switch (this.DimensionProperty.DisplayType)
                        {
                            case VariableTypeEnum.Continuous:
                            case VariableTypeEnum.Discrete:
                            case VariableTypeEnum.Nominal:
                                results.Add(ChartTypeEnum.Bubble.ToString());
                                break;
                            default:
                                throw new NotSupportedException();
                        }
                        break;
                    default:
                        throw new NotSupportedException();
                }
                
                //TODO: consultar esto con monitor o yussef
                //results.Add(ChartTypeEnum.Bar.ToString());
                //results.Add(ChartTypeEnum.Line.ToString());
                //results.Add(ChartTypeEnum.Pie.ToString());
                //results.Add(ChartTypeEnum.Radar.ToString());
                //results.Add(ChartTypeEnum.Scatter.ToString());
            }

            return results.ToArray();
        }

    }
}
