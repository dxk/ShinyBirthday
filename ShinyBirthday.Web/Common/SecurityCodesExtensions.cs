using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace ShinyBirthday.Web.Common
{
    public static class SecurityCodesExtensions
    {
        public static string SecurityCodes(this HtmlHelper htmlHelper, string name)
        {
            return SecurityCodes(htmlHelper, name, null);
        }

        public static string SecurityCodes(this HtmlHelper htmlHelper, string name, object htmlAttributes)
        {
            var contextUrl = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            StringBuilder sb = new StringBuilder(htmlHelper.TextBox(name, null, htmlAttributes).ToHtmlString());

            var img = new TagBuilder("img");
            img.MergeAttribute("src", contextUrl.Action("GetCode", "SecurityCode"));
            img.MergeAttribute("style", "width: 50px; height: 16px; margin: 0px 0px 0px 0px; padding: 0px 0px 0px 0px;position: absolute;");
            img.MergeAttribute("onclick", "javascript:_Click_SecurityCode_GetNewCode_2011_4_6_17_36();");
            img.MergeAttribute("alt", "点击更换:该验证码只有(+)加(-)减(x)乘运算");

            var imgStr = img.ToString(TagRenderMode.SelfClosing);

            var a = new TagBuilder("a");
            a.AddCssClass("change-code");
            a.MergeAttribute("style", "cursor:pointer;font-size:10px;");
            a.MergeAttribute("href", "javascript:_Click_SecurityCode_GetNewCode_2011_4_6_17_36();");
            a.InnerHtml = "";

            var aStr = a.ToString(TagRenderMode.Normal);

            //var result = sb.Append(imgStr).Append(aStr).ToString();

            var result = imgStr+aStr;//sb.Append(imgStr).ToString();

            //System.Web.HttpContext.Current.Response.Write("<script language=javascript>function _Click_SecurityCode_GetNewCode_2011_4_6_17_36() { var img = $(\".change-code\"); var dateMill = new Date().getMilliseconds();img.prev().attr(\"src\", '/SecurityCodes/GetCode' + \"?item=\" + dateMill);}</script>");

            return result;
        }

        public static string TrificImg(this HtmlHelper htmlHelper)
        {
            var contextUrl = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            var img = new TagBuilder("img");
            img.AddCssClass("trigic_Img");
            img.MergeAttribute("src", contextUrl.Action("GetTraffic", "SecurityCode"));
            img.MergeAttribute("style", "width: 50px; height: 16px;");
            img.MergeAttribute("alt", "访问量");
            var imgStr = img.ToString(TagRenderMode.SelfClosing);
            var result = imgStr;
            return result;
        }

    }
}
