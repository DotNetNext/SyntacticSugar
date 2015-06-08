using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SyntacticSugar;

namespace Test.Work
{
    public partial class MailSmtpTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            MailSmtp ms = new MailSmtp("smtp.qq.com","1215247044","xx");

            //可选参数
            ms.SetCC("610262374@qq.com");//抄送可以多个
            ms.SetBC("610262374@qq.com");//暗送可以多个
            ms.SetIsHtml(true);//默认:true
            ms.SetEncoding(System.Text.Encoding.UTF8);//设置格式 默认utf-8
            ms.SetIsSSL(true);//是否ssl加密 默认为false

            //调用函数
            bool isSuccess = ms.Send("1215247044@qq.com", "test", "610262374@qq.com", "哈哈", "哈哈");

            //输出结果
            Response.Write(ms.Result);
        }
    }
}