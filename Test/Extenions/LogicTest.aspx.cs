using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SyntacticSugar;
namespace Test.Extenions
{
    public partial class LogicTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            //【Switch】
            string bookKey = "c#";

            //以前写法
            string myBook = "";
            switch (bookKey)
            {
                case "c#":
                    myBook = "asp.net技术";
                    break;
                case "java":
                    myBook = "java技术";
                    break;
                case "sql":
                    myBook = "mssql技术";
                    break;
                default:
                    myBook = "要饭技术";
                    break;//打这么多break和封号,手痛吗?
            }

            //现在写法 
            myBook =bookKey.Switch().Case("c#", "asp.net技术").Case("java", "java技术").Case("sql", "sql技术").Default("要饭技术").Break();//点的爽啊




            /**
                  C#类里看不出效果，  在mvc razor里   ? 、&& 或者 || 直接使用都会报错,需要外面加一个括号，嵌套多了很不美观，使用自定义扩展函数就没有这种问题了。
           
             */

            bool isSuccess = true;

            //【IIF】
            //以前写法
            var trueVal1 = isSuccess ? 100 :0;
            //现在写法
            var trueVal2 = isSuccess.IIF(100);

            //以前写法
            var str = isSuccess ? "我是true" : "";
            //现在写法
            var str2 = isSuccess.IIF("我是true");


            //以前写法
            var trueVal3 = isSuccess ? 1 : 2;
            //现在写法
            var trueVal4 = isSuccess.IIF(1, 2);



            string id = "";
            string id2 = "";

            //以前写法 
            isSuccess = (id == id2) && (id != null && Convert.ToInt32(id) > 0);
            //现在写法
            isSuccess = (id == id2).And(id != null, Convert.ToInt32(id) > 0);

            //以前写法
            isSuccess = id != null || id != id2;
            //现在写法
            isSuccess = (id != null).Or(id != id2);


        }
    }
}