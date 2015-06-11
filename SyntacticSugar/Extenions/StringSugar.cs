using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace SyntacticSugar
{
    public static class StringSugar
    {
        /// <summary>
        /// 获取格式化字符串,等同于string.Format
        /// </summary>
        /// <param name="value"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string ToFormat(this string value, params object[] args)
        {
            return string.Format(value, args);
        }

        /// <summary>
        /// 截取字符。不区分中英文
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cutLength"></param>
        /// <param name="bdot"></param>
        /// <returns></returns>
        public static string ToCutString(this string value, int cutLength, string appendString = null)
        {
            string str = "";

            if (cutLength >= value.Length)
                return value;

            int nRealLen = cutLength * 2;
            if (appendString != null)
                nRealLen = nRealLen - appendString.Length;

            Encoding encoding = Encoding.GetEncoding("gb2312");
            for (int i = value.Length; i >= 0; i--)
            {
                str = value.Substring(0, i);
                if (encoding.GetBytes(str).Length > nRealLen)
                    continue;
                else
                    break;
            }
            str += appendString;
            return str;
        }
        /// <summary>
        /// 对 HTML 编码的字符串进行解码，并返回已解码的字符串。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToHtmlDecode(this string value)
        {
            return System.Web.HttpContext.Current.Server.HtmlDecode(value);
        }
        /// <summary>
        ///   对字符串进行 HTML 编码并返回已编码的字符串。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToHtmlEncode(this string value)
        {
            return System.Web.HttpContext.Current.Server.HtmlEncode(value);
        }
        /// <summary>
        /// 对字符串进行 URL 解码并返回已解码的字符串。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToUrlDecode(this string value)
        {
            return System.Web.HttpContext.Current.Server.UrlDecode(value);
        }
        /// <summary>
        ///  对字符串进行 URL 编码，并返回已编码的字符串。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToUrlEncode(this string value)
        {
            return System.Web.HttpContext.Current.Server.UrlEncode(value);
        }
        /// <summary>
        /// 将文本转换成html
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToHtmlByText(this string value)
        {
            value = value.Replace("\r\n", "\r");
            value = value.Replace("\n", "\r");
            value = value.Replace("\r", "<br>\r\n");
            value = value.Replace("\t", " ");
            return value;
        }
 
    }
}
