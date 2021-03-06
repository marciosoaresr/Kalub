using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using NHibernate.Tool.hbm2ddl;
using Configuration = NHibernate.Cfg.Configuration;

namespace ProfitManager.Negocio.Repositorio
{
    public class SessionFactory
    {
        private static ISessionFactory _sessionFactory;

        private static ISession GetCurrentSession()
        {
            return _sessionFactory.GetCurrentSession();
        }
        public static ISessionFactory CreateSessionFactory()
        {
            if (_sessionFactory != null) return _sessionFactory;

            Configuration conf = new Configuration();

            conf.Configure();
            conf.CurrentSessionContext<WebSessionContext>();

            string strAssemblyDTO = "ProfitManager.Fabrica"; //ConfigurationManager.AppSettings["AssemblyMapping"];
            if (string.IsNullOrWhiteSpace(strAssemblyDTO)) throw new Exception("Não foi informado a configuração AssemblyMapping");

            string[] arrAssemblyDTO = strAssemblyDTO.Split(';');
            foreach (var s in arrAssemblyDTO)
            {
                conf.AddAssembly(s);
            }

            //PROXY CLASS
            //conf.SetProperty("proxyfactory.factory_class", "NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle");
            conf.SetProperty("proxyfactory.factory_class", "NHibernate.Bytecode.DefaultProxyFactoryFactory, NHibernate");
            //conf.SetProperty("default_schema", "esolution");
            conf.SetProperty("adonet.batch_size", "1");

            ISessionFactory sessionFactory = conf.BuildSessionFactory();

            //RepositoryLogClassStructure.Load(sessionFactory);

            if (!Directory.Exists(@"c:\eSolution\Logs\KALUB\")) Directory.CreateDirectory(@"c:\eSolution\Logs\KALUB\");
            string path = @"c:\eSolution\Logs\KALUB\nHibernateDatabase.txt";
            Action<string> updateExport = x =>
            {
                using (var file = new FileStream(path, FileMode.Append))
                using (var sw = new StreamWriter(file))
                {
                    sw.WriteLine(x + ";");
                    sw.Close();
                }
            };

            string atualizarDatabase = ConfigurationManager.AppSettings["AtualizarDatabase"];
            if (!string.IsNullOrWhiteSpace(atualizarDatabase) && atualizarDatabase == "S")
            {
                SchemaUpdate su = new SchemaUpdate(conf);
                //su.Execute(true, true);
                su.Execute(updateExport, true);
            }

            string exibirQueryOutput = ConfigurationManager.AppSettings["ExibirQueryOutput"];
            if (!string.IsNullOrWhiteSpace(exibirQueryOutput) && exibirQueryOutput == "S")
            {
                //XmlConfigurator.Configure();
            }

            _sessionFactory = sessionFactory;
            return sessionFactory;
        }

        public static Repositorio CreateRepositorio()
        {
            Repositorio r = new Repositorio();
            r.Session = GetCurrentSession();
            return r;
        }


    }
}
