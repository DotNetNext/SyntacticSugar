using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace SyntacticSugar
{
    /// <summary>
    /// ** 描述：Request信息
    /// ** 创始时间：2015-6-30
    /// ** 修改时间：-
    /// ** 作者：sunkaixuan
    /// ** 使用说明：
    /// </summary>
    public class RequestInfo
    {
        /// <summary>
        /// 当前 HTTP 请求的完整 URL （包含参数信息）
        /// </summary>
        public static string Url
        {
            get
            {
                return (HttpContext.Current.Request.ServerVariables["URL"] + "?" + HttpContext.Current.Request.ServerVariables["QUERY_STRING"]);
            }
        }

        /// <summary>
        /// 获取当前域名加HTTP
        /// </summary>
        public static string HttpDomain
        {
            get
            {
                return Http + Domain;
            }
        }

        /// <summary>
        /// 域名
        /// </summary>
        public static string Domain
        {
            get
            {
                if (((HttpContext.Current.Request.ServerVariables["SERVER_PORT"] == null) || (HttpContext.Current.Request.ServerVariables["SERVER_PORT"] == "")) || (HttpContext.Current.Request.ServerVariables["SERVER_PORT"] == "80"))
                {
                    return HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
                }
                return (HttpContext.Current.Request.ServerVariables["SERVER_NAME"] + ":" + HttpContext.Current.Request.ServerVariables["SERVER_PORT"]);
            }
        }
        /// <summary>
        /// http or https
        /// </summary>
        public static string Http
        {
            get
            {
                if (HttpContext.Current.Request.ServerVariables["HTTPS"].ToLower() == "on")
                {
                    return "https://";
                }
                return "http://";
            }
        }
        /// <summary>
        /// 当前 HTTP 请求的页面 URL 
        /// </summary>
        public static string Page
        {
            get
            {
                return HttpContext.Current.Request.ServerVariables["URL"];
            }
        }
        /// <summary>
        /// 物理路径
        /// </summary>
        public static string PhysicalPath
        {
            get
            {
                string str = HttpContext.Current.Request.ServerVariables["APPL_PHYSICAL_PATH"];
                if (!str.EndsWith(@"\"))
                {
                    str = str + @"\";
                }
                return str;
            }
        }

        public static System.Collections.Specialized.NameValueCollection RequestQueryString
        {
            get
            {
                return System.Web.HttpContext.Current.Request.QueryString;
            }
        }

        /// <summary>
        /// ip地址
        /// </summary>
        public static string UserAddress
        {
            get
            {
                if ((HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] == null) || (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] == ""))
                {
                    return HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                return HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }
        }
        //虚拟目录
        public static string Virtual
        {
            get
            {
                if ((HttpContext.Current.Request.ApplicationPath != null) && HttpContext.Current.Request.ApplicationPath.EndsWith("/"))
                {
                    return HttpContext.Current.Request.ApplicationPath;
                }
                return (HttpContext.Current.Request.ApplicationPath + "/");
            }
        }

        /// <summary>
        /// 获取GET或POST参数
        /// </summary>
        /// <param name="parName"></param>
        /// <returns></returns>
        public static string QueryString(string name)
        {
            return HttpContext.Current.Request[name];
        }
    }
}
