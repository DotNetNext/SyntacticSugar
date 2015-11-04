using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace SyntacticSugar
{
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
                    action(item,i);
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
    }
}
