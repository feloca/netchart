using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChart
{
    /// <summary>
    /// 
    /// </summary>
    public class Property<T> where T : class
    {
        private string _name;
        private AggregateEnum _aggregation;
        private VariableTypeEnum _displayType;
        private bool _isDisplayTypeManual = false;

        private Type WorkType
        {
            get
            {
                return typeof(T);
            }
        }

        /// <summary>
        /// Obtiene o establece el nombre de la propiedad que contiene los datos de entre las disponibles en la clase T
        /// </summary>
        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                if (string.IsNullOrEmpty(this._name) || !this._name.Equals(value.ToLower()))
                {
                    //validar que pone una propiedad existente
                    if (DataHelper.GetProperty(WorkType, value) == null)
                    {
                        throw new NetChartException(string.Format(Message.ErrorInvalidPropertyName, value, WorkType.Name));
                    }
                    this._name = value;
                }
            }
        }

        /// <summary>
        /// Obtiene o establece el criterio de agregacion de los datos
        /// </summary>
        public AggregateEnum Aggregation
        {
            get
            {
                return this._aggregation;
            }
            set
            {
                if (this._aggregation != value)
                {
                    this._aggregation = value;
                }
            }
        }

        /// <summary>
        /// Obtiene el tipo de variable de representacion gráfica asociado al tipo de propiedad
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        public VariableTypeEnum DisplayType
        {
            //TODO: deberia soportar un set?, el usuario podria cambiar la forma en la que se interpreta la variable
            //TODO: creo que lo correcto seria ofrecer un valor por defecto, pero si el usuario configura en el set este valor respetarlo
            get
            {
                if (_isDisplayTypeManual)
                {
                    return _displayType;
                }

                if (this.IsDefined)
                {
                    //da igua que este agregada, porque el tipo resultado de la agregacion es el mismo que el de la propiedad sin agregar
                    //salvo en el caso de contar que tendre siempre un valor entero
                    //con tipos cadena no se puede sumar ni hacer la media, validado en el set
                    if (this.Aggregation == AggregateEnum.Count)
                    {
                        return VariableTypeEnum.Discrete;
                    }

                    return DataHelper.GetPropertyDisplayType(WorkType, this.Name);
                }

                return VariableTypeEnum.Discrete;
            }
            set
            {
                //todo: validar si es un string que no se establezcan los tipos invalidos
                //basicamente si el tipo es un numero y lo tratamos como cadena puede dar 
                //error al agregar
                //todo: esto hay que revisarlo, un ordinal puede ser un discreto
                var realType = DataHelper.GetPropertyDisplayType(WorkType, this.Name);
                if (realType != VariableTypeEnum.Discrete
                    && realType != VariableTypeEnum.Continuous)
                {
                    if (value == VariableTypeEnum.Continuous ||
                        value == VariableTypeEnum.Discrete)
                    {
                        throw new NetChartException(Message.ErrorConfigurationInvalidDisplayType);
                    }
                }

                this._isDisplayTypeManual = true;
                this._displayType = value;
            }
        }

        /// <summary>
        /// Obtiene un objeto comparador configurado segun las caracteristicas de la propiedad
        /// </summary>
        public DataComparer Comparer
        {
            get
            {
                var comparer = new DataComparer();
                if (this.IsDefined)
                {
                    comparer.PropertyType = DataHelper.GetProperty(this.WorkType, this.Name).PropertyType;
                }
                else
                {
                    comparer.PropertyType =  default(int).GetType();
                }                
                return comparer;                
            }
        }

        /// <summary>
        /// Obtiene si la propiedad ha sido definida o no
        /// </summary>
        public bool IsDefined
        {
            get
            {
                return !string.IsNullOrEmpty(this._name);
            }
        }
    }
}
