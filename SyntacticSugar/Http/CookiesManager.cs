using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SyntacticSugar
{

    /// <summary>
    /// ** 描述：cookies操作类
    /// ** 创始时间：2015-6-9
    /// ** 修改时间：-
    /// ** 作者：sunkaixuan
    /// ** 使用说明：http://www.cnblogs.com/sunkaixuan/p/4564799.html
    /// </summary>
    /// <typeparam name="V">值</typeparam>
    public class CookiesManager<V> : IHttpStorageObject<V>
    {

        #region 全局变量
        private static CookiesManager<V> _instance = null;
        private static readonly object _instanceLock = new object();
        #endregion

        /// <summary>         
        /// 获取实例 （单例模式）       
        /// </summary>         
        /// <returns></returns>         
        public static CookiesManager<V> GetInstance()
        {
            if (_instance == null)
                lock (_instanceLock)
                    if (_instance == null)
                        _instance = new CookiesManager<V>();
            return _instance;
        }


        /// <summary>
        /// 添加cookies ,注意value最大4K (默认1天)
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        public override void Add(string key, V value)
        {
            Add(key, value, Day);
        }
        /// <summary>
        /// 添加cookies ,注意value最大4K
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="cookiesDurationInSeconds">有效时间单位秒</param>
        public void Add(string key, V value, int cookiesDurationInSeconds)
        {
            HttpResponse response = HttpContext.Current.Response;
            if (response != null)
            {
                HttpCookie cookie = response.Cookies[key];
                if (cookie != null)
                {
                    if (typeof(V) == typeof(string))
                    {
                        string setValue = value.ToString();
                        Add(key, cookiesDurationInSeconds, cookie, setValue, response);
                    }
                    else
                    {
                        System.Web.Script.Serialization.JavaScriptSerializer jss = new System.Web.Script.Serialization.JavaScriptSerializer();
                        string setValue = jss.Serialize(value);
                        Add(key, cookiesDurationInSeconds, cookie, setValue, response);

                    }
                }
            }
        }

        private void Add(string key, int cookiesDurationInSeconds, HttpCookie cookie, string setValue, HttpResponse response)
        {
            if (!string.IsNullOrEmpty(key) && cookie.HasKeys)
                cookie.Values.Set(key, setValue);
            else
                if (!string.IsNullOrEmpty(setValue))
                    cookie.Value = setValue;
            if (cookiesDurationInSeconds > 0)
                cookie.Expires = DateTime.Now.AddSeconds(cookiesDurationInSeconds);
            response.SetCookie(cookie);
        }

        public override bool ContainsKey(string key)
        {
            return Get(key) != null;
        }

        public override V Get(string key)
        {
            string value = string.Empty;
            if (context.Request.Cookies[key] != null)
                value = context.Request.Cookies[key].Value;
            if (typeof(V) == typeof(string))
            {
                return (V)Convert.ChangeType(value, typeof(V));
            }
            else
            {
                System.Web.Script.Serialization.JavaScriptSerializer jss = new System.Web.Script.Serialization.JavaScriptSerializer();
                return jss.Deserialize<V>(value);
            }
        }

        public override IEnumerable<string> GetAllKey()
        {
            var allKeyList = context.Request.Cookies.AllKeys.ToList();
            foreach (var key in allKeyList)
            {
                yield return key;
            }
        }

        public override void Remove(string key)
        {
            HttpRequest request = HttpContext.Current.Request;
            if (request != null)
            {
                HttpCookie cookie = request.Cookies[key];
                cookie.Expires = DateTime.Now.AddDays(-1);
                if (cookie != null)
                {
                    if (!string.IsNullOrEmpty(key) && cookie.HasKeys)
                        cookie.Values.Remove(key);
                    else
                        request.Cookies.Remove(key);
                }
            }
        }

        public override void RemoveAll()
        {
            foreach (var key in GetAllKey())
            {
                Remove(key);
            }
        }

        public override void RemoveAll(Func<string, bool> removeExpression)
        {
            var removeList = GetAllKey().Where(removeExpression).ToList();
            foreach (var key in removeList)
            {
                Remove(key);
            }
        }

        public override V this[string key]
        {
            get { return Get(key); }
        }
    }
}
