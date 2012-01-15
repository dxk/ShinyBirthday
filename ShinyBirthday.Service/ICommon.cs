using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShinyBirthday.Entity;

namespace ShinyBirthday.Service
{
    public interface ICommon
    {
        ShinyInformation GetShiny();

        int GetVisitorvolume(string ip);

    }
}
