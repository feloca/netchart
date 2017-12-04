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
                    Description = "Test gráfico de barras",
                    Method = "TestBar"
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

        public ActionResult TestBar(int? tipo)
        {
            var data = Generator.GenerarPersonas(10);
            var chart = new Chart<Persona>();
            chart.Data = data;
            chart.ChartType = ChartTypeEnum.Bar;
            if(tipo != null)
            {
                chart.ChartType = ChartTypeEnum.Debug;
            }
            chart.VariablePropertyName = "Altura";
            //chart.VariableProperty.Aggregation;
            chart.DimensionPropertyName = "Nacionalidad";
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
            chart.VariablePropertyName = "Altura";            
            chart.VariableProperty.Aggregation = AggregateEnum.Average;
            chart.DimensionPropertyName = "Edad";
            ViewBag.nc_data = chart.Generate();
            return View(data);
        }

        public ActionResult TestScatter(int? tipo)
        {
            var data = Generator.GenerarPersonas(10);
            var chart = new Chart<Persona>();
            chart.Data = data;
            chart.ChartType = ChartTypeEnum.Bar;
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

        public ActionResult TestBubble(int? tipo)
        {
            var data = Generator.GenerarPersonas(10);
            var chart = new Chart<Persona>();
            chart.Data = data;
            chart.ChartType = ChartTypeEnum.Bar;
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
            chart.ChartType = ChartTypeEnum.Bar;
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
            chart.ChartType = ChartTypeEnum.Bar;
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