using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Drawing.Imaging;
using System.Text;
using ShinyBirthday.Service;
using ShinyBirthday.Service.Impl;

namespace ShinyBirthday.Web.Controllers
{
    public class SecurityCodesController : Controller
    {
        //
        // GET: /SecurityCode/
        private IMessage messageservice = new MessageService();

        [OutputCache(Location = OutputCacheLocation.None, Duration = 0, NoStore = false)]
        public ActionResult GetCode()
        {
            string code = CreateRandomCodeCalculateNumber();
            return File(CreateValidateGraphic(code), "image/Jpeg");
        }

        [OutputCache(Location = OutputCacheLocation.None, Duration = 0, NoStore = false)]
        public ActionResult GetTraffic()
        {
            string messageCount = messageservice.GetLiveMessageCount().ToString();
            return File(CreateCountGraphic(messageCount), "image/Jpeg");
        }

        public byte[] CreateCountGraphic(string validateCode)
        {
            Double multiple = 0.0;
            int validateLength = 0;
            string validateText = "";
            multiple = 10;
            validateText = validateCode;
            validateLength = validateText.Length + 2;

            Bitmap image = new Bitmap((int)Math.Ceiling(validateLength * multiple) + 10, 22);
            Graphics g = Graphics.FromImage(image);
            try
            {
                //生成随机生成器
                Random random = new Random();
                //清空图片背景色
                g.Clear(Color.FromArgb(174,168,146));
                //画图片的干扰线
                for (int i = 0; i < 1; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }
                Font font = new Font("Arial", 16, (FontStyle.Bold | FontStyle.Italic));
                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height),
                 Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(validateText, font, brush, 3, 2);
                //画图片的前景干扰点
                for (int i = 0; i < 1; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }
                //画图片的边框线
                g.DrawRectangle(new Pen(Color.FromArgb(174, 168, 146)), 0, 0, image.Width - 1, image.Height - 1);
                //保存图片数据
                MemoryStream stream = new MemoryStream();
                image.Save(stream, ImageFormat.Jpeg);
                //输出图片流
                return stream.ToArray();
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }

        /// <summary>
        /// 根据内容创建图片
        /// </summary>
        /// <param name="containsPage">要输出到的page对象</param>
        /// <param name="validateNum">验证码</param>
        /// <param name="type">验证码类型'1'为字母数字、'2'为汉字、'3'为一汉三字</param>
        public byte[] CreateValidateGraphic(string validateCode)
        {
            Double multiple = 0.0;
            int validateLength = 0;
            string validateText = "";
            if (validateCode.Contains("_Mix_"))
            {
                string str = validateCode.Substring(validateCode.IndexOf("_Mix_") + 5);
                int n = Int32.Parse(str.Substring(0, str.IndexOf("_")));
                int m = Int32.Parse(str.Substring(str.IndexOf("_") + 1));
                multiple = (n * 12.0 + m * 20.0) / (n + m);
                validateLength = m + n;
                validateText = validateCode.Substring(0, validateCode.IndexOf("_Mix_"));
            }
            else
            {
                multiple = validateCode.Contains("_0_") ? 12.0 : validateCode.Contains("_1_") ? 12.0 : (validateCode.Contains("_2_") ? 20.0 : 15.0);
                validateText = validateCode.Substring(0, validateCode.IndexOf("_0_"));
                validateLength = validateText.Length;
            }
            TempData["SecurityCodeKey2011_3_29_17_34"] = validateCode.Contains("_0_") ? validateCode.Substring(validateCode.IndexOf("_0_") + 3) : validateText;
            Bitmap image = new Bitmap((int)Math.Ceiling(validateLength * multiple) + 10, 22);
            Graphics g = Graphics.FromImage(image);
            try
            {
                //生成随机生成器
                Random random = new Random();
                //清空图片背景色
                g.Clear(Color.White);
                //画图片的干扰线
                for (int i = 0; i < 25; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }
                Font font = new Font("Arial", 16, (FontStyle.Bold | FontStyle.Italic));
                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height),
                 Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(validateText, font, brush, 3, 2);
                //画图片的前景干扰点
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }
                //画图片的边框线
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                //保存图片数据
                MemoryStream stream = new MemoryStream();
                image.Save(stream, ImageFormat.Jpeg);
                //输出图片流
                return stream.ToArray();
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }

        /// <summary>
        /// 随机创建图片内容(数字运算)
        /// </summary>
        /// <param name="codeCount"></param>
        /// <returns></returns>
        private string CreateRandomCodeCalculateNumber()
        {
            string allChar = "+,-,x";
            string[] allCharArray = allChar.Split(',');
            string Calculate = "";
            string Number1 = "";
            string Number2 = "";

            Random rand = new Random();
            rand = new Random(1 + (int)DateTime.Now.Ticks);
            int t = rand.Next(3);
            Calculate = allCharArray[t];

            for (int i = 0; i < 2; i++)
            {
                rand = new Random(2 + i + (int)DateTime.Now.Ticks);
                Number1 += rand.Next(9).ToString();
            }

            rand = new Random(5 + (int)DateTime.Now.Ticks);
            Number2 = rand.Next(9).ToString();

            string str = "   " + Number1 + " " + Calculate + " " + Number2;
            int result = 0;
            if ("+".Equals(Calculate))
                result = Convert.ToInt32(Number1) + Convert.ToInt32(Number2);
            else if ("-".Equals(Calculate))
                result = Convert.ToInt32(Number1) - Convert.ToInt32(Number2);
            else if ("x".Equals(Calculate))
                result = Convert.ToInt32(Number1) * Convert.ToInt32(Number2);
            return str + "_0_" + result.ToString();
        }


        /// <summary>
        /// 随机创建图片内容(数字加字母)
        /// </summary>
        /// <param name="codeCount"></param>
        /// <returns></returns>
        private string CreateRandomCode(int codeCount)
        {
            string allChar = "0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,W,X,Y,Z";
            string[] allCharArray = allChar.Split(',');
            string randomCode = "";
            int temp = -1;

            Random rand = new Random();
            for (int i = 0; i < codeCount; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * ((int)DateTime.Now.Ticks));
                }
                int t = rand.Next(35);
                if (temp == t)
                {
                    return CreateRandomCode(codeCount);
                }
                temp = t;
                randomCode += allCharArray[t];
            }
            return randomCode + "_1_";
        }

        /// <summary>
        /// 创建4个随机的汉字(纯随机汉字)
        /// </summary>
        /// <param name="strlength"></param>
        /// <returns></returns>
        private string CreateRegionCode(int strlength)
        {
            //定义一个字符串数组储存汉字编码的组成元素
            string[] rBase = new String[16] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f" };

            Random rnd = new Random();

            //定义一个object数组用来
            object[] bytes = new object[strlength];
            //定一个string类型的返回值
            string str = "";
            /**/
            /*每循环一次产生一个含两个元素的十六进制字节数组，并将其放入bject数组中
             每个汉字有四个区位码组成
             区位码第1位和区位码第2位作为字节数组第一个元素
             区位码第3位和区位码第4位作为字节数组第二个元素
             */
            for (int i = 0; i < strlength; i++)
            {
                //区位码第1位
                int r1 = rnd.Next(11, 14);
                string str_r1 = rBase[r1].Trim();
                //区位码第2位
                rnd = new Random(r1 * unchecked((int)DateTime.Now.Ticks) + i);//更换随机数发生器的种子避免产生重复值
                int r2;
                if (r1 == 13)
                {
                    r2 = rnd.Next(0, 7);
                }
                else
                {
                    r2 = rnd.Next(0, 16);
                }
                string str_r2 = rBase[r2].Trim();
                //区位码第3位
                rnd = new Random(r2 * unchecked((int)DateTime.Now.Ticks) + i);
                int r3 = rnd.Next(10, 16);
                string str_r3 = rBase[r3].Trim();
                //区位码第4位
                rnd = new Random(r3 * unchecked((int)DateTime.Now.Ticks) + i);
                int r4;
                if (r3 == 10)
                {
                    r4 = rnd.Next(1, 16);
                }
                else if (r3 == 15)
                {
                    r4 = rnd.Next(0, 15);
                }
                else
                {
                    r4 = rnd.Next(0, 16);
                }
                string str_r4 = rBase[r4].Trim();
                //定义两个字节变量存储产生的随机汉字区位码
                byte byte1 = Convert.ToByte(str_r1 + str_r2, 16);
                byte byte2 = Convert.ToByte(str_r3 + str_r4, 16);
                //将两个字节变量存储在字节数组中
                byte[] str_r = new byte[] { byte1, byte2 };
                //将产生的一个汉字的字节数组放入object数组中
                bytes.SetValue(str_r, i);
                //获取GB2312编码页（表）
                Encoding gb = Encoding.GetEncoding("gb2312");
                str += gb.GetString((byte[])Convert.ChangeType(bytes[i], typeof(byte[])));
            }
            return str + "_2_";
        }

        /// <summary>
        /// 创建N个数字字母 和 M个汉字
        /// </summary>
        /// <param name="mixlength_1">N个数字字母</param>
        /// <param name="mixlength_2">M个汉字</param>
        /// <returns>返回混合型字符串</returns>
        private string CreateMixCode(int N, int M)
        {
            return CreateRandomCode(N).Substring(0, N) + CreateRegionCode(M).Substring(0, M) + "_Mix_" + N + "_" + M;
        }
    }
}
