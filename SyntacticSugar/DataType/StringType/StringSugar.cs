using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace SyntacticSugar
{
    /// <summary>
    /// ** 描述：string操作类
    /// ** 创始时间：2015-6-30
    /// ** 修改时间：-
    /// ** 作者：sunkaixuan
    /// ** 使用说明：
    /// </summary>
    public static class StringSugar
    {
        /// <summary>
        /// 分割函数
        /// </summary>
        /// <param name="value"></param>
        /// <param name="splString">分割符号</param>
        /// <param name="options">分割选项</param>
        /// <returns></returns>
        public static string[] Split(this string value, string splString, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
        {
            return value.Split(new string[] { splString }, options);
        }

        ///转全角的函数(SBC case)
        ///任意字符串
        ///全角字符串
        ///全角空格为12288，半角空格为32
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        public static string ToSBC(this string input)
        {
            // 半角转全角：
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new string(c);
        }
        /**/
        /// 转半角的函数(DBC case)
        ///任意字符串
        ///半角字符串
        ///全角空格为12288，半角空格为32
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        public static string ToDBC(this string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }

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
        ///  对字符串进行 URL 解码并返回已解码的字符串。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToUrlDecodeWithPlusSign(this string value)
        {
            if (value == null) return null;
            var guid = Guid.NewGuid().ToString();
            value = value.Replace("+", guid);
            return System.Web.HttpContext.Current.Server.UrlDecode(value).Replace(guid, "+");
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

        /// <summary>
        /// 追加字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="appendString"></param>
        /// <param name="symbol">两个字符串之间的符号</param>
        /// <param name="valueIsNullAppendSymbol">当VALUE是NULL时默认 symbol是追加的 ，设成false则不会添加到拼接当中</param>
        /// <returns></returns>
        public static string AppendString(this string value, string appendString, string symbol = null, bool valueIsNullAppendSymbol = true)
        {

            if (valueIsNullAppendSymbol)
            {
                return value + symbol + appendString;
            }
            else
            {
                return value + (string.IsNullOrEmpty(value) ? "" : symbol) + appendString;
            }

        }
    }
}
