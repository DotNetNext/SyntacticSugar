using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SyntacticSugar;
using System.Reflection;
using System.Data;

namespace Test.Assembly
{
    public partial class CreateReflectionTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PerformanceTest p = new PerformanceTest();
            p.SetCount(1);//循环次数(默认:1)
            p.SetIsMultithread(false);//是否启动多线程测试 (默认:false)
            p.Execute(
            i =>
            {
            

                Object[] args = new Object[] {0};
                var assembly= CreateReflection.NewAssembly();
                var type = ReflectionUtil.GetType(assembly, "Test.Class1");
                Object obj =type.InvokeMember(null,
           BindingFlags.DeclaredOnly |
           BindingFlags.Public | BindingFlags.NonPublic |
           BindingFlags.Instance | BindingFlags.CreateInstance, null, null, null);
                Console.WriteLine("Type: " + obj.GetType().ToString());
                Console.WriteLine("The value of x after the constructor returns is {0}.", args[0]);
                DataTable dt = new DataTable();
                DataRow dr1 = dt.NewRow();
                dt.Columns.Add("key");
                dt.Columns.Add("value");
                dr1["key"] = "1";
                dr1["value"] = "2";
                dt.Rows.Add(dr1);

                type.InvokeMember("GetList",
               BindingFlags.DeclaredOnly |
               BindingFlags.Public | BindingFlags.NonPublic |
               BindingFlags.Instance | BindingFlags.SetProperty, null, obj, new Object[] { dt });
 
            },
            message =>
            {

                //输出总共运行时间
                Response.Write(message);   //总共执行时间：1.02206秒

            }
            );
        }
    }
}