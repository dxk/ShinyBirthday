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

        public static int YIK = 0;

        public ActionResult Index()
        {
            Random random = new Random();
            YIK = Convert.ToInt32(random.Next(0, 3));
            ShinyInfoView siv = new ShinyInfoView(common.GetShiny(), GetCheckImgList()[YIK]);
            return View(siv);
        }

        public ActionResult AddMessage(AddMessageForm message)
        {
            if (ModelState.IsValid)
            {
                string str = GetCheckImgList()[YIK];
                str = str.Substring(0, str.IndexOf('.'));

                if (str.IndexOf('+') > 0)
                {
                    int fi = Convert.ToInt32(str.Substring(0, str.IndexOf('+')));
                    int ei = Convert.ToInt32(str.Substring(str.IndexOf('+') + 1));
                    if ((fi + ei) != Convert.ToInt32(message.YanzhengValue))
                        return Json(null, false);
                }
                else if (str.IndexOf('-') > 0)
                {
                    int fi = Convert.ToInt32(str.Substring(0, str.IndexOf('-')));
                    int ei = Convert.ToInt32(str.Substring(str.IndexOf('-') + 1));
                    if ((fi - ei) != Convert.ToInt32(message.YanzhengValue))
                        return Json(null, false);
                }
                else if (str.IndexOf('X') > 0)
                {
                    int fi = Convert.ToInt32(str.Substring(0, str.IndexOf('*')));
                    int ei = Convert.ToInt32(str.Substring(str.IndexOf('*') + 1));
                    if ((fi * ei) != Convert.ToInt32(message.YanzhengValue))
                        return Json(null, false);
                }
                else if (str.IndexOf('C') > 0)
                {
                    int fi = Convert.ToInt32(str.Substring(0, str.IndexOf('/')));
                    int ei = Convert.ToInt32(str.Substring(str.IndexOf('/') + 1));
                    if ((fi / ei) != Convert.ToInt32(message.YanzhengValue))
                        return Json(null, false);
                }
                messageservice.InsertInto(new Messages
                {
                    Message = message.MessageWords,
                    Friender = message.FriendName
                });

                Random random = new Random();
                YIK = Convert.ToInt32(random.Next(0, 3));
                string str_ = GetCheckImgList()[YIK];
                return Json(str_, true);
            }
            return AjaxJson();
        }

        public ActionResult GetFiveMessage()
        {
            return Json(messageservice.GetFiveMessage());
        }

        public ActionResult ShowShiny()
        {
            return View();
        }

        public List<string> GetCheckImgList()
        {
            List<string> listImgs = new List<string>() { "1+1.png", "2+4.png", "2+8.png", "2X9.png" };
            return listImgs;
        }

    }
}
