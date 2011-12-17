using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShinyBirthday.Service.Impl;
using ShinyBirthday.Service;
using ShinyBirthday.Web.Models.Shiny;
using Youmay.Web;
using ShinyBirthday.Entity;

namespace ShinyBirthday.Web.Controllers
{
    public class ShinyController : BaseController
    {
        //
        // GET: /Shiny/

        private ICommon common = new CommonService();
        private IMessage messageservice = new MessageService();

        public ShinyController()
        {

        }

        public ActionResult Index()
        {
            ShinyInfoView siv = new ShinyInfoView(common.GetShiny());
            return View(siv);
        }

        public ActionResult AddMessage(AddMessageForm message)
        {
            if (ModelState.IsValid)
            {
                messageservice.InsertInto(new Messages
                {
                    Message = message.MessageWords
                });
            }
            return AjaxJson();
        }

        public ActionResult GetFiveMessage()
        {
            return Json(messageservice.GetFiveMessage());
        }

    }
}
