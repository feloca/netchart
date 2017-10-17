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
                }
                });
            return View(testList);
        }

        public ActionResult Desa()
        {
            return View();
        }
    }
}