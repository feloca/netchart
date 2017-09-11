﻿using NetChart.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChart.GeneradorDatos
{
    class Program
    {
        public const int NumeroDatos = 50;
        public const int Semilla = 1234;

        static void Main(string[] args)
        {
            var listaPersonas = GenerarPersonas();
            //var salidaObjetos = GenerarDatosPersonas();
            var salidaCsv = GenerarCSVPersonas(listaPersonas);
            System.Console.WriteLine("Pulsa intro para terminar");
            System.Console.ReadLine();
        }

        private static Random rnd = new Random(Semilla);

        private static string GenerarDatosPersonas()
        {
            string mascara = "new Persona(){0}Edad={2}, Altura={3}, Coeficiente={4}{1}, ";
            var rnd = new Random();
            var sbPersonas = new StringBuilder();

            for (int i = 0; i < NumeroDatos; ++i)
            {
                sbPersonas.AppendLine(string.Format(mascara, "{", "}", rnd.Next(15, 65), rnd.Next(140, 210), rnd.Next(70, 150)));
            }
            
            sbPersonas = sbPersonas.Remove(sbPersonas.Length - 4, 4);

            
            return sbPersonas.ToString();
        }

        private static List<Persona> GenerarPersonas()
        {
            var resultados = new List<Persona>();            

            var nacionalidades = new string[] { "alemana", "portuguesa", "francesa", "italiana" };
            var ocupaciones = new string[] { "medico", "secretario", "estudiante", "repartidor", "empresario" };
            var estudios = new string[] { "primaria", "secundaria", "fp", "diplomado", "doctor" };
            var ingresos = new string[] { "bajos", "medios", "altos", "muy_altos" };

            for (int i = 0; i < NumeroDatos; ++i)
            {
                var persona = new Persona()
                {
                    PersonaId = i + 1,
                    Edad = rnd.Next(15, 80),
                    Altura = rnd.Next(145, 215),
                    Peso = (float)GetRandomFloat(65.0, 125.0),
                    Tension = (float)GetRandomFloat(6.0, 14.0),
                    Nacionalidad = nacionalidades[rnd.Next(0, nacionalidades.Length)],
                    Ocupacion = ocupaciones[rnd.Next(0, ocupaciones.Length)],
                    Estudios = estudios[rnd.Next(0, estudios.Length)],
                    Ingresos = ingresos[rnd.Next(0, ingresos.Length)]
                };
                resultados.Add(persona);
            }

            return resultados;
        }

        private static string GenerarCSVPersonas(List<Persona> lista)
        {
            //aqui hay que montar un csv
            var sbCsv = new StringBuilder();
            sbCsv.AppendLine("PersonaID,Edad,Altura,Peso,Tension,Nacionalidad,Ocupacion,Estudios,Ingresos");
            string mascara = "{0},{1},{2},{3},{4},{5},{6},{7},{8}";
            for(int i = 0; i < lista.Count; ++i)
            {
                sbCsv.AppendLine(string.Format(
                    mascara, lista[i].PersonaId, lista[i].Edad, lista[i].Altura, 
                    lista[i].Peso.ToString().Replace(",","."), 
                    lista[i].Tension.ToString().Replace(",", "."), 
                    lista[i].Nacionalidad, lista[i].Ocupacion, lista[i].Estudios, 
                    lista[i].Ingresos
                    ));
            }

            return sbCsv.ToString();
        }

        public static double GetRandomFloat(double minimum, double maximum)
        {        
            var numeroAleatorio = rnd.NextDouble();
            var compuesto = numeroAleatorio * (maximum - minimum) + minimum;
            return Math.Round(compuesto, 1);
        }
    }
}
