using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SyntacticSugar;
namespace Test.Test
{
    public partial class PerformanceTest2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PerformanceTest p = new PerformanceTest();
            p.SetCount(10000*100);//循环次数(默认:1)
            p.SetIsMultithread(false);//是否启动多线程测试 (默认:false)
            p.Execute(
            i =>
            {
                Response.Write(i);
               //
            },
            message =>
            {

                //输出总共运行时间
                Response.Write(message);   //总共执行时间：1.02206秒
             
            }
            );
            Gridview1.DataSource = p.GetChartSource();
            Gridview1.DataBind();
        }
    }
}