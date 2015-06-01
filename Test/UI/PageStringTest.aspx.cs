using SyntacticSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Test.UI
{
    public partial class PageStringTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var ps=new PageString();



            /*可选参数*/

            ps.SetIsEnglish(true);// 是否是英文       (默认：false)
            ps.SetIsShowText(true);//是否显示分页文字 (默认：true)
            //ps.SetTextFormat                       (默认值：《span class=\"pagetext\"》《strong》总共《/strong》:{0} 条 《strong》当前《/strong》:{1}/{2}《/span》)
            //ps.SetPageIndexName  Request["pageIndex"](默认值："pageIndex")
            ps.SetIsAjax(false);//                    (默认值："false")

            /*函数参数*/
            int total = 10000;
            int pageSize = 10;
            int pageIndex = Convert.ToInt32(Request["pageIndex"]);

            var page = ps.ToString(total, pageSize, pageIndex, "/UI/PageStringTest.aspx?");

            //获取 page html 输出
           Response.Write(page);

      
        }
    }
}