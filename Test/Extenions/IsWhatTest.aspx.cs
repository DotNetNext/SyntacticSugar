using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SyntacticSugar;
namespace Test.Extenions
{
    public partial class IsWhatTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            /***扩展函数名细***/

            //【IsInRange】  
            int num = 100;
            //以前写法
            if (num > 100 & num < 1000) { }
            //现在写法
            if (num.IsInRange(100, 1000)) { } //datetime类型也支持



            //【IsNullOrEmpty】
            object s = "";
            //以前写法
            if (s == null || string.IsNullOrEmpty(s.ToString())) { }
            //现在写法
            if (s.IsNullOrEmpty()) { }
            //更顺手了没有 }


            //【IsIn】
            string value = "a";
            //以前写法我在很多项目中看到
            if (value == "a" || value == "b" || value == "c") { 
            }
            //现在写法
            if (value.IsIn("a", "b", "c")) { 
            
            }
            //【IsContainsIn】
            //以前写法
            if ("abcd".Contains("abc") || "abcd".Contains("xxx"))
            { 
            
            }
            //现在写法
            if ("abcd".IsContainsIn("abc", "xxx")) { 
            
            }

            //【IsValuable与IsNullOrEmpty相反】
            string ss = "";
            //以前写法
            if (!string.IsNullOrEmpty(ss)) { }
            //现在写法
            if (s.IsValuable()) { }


            List<string> list = null;
            //以前写法
            if (list != null && list.Count > 0) { }
            //现在写法
            if (list.IsValuable()) { }




            //IsIDcard
            if ("32061119810104311x".IsIDcard())
            {

            }

            //IsTelephone
            if ("0513-85669884".IsTelephone())
            {

            }

            //IsMatch 节约你引用Regex的命名空间了
            if ("我中国人12".IsMatch(@"人\d{2}")) { }


            //下面还有很多太简单了的就不介绍了
            //IsZero
            //IsInt
            //IsNoInt
            //IsMoney 
            //IsEamil 
            //IsMobile 
         





        }

    }
}