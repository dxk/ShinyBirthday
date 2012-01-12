using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShinyBirthday.Entity
{
    public class SystemTable
    {
        public virtual int Id { get; set; }

        public virtual int Visitorvolume { get; set; }

        public virtual DateTime Vtime { get; set; }
    }
}
