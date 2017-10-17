
using System.ComponentModel.DataAnnotations;

namespace NetChart.Web.ViewModels
{
    //Esta clase permite dar formato al catálogo de pruebas
    public class TestVM
    {
        //[Key]        
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "Método")]
        public string Method { get; set; }
    }
}