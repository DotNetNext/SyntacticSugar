using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SyntacticSugar;

namespace Test.Http
{
    public partial class WebRequestSugarTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            WebRequestSugar ws = new WebRequestSugar();
            //可选参数
            //ws.SetAccept
            //ws.SetContentType
            //ws.SetCookie
            //ws.SetTimeOut
            //ws.SetIsAllowAutoRedirect

            //GET
            var html= ws.HttpGet("http://localhost:24587/Http/HttpTest.aspx");

            //带参GET
            var paras=new Dictionary<string, string>() ;
            paras.Add("name","skx");
            paras.Add("id", "100");
            var html2 = ws.HttpGet("http://localhost:24587/Http/HttpTest.aspx",paras );

            //POST
            var postHtml= ws.HttpPost("http://localhost:24587/Http/HttpTest.aspx", paras);

            //post and file
            var postHtml2 = ws.HttpUploadFile("http://localhost:24587/Http/HttpTest.aspx", "文件地址可以是数组", paras);
        }
    }
}