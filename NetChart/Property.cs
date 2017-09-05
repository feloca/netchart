using System;
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
        private Type WorkType
        {
            get
            {
                return typeof(T);
            }
        }

        //llamar a la propiedad secundaria dimension, esta no es de este tipo => dimensionDataProperty
        //a la propiedad principal dataproperty
        //a la propiedad z zproperty

        //tiene que ir tipado

        //Name
        //VariableType
        //Aggregation

        //IsDefined : bool

        //privado WorkType
        private string _name;
        private AggregateEnum _aggregation;

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
        /// 
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
        /// 
        /// </summary>
        public VariableTypeEnum DisplayType
        {
            get
            {
                if (this.IsDefined)
                {
                    return DataHelper.GetPropertyDisplayType(WorkType, this.Name);
                }

                return VariableTypeEnum.Discrete;
            }
        }

        /// <summary>
        /// Obtiene si la propiedad ha sido definida
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
