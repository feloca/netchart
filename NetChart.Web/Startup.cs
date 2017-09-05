using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NetChart.Web.Startup))]
namespace NetChart.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
