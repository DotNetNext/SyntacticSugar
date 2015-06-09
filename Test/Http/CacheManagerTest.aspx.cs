using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SyntacticSugar;
namespace Test.Http
{
    public partial class CacheManagerTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //声名一个数据集合
            var listString = new List<string>() { "a", "b", "c" };
            //缓存key
            string key = "cmkey";

            //获取实例
            var cacheManager = CacheManager<List<string>>.GetInstance();

            //插入缓存
            cacheManager.Add(key,listString, cacheManager.Minutes * 30);//过期30分钟 
            //add有其它重载 上面是最基本的

            //获取
            List<string> cacheList=cacheManager[key];

            //其它方法
            cacheManager.ContainsKey(key);

            cacheManager.Remove(key);//删除

            cacheManager.RemoveAll(c=>c.Contains("sales_"));//删除key包含sales_缓存

            cacheManager.GetAllKey();//获取所有key
        }
    }
}