using NetChart;
using NetChart.GeneradorDatos;
using NetChart.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace NetChart.Web.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            var testList = new List<TestVM>();

            testList.AddRange(new[] {
                new TestVM() {
                    Description = "Pruebas de concepto",
                    Method = "desa"
                },
                new TestVM()
                {
                    Description = "Test gráfico histograma",
                    Method = "TestHistogram"
                },
                new TestVM()
                {
                    Description = "Test gráfico de líneas",
                    Method = "TestLine"
                },
                new TestVM()
                {
                    Description = "Test gráfico de dispersión",
                    Method = "TestScatter"
                },
                new TestVM()
                {
                    Description = "Test gráfico de burbujas",
                    Method = "TestBubble"
                },
                new TestVM()
                {
                    Description = "Test gráfico de temperatura",
                    Method = "TestTemperature"
                },
                new TestVM()
                {
                    Description = "Test gráfico de tarta",
                    Method =  "TestPie"
                },
                new TestVM()
                {
                    Description = "Test gráfico de radar",
                    Method = "TestRadar"
                },
                new TestVM()
                {
                    Description = "Test gráfico de area 3d",
                    Method = "TestArea3D"
                },
                new TestVM()
                {
                    Description = "Test gráfico de cascada",
                    Method = "TestWaterfall"
                },
                new TestVM()
                {
                    Description = "Test gráfico de columnas adosadas con %",
                    Method = "TestAttachedColumnPercentage"
                },
                new TestVM()
                {
                    Description = "Test gráfico de columnas adosadas",
                    Method = "TestAttachedColumn"
                },
                new TestVM()
                {
                    Description = "Test gráfico de areas adosadas con %",
                    Method = "TestOverlapAreaPercentage"
                },        
                new TestVM()
                {
                    Description = "Test gráfico de areas adosadas",
                    Method = "TestOverlapArea"
                },
                new TestVM()
                {
                    Description = "Test gráfico de multiples columnas",
                    Method = "TestMultipleColumn"
                },        
                new TestVM()
                {
                    Description = "Test gráfico de multiples líneas",
                    Method = "TestMultipleLine"
                },        
                new TestVM()
                {
                    Description = "Test gráfico de multiples barras",
                    Method = "TestMultipleBar"
                }
                });
            return View(testList);
        }

        public ActionResult Desa()
        {
            return View();
        }

        public ActionResult TestHistogram(int? tipo)
        {
            var data = Generator.GenerarPersonas(10);
            //Nuevo gráfico asociado a un tipo especifico
            var chart = new Chart<Persona>();
            //Asignamos colección de datos
            chart.Data = data;
            //Asignarmos el tipo elegido
            chart.ChartType = ChartTypeEnum.Histogram;
            //Si quisiéramos recomendaciones indicamos modo depuración
            if (tipo != null)
            {
                chart.ChartType = ChartTypeEnum.Debug;
            }
            //Asignamos la variable principal
            chart.VariablePropertyName = "Altura";
            //Indicamos el titulo del grafico
            chart.Title = "Altura del grupo";
            //Generamos datos JSON y los compartimos con la vista
            ViewBag.nc_data = chart.Generate();
            return View(data);
        }

        public ActionResult TestLine(int? tipo)
        {
            var data = Generator.GenerarPersonas(10);
            var chart = new Chart<Persona>();
            chart.Data = data;
            chart.ChartType = ChartTypeEnum.Line;
            if (tipo != null)
            {
                chart.ChartType = ChartTypeEnum.Debug;
            }
            chart.VariablePropertyName = "Edad";
            chart.Title = "Edad del grupo";
            ViewBag.nc_data = chart.Generate();
            return View(data);
        }

        public ActionResult TestScatter(int? tipo)
        {
            var data = Generator.GenerarPersonas(10);
            var chart = new Chart<Persona>();
            chart.Data = data;
            chart.ChartType = ChartTypeEnum.Scatter;
            if (tipo != null)
            {
                chart.ChartType = ChartTypeEnum.Debug;
            }
            chart.VariablePropertyName = "Altura";
            chart.VariableProperty.Aggregation = AggregateEnum.Average;
            chart.DimensionPropertyName = "Estudios";
            chart.Title = "Media de altura por nacionalidad";
            ViewBag.nc_data = chart.Generate();
            return View(data);
        }

        public ActionResult TestBubble(int? tipo)
        {
            var data = Generator.GenerarPersonas(10);
            var chart = new Chart<Persona>();
            chart.Data = data;
            chart.ChartType = ChartTypeEnum.Bubble;
            if (tipo != null)
            {
                chart.ChartType = ChartTypeEnum.Debug;
            }

            chart.VariablePropertyName = "Altura";
            chart.DimensionPropertyName = "Edad";
            chart.ZVariablePropertyName = "Peso";

            //ordenamos la salida
            chart.OrderDimensionProperty = OrderTypeEnum.Ascending;
            chart.Title = "Peso por altura/edad";
            ViewBag.nc_data = chart.Generate();
            return View(data);
        }

        public ActionResult TestTemperature(int? tipo)
        {
            var data = Generator.GenerarPersonas(20);
            var chart = new Chart<Persona>();
            chart.Data = data;
            chart.ChartType = ChartTypeEnum.Temperature;
            if (tipo != null)
            {
                chart.ChartType = ChartTypeEnum.Debug;
            }

            chart.VariablePropertyName = "Altura";
            //chart.VariableProperty.Aggregation;
            chart.DimensionPropertyName = "Edad";
            chart.SeriePropertyName = "Nacionalidad";
            chart.Title = "Altura por edad segun nacionalidad";
            ViewBag.nc_data = chart.Generate();
            return View(data);
        }

        public ActionResult TestPie(int? tipo)
        {
            var data = Generator.GenerarPersonas(10);
            var chart = new Chart<Persona>();
            chart.Data = data;
            chart.ChartType = ChartTypeEnum.Pie;
            if (tipo != null)
            {
                chart.ChartType = ChartTypeEnum.Debug;
            }
            chart.VariablePropertyName = "Altura";
            chart.VariableProperty.Aggregation = AggregateEnum.Average;
            chart.SeriePropertyName = "Nacionalidad";
            chart.Title = "Media de edad por nacionalidades";
            ViewBag.nc_data = chart.Generate();
            return View(data);
        }

        public ActionResult TestRadar(int? tipo)
        {
            var data = Generator.GenerarPersonas(10);
            var chart = new Chart<Persona>();
            chart.Data = data;
            chart.ChartType = ChartTypeEnum.Radar;
            if (tipo != null)
            {
                chart.ChartType = ChartTypeEnum.Debug;
            }
            chart.VariablePropertyName = "Altura";
            //chart.VariableProperty.Aggregation;
            chart.DimensionPropertyName = "Nacionalidad";
            chart.Title = "";
            ViewBag.nc_data = chart.Generate();
            return View(data);
        }

        public ActionResult TestArea3D(int? tipo)
        {
            var data = Generator.GenerarPersonas(10);
            var chart = new Chart<Persona>();
            chart.Data = data;
            chart.ChartType = ChartTypeEnum.Area3D;
            if (tipo != null)
            {
                chart.ChartType = ChartTypeEnum.Debug;
            }

            chart.VariablePropertyName = "Tension";
            chart.DimensionPropertyName = "Estudios";
            chart.ZVariablePropertyName = "Edad";            

            chart.Title = "Tension según nivel de estudios por edades";
            ViewBag.nc_data = chart.Generate();
            return View(data);
        }

        public ActionResult TestWaterfall(int? tipo)
        {
            var data = Generator.GenerarPersonas(10);
            var chart = new Chart<Persona>();
            chart.Data = data;
            chart.ChartType = ChartTypeEnum.Waterfall;
            if (tipo != null)
            {
                chart.ChartType = ChartTypeEnum.Debug;
            }

            chart.VariablePropertyName = "Peso";
            //chart.DimensionPropertyName = "Estudios";
            //chart.ZVariablePropertyName = "Edad";

            chart.Title = "Evolución pesos de la población";
            ViewBag.nc_data = chart.Generate();
            return View(data);
        }

        public ActionResult TestAttachedColumnPercentage(int? tipo)
        {
            var data = Generator.GenerarPersonas(10);
            var chart = new Chart<Persona>();
            chart.Data = data;
            chart.ChartType = ChartTypeEnum.AttachedColumnPercentage;
            if (tipo != null)
            {
                chart.ChartType = ChartTypeEnum.Debug;
            }

            chart.VariablePropertyName = "Tension";
            chart.VariableProperty.Aggregation = AggregateEnum.Average;
            chart.SeriePropertyName = "Nacionalidad";

            chart.Title = "Tensiones por nacionalidad en %";
            ViewBag.nc_data = chart.Generate();
            return View(data);
        }

        public ActionResult TestAttachedColumn(int? tipo)
        {
            var data = Generator.GenerarPersonas(10);
            var chart = new Chart<Persona>();
            chart.Data = data;
            chart.ChartType = ChartTypeEnum.AttachedColumn;
            if (tipo != null)
            {
                chart.ChartType = ChartTypeEnum.Debug;
            }

            chart.VariablePropertyName = "Tension";
            chart.VariableProperty.Aggregation = AggregateEnum.Average;
            chart.SeriePropertyName = "Nacionalidad";

            chart.Title = "Tensiones por nacionalidad";
            ViewBag.nc_data = chart.Generate();
            return View(data);
        }

        public ActionResult TestOverlapAreaPercentage(int? tipo)
        {
            var data = Generator.GenerarPersonas(10);
            var chart = new Chart<Persona>();
            chart.Data = data;
            chart.ChartType = ChartTypeEnum.OverlapAreaPercentage;
            if (tipo != null)
            {
                chart.ChartType = ChartTypeEnum.Debug;
            }

            chart.VariablePropertyName = "Tension";
            chart.VariableProperty.Aggregation = AggregateEnum.Average;
            chart.SeriePropertyName = "Nacionalidad";

            chart.Title = "Tensiones por nacionalidad";
            ViewBag.nc_data = chart.Generate();
            return View(data);
        }

        public ActionResult TestOverlapArea(int? tipo)
        {
            var data = Generator.GenerarPersonas(10);
            var chart = new Chart<Persona>();
            chart.Data = data;
            chart.ChartType = ChartTypeEnum.OverlapArea;
            if (tipo != null)
            {
                chart.ChartType = ChartTypeEnum.Debug;
            }

            chart.VariablePropertyName = "Tension";
            chart.VariableProperty.Aggregation = AggregateEnum.Average;
            chart.SeriePropertyName = "Nacionalidad";

            chart.Title = "Tensiones por nacionalidad";
            ViewBag.nc_data = chart.Generate();
            return View(data);
        }

        public ActionResult TestMultipleColumn(int? tipo)
        {
            var data = Generator.GenerarPersonas(10);
            var chart = new Chart<Persona>();
            chart.Data = data;
            chart.ChartType = ChartTypeEnum.MultipleColumn;
            if (tipo != null)
            {
                chart.ChartType = ChartTypeEnum.Debug;
            }

            chart.VariablePropertyName = "Edad";
            chart.DimensionPropertyName = "Estudios";
            chart.SeriePropertyName = "Nacionalidad";

            chart.Title = "Nivel de estudios según edad y nacionalidad";
            ViewBag.nc_data = chart.Generate();
            return View(data);
        }

        public ActionResult TestMultipleLine(int? tipo)
        {
            var data = Generator.GenerarPersonas(10);
            var chart = new Chart<Persona>();
            chart.Data = data;
            chart.ChartType = ChartTypeEnum.MultipleLine;
            if (tipo != null)
            {
                chart.ChartType = ChartTypeEnum.Debug;
            }

            chart.VariablePropertyName = "Altura";            
            chart.SeriePropertyName = "Nacionalidad";

            chart.Title = "Evolución de la altura por nacionalidad";
            ViewBag.nc_data = chart.Generate();
            return View(data);
        }

        public ActionResult TestMultipleBar(int? tipo)
        {
            var data = Generator.GenerarPersonas(10);
            var chart = new Chart<Persona>();
            chart.Data = data;
            chart.ChartType = ChartTypeEnum.MultipleBar;
            if (tipo != null)
            {
                chart.ChartType = ChartTypeEnum.Debug;
            }

            chart.VariablePropertyName = "Altura";
            chart.SeriePropertyName = "Nacionalidad";

            chart.Title = "Evolución de la altura por nacionalidad";
            ViewBag.nc_data = chart.Generate();
            return View(data);
        }
    }
}