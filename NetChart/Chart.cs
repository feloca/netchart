using System;
using System.Collections;
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
            this._serieProperty = new Property<T>();
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
        private Property<T> _serieProperty;

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
        /// Obtiene o establece el nombre de la propiedad que define las series
        /// </summary>
        public string SeriePropertyName
        {
            get
            {
                return this.SerieProperty.Name;
            }
            set
            {
                this.SerieProperty.Name = value;
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
        /// Obtiene la configuracion de la serie
        /// </summary>
        /// <remarks>
        /// Es privada porque para el usuario solamente es necesario definir la serie
        /// </remarks>
        private Property<T> SerieProperty
        {
            get
            {
                return this._serieProperty;
            }
        }

        /// <summary>
        /// Obtiene o establece el tipo de grafico a representar
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
        /// Obtiene o establece el orden de los datos sobre la propiedad dimension
        /// </summary>
        public OrderTypeEnum OrderDimensionProperty
        {
            get;
            set;
        }

        /// <summary>
        /// Obtiene o establece el título del gráfico a mostrar
        /// </summary>
        public string Title { get; set; }

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
            output.ChartType = ((int)this.ChartType);

            if (this.ChartType == ChartTypeEnum.Debug)
            {
                output.Suggestions = this.GetSuggestions();
                output.VariableInfo = this.GetPropertyDebugInfo(this.VariableProperty);
                output.DimensionInfo = this.GetPropertyDebugInfo(this.DimensionProperty);
                output.ZVariableInfo = this.GetPropertyDebugInfo(this.ZVariableProperty);
            }

            //Configuramos los aspectos visuales
            this.AddDisplayOutputData(output);

            //Formateamos y cargamos los datos en la salida
            this.AddOutputData(output);

            //Ordenamos la salida            
            this.SortOutputData(output);

            string result = (new JavaScriptSerializer()).Serialize(output);
            //Añado los apostrofes para que cuando alcance el código javascript tener una cadena JSON
            return "'" + result + "'";
        }

        /// <summary>
        /// Obtiene un texto descriptivo de la propiedad indicada
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        private string GetPropertyDebugInfo(Property<T> property)
        {
            var result = string.Empty;
            if (property.IsDefined)
            {
                result = property.Name;
                if (property.Aggregation != AggregateEnum.NoAggregate)
                {
                    result += " (" + property.Aggregation.ToString() + ")";
                }
                result += " - " + property.DisplayType.ToString();
            }
            else
            {
                result = "Not defined";
            }
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

            //Validamos que no esten agregadas a la vez los dos ejes principales
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
            //aqui me he quedado, mirar lo de las series
            //y para cada serie llamar a una funcion que procese los datos como esto que esta aqui

            //aunque no exista serie definida devolveremos los datos en una serie
            //si serie definida agrupar datos por series sino todos los datos son la serie
            //para cada serie generar un output de serie

            //OutputSerie
            var results = new List<OutputSeries>();
            if (this.SerieProperty.IsDefined)
            {
                var seriesValues = DataHelper.GetPropertyValues<T>(this.SeriePropertyName, this.Data);
                var listSeries = new List<OutputSeries>();

                for (int i = 0; i < seriesValues.Count; ++i)
                {
                    var seriesData = DataHelper.GetGroupRows<T>(this.DimensionPropertyName, seriesValues[i], this.Data);
                    var outputSeries = new OutputSeries();
                    outputSeries.Descriptor = seriesValues[i];
                    ProcessOutputSeries(outputSeries, seriesData);
                    results.Add(outputSeries);
                }
            }
            else
            {
                var outputSeries = new OutputSeries();
                outputSeries.Descriptor = "BASE";
                ProcessOutputSeries(outputSeries, this.Data);
                results.Add(outputSeries);
            }

            output.Series = results.ToArray();

            if (this.SerieProperty.IsDefined)
            {
                List<object> dimensions = new List<object>();
                
                for(int i = 0; i < output.Series.Length; ++i)
                {
                    dimensions.AddRange(output.Series[i].DimensionData);
                }

                var dataComparer =((IEqualityComparer<object>)this.DimensionProperty.Comparer);                
                output.SeriesDimensions = dimensions.Distinct(dataComparer).ToArray();
            }
        }

        /// <summary>
        /// Procesa los datos de una serie dandolos formato adaptado a la salida grafica
        /// </summary>
        /// <param name="outputSeries"></param>
        /// <param name="seriesData"></param>
        private void ProcessOutputSeries(OutputSeries outputSeries, List<T> seriesData)
        {
            var details = new List<OutputDetail<T>>();
            var zDetails = new List<OutputDetail<T>>();

            //TODO: este if se puede refactorizar, los dos else internos son iguales
            if (this.VariableProperty.Aggregation != AggregateEnum.NoAggregate)
            {
                if (this.DimensionProperty.IsDefined)
                {
                    //no es nula la dimension, para cada valor de dimension hacer un grupo, en dimension poner la llave del 
                    //grupo, y en variable el valor del agregado. Poner todos los elementos del grupo en la propiedad outputdetail.DATA
                    var dimensionValues = DataHelper.GetPropertyValues<T>(this.DimensionPropertyName, seriesData);
                    for (int i = 0; i < dimensionValues.Count; ++i)
                    {
                        var groupRows = DataHelper.GetGroupRows<T>(this.DimensionPropertyName, dimensionValues[i], seriesData);
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
                    for (int i = 0; i < seriesData.Count; ++i)
                    {
                        details.Add(new OutputDetail<T>()
                        {
                            VariableDatum = propertyInfo.GetValue(seriesData[i]),
                            DimensionDatum = i,
                            Data = new List<T>() { seriesData[i] }
                        });
                    }
                }
            }
            else
            {
                //caso de no agregacion
                if (this.DimensionProperty.IsDefined)
                {
                    //no es nula la dimension, hace falta sacar los valores de dimension y de variable, 
                    //Poner todos los elementos del grupo en la propiedad outputdetail.DATA
                    var propertyInfo = DataHelper.GetProperty(this.WorkType, this.VariableProperty.Name);
                    var dimensionPropertyInfo = DataHelper.GetProperty(this.WorkType, this.DimensionPropertyName);
                    for (int i = 0; i < seriesData.Count; ++i)
                    {
                        details.Add(new OutputDetail<T>()
                        {
                            VariableDatum = propertyInfo.GetValue(seriesData[i]),
                            DimensionDatum = dimensionPropertyInfo.GetValue(seriesData[i]),
                            Data = new List<T>() { seriesData[i] }
                        });
                    }
                }
                else
                {
                    //es nula la dimension, luego la dimension es 0, 1, 2, 3, etc.. usar el orden de los datos
                    //Poner la fila en DATA
                    var propertyInfo = DataHelper.GetProperty(this.WorkType, this.VariableProperty.Name);
                    for (int i = 0; i < seriesData.Count; ++i)
                    {
                        details.Add(new OutputDetail<T>()
                        {
                            VariableDatum = propertyInfo.GetValue(seriesData[i]),
                            DimensionDatum = i,
                            Data = new List<T>() { seriesData[i] }
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
                    outputSeries.VariableData = new object[details.Count];
                    outputSeries.DimensionData = new object[details.Count];
                    outputSeries.ZVariableData = new object[details.Count];

                    for (int i = 0; i < details.Count; ++i)
                    {
                        outputSeries.VariableData[i] = details[i].VariableDatum;
                        outputSeries.DimensionData[i] = details[i].DimensionDatum;
                        outputSeries.ZVariableData[i] = DataHelper.CalculateAggregate<T>(this.ZVariableProperty.Name, this.ZVariableProperty.Aggregation, details[i].Data);
                    }
                }
                else
                {
                    //caso de no agregacion en z
                    //poner el valor de z del primer elemento de la coleccion de datos
                    var zPropertyInfo = DataHelper.GetProperty(this.WorkType, this.ZVariableProperty.Name);

                    outputSeries.VariableData = new object[details.Count];
                    outputSeries.DimensionData = new object[details.Count];
                    outputSeries.ZVariableData = new object[details.Count];

                    for (int i = 0; i < details.Count; ++i)
                    {
                        outputSeries.VariableData[i] = details[i].VariableDatum;
                        outputSeries.DimensionData[i] = details[i].DimensionDatum;
                        outputSeries.ZVariableData[i] = zPropertyInfo.GetValue(details[i].Data.First());
                    }
                }
            }
            else
            {
                //caso, no hay z         
                outputSeries.VariableData = new object[details.Count];
                outputSeries.DimensionData = new object[details.Count];
                outputSeries.ZVariableData = null;
                for (int i = 0; i < details.Count; ++i)
                {
                    outputSeries.VariableData[i] = details[i].VariableDatum;
                    outputSeries.DimensionData[i] = details[i].DimensionDatum;
                }
            }
        }

        /// <summary>
        /// Ordena la información de salida según la configuracion indicada por el usuario o según
        /// criterios por defecto
        /// </summary>
        /// <param name="output"></param>
        /// <remarks>
        /// 
        /// </remarks>
        private void SortOutputData(Output output)
        {
            var comparer = this.ConfigureComparer();
            if(comparer == null)
            {
                return;
            }

            for (int i = 0; i < output.Series.Length; ++i)
            {
                SortOutputData(output.Series[i], comparer);
            }

            //Ordeno las dimensiones en el caso de existir series
            if (SerieProperty.IsDefined)
            {
                Array.Sort(output.SeriesDimensions, comparer);                
            }
        }

        private DataComparer ConfigureComparer()
        {
            DataComparer comparer = this.DimensionProperty.Comparer;

            //Si no esta definido el tipo de orden por defecto hace falta segun que tipos aplicar
            //un tipo por defecto
            if (this.OrderDimensionProperty == OrderTypeEnum.NotDefined)
            {
                switch (this.DimensionProperty.DisplayType)
                {
                    case VariableTypeEnum.Discrete:
                        //ordenar ascendente
                        return comparer;
                    case VariableTypeEnum.Continuous:
                        //ordenar ascendente
                        return comparer;
                    case VariableTypeEnum.Nominal:
                        //nada
                        return null;
                    case VariableTypeEnum.Ordinal:
                        //todo: aqui entraria un enumerado (¿y puede que un string???), ordenar por el enumerado
                        //Este no tiene ordenacion por defecto, es parecido al nominal
                        return comparer;
                    default:
                        throw new NotSupportedException();
                }
            }
            else
            {
                if (this.OrderDimensionProperty == OrderTypeEnum.Descending)
                {
                    comparer.Descending = true;
                }

                switch (this.DimensionProperty.DisplayType)
                {
                    case VariableTypeEnum.Discrete:
                        return comparer;
                    case VariableTypeEnum.Continuous:
                        return comparer;
                    case VariableTypeEnum.Nominal:
                        return comparer;
                    case VariableTypeEnum.Ordinal:
                        //todo: aqui entraria un enumerado (¿y puede que un string???), ordenar por el enumerado
                        return comparer;
                    default:
                        throw new NotSupportedException();
                }
            }            
        }

        /// <summary>
        /// Esta funcion se encarga de ordenar los datos de salida segun la configuracion indicada a traves del 
        /// objeto comparador
        /// </summary>
        /// <param name="output"></param>
        /// <param name="comparer"></param>
        private void SortOutputData(OutputSeries outputSeries, DataComparer comparer)
        {
            //var a1 =output.VariableData;
            //var a2 = output.DimensionData;
            //var a3 = output.ZVariableData;
            object auxVariable = null;
            object auxDimension = null;
            object auxZVariable = null;

            //la z puede o no aparecer
            bool checkZ = (outputSeries.ZVariableData != null && (outputSeries.DimensionData.Length == outputSeries.ZVariableData.Length));
            bool ordered = true;

            do
            {
                ordered = true;
                for (int i = 0; i < outputSeries.DimensionData.Length - 1; ++i)
                {
                    if (comparer.Compare(outputSeries.DimensionData[i], outputSeries.DimensionData[i + 1]) > 0)
                    {
                        ordered = false;
                        auxVariable = outputSeries.VariableData[i];
                        outputSeries.VariableData[i] = outputSeries.VariableData[i + 1];
                        outputSeries.VariableData[i + 1] = auxVariable;

                        auxDimension = outputSeries.DimensionData[i];
                        outputSeries.DimensionData[i] = outputSeries.DimensionData[i + 1];
                        outputSeries.DimensionData[i + 1] = auxDimension;

                        if (checkZ)
                        {
                            auxZVariable = outputSeries.ZVariableData[i];
                            outputSeries.ZVariableData[i] = outputSeries.ZVariableData[i + 1];
                            outputSeries.ZVariableData[i + 1] = outputSeries.ZVariableData[i];
                        }
                    }
                }
            } while (ordered == false);
        }

        /// <summary>
        /// Añade a la salida los parametros de visualizacion
        /// </summary>
        /// <param name="output"></param>
        private void AddDisplayOutputData(Output output)
        {
            output.Display = new OutputDisplay();
            output.Display.Title = this.Title ?? string.Empty;
            output.Display.VariableDisplayType = (int)this.VariableProperty.DisplayType;
            output.Display.DimensionDisplayType = (int)this.DimensionProperty.DisplayType;
            output.Display.ZVariableDisplayType = (int)this.ZVariableProperty.DisplayType;
        }

        /// <summary>
        /// Obtine las sugerencias recomendadas para los datos especificados
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// El codigo de las sugerencias se podría refactorizar pero por legilibilidad se define las reglas
        /// de cada gráfico de manera independiente
        /// </remarks>
        private int[] GetSuggestions()
        {
            var results = new List<int>();
            results.Add((int)ChartTypeEnum.Debug);

            //defino algunas funciones auxiliares con las comprobaciones habituales para facilitar la 
            //lectura de las reglas
            Func<bool> varDiscreteOrContinuous = () =>
            {
                return this.VariableProperty.DisplayType == VariableTypeEnum.Discrete
                || this.VariableProperty.DisplayType == VariableTypeEnum.Continuous;
            };
            Func<bool> dimNoDefinedOrOrdinal = () =>
            {
                return this.DimensionProperty.IsDefined == false
                || (this.DimensionProperty.IsDefined && this.DimensionProperty.DisplayType == VariableTypeEnum.Ordinal);
            };
            Func<bool> dimDefined = () =>
            {
                return this.DimensionProperty.IsDefined;
            };
            Func<bool> dimDiscreteOrContinuous = () =>
            {
                return this.DimensionProperty.IsDefined
                    && (this.DimensionProperty.DisplayType == VariableTypeEnum.Discrete
                    || this.DimensionProperty.DisplayType == VariableTypeEnum.Continuous);
            };
            Func<bool> dimDiscreteOrContinuousOrOrdinal = () =>
            {
                return this.DimensionProperty.IsDefined
                    && (this.DimensionProperty.DisplayType == VariableTypeEnum.Discrete
                    || this.DimensionProperty.DisplayType == VariableTypeEnum.Continuous
                    || this.DimensionProperty.DisplayType == VariableTypeEnum.Ordinal);
            };
            Func<bool> zVarDiscreteOrContinuous = () =>
            {
                return this.ZVariableProperty.IsDefined &&
                (this.ZVariableProperty.DisplayType == VariableTypeEnum.Discrete
                || this.ZVariableProperty.DisplayType == VariableTypeEnum.Continuous);
            };
            Func<bool> zVarDefined = () =>
            {
                return this.ZVariableProperty.IsDefined;
            };
            Func<bool> seriesDefined = () =>
            {
                return this.SerieProperty.IsDefined;
            };

            //variable siempre esta definida, se valida en ValidateConfiguration            

            //1-Histogram
            if (varDiscreteOrContinuous() && !dimDefined()
                && !zVarDefined() && !seriesDefined())
            {
                results.Add((int)ChartTypeEnum.Histogram);
            }

            //2-Line
            if (varDiscreteOrContinuous() && !dimDefined()
                && !zVarDefined() && !seriesDefined())
            {
                results.Add((int)ChartTypeEnum.Line);
            }

            //3-Scatter
            if (varDiscreteOrContinuous() && dimDiscreteOrContinuousOrOrdinal()
                && !zVarDefined() && !seriesDefined())
            {
                results.Add((int)ChartTypeEnum.Scatter);
            }

            //4-Bubble
            if (varDiscreteOrContinuous() && dimDiscreteOrContinuousOrOrdinal()
                && zVarDefined() && !seriesDefined())
            {
                results.Add((int)ChartTypeEnum.Bubble);
            }

            //5-Temperature
            if (varDiscreteOrContinuous() && dimDiscreteOrContinuous()
                && !zVarDefined() && seriesDefined())
            {
                results.Add((int)ChartTypeEnum.Temperature);
            }

            //6-Pie
            if (varDiscreteOrContinuous() && !dimDefined()
                && !zVarDefined() && seriesDefined()) //con la serie definimos el quesito
            {
                results.Add((int)ChartTypeEnum.Pie);
            }

            //7-Radar
            if (varDiscreteOrContinuous() && dimDefined()
                && !zVarDefined() && !seriesDefined())
            {
                results.Add((int)ChartTypeEnum.Radar);
            }

            //8-Area3D
            if (varDiscreteOrContinuous() && dimDiscreteOrContinuousOrOrdinal()
                && zVarDiscreteOrContinuous() && !seriesDefined())
            {
                results.Add((int)ChartTypeEnum.Area3D);
            }

            //9-Waterfall
            if (varDiscreteOrContinuous() && !dimDefined()
                && !zVarDefined() && !seriesDefined())
            {
                results.Add((int)ChartTypeEnum.Waterfall);
            }

            //10-AttachedColumnPercentage
            if (varDiscreteOrContinuous() && !dimDefined()
                && !zVarDefined() && seriesDefined())
            {
                results.Add((int)ChartTypeEnum.AttachedColumnPercentage);
            }

            //11-AttachedColumn
            if (varDiscreteOrContinuous() && !dimDefined()
                && !zVarDefined() && seriesDefined())
            {
                results.Add((int)ChartTypeEnum.AttachedColumn);
            }

            //12-OverlapAreaPercentage
            if (varDiscreteOrContinuous() && !dimDefined()
                && !zVarDefined() && seriesDefined())
            {
                results.Add((int)ChartTypeEnum.OverlapAreaPercentage);
            }

            //13-OverlapArea
            if (varDiscreteOrContinuous() && !dimDefined()
                && !zVarDefined() && seriesDefined())
            {
                results.Add((int)ChartTypeEnum.OverlapArea);
            }

            //todo: Comparacion (Unico) lineal o de fiebre ?? es un tipo nuevo o linea

            //14-MultipleColumn
            if (varDiscreteOrContinuous() && dimDefined()
                && !zVarDefined() && seriesDefined())
            {
                results.Add((int)ChartTypeEnum.MultipleColumn);
            }

            //15-MultipleLine
            if (varDiscreteOrContinuous() && !dimDefined()
                && !zVarDefined() && seriesDefined())
            {
                results.Add((int)ChartTypeEnum.MultipleLine);
            }

            //16-MultipleBar
            if (varDiscreteOrContinuous() && !dimDefined()
                && !zVarDefined() && seriesDefined())
            {
                results.Add((int)ChartTypeEnum.MultipleBar);
            }

            //todo: comparacion (entre objetos) columnas

            return results.ToArray();

            /* ESTO ES LO VIEJO, BORRARLO CUANDO ACABE
            var results = new List<int>();
            results.Add((int)ChartTypeEnum.Debug);
            //OJO, las variables cuantitaticas discretas pueden tratarse como continuas,
            //contemplar la posibilidad de poner un comentario al usuario

            //cualitativas -> ordinal y nominal

            //para los agregados -> recomendar barras
            //para no agragados: -si tipo int o long -> recomendar barras, aunque podria usar linea
            //                   -si tipo float o decimal -> recomendar linea

            //TODO: Hacer un arbol con las propiedades y meterlo en el documento del TFM
            VariableTypeEnum dimensionDisplayType = this.DimensionProperty.DisplayType;

            //

            if (this.ZVariableProperty.IsDefined)
            {
                //creo que en este caso no importa si la z es agregada o no, aquí va un valor discreto de z
                switch (this.ZVariableProperty.DisplayType)
                {
                    case VariableTypeEnum.Continuous:
                        results.Add((int)ChartTypeEnum.Bubble);
                        results.Add((int)ChartTypeEnum.Temperature);
                        break;
                    case VariableTypeEnum.Discrete:
                        results.Add((int)ChartTypeEnum.Bubble);
                        results.Add((int)ChartTypeEnum.Temperature);
                        break;
                    case VariableTypeEnum.Nominal:
                        //TODO: No se que poner aqui, creo deberiamos de poner una etiqueta, ¿usar una gráfico de burbujas?
                        //results.Add(ChartTypeEnum.Bubble.ToString());
                        throw new NotImplementedException();
                        break;
                    case VariableTypeEnum.Ordinal:                        
                        throw new NotImplementedException();
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
                                results.Add((int)ChartTypeEnum.Line);
                                break;
                            case VariableTypeEnum.Discrete:
                                results.Add((int)ChartTypeEnum.Bar);
                                results.Add((int)ChartTypeEnum.Pie);
                                results.Add((int)ChartTypeEnum.Radar);
                                break;
                            case VariableTypeEnum.Nominal:
                                results.Add((int)ChartTypeEnum.Bar);
                                results.Add((int)ChartTypeEnum.Pie);
                                results.Add((int)ChartTypeEnum.Radar);
                                break;
                            case VariableTypeEnum.Ordinal:
                                throw new NotImplementedException();
                            default:
                                throw new NotSupportedException();
                        }
                        break;
                    case VariableTypeEnum.Discrete:
                        switch (this.DimensionProperty.DisplayType)
                        {
                            case VariableTypeEnum.Continuous:
                                results.Add((int)ChartTypeEnum.Line);
                                results.Add((int)ChartTypeEnum.Scatter); //este no lo tengo claro
                                break;
                            case VariableTypeEnum.Discrete:
                                results.Add((int)ChartTypeEnum.Scatter);
                                break;
                            case VariableTypeEnum.Nominal:
                                results.Add((int)ChartTypeEnum.Bar);
                                results.Add((int)ChartTypeEnum.Pie);
                                results.Add((int)ChartTypeEnum.Radar);
                                break;
                            case VariableTypeEnum.Ordinal:
                                throw new NotImplementedException();
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
                                results.Add((int)ChartTypeEnum.Bubble);
                                break;
                            case VariableTypeEnum.Ordinal:
                                throw new NotImplementedException();
                            default:
                                throw new NotSupportedException();
                        }
                        break;
                    default:
                        throw new NotSupportedException();
                }



            }

            return results.ToArray();
            */
        }

    }
}
