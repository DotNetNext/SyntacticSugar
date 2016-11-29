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
        public static string DateStringFromNow(DateTime dt)
        {
            TimeSpan span = DateTime.Now - dt;
            if (span.TotalDays > 60)
            {
                return dt.ToShortDateString();
            }
            else
            {
                if (span.TotalDays > 30)
                {
                    return
                    "1个月前";
                }
                else
                {
                    if (span.TotalDays > 14)
                    {
                        return
                        "2周前";
                    }
                    else
                    {
                        if (span.TotalDays > 7)
                        {
                            return
                            "1周前";
                        }
                        else
                        {
                            if (span.TotalDays > 1)
                            {
                                return
                                string.Format("{0}天前", (int)Math.Floor(span.TotalDays));
                            }
                            else
                            {
                                if (span.TotalHours > 1)
                                {
                                    return
                                    string.Format("{0}小时前", (int)Math.Floor(span.TotalHours));
                                }
                                else
                                {
                                    if (span.TotalMinutes > 1)
                                    {
                                        return
                                        string.Format("{0}分钟前", (int)Math.Floor(span.TotalMinutes));
                                    }
                                    else
                                    {
                                        if (span.TotalSeconds >= 1)
                                        {
                                            return
                                            string.Format("{0}秒前", (int)Math.Floor(span.TotalSeconds));
                                        }
                                        else
                                        {
                                            return
                                            "1秒前";
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

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
        /// 判段两个时间是否是同一天
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <returns></returns>
        public static bool IsSameDay(DateTime? d1, DateTime? d2)
        {
            if (d1 == null)
            {
                d1 = DateTime.MinValue;
            }
            if (d2 == null)
            {
                d2 = DateTime.MinValue;
            }
            return
                IsSameDay(Convert.ToDateTime(d1), Convert.ToDateTime(d2));
        }
        /// <summary>
        /// 判段两个时间是否是同一天
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <returns></returns>
        public static bool IsSameDay(DateTime d1, DateTime d2)
        {
            return d1.ToString("yyyy-MM-dd")==d2.ToString("yyyy-MM-dd");
        }


        /// <summary>
        /// 返回时间之差
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
        /// <summary>
        /// 取时间的交集部分的分钟数
        /// </summary>
        /// <param name="minT">minT为交集的最小时</param>
        /// <param name="maxT">maxT为交集的最大时间</param>
        /// <param name="t1">时间1</param>
        /// <param name="t2">时间2</param>
        /// <returns></returns>
        public static int GetTwoTimeAreaIntersection(DateTime minT, DateTime maxT, DateTime t1, DateTime t2)
        {
            var range1 = new { start = minT, end = maxT };
            var range2 = new { start = t1, end = t2 };
            var iStart = range1.start < range2.start ? range2.start : range1.start;
            var iEnd = range1.end < range2.end ? range1.end : range2.end;
            var newRange = iStart < iEnd ? new { start = iStart, end = iEnd } : null;
            if (newRange == null) return 0;
            //返回区间不能超出请假单范围
            DateTime resBegin = DateTime.Now;
            DateTime resEnd = DateTime.Now;
            if (newRange.end > maxT)
            {
                resEnd = maxT;
            }
            else
            {
                resEnd = newRange.end;
            }
            if (newRange.start < minT)
            {
                resBegin = minT;
            }
            else
            {
                resBegin = newRange.start;
            }

            return Convert.ToInt32((resEnd - resBegin).TotalMinutes);
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
