using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Linq;
using ShinyBirthday.Service.Session;
using ShinyBirthday.Entity;
using Youmay;

namespace ShinyBirthday.Service.Impl
{
    public class MessageService : IMessage
    {
        ISession session = GetSession.getSession();

        public void InsertInto(Messages message)
        {
            session.Save(message);
        }

        public List<NameIdView> GetFiveMessage()
        {
            var query = session.Query<Messages>().ToList();
            return query.Skip(query.Count - 5).Take(5).Select(q =>
                 new NameIdView
                    {
                        Id = q.Id,
                        Name = q.Friender + ":" + q.Message
                    }
                ).ToList<NameIdView>();
        }
    }
}
