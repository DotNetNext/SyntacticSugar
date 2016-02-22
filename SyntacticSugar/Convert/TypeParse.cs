using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Data;
using System.Reflection;
using System.Collections;
using System.IO;

namespace SyntacticSugar
{
    /// <summary>
    /// ** 描述：类型转换
    /// ** 创始时间：2015-6-2
    /// ** 修改时间：-
    /// ** 作者：sunkaixuan
    /// ** 使用说明：http://www.cnblogs.com/sunkaixuan/p/4548028.html
    /// </summary>
    public static class TypeParseExtenions
    {
        #region 强转成bool 如果失败返回 0
        /// <summary>
        /// 强转成bool 如果失败返回 false
        /// </summary>
        /// <param name="thisValue"></param>
        /// <returns></returns>
        public static bool TryToBoolean(this object thisValue)
        {
            bool reval = false;
            if (thisValue != null && Boolean.TryParse(thisValue.ToString(), out reval))
            {
                return reval;
            }
            return reval;
        }
        #endregion
        #region 强转成int 如果失败返回 0
        /// <summary>
        /// 强转成int 如果失败返回 0
        /// </summary>
        /// <param name="thisValue"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static int TryToInt(this object thisValue)
        {
            int reval = 0;
            if (thisValue != null && int.TryParse(thisValue.ToString(), out reval))
            {
                return reval;
            }
            return reval;
        }
        #endregion
        #region 强转成int 如果失败返回 errorValue
        /// <summary>
        /// 强转成int 如果失败返回 errorValue
        /// </summary>
        /// <param name="thisValue"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static int TryToInt(this object thisValue, int errorValue)
        {
            int reval = 0;
            if (thisValue != null && int.TryParse(thisValue.ToString(), out reval))
            {
                return reval;
            }
            return errorValue;
        }
        #endregion
        #region 强转成double 如果失败返回 0
        /// <summary>
        /// 强转成money 如果失败返回 0
        /// </summary>
        /// <param name="thisValue"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static double TryToMoney(this object thisValue)
        {
            double reval = 0;
            if (thisValue != null && double.TryParse(thisValue.ToString(), out reval))
            {
                return reval;
            }
            return 0;
        }
        #endregion
        #region 强转成double 如果失败返回 errorValue
        /// <summary>
        /// 强转成double 如果失败返回 errorValue
        /// </summary>
        /// <param name="thisValue"></param>
        /// <param name="errorValue"></param>
        /// <returns></returns>
        public static double TryToMoney(this object thisValue, double errorValue)
        {
            double reval = 0;
            if (thisValue != null && double.TryParse(thisValue.ToString(), out reval))
            {
                return reval;
            }
            return errorValue;
        }
        #endregion
        #region 强转成string 如果失败返回 ""
        /// <summary>
        /// 强转成string 如果失败返回 ""
        /// </summary>
        /// <param name="thisValue"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static string TryToString(this object thisValue)
        {
            if (thisValue != null) return thisValue.ToString().Trim();
            return "";
        }
        #endregion
        #region  强转成string 如果失败返回 errorValue
        /// <summary>
        /// 强转成string 如果失败返回 str
        /// </summary>
        /// <param name="thisValue"></param>
        /// <param name="errorValue"></param>
        /// <returns></returns>
        public static string TryToString(this object thisValue, string errorValue)
        {
            if (thisValue != null) return thisValue.ToString().Trim();
            return errorValue;
        }
        #endregion
        #region 强转成Decimal 如果失败返回 0
        /// <summary>
        /// 强转成Decimal 如果失败返回 0
        /// </summary>
        /// <param name="thisValue"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static Decimal TryToDecimal(this object thisValue)
        {
            Decimal reval = 0;
            if (thisValue != null && decimal.TryParse(thisValue.ToString(), out reval))
            {
                return reval;
            }
            return 0;
        }
        #endregion
        #region 强转成Decimal 如果失败返回 errorValue
        /// <summary>
        /// 强转成Decimal 如果失败返回 errorValue
        /// </summary>
        /// <param name="thisValue"></param>
        /// <param name="errorValue"></param>
        /// <returns></returns>
        public static Decimal TryToDecimal(this object thisValue, decimal errorValue)
        {
            Decimal reval = 0;
            if (thisValue != null && decimal.TryParse(thisValue.ToString(), out reval))
            {
                return reval;
            }
            return errorValue;
        }
        #endregion
        #region 强转成DateTime 如果失败返回 DateTime.MinValue
        /// <summary>
        /// 强转成DateTime 如果失败返回 DateTime.MinValue
        /// </summary>
        /// <param name="thisValue"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static DateTime TryToDate(this object thisValue)
        {
            DateTime reval = DateTime.MinValue;
            if (thisValue != null && DateTime.TryParse(thisValue.ToString(), out reval))
            {
                return reval;
            }
            return reval;
        }
        #endregion
        #region 强转成DateTime 如果失败返回 errorValue
        /// <summary>
        /// 强转成DateTime 如果失败返回 errorValue
        /// </summary>
        /// <param name="thisValue"></param>
        /// <param name="errorValue"></param>
        /// <returns></returns>
        public static DateTime TryToDate(this object thisValue, DateTime errorValue)
        {
            DateTime reval = DateTime.MinValue;
            if (thisValue != null && DateTime.TryParse(thisValue.ToString(), out reval))
            {
                return reval;
            }
            return errorValue;
        }
        #endregion

        #region json转换
        /// <summary>
        /// 将json序列化为实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static TEntity JsonToModel<TEntity>(this string json)
        {
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            return jsSerializer.Deserialize<TEntity>(json);
        }
        /// <summary>
        /// 将实体序列化为json
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string ModelToJson<T>(this T model)
        {
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            return jsSerializer.Serialize(model);
        }

        #endregion

        #region  DataTable List
        /// <summary>
        /// 将集合类转换成DataTable
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns></returns>
        public static DataTable TryToDataTable<T>(this List<T> list)
        {
            DataTable result = new DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = typeof(T).GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    result.Columns.Add(pi.Name);
                }

                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        object obj = pi.GetValue(list[i], null);
                        if (obj != null && obj != DBNull.Value)
                            tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }
        /// <summary>
        /// 将datatable转为list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> TryToList<T>(this DataTable dt)
        {
            var list = new List<T>();
            Type t = typeof(T);
            var plist = new List<PropertyInfo>(typeof(T).GetProperties());

            foreach (DataRow item in dt.Rows)
            {
                T s = System.Activator.CreateInstance<T>();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    PropertyInfo info = plist.Find(p => p.Name == dt.Columns[i].ColumnName);
                    if (info != null)
                    {
                        if (!Convert.IsDBNull(item[i]) && item[i]!=null)
                        {
                            info.SetValue(s, item[i], null);
                        }
                    }
                }
                list.Add(s);
            }
            return list;
        } 
        #endregion

        #region IO
        /// <summary>
        /// 将流转成btye
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] TryToBytes(this Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }

        /// <summary>
        /// 将btye转成流
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Stream TryToStream(this byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        } 
        #endregion

    }
}
