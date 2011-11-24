using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShinyBirthday.Service.Impl;
using ShinyBirthday.Service;
using ShinyBirthday.Web.Models.Shiny;

namespace ShinyBirthday.Web.Controllers
{
    public class ShinyController : Controller
    {
        //
        // GET: /Shiny/

        private ICommon common = new Common();

        public ShinyController()
        {

        }

        public ActionResult Index()
        {
            ShinyInfoView siv = new ShinyInfoView(common.GetShiny());
            return View(siv);
        }

    }
}
