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
            return session.Query<ShinyInformation>().FirstOrDefault();
        }

        public List<TopTitle> GetTitles()
        {
            return session.Query<TopTitle>().Where(p => p.Enable == 1).ToList();
        }

        public List<TopTitle> GetAllTitles()
        {
            return session.Query<TopTitle>().ToList();
        }

        public void AddTitles(TopTitle tt)
        {
            session.Save(tt);
            session.Flush();
        }

        public void GLYDeleteTops(int id, bool yn)
        {
            TopTitle message = session.Get<TopTitle>(id);
            if (message != null)
            {
                if (yn)
                    message.Enable = 1;
                else
                    message.Enable = 0;
                session.SaveOrUpdate(message);
                session.Flush();
            }
        }

        public int GetVisitorvolume(string ip)
        {
            SystemTable st = session.Query<SystemTable>().ToList().Last();
            if (CheckTimeIp(ip))
            {
                st.Visitorvolume = st.Visitorvolume + 1;
                if (st.Vtime < DateTime.Now.AddHours(-1))
                {
                    SystemTable stnew = new SystemTable();
                    stnew.Visitorvolume = st.Visitorvolume;
                    stnew.Vtime = DateTime.Now;
                    session.Save(stnew);
                    session.Flush();
                }
                else
                {
                    session.SaveOrUpdate(st);
                    session.Flush();
                }
            }
            return st.Visitorvolume;
        }

        public bool CheckTimeIp(string ip)
        {
            IpAddress ipadd = session.Query<IpAddress>().Where(p => p.Ipstring.Equals(ip)).FirstOrDefault();
            if (ipadd == null)
            {
                IpAddress ianew = new IpAddress();
                ianew.Ipstring = ip;
                ianew.Iptime = DateTime.Now;
                session.Save(ianew);
                session.Flush();
                return true;
            }
            else
            {
                if (ipadd.Iptime < DateTime.Now.AddMinutes(-5))
                {
                    ipadd.Iptime = DateTime.Now;
                    session.SaveOrUpdate(ipadd);
                    session.Flush();
                    return true;
                }
                return false;
            }
        }

        public void AddIpaddress(string ip)
        {
            int ia = session.Query<IpAddress>().Where(p => p.Ipstring.Equals(ip)).Count();
            if (ia > 0)
            {
                return;
            }
            else
            {
                IpAddress ianew = new IpAddress();
                ianew.Ipstring = ip;
                ianew.Iptime = DateTime.Now;
                session.Save(ianew);
                session.Flush();
            }
        }
    }
}
