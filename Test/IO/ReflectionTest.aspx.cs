using SyntacticSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Test.IO
{
    public partial class ReflectionTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //性能测试类
            PerformanceTest p = new PerformanceTest();
            p.SetCount(100000);//循环次数(默认:1)
            p.SetIsMultithread(false);//是否启动多线程测试 (默认:false)

            /******************************CreateInstance********************************/
            //带cache
            p.Execute(
            i =>
            {
                CreateInstanceByCache();//调用函数
            },
            message => { Response.Write(message); });   //总共执行时间：0.09901秒

            //不带cache
            p.Execute(
              i =>
              {
                  CreateInstanceNoCache();//调用函数
              },
             message => { Response.Write(message); });   //总共执行时间：0.32002秒


            /******************************ExecuteMethod********************************/
            //带cache
            p.Execute(
            i =>
            {
                ExecuteMethodByCache();//调用函数
            },
            message => { Response.Write(message); });   //总共执行时间：0.36202秒

            //不带cache
            p.Execute(
              i =>
              {
                  ExecuteMethodNoCache();//调用函数
              },
             message => { Response.Write(message); });   //总共执行时间：1.35508秒

            //还有其它方法就不测试了
        }

        //获取实列
        private static void CreateInstanceByCache()
        {
            ReflectionSugar rs = new ReflectionSugar(100);//缓存100秒
            var f = rs.CreateInstance<FileSugar>("SyntacticSugar.FileSugar", "SyntacticSugar");
        }
        //获取实列
        private static void CreateInstanceNoCache()
        {
            string path = "SyntacticSugar.FileSugar,SyntacticSugar";//命名空间.类型名,程序集
            Type o = Type.GetType(path);//加载类型
            FileSugar obj = (FileSugar)Activator.CreateInstance(o, true);//根据类型创建实例
        }
        //执行函数
        private static void ExecuteMethodByCache()
        {
            ReflectionSugar rs = new ReflectionSugar(100);//缓存100秒
            var path = rs.ExecuteMethod("SyntacticSugar", "FileSugar", "GetMapPath", "~/");
        }
        //执行函数
        private static void ExecuteMethodNoCache()
        {
            ReflectionSugar rs = new ReflectionSugar(0);//缓存0秒
            var path = rs.ExecuteMethod("SyntacticSugar", "FileSugar", "GetMapPath", "~/");
        }
    }
}