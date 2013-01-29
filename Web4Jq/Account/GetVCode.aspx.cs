using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

public partial class Account_GetVCode : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Create_Image_check.DrawImage();
    }

    public class Create_Image_check
    {
        public static void DrawImage()
        {
            //string chineseletter = "一是在不有人这中为上个国我以要他时烂们生到作地于出就分对成可主发年动同工也能下过子产种面而方后多定学法所民经十三之进等部家电力里如水化高自二理起小物现实加量两体制机当使点业本去把性好应开它合因由其些然前外天政四日社义事平形相全表间样与关各新线内数正心反你明看原又利比或但质气第向道命此变条只结问意建月公无军很情者最立代想已通并提直题党程展五果料象员革位入常文总次品式活设及管特件求老头基资边流路级少图山统接知较将组见计别她手期根论运农指几九区强放决西被干做必战先回则任取据处队南光门即保治北造百规热领七海口东导器压志世金增争济阶油思术极交受联认六共权收证改清己美再采转更风切打白教速花带安场身车真务具万每目至达走积示议声报斗完类八离华名确才科张信马节话米整空元况今集温土许步群广记需段研界拉林律叫且究观越织装影算低音众书布复容儿须际商非验连断深难近矿千周委素技备半办青列习响约支般史感劳团往酸历市克何除消构府太准精值号率族维划选标写存候毛亲快效斯院查江型眼王按格养置派层片始却专厂京适属圆包火住满县局照细引听该铁价严";
            string chineseletter = "ABCDEFGHIJKLMNOPQRSTUVWXYZ123456789abdefghijmnqrty";

            Create_Image_check img = new Create_Image_check();
            string strCheckCode = String.Empty;
            Random rand = new Random();
            int strlen = chineseletter.Length - 1;
            int r = rand.Next(strlen);
            int radomlength = 4;

            for (int i = 0; i < radomlength; i++)
            {
                r = rand.Next(strlen);
                strCheckCode += chineseletter.Substring(r, 1);
            }

            img.CreateImage(strCheckCode);
        }

        ///  <summary>
        ///  创建随机码图片
        ///  </summary>
        ///  <param  name="randomcode">随机码</param>
        private void CreateImage(string randomcode)
        {
            int randAngle = 45; //随机转动角度
            int mapwidth = 75;

            //数字字母类型的验证码
            //if (Comm.Filter.GetRequestStr("codetype") == "1")
            //{
            //    mapwidth = 75;
            //}
            Bitmap map = new Bitmap(mapwidth, 30);//创建图片背景
            Graphics graph = Graphics.FromImage(map);
            graph.Clear(Color.FromArgb(240, 243, 248));//清除画面，填充背景

            Random rand = new Random();

            Point[] points = new Point[10]; //定义曲线转折点
            for (int i = 0; i < 10; i++)
            {
                int rnum = rand.Next(8);
                int iWidth = 21;
                if (i == 0)
                {
                    points[i].X = 0;
                    points[i].Y = rand.Next(10, 12);
                }
                else if (i == 10)
                {
                    points[i].X = 75;
                    points[i].Y = rand.Next(35);
                }
                else
                {
                    points[i].X = (i + 1) * iWidth - rnum;
                    int rums = rand.Next(35);
                    if (rums < 10)
                    {
                        rums += 5;
                    }
                    else if (rums > 25)
                    {
                        rums = rums - 4;

                    }
                    points[i].Y = rums;
                }
            }
            Pen pen = new Pen(Color.Black, 2);
            graph.DrawCurve(pen, points, 0.6f);
            //qq是很多线连接一起的
            //验证码旋转，防止机器识别
            char[] chars = randomcode.ToCharArray();//拆散字符串成单字符数组

            //文字距中
            StringFormat format = new StringFormat(StringFormatFlags.NoClip);
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            //定义颜色
            Color[] c = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };
            //定义字体
            string[] font = { "Verdana", "Microsoft Sans Serif", "Basemic Symbol", "Arial", "Times New Roman", "GungsuhChe" };
            //string[] font = {"Times New Roman"};
            int cindex1 = rand.Next(7);
            int findex = rand.Next(6);
            Brush b = new System.Drawing.SolidBrush(Color.Black);
            for (int i = 0; i < chars.Length; i++)
            {
                int cindex = rand.Next(7);
                Font f = new System.Drawing.Font(font[findex], 18, System.Drawing.FontStyle.Bold);//字体样式(参数2为字体大小)

                Point dot = new Point(10, 10);

                if (chars[i] >= 'a' && chars[i] <= 'z')
                {
                    dot = new Point(8, 8);
                }


                //graph.DrawString(dot.X.ToString(),fontstyle,new SolidBrush(Color.Black),10,150);//测试X坐标显示间距的
                float angle = rand.Next(-randAngle, randAngle);//转动的度数

                graph.TranslateTransform(dot.X, dot.Y);//移动光标到指定位置
                graph.RotateTransform(angle);
                graph.DrawString(chars[i].ToString(), f, b, 1, 1, format);
                graph.RotateTransform(-angle);//转回去
                graph.TranslateTransform(2, -dot.Y);//移动光标到指定位置
            }

            //生成图片
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            map.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            //			HttpContext.Current.Response.ClearContent();

            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ContentType = "image/Jpeg";
            HttpContext.Current.Response.BinaryWrite(ms.ToArray());
            graph.Dispose();
            map.Dispose();
            //AppCode.Cookie.SetCookie(HttpUtility.UrlEncode(randomcode, System.Text.Encoding.GetEncoding(936)), "imgCode");
            HttpResponse response = HttpContext.Current.Response;
            response.Cookies["imgCode"].Value = randomcode;  
        }
    }
}

