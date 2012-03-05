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
using System.Configuration;

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
            List<string> listTops = common.GetTitles().Select(p => p.Message).ToList();
            ShinyInfoView siv = new ShinyInfoView(common.GetShiny(), visitorNum, listTops);
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
                        Friender = message.FriendName,
                        Qq = message.FriendQQ,
                        Truename = message.FriendRelayName,
                        Usable = 1
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

        public ActionResult ShinyImgs()
        {
            string path = Server.MapPath("~/Content/ImgWall/thumbs");
            string imgcounts = ConfigurationManager.AppSettings["ImageCount"];
            ShinyInfoView siv = new ShinyInfoView(common.GetShiny(), Convert.ToInt32(imgcounts), null);
            return View(siv);
        }

        public ActionResult AllMessages(int pageNum, string searchStr)
        {
            int sc = 10;
            int pageNos = 0;
            List<Messages> list = messageservice.GetMessagesByPage(pageNum, sc, out pageNos, searchStr.Trim());
            return View(new AllMessageViewModel()
            {
                ListMessage = list,
                PageNos = pageNos,
                CurrentPagenum = pageNum,
                SearchStr = searchStr
            });
        }

        public ActionResult ShowShiny()
        {
            return View();
        }

        public ActionResult AddTopMessage(AddTopMessage atm)
        {
            if (atm.TopMessage.Length > 16)
                return RedirectToAction("Index");
            TopTitle tt = new TopTitle();
            tt.Message = atm.TopMessage;
            tt.Identity = atm.TopIdentity;
            tt.Leavetime = DateTime.Now;
            tt.Enable = 0;
            tt.Color = "white";

            common.AddTitles(tt);
            return RedirectToAction("Index");
        }

        //-----------
        public ActionResult MyLoveXY(int pageNum)
        {
            int sc = 20;
            int pageNos = 0;
            List<Messages> list = messageservice.GLYGetMessagesByPage(pageNum, sc, out pageNos);
            List<TopTitle> listTops = common.GetAllTitles();
            return View(new AllMessageViewModel()
            {
                ListTops = listTops,
                ListMessage = list,
                PageNos = pageNos,
                CurrentPagenum = pageNum
            });
        }

        public ActionResult DisableMessage(int id, int pageNum)
        {
            messageservice.GLYDelete(id, false);
            return RedirectToAction("MyLoveXY", new { pageNum = pageNum });
        }

        public ActionResult EnableMessage(int id, int pageNum)
        {
            messageservice.GLYDelete(id, true);
            return RedirectToAction("MyLoveXY", new { pageNum = pageNum });
        }

        public ActionResult DisableTopMessage(int id)
        {
            common.GLYDeleteTops(id, false);
            return RedirectToAction("MyLoveXY", new { pageNum = 0 });
        }

        public ActionResult EnableTopMessage(int id)
        {
            common.GLYDeleteTops(id, true);
            return RedirectToAction("MyLoveXY", new { pageNum = 0 });
        }
    }
}
