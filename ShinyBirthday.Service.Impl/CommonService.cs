using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShinyBirthday.Entity;
using NHibernate;
using NHibernate.Linq;
using ShinyBirthday.Service.Session;

namespace ShinyBirthday.Service.Impl
{
    public class CommonService : ICommon
    {
        ISession session = GetSession.getSession();

        public ShinyInformation GetShiny()
        {
            ShinyInformation pig = session.Query<ShinyInformation>().First();
            return pig;
        }
    }
}
