using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChart.GeneradorDatos
{
    class Program
    {
        static void Main(string[] args)
        {
            var salida = GenerarDatosPersonas();

            Console.WriteLine("Pulsa intro para terminar");
            Console.ReadLine();
        }

        private static string GenerarDatosPersonas()
        {
            string mascara = "new Persona(){0}Edad={2}, Altura={3}, Coeficiente={4}{1}, ";
            var rnd = new Random();
            var sbPersonas = new StringBuilder();

            for (int i = 0; i < 50; ++i)
            {
                sbPersonas.AppendLine(string.Format(mascara, "{", "}", rnd.Next(15, 65), rnd.Next(140, 210), rnd.Next(70, 150)));
            }
            
            sbPersonas = sbPersonas.Remove(sbPersonas.Length - 4, 4);

            
            return sbPersonas.ToString();
        }
    }
}
