using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace COM.Utility
{
    /// <summary>
    /// 1. 功能：session操作类
    /// 2. 作者：kaixuan
    /// 3. 创建日期：2014-10-24
    /// 4. 最后修改日期：2014-10-24
    /// </summary>
    public class SessionHelper
    {

        /// <summary>
        /// 添加Session，调动有效期为20分钟
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        /// <param name="sesValue">Session值</param>
        public static void Add(string strSessionName, object sesValue)
        {
            HttpContext.Current.Session[strSessionName] = sesValue;
        }

        /// <summary>
        /// 添加Session，调动有效期为20分钟
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        /// <param name="strValues">Session值数组</param>
        public static void Adds(string strSessionName, string[] strValues)
        {
            HttpContext.Current.Session[strSessionName] = strValues;
            HttpContext.Current.Session.Timeout = 20;
        }

        /// <summary>
        /// 添加Session
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        /// <param name="seValue">Session值</param>
        /// <param name="iExpires">调动有效期（分钟）</param>
        public static void Add(string strSessionName, object seValue, int iExpires)
        {
            HttpContext.Current.Session[strSessionName] = seValue;
            HttpContext.Current.Session.Timeout = iExpires;
        }

        /// <summary>
        /// 添加Session
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        /// <param name="strValues">Session值数组</param>
        /// <param name="iExpires">调动有效期（分钟）</param>
        public static void Adds(string strSessionName, string[] strValues, int iExpires)
        {
            HttpContext.Current.Session[strSessionName] = strValues;
            HttpContext.Current.Session.Timeout = iExpires;
        }

        /// <summary>
        /// 读取某个Session对象值
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        /// <returns>Session对象值</returns>
        public static object Get(string strSessionName)
        {
            if (HttpContext.Current.Session[strSessionName] == null)
            {
                return null;
            }
            else
            {
                return HttpContext.Current.Session[strSessionName];
            }
        }
        /// <summary>
        /// 读取某个Session对象值
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        /// <returns>Session对象值</returns>
        public static T Get<T>(string strSessionName)
        {
            return (T)HttpContext.Current.Session[strSessionName];
        }

        /// <summary>
        /// 读取某个Session对象值数组
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        /// <returns>Session对象值数组</returns>
        public static string[] Gets(string strSessionName)
        {
            if (HttpContext.Current.Session[strSessionName] == null)
            {
                return null;
            }
            else
            {
                return (string[])HttpContext.Current.Session[strSessionName];
            }
        }

        /// <summary>
        /// 删除某个Session对象
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        public static void Del(string strSessionName)
        {
            HttpContext.Current.Session[strSessionName] = null;
        }


    }
}
