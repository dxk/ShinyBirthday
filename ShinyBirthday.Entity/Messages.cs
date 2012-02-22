using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShinyBirthday.Entity
{
    public class Messages
    {
        public virtual int Id { get; set; }

        public virtual string Message { get; set; }

        public virtual string Friender { get; set; }

        public virtual string Truename { get; set; }

        public virtual string Qq { get; set; }

        public virtual int Usable { get; set; }
    }
}
