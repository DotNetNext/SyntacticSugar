using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SyntacticSugar
{
    /// <summary>
    /// ** 描述：表单帮助类
    /// ** 创始时间：2015-4-17
    /// ** 修改时间：-
    /// ** 作者：sunkaixuan
    /// ** qq：610262374 欢迎交流,共同提高 ,命名语法等写的不好的地方欢迎大家的给出宝贵建议
    /// ** 使用说明：http://www.cnblogs.com/sunkaixuan/p/4532558.html
    /// </summary>
    public class RequestToModel
    {

        /// <summary>
        /// 提交表单通过反射获取单个像
        /// 注意：表单控件name必包含对应类中的第一个字段，否则将报错
        /// </summary>
        public static T GetSingleForm<T>() where T : new()
        {
            T t = SetList<T>(null, 0).Single();
            return t;
        }


        /// <summary>
        /// 提交表单通过反射获取单个像
        /// 注意：表单控件name必包含对应类中的第一个字段，否则将报错
        /// <param name="appstr">控件前缀,比如 name="form1.sex" appstr可以设为form1</param>
        /// </summary>
        public static T GetSingleForm<T>(string appstr) where T : new()
        {
            T t = SetList<T>(appstr, 0).Single();
            return t;
        }

        /// <summary>
        /// 提交表单通过反射获取多个对像
        /// 注意：表单控件name必包含对应类中的第一个字段，否则将报错
        /// </summary>
        /// <typeparam name="type"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<T> GetListByForm<T>() where T : new()
        {
            List<T> t = SetList<T>(null, 0);
            return t;
        }

        /// <summary>
        /// 提交表单通过反射获取多个对像
        /// 注意：表单控件name必包含对应类中的第一个字段，否则将报错
        /// </summary>
        /// <typeparam name="type"></typeparam>
        /// <param name="appstr">控件前缀,比如 name="form1.sex" appstr可以设为form1</param>
        /// <returns></returns>
        public static List<T> GetListByForm<T>(string appstr) where T : new()
        {
            List<T> t = SetList<T>(appstr, 0);
            return t;
        }


        /// <summary>
        /// 提交表单通过反射获取多个对像
        /// </summary>
        /// <typeparam name="type"></typeparam>
        /// <param name="appstr">控件前缀,比如 name="form1.sex" appstr可以设为form1</param>
        /// <typeparam name="index">表单控件中第一个控件，对应类中字段在该类中的索引号,特殊情况可以是第二第三控件</typeparam>
        /// <returns></returns>
        private static List<T> GetListByForm<T>(string appstr, int index) where T : new()
        {
            List<T> t = SetList<T>(appstr, index);
            return t;
        }
        /// <summary>
        /// 需要允许特殊字符使用外部函数获取Request，为NULL使用默认用Requst[]获取参数
        /// </summary>
        public static Func<string, string> SetIsUnvalidatedFrom = null;
        private static string RequestPars(string key)
        {
            if (SetIsUnvalidatedFrom == null)
                return System.Web.HttpContext.Current.Request[key];
            else
                return SetIsUnvalidatedFrom(key);

        }

        private static List<T> SetList<T>(string appendstr, int index) where T : new()
        {
            List<T> t = new List<T>();
            try
            {
                var properties = new T().GetType().GetProperties();
                var subNum = RequestPars(appendstr + properties[index].Name).Split(',').Length;
                for (int i = 0; i < subNum; i++)
                {
                    var r = properties;
                    var model = new T();
                    foreach (var p in properties)
                    {
                        string pval = RequestPars(appendstr + p.Name + "");
                        if (!string.IsNullOrEmpty(pval))
                        {
                            pval = pval.Split(',')[i];
                            string pptypeName = p.PropertyType.Name;
                            p.SetValue(model, Convert.ChangeType(pval, p.PropertyType), null);
                        }
                    }
                    t.Add(model);
                }
            }
            catch (Exception ex)
            {


                throw ex;
            }


            return t;
        }
    }
}