using System;
using System.Web.Routing;
using Xunit;

namespace Deerso.TestHelpers
{
    public static class RouterMixins
    {
        public static RouteData GetDeersoRouteData(this IHelpTestDeerso This, string requestUrl, Action<RouteCollection> registerRoutes )
        {
            var context = new StubHttpContextForRouting(requestUrl: requestUrl);
            var routes = new RouteCollection();
            registerRoutes(routes);
            return routes.GetRouteData(context);
        }
        public static void AssertController(this IHelpTestDeerso This, RouteData routeData, Type controllerType)
        {
            var controllerName = controllerType.Name.Replace("Controller", "");
            Assert.Equal(controllerName, routeData.Values["controller"]);
        }
    }
}