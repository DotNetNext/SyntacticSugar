using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SyntacticSugar;

namespace Test.Http
{
    public partial class CookiesManagerTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //声名一个数据集合
            var listString = new List<string>() { "a", "b", "c" };
            //缓存key
            string key = "cokey";

            //获取实例
            var cookiesManager = CookiesManager<List<string>>.GetInstance();

            //插入缓存
            cookiesManager.Add(key, listString, cookiesManager.Minutes * 30);//过期30分钟 
            //add有其它重载 上面是最基本的

            //获取
            List<string> cookiesList = cookiesManager[key];

            //其它方法
            cookiesManager.ContainsKey(key);

            cookiesManager.Remove(key);//删除

            cookiesManager.RemoveAll(c => c.Contains("sales_"));//删除key包含sales_的cookies

            cookiesManager.GetAllKey();//获取所有key
        }
    }
}