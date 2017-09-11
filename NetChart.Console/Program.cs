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
            Test1();
            Test2();
            Test3();
            Test4();
            Test5();

            System.Console.WriteLine("Pulsa intro para terminar");
            System.Console.ReadLine();
        }

        public static void Test1()
        {
            List<Persona> datos = CargarDatos();
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
            List<Persona> datos = CargarDatos();
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
            List<Persona> datos = CargarDatos();
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
            List<Persona> datos = CargarDatos();
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
            List<Persona> datos = CargarDatos();
            var nc = new Chart<Persona>();
            nc.Data = datos;

            nc.VariableProperty.Name = "Altura";
            nc.DimensionPropertyName = "Edad";

            nc.ChartType = ChartTypeEnum.Debug;
            nc.VariableProperty.Aggregation = AggregateEnum.Minimum;

            var json = nc.Generate();
        }

        private static List<Persona> CargarDatos()
        {
            throw new NotImplementedException();
            List<Persona> lista = new List<Persona>();
            /*
            lista.AddRange(new Persona[] {
                new Persona(){Edad=59, Altura=207, Coeficiente=119},
                new Persona(){Edad=50, Altura=185, Coeficiente=133},
                new Persona(){Edad=35, Altura=195, Coeficiente=101},
                new Persona(){Edad=29, Altura=186, Coeficiente=103},
                new Persona(){Edad=27, Altura=175, Coeficiente=81},
                new Persona(){Edad=18, Altura=185, Coeficiente=146},
                new Persona(){Edad=52, Altura=167, Coeficiente=133},
                new Persona(){Edad=22, Altura=173, Coeficiente=94},
                new Persona(){Edad=55, Altura=144, Coeficiente=113},
                new Persona(){Edad=39, Altura=209, Coeficiente=147},
                new Persona(){Edad=19, Altura=173, Coeficiente=106},
                new Persona(){Edad=63, Altura=144, Coeficiente=111},
                new Persona(){Edad=52, Altura=168, Coeficiente=132},
                new Persona(){Edad=48, Altura=179, Coeficiente=142},
                new Persona(){Edad=56, Altura=199, Coeficiente=144},
                new Persona(){Edad=43, Altura=196, Coeficiente=125},
                new Persona(){Edad=15, Altura=193, Coeficiente=131},
                new Persona(){Edad=54, Altura=182, Coeficiente=149},
                new Persona(){Edad=52, Altura=191, Coeficiente=108},
                new Persona(){Edad=30, Altura=202, Coeficiente=117},
                new Persona(){Edad=27, Altura=203, Coeficiente=133},
                new Persona(){Edad=36, Altura=153, Coeficiente=84},
                new Persona(){Edad=63, Altura=160, Coeficiente=104},
                new Persona(){Edad=45, Altura=162, Coeficiente=89},
                new Persona(){Edad=23, Altura=145, Coeficiente=136},
                new Persona(){Edad=59, Altura=161, Coeficiente=120},
                new Persona(){Edad=33, Altura=156, Coeficiente=89},
                new Persona(){Edad=57, Altura=174, Coeficiente=87},
                new Persona(){Edad=24, Altura=160, Coeficiente=139},
                new Persona(){Edad=37, Altura=155, Coeficiente=96},
                new Persona(){Edad=16, Altura=170, Coeficiente=110},
                new Persona(){Edad=24, Altura=169, Coeficiente=122},
                new Persona(){Edad=20, Altura=168, Coeficiente=123},
                new Persona(){Edad=52, Altura=182, Coeficiente=111},
                new Persona(){Edad=28, Altura=166, Coeficiente=106},
                new Persona(){Edad=41, Altura=183, Coeficiente=112},
                new Persona(){Edad=23, Altura=199, Coeficiente=103},
                new Persona(){Edad=57, Altura=205, Coeficiente=122},
                new Persona(){Edad=32, Altura=167, Coeficiente=103},
                new Persona(){Edad=43, Altura=156, Coeficiente=141},
                new Persona(){Edad=30, Altura=176, Coeficiente=75},
                new Persona(){Edad=20, Altura=180, Coeficiente=141},
                new Persona(){Edad=51, Altura=208, Coeficiente=122},
                new Persona(){Edad=23, Altura=195, Coeficiente=142},
                new Persona(){Edad=63, Altura=183, Coeficiente=120},
                new Persona(){Edad=51, Altura=181, Coeficiente=79},
                new Persona(){Edad=53, Altura=186, Coeficiente=123},
                new Persona(){Edad=31, Altura=160, Coeficiente=99},
                new Persona(){Edad=60, Altura=153, Coeficiente=110},
                new Persona(){Edad=57, Altura=199, Coeficiente=72}
            });
            */
            return lista;
        }
    }
}
