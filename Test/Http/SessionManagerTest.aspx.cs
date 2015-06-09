using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SyntacticSugar;

namespace Test.Http
{
    public partial class SessionManagerTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //声名一个数据集合
            var listString = new List<string>() { "a", "b", "c" };
            //session key
            string key = "sekey";

            //获取实例
            var sessionManager = SessionManager<List<string>>.GetInstance();

            //添加session
            sessionManager.Add(key, listString);
            //add有其它重载 上面是最基本的

            //获取
            List<string> sessionList = sessionManager[key];

            //其它方法
            sessionManager.ContainsKey(key);

            sessionManager.Remove(key);//删除

            sessionManager.RemoveAll(c => c.Contains("sales_"));//删除key包含sales_缓存

            sessionManager.GetAllKey();//获取所有key
        }
    }
}