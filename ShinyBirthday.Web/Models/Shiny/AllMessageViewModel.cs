using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShinyBirthday.Entity;

namespace ShinyBirthday.Web.Models.Shiny
{
    public class AllMessageViewModel
    {
        public List<Messages> ListMessage { get; set; }

        public List<TopTitle> ListTops { get; set; }

        public int PageNos { get; set; }

        public int CurrentPagenum { get; set; }

        public string SearchStr { get; set; }

    }
}