using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SyntacticSugar
{
    /// <summary>
    /// 时间帮助类
    /// </summary>
    public class DateSugar
    {
        /// <summary>
        /// 时间类型
        /// </summary>
        public enum DateInterval
        {
            /// <summary>
            /// 年
            /// </summary>
            Year,
            /// <summary>
            /// 月
            /// </summary>
            Month,
            /// <summary>
            /// 周
            /// </summary>
            Weekday,
            /// <summary>
            /// 天
            /// </summary>
            Day,
            /// <summary>
            /// 小时
            /// </summary>
            Hour,
            /// <summary>
            /// 分
            /// </summary>
            Minute,
            /// <summary>
            /// 秒
            /// </summary>
            Second
        }

        /// <summary>
        /// t1和t2哪个更接近comparisonTime ，t1接近返回1 ,t2接近近回2,相等返回0
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="comparisonTime"></param>
        /// <returns></returns>
        public static int ComparisonTime(DateTime t1, DateTime t2, DateTime comparisonTime)
        {
            var t1Result = Math.Abs(DateDiff(DateInterval.Second, t1, comparisonTime));
            var t2Result = Math.Abs(DateDiff(DateInterval.Second, t2, comparisonTime));
            if (t1Result < t2Result)
            {
                return 1;
            }
            else if (t1Result > t2Result)
            {
                return 2;
            }
            else
            {
                return 0;
            }
        }


        /// <summary>
        /// 近回时间之差
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        public static long DateDiff(DateInterval interval, DateTime date1, DateTime date2)
        {

            TimeSpan ts = date2 - date1;

            switch (interval)
            {
                case DateInterval.Year:
                    return date2.Year - date1.Year;
                case DateInterval.Month:
                    return (date2.Month - date1.Month) + (12 * (date2.Year - date1.Year));
                case DateInterval.Weekday:
                    return Fix(ts.TotalDays) / 7;
                case DateInterval.Day:
                    return Fix(ts.TotalDays);
                case DateInterval.Hour:
                    return Fix(ts.TotalHours);
                case DateInterval.Minute:
                    return Fix(ts.TotalMinutes);
                default:
                    return Fix(ts.TotalSeconds);
            }
        }
        private static long Fix(double Number)
        {
            if (Number >= 0)
            {
                return (long)Math.Floor(Number);
            }
            return (long)Math.Ceiling(Number);
        }
    }
}
