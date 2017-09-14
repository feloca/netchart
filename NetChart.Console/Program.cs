using NetChart.GeneradorDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChart.Console
{
    public class Program
    {
        static void Main(string[] args)
        {
            //Test1();
            //Test2();
            //Test3();
            //Test4();
            //Test5();
            TestAgregacionNominales();

            System.Console.WriteLine("Pulsa intro para terminar");
            System.Console.ReadLine();
        }

        public static void Test1()
        {
            List<Persona> datos = Generator.GenerarPersonas();
            var nc = new Chart<Persona>();
            nc.Data = datos;

            nc.VariableProperty.Name = "Altura";
            nc.DimensionPropertyName = "Edad";

            nc.ChartType = ChartTypeEnum.Debug;
            nc.VariableProperty.Aggregation = AggregateEnum.Sum;

            var json = nc.Generate();
        }

        public static void Test2()
        {
            List<Persona> datos = Generator.GenerarPersonas();
            var nc = new Chart<Persona>();
            nc.Data = datos;

            nc.VariableProperty.Name = "Altura";
            nc.DimensionPropertyName = "Edad";

            nc.ChartType = ChartTypeEnum.Debug;
            nc.VariableProperty.Aggregation = AggregateEnum.Average;

            var json = nc.Generate();
        }

        public static void Test3()
        {
            List<Persona> datos = Generator.GenerarPersonas();
            var nc = new Chart<Persona>();
            nc.Data = datos;

            nc.VariableProperty.Name = "Altura";
            nc.DimensionPropertyName = "Edad";

            nc.ChartType = ChartTypeEnum.Debug;
            nc.VariableProperty.Aggregation = AggregateEnum.Count;

            var json = nc.Generate();
        }

        public static void Test4()
        {
            List<Persona> datos = Generator.GenerarPersonas();
            var nc = new Chart<Persona>();
            nc.Data = datos;

            nc.VariableProperty.Name = "Altura";
            nc.DimensionPropertyName = "Edad";

            nc.ChartType = ChartTypeEnum.Debug;
            nc.VariableProperty.Aggregation = AggregateEnum.Maximum;

            var json = nc.Generate();
        }

        public static void Test5()
        {
            List<Persona> datos = Generator.GenerarPersonas();
            var nc = new Chart<Persona>();
            nc.Data = datos;

            nc.VariableProperty.Name = "Altura";
            nc.DimensionPropertyName = "Edad";

            nc.ChartType = ChartTypeEnum.Debug;
            nc.VariableProperty.Aggregation = AggregateEnum.Minimum;

            var json = nc.Generate();
        }

        /// <summary>
        /// Test para conocer que tipos de agregados permite linq con formato string
        /// </summary>
        public static void TestAgregacionNominales()
        {
            var lista = Generator.GenerarPersonas();
            var nacionalidadMax = lista.Max(x => x.Nacionalidad);
            var nacionalidadMin = lista.Min(x => x.Nacionalidad);
            var nacionalidadCount = lista.Count();
            //var nacionalidadAvg = lista.Average(x => x.Nacionalidad);
            //var nacionalidadSum = lista.Sum(x => x.Nacionalidad);
        }
    }
}
