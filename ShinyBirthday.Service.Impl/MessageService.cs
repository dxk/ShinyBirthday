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

        #region 非管理员
        public void InsertInto(Messages message)
        {
            int num = session.Query<Messages>().Where(p => message.Friender.Equals(p.Friender) && message.Message.Equals(p.Message)).Count();
            if (num > 0)
            {
                throw new Exception();
            }
            message.Leavetime = DateTime.Now;
            session.Save(message);
        }

        public List<NameIdView> GetFiveMessage()
        {
            var query = session.Query<Messages>().Where(p => p.Usable == 1).ToList();
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
            return session.Query<Messages>().Where(p => p.Usable == 1).Count();
        }

        public List<Messages> GetMessagesByPage(int pageNum, int count, out int pageNos, string sraechStr)
        {
            var _query = session.Query<Messages>().Where(p => p.Usable == 1);
            if (!string.IsNullOrEmpty(sraechStr) && sraechStr!="")
                _query = _query.Where(q => q.Friender.Contains(sraechStr) || q.Message.Contains(sraechStr));
            var query = _query.ToList();
            if (query.Count % count > 0)
                pageNos = query.Count / count + 1;
            else
                pageNos = query.Count / count;
            return query.OrderByDescending(k => k.Leavetime).Skip(pageNum * count).Take(count).Select(q =>
                    new Messages
                    {
                        Id = q.Id,
                        Message = q.Message,
                        //Qq = q.Qq,
                        //Truename = q.Truename,
                        Friender = q.Friender,
                        Leavetime = q.Leavetime
                    }
                ).ToList<Messages>();
        }
        #endregion

        #region 管理员
        public void GLYDelete(int id, bool bo)
        {
            Messages message = session.Get<Messages>(id);
            if (message != null)
            {
                if (bo)
                    message.Usable = 1;
                else
                    message.Usable = 0;
                session.SaveOrUpdate(message);
                session.Flush();
            }
        }

        public List<Messages> GLYGetMessagesByPage(int pageNum, int count, out int pageNos)
        {
            var query = session.Query<Messages>().ToList();
            if (query.Count % count > 0)
                pageNos = query.Count / count + 1;
            else
                pageNos = query.Count / count;
            return query.Skip(pageNum * count).Take(count).Select(q =>
                    new Messages
                    {
                        Id = q.Id,
                        Message = q.Message,
                        Qq = q.Qq,
                        Truename = q.Truename,
                        Friender = q.Friender,
                        Leavetime = q.Leavetime,
                        Usable = q.Usable
                    }
                ).ToList<Messages>();
        }
        #endregion
    }
}
