using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FYH.Cookbook.Web.Startup))]
namespace FYH.Cookbook.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
