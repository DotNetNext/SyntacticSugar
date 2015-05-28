using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace SyntacticSugar
{
    /// <summary>
    /// ** 描述：实体生成类
    /// ** 创始时间：2015-4-17
    /// ** 修改时间：-
    /// ** 作者：sunkaixuan
    /// ** qq：610262374 欢迎交流,共同提高 ,命名语法等写的不好的地方欢迎大家的给出宝贵建议
    /// ** 使用说明：http://www.cnblogs.com/sunkaixuan/p/4482152.html
    /// </summary>
    public class ClassGenerating
    {
        /// <summary>
        /// 根据匿名类获取实体类的字符串
        /// </summary>
        /// <param name="entity">匿名对象</param>
        /// <param name="className">生成的类名</param>
        /// <returns></returns>
        public static string DynamicToClass(object entity, string className)
        {
            StringBuilder reval = new StringBuilder();
            StringBuilder propertiesValue = new StringBuilder();
            var propertiesObj = entity.GetType().GetProperties();
            string replaceGuid = Guid.NewGuid().ToString();
            string nullable = string.Empty;
            foreach (var r in propertiesObj)
            {

                var type = r.PropertyType;
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    type = type.GetGenericArguments()[0];
                    nullable = "?";
                }
                if (!type.Namespace.Contains("System.Collections.Generic"))
                {
                    propertiesValue.AppendLine();
                    string typeName = ChangeType(type);
                    propertiesValue.AppendFormat("public {0}{3} {1} {2}", typeName, r.Name, "{get;set;}", nullable);
                    propertiesValue.AppendLine();
                }
            }

            reval.AppendFormat(@"
                 public class {0}{{
                        {1}
                 }}
            ", className, propertiesValue);


            return reval.ToString();
        }
 

        /// <summary>
        /// 根据DataTable获取实体类的字符串
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        public static string DataTableToClass(DataTable dt, string className)
        {
            StringBuilder reval = new StringBuilder();
            StringBuilder propertiesValue = new StringBuilder();
            string replaceGuid = Guid.NewGuid().ToString();
            foreach (DataColumn r in dt.Columns)
            {
                propertiesValue.AppendLine();
                string typeName = ChangeType(r.DataType);
                propertiesValue.AppendFormat("public {0} {1} {2}", typeName, r.ColumnName, "{get;set;}");
                propertiesValue.AppendLine();
            }
            reval.AppendFormat(@"
                 public class {0}{{
                        {1}
                 }}
            ", className, propertiesValue);


            return reval.ToString();
        }

        /// <summary>
        ///  根据SQL语句获取实体类的字符串
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="className">生成的类名</param>
        /// <param name="server">服务名</param>
        /// <param name="database">数据库名称</param>
        /// <param name="uid">账号</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public static string SqlToClass(string sql, string className, string server, string database, string uid, string pwd)
        {
            using (SqlConnection conn = new SqlConnection(string.Format("server={0};uid={2};pwd={3};database={1}", server, database, uid, pwd)))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandText = sql;
                DataTable dt = new DataTable();
                SqlDataAdapter sad = new SqlDataAdapter(command);
                sad.Fill(dt);
                var reval = DataTableToClass(dt, className);
                return reval;
            }
        }
        /// <summary>
        ///  根据SQL语句获取实体类的字符串
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="className">生成的类名</param>
        /// <param name="connName">webconfig的connectionStrings name</param>
        /// <returns></returns>
        public static string SqlToClass(string sql, string className, string connName)
        {
            string connstr = System.Configuration.ConfigurationManager.ConnectionStrings[connName].ToString();
            if (connstr.Contains("metadata"))//ef
                connstr = Regex.Match(connstr, @"connection string\=""(.+)""").Groups[1].Value;
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandText = sql;
                DataTable dt = new DataTable();
                SqlDataAdapter sad = new SqlDataAdapter(command);
                sad.Fill(dt);
                var reval = DataTableToClass(dt, className);
                return reval;
            }
        }
        /// <summary>
        /// 匹配类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static string ChangeType(Type type)
        {
            string typeName = type.Name;
            switch (typeName)
            {
                case "Int32": typeName = "int"; break;
                case "String": typeName = "string"; break;
            }
            return typeName;
        }
    }
}
