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
            int visitorNum = common.GetVisitorvolume(GetClientIp());
            ShinyInfoView siv = new ShinyInfoView(common.GetShiny(), visitorNum);
            return View(siv);
        }

        /// <summary>
        /// 获取客户端的IP，可以取到代理后的IP
        /// </summary>
        public static string GetClientIp()
        {
            string l_ret = string.Empty;
            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"]))
                l_ret = Convert.ToString(System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]);
            if (string.IsNullOrEmpty(l_ret))
                l_ret = Convert.ToString(System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
            return l_ret;
        }

        public ActionResult AddMessage(AddMessageForm message)
        {
            if (ModelState.IsValid)
            {
                var securityCode = TempData["SecurityCodeKey2011_3_29_17_34"] as string;
                if (string.IsNullOrEmpty(securityCode))
                {
                    //验证码不存在或过期
                    return Json(null, false);
                }
                else
                {
                    //不区分大小写C#+IBatis+MVC3.0+Sql+验证码实例 
                    if (!securityCode.Equals(message.YanzhengValue))
                        //验证码错误
                        return Json(null, false);
                    else
                        TempData["SecurityCodeKey2011_3_29_17_34"] = null;
                }

                try
                {
                    messageservice.InsertInto(new Messages
                    {
                        Message = message.MessageWords,
                        Friender = message.FriendName
                    });
                }
                catch (Exception)
                {
                    return Json(null, false);
                }

                return Json(null, true);
            }
            return AjaxJson();
        }

        public ActionResult GetFiveMessage()
        {
            return Json(messageservice.GetFiveMessage());
        }

        public ActionResult AllLeaveMessages()
        {
            int visitorNum = common.GetVisitorvolume(GetClientIp());
            ShinyInfoView siv = new ShinyInfoView(common.GetShiny(), visitorNum);
            return View(siv);
        }

        public ActionResult ShowShiny()
        {
            return View();
        }
    }
}
