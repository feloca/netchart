﻿using NetChart.GeneradorDatos;
using NetChart.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        public ActionResult TestBar()
        {
            var datos = Generator.GenerarPersonas();
            return View();
        }

        public ActionResult TestLine()
        {
            return View();
        }

        public ActionResult TestScatter()
        {
            return View();
        }

        public ActionResult TestBubble()
        {
            return View();
        }

        public ActionResult TestPie()
        {
            return View();
        }

        public ActionResult TestRadar()
        {
            return View();
        }
    }
}