using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShinyBirthday.Entity
{
    public class IpAddress
    {
        public virtual int Id { get; set; }

        public virtual string Ipstring { get; set; }

        public virtual DateTime Iptime { get; set; }
    }
}
