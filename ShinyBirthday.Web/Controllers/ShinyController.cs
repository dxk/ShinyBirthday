using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShinyBirthday.Service.Impl;
using ShinyBirthday.Service;

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
            common.GetShiny();
            return View();
        }

    }
}
