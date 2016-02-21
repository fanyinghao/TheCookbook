using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using FYH.Cookbook.Core.Extensions;

namespace FYH.Cookbook.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            // spring controller
            ControllerBuilder.Current.SetControllerFactory(typeof(SpringControllerFactory));
        }
    }
}
