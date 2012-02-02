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
            int num = session.Query<Messages>().Where(p => message.Friender.Equals(p.Friender) && message.Message.Equals(p.Message)).Count();
            if (num > 0)
            {
                throw new Exception();
            }
            session.Save(message);
        }

        public List<NameIdView> GetFiveMessage()
        {
            var query = session.Query<Messages>().ToList();
            return query.Skip(query.Count - 9).Take(9).Select(q =>
                 new NameIdView
                    {
                        Id = q.Id,
                        Name = q.Friender + ":" + q.Message
                    }
                ).ToList<NameIdView>();
        }

        public int GetLiveMessageCount()
        {
            return session.Query<Messages>().Count();
        }


        public List<NameIdView> GetMessagesByPage(int pageNum, int count)
        {
            var query = session.Query<Messages>().ToList();
            return query.Skip(pageNum * count).Take(count).Select(q =>
                    new NameIdView
                    {
                        Id = q.Id,
                        Name = q.Friender + ":" + q.Message
                    }
                ).ToList<NameIdView>();
        }
    }
}
