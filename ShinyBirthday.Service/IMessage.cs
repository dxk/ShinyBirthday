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
    }
}
