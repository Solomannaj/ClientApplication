using ClientApplication.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Unity;
using Unity.AspNet.Mvc;

namespace ClientApplication
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Bootstrapper.Initialise();
        }

        public class Bootstrapper
        {

            public static void Initialise()
            {
                var container = new UnityContainer();

                container.RegisterType<IAPIHandler, APIHandler>();
                container.RegisterType<IDataSubscriber, DataSubscriber>();

                DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            }

        }
    }
}
