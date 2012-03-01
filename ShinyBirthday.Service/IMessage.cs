using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShinyBirthday.Entity;
using Youmay;

namespace ShinyBirthday.Service
{
    public interface IMessage
    {
        void InsertInto(Messages message);

        List<NameIdView> GetFiveMessage();

        int GetLiveMessageCount();

        List<Messages> GetMessagesByPage(int pageNum,int count,out int pageNos,string sraechStr);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="bo">True 就是显示/False 就是不显示</param>
        void GLYDelete(int id,bool bo);

        List<Messages> GLYGetMessagesByPage(int pageNum, int count, out int pageNos);
    }
}
