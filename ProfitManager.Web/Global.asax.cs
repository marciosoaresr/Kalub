using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using NHibernate;
using NHibernate.Context;
using ProfitManager.Negocio.Implementacao;
using ProfitManager.Web.App_Start;

namespace ProfitManager.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {

        private static ISessionFactory SessionFactory;

        public override void Init()
        {
            base.Init();

            this.BeginRequest += OnBeginRequest;
            this.EndRequest += OnEndRequest;
        }

        private void OnEndRequest(object sender, EventArgs eventArgs)
        {
            var session = CurrentSessionContext.Unbind(SessionFactory);
            session.Dispose();
        }

        private void OnBeginRequest(object sender, EventArgs eventArgs)
        {
            var session = SessionFactory.OpenSession();
            CurrentSessionContext.Bind(session);
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            SessionFactory = ProfitManager.Negocio.Repositorio.SessionFactory.CreateSessionFactory();

        }

    }
}