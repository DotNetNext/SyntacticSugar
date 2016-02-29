using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SyntacticSugar
{
    /// <summary>
    /// ** 描述：session操作类
    /// ** 创始时间：2015-6-9
    /// ** 修改时间：-
    /// ** 作者：sunkaixuan
    /// ** 使用说明：http://www.cnblogs.com/sunkaixuan/p/4564612.html
    /// </summary>
    /// <typeparam name="K">键</typeparam>
    /// <typeparam name="V">值</typeparam>
    public class SessionManager<V> : IHttpStorageObject<V>
    {
        private static readonly object _instancelock = new object();
        private static SessionManager<V> _instance = null;

        public static SessionManager<V> GetInstance()
        {
            if (_instance == null)
            {
                lock (_instancelock)
                {
                    if (_instance == null)
                    {
                        _instance = new SessionManager<V>();
                    }
                }

            }
            return _instance;
        }

        public override void Add(string key, V value)
        {
            context.Session.Add(key, value);
        }

        public override bool ContainsKey(string key)
        {
            return context.Session[key] != null;
        }

        public override V Get(string key)
        {
            return (V)context.Session[key];
        }

        public override IEnumerable<string> GetAllKey()
        {
            foreach (var key in context.Session.Keys)
            {
                yield return key.ToString();
            }
        }

        public override void Remove(string key)
        {
            context.Session[key] = null;
            context.Session.Remove(key);
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
            var allKeyList = GetAllKey().ToList();
            var removeKeyList = allKeyList.Where(removeExpression).ToList();
            foreach (var key in removeKeyList)
            {
                Remove(key);
            }
        }

        public override V this[string key]
        {
            get { return (V)context.Session[key]; }
        }

        public override void Add(string key, V value, int cacheDurationInSeconds)
        {
            throw new NotImplementedException("session无法设置过期时间,请到webconfig更改设置");
        }
    }
}
