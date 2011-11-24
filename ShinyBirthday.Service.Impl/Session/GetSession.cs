using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Cfg;
using EntityMapping;

namespace ShinyBirthday.Service.Session
{
    public static class GetSession
    {
        public static ISession getSession()
        {
            var nhConfig = new Configuration().Configure();
            var mappingFactory = new MappingFactory();
            var mapper = mappingFactory.CreateMapping();
            nhConfig.AddDeserializedMapping(mapper, null);
            var sessionFactory = nhConfig.BuildSessionFactory();
            ISession _session = sessionFactory.OpenSession();
            return _session;
        }
    }
}
