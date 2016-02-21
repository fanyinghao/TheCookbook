using System;
using System.Web.Mvc;
using System.Web.Routing;
using Spring.Context.Support;

namespace FYH.Cookbook.Core.Extensions
{
    public class SpringControllerFactory : DefaultControllerFactory
    {
        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            if (requestContext == null)
                throw new ArgumentNullException("requestContext");
            if (string.IsNullOrEmpty(controllerName))
                throw new ArgumentNullException("controllerName");

            var applicationContext = ContextRegistry.GetContext();
            var setName = (controllerName + "Controller").ToLower();

            if (applicationContext.ContainsObject(setName))
                return applicationContext.GetObject(setName) as IController;
            else
                return base.CreateController(requestContext, controllerName);
        }
    }
}
