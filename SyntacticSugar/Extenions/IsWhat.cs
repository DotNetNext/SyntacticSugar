using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SyntacticSugar
{
    /// <summary>
    /// ** 描述：逻辑判段是什么？
    /// ** 创始时间：2015-5-29
    /// ** 修改时间：-
    /// ** 作者：sunkaixuan
    /// ** 使用说明：http://www.cnblogs.com/sunkaixuan/p/4539654.html
    /// </summary>
    public static class IsWhat
    {
        /// <summary>
        /// 值在的范围？
        /// </summary>
        /// <param name="o"></param>
        /// <param name="begin">大于等于begin</param>
        /// <param name="end">小于等于end</param>
        /// <returns></returns>
        public static bool IsInRange(this int o, int begin, int end)
        {
            return o >= begin && o <= end;
        }
        /// <summary>
        /// 值在的范围？
        /// </summary>
        /// <param name="o"></param>
        /// <param name="begin">大于等于begin</param>
        /// <param name="end">小于等于end</param>
        /// <returns></returns>
        public static bool IsInRange(this DateTime o, DateTime begin, DateTime end)
        {
            return o >= begin && o <= end;
        }

        /// <summary>
        /// 在里面吗?
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool IsIn<T>(this T o, params T[] values)
        {
            return values.Contains(o);
        }

        /// <summary>
        /// 是null或""?
        /// </summary>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this object o)
        {
            if (o == null || o == DBNull.Value) return true;
            return o.ToString() == "";
        }
        /// <summary>
        /// 是null或""?
        /// </summary>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this Guid? o)
        {
            if (o == null) return true;
            return o == Guid.Empty;
        }
        /// <summary>
        /// 是null或""?
        /// </summary>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this Guid o)
        {
            if (o == null) return true;
            return o == Guid.Empty;
        }

        /// <summary>
        /// 有值?(与IsNullOrEmpty相反)
        /// </summary>
        /// <returns></returns>
        public static bool IsValuable(this object o)
        {
            if (o == null) return false;
            return o.ToString() != "";
        }
        /// <summary>
        /// 有值?(与IsNullOrEmpty相反)
        /// </summary>
        /// <returns></returns>
        public static bool IsValuable(this IEnumerable<object> o)
        {
            if (o == null || o.Count() == 0) return false;
            return true;
        }

        /// <summary>
        /// 是零?
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool IsZero(this object o)
        {
            return (o == null || o.ToString() == "0");
        }

        /// <summary>
        /// 是INT?
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool IsInt(this object o)
        {
            if (o == null) return false;
            return Regex.IsMatch(o.ToString(), @"^\d+$");
        }
        /// <summary>
        /// 不是INT?
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool IsNoInt(this object o)
        {
            if (o == null) return true;
            return !Regex.IsMatch(o.ToString(), @"^\d+$");
        }

        /// <summary>
        /// 是金钱?
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool IsMoney(this object o)
        {
            if (o == null) return false;
            double outValue = 0;
            return double.TryParse(o.ToString(), out outValue);
        }

        /// <summary>
        /// 是邮箱?
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool IsEamil(this object o)
        {
            if (o == null) return false;
            return Regex.IsMatch(o.ToString(), @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$");
        }

        /// <summary>
        /// 是手机?
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool IsMobile(this object o)
        {
            if (o == null) return false;
            return Regex.IsMatch(o.ToString(), @"^\d{11}$");
        }

        /// <summary>
        /// 是座机?
        /// </summary>
        public static bool IsTelephone(this object o)
        {
            if (o == null) return false;
            return System.Text.RegularExpressions.Regex.IsMatch(o.ToString(), @"^(\(\d{3,4}\)|\d{3,4}-|\s)?\d{8}$");

        }

        /// <summary>
        /// 是身份证?
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool IsIDcard(this object o)
        {
            if (o == null) return false;
            return System.Text.RegularExpressions.Regex.IsMatch(o.ToString(), @"^(\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$");
        }

        /// <summary>
        ///是适合正则匹配?
        /// </summary>
        /// <param name="o"></param>
        /// <param name="begin">大于等于begin</param>
        /// <param name="end">小于等于end</param>
        /// <returns></returns>
        public static bool IsMatch(this object o, string pattern)
        {
            if (o == null) return false;
            Regex reg = new Regex(pattern);
            return reg.IsMatch(o.ToString());
        }
    }
}
