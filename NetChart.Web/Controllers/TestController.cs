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
                    Description = "Test gráfico de tarta",
                    Method =  "TestPie"
                }, 
                new TestVM()
                {
                    Description = "Test gráfico de radar",
                    Method = "TestRadar"
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
            if(tipo != null)
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
            //chart.VariableProperty.Aggregation;
            chart.DimensionPropertyName = "Nacionalidad";
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
            //chart.VariableProperty.Aggregation;
            chart.DimensionPropertyName = "Nacionalidad";
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
            ViewBag.nc_data = chart.Generate();
            return View(data);
        }
    }
}