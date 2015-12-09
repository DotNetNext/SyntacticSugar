using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
namespace SyntacticSugar
{
    /// <summary>
    /// IEnumerable扩展函数
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// foreach 用法:xx.ForEach(i=>{  })
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="?"></typeparam>
        /// <param name="source"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static void TryForEach<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            if (action != null)
            {
                int i = 0;
                foreach (var item in source)
                {
                    action(item, i);
                    ++i;
                }
            }
        }
        /// <summary>
        /// foreach 用法:xx.ForEach({  })
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="?"></typeparam>
        /// <param name="source"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static void TryForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (action != null)
            {
                foreach (var item in source)
                {
                    action(item);
                }
            }
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="sortField"></param>
        /// <param name="orderByType"></param>
        /// <returns></returns>
        public static IOrderedEnumerable<T> OrderBy<T>(this IEnumerable<T> list, string sortField, OrderByType orderByType)
        {
            PropertyInfo prop = typeof(T).GetProperty(sortField);
            if (prop == null)
            {
                throw new Exception("No property '" + sortField + "' in + " + typeof(T).Name + "'");
            }
            if (orderByType == OrderByType.desc)
                return list.OrderByDescending(x => prop.GetValue(x, null));
            else
                return list.OrderBy(x => prop.GetValue(x, null));
        }
        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="sortField"></param>
        /// <param name="orderByType"></param>
        /// <returns></returns>
        public static IOrderedEnumerable<T> ThenBy<T>(this IOrderedEnumerable<T> list, string sortField, OrderByType orderByType)
        {
            PropertyInfo prop = typeof(T).GetProperty(sortField);

            if (orderByType == OrderByType.desc)
                return list.OrderByDescending(x => prop.GetValue(x, null));

            if (orderByType == OrderByType.desc)
                return list.ThenByDescending(x => prop.GetValue(x, null));
            else
                return list.ThenBy(x => prop.GetValue(x, null));

        }
        /// <summary>
        /// 排序类型
        /// </summary>
        public enum OrderByType
        {
            asc = 1,
            desc = 2
        }
    }
}
