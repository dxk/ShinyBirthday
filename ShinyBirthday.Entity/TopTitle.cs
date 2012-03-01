using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShinyBirthday.Entity
{
    public class TopTitle
    {
        public virtual int Id { get; set; }

        public virtual string Identity { get; set; }

        public virtual string Message { get; set; }

        public virtual DateTime Leavetime { get; set; }

        public virtual string Color { get; set; }

        public virtual int Enable { get; set; }
    }
}
