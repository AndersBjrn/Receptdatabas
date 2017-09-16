using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;
using System;
using System.Reflection;

namespace Recepies
{
    public class DbService
    {
        public static Configuration Configure()
        {
            Configuration cfg = new Configuration()
                           .DataBaseIntegration(db =>
                           {
                               db.ConnectionString = @"Server = (localdb)\mssqllocaldb; Database = Recepies; Trusted_Connection = True;";
                               db.Dialect<MsSql2008Dialect>();
                           });

            var mapper = new ModelMapper();
            Type[] myTypes = Assembly.GetExecutingAssembly().GetExportedTypes();
            mapper.AddMappings(myTypes);

            HbmMapping mapping = new NHibernateMapper().Map();
            cfg.AddMapping(mapping);

            return cfg;
        }

        public static ISession OpenSession()
        {
            if (_sessionFactory == null)
            {
                _sessionFactory = Configure().BuildSessionFactory();
            }
            ISession session = _sessionFactory.OpenSession();
            session.BeginTransaction();
            return session;
        }

        public static void CloseSession(ISession session)
        {
            if (session != null)
            {
                if (session.IsOpen)
                {
                    session.Transaction.Commit();
                    session.Close();
                }
            }
        }

        private static ISessionFactory _sessionFactory;

    }
}