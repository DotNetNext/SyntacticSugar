using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SyntacticSugar
{
    /// <summary>
    /// ** 描述：逻辑糖来简化你的代码
    /// ** 创始时间：2015-6-1
    /// ** 修改时间：-
    /// ** 作者：sunkaixuan
    /// ** 使用说明：http://www.cnblogs.com/sunkaixuan/p/4545338.html
    /// </summary>
    public static class LogicSugarExtenions
    {
        /// <summary>
        /// 根据表达式的值，来返回两部分中的其中一个。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisValue"></param>
        /// <param name="trueValue">值为true返回 trueValue</param>
        /// <param name="falseValue">值为false返回 falseValue</param>
        /// <returns></returns>
        public static T IIF<T>(this bool thisValue, T trueValue, T falseValue)
        {
            if (thisValue)
            {
                return trueValue;
            }
            else
            {
                return falseValue;
            }
        }


        /// <summary>
        /// 根据表达式的值,true返回trueValue,false返回string.Empty;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisValue"></param>
        /// <param name="trueValue">值为true返回 trueValue</param>
        /// <returns></returns>
        public static string IIF(this bool thisValue, string trueValue)
        {
            if (thisValue)
            {
                return trueValue;
            }
            else
            {
                return string.Empty;
            }
        }



        /// <summary>
        /// 根据表达式的值,true返回trueValue,false返回0
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisValue"></param>
        /// <param name="trueValue">值为true返回 trueValue</param>
        /// <returns></returns>
        public static int IIF(this bool thisValue, int trueValue)
        {
            if (thisValue)
            {
                return trueValue;
            }
            else
            {
                return 0;
            }
        }


        /// <summary>
        /// 根据表达式的值，来返回两部分中的其中一个。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisValue"></param>
        /// <param name="trueValue">值为true返回 trueValue</param>
        /// <param name="falseValue">值为false返回 falseValue</param>
        /// <returns></returns>
        public static T IIF<T>(this bool? thisValue, T trueValue, T falseValue)
        {
            if (thisValue == true)
            {
                return trueValue;
            }
            else
            {
                return falseValue;
            }
        }



        /// <summary>
        /// 所有值为true，则返回true否则返回false
        /// </summary>
        /// <param name="thisValue"></param>
        /// <param name="andValues"></param>
        /// <returns></returns>
        public static bool And(this bool thisValue, params bool[] andValues)
        {
            return thisValue && !andValues.Where(c => c == false).Any();
        }


        /// <summary>
        /// 只要有一个值为true,则返回true否则返回false
        /// </summary>
        /// <param name="thisValue"></param>
        /// <param name="andValues"></param>
        /// <returns></returns>
        public static bool Or(this bool thisValue, params bool[] andValues)
        {
            return thisValue || andValues.Where(c => c == true).Any();
        }


        /// <summary>
        /// Switch().Case().Default().Break()
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisValue"></param>
        /// <returns></returns>
        public static SwitchSugarModel<T> Switch<T>(this T thisValue)
        {
            var reval = new SwitchSugarModel<T>();
            reval.SourceValue = thisValue;
            return reval;

        }

        public static SwitchSugarModel<T> Case<T, TReturn>(this SwitchSugarModel<T> switchSugar, T caseValue, TReturn returnValue)
        {
            if (switchSugar.SourceValue.Equals(caseValue))
            {
                switchSugar.IsEquals = true;
                switchSugar.ReturnVal = returnValue;
            }
            return switchSugar;
        }

        public static SwitchSugarModel<T> Default<T, TReturn>(this SwitchSugarModel<T> switchSugar, TReturn returnValue)
        {
            if (switchSugar.IsEquals == false)
                switchSugar.ReturnVal = returnValue;
            return switchSugar;
        }

        public static dynamic Break<T>(this SwitchSugarModel<T> switchSugar)
        {
            string reval = switchSugar.ReturnVal;
            switchSugar = null;//清空对象,节约性能
            return reval;
        }

        public class SwitchSugarModel<T>
        {
            public T SourceValue { get; set; }
            public bool IsEquals { get; set; }
            public dynamic ReturnVal { get; set; }
        }

    }


}
