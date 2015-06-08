using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace SyntacticSugar
{
    /// <summary>     
    /// 1. 功能：反射操作类
    /// 2. 作者：kaixuan
    /// 3. 创建日期：2014-10-31
    /// 4. 最后修改日期：2014-10-31
    /// </summary>  
    public class ReflectionUtil
    {
        #region 加载程序集
        /// <summary>         
        /// 加载程序集         
        /// </summary>         
        /// <param name="assemblyName">程序集名称,不要加上程序集的后缀，如.dll</param>                 
        public static Assembly LoadAssembly(string assemblyName)
        {
            try { return Assembly.Load(assemblyName); }
            catch (Exception ex)
            {
                string errMsg = ex.Message; return null;
            }
        }
        #endregion
        #region 获取程序集中的类型
        /// <summary>         
        /// 获取本地程序集中的类型         
        /// </summary>        
        /// /// <param name="typeName">类型名称，范例格式："命名空间.类名",类型名称必须在本地程序集中</param>                 
        public static Type GetType(string typeName)
        {
            try { return Type.GetType(typeName); }
            catch (Exception ex)
            {
                string errMsg = ex.Message; return null;
            }
        }
        /// <summary>         
        /// 获取指定程序集中的类型         
        /// </summary>         
        /// <param name="assembly">指定的程序集</param>         
        /// <param name="typeName">类型名称，范例格式："命名空间.类名",类型名称必须在assembly程序集中</param>         
        public static Type GetType(Assembly assembly, string typeName)
        {
            try
            {
                return assembly.GetType(typeName);
            }
            catch (Exception ex) { string errMsg = ex.Message; return null; }
        }
        #endregion
        #region 动态创建对象实例
        /// <summary>         
        /// 创建类的实例         
        /// </summary>         
        /// <param name="className">类名，格式:"命名空间.类名"</param>         
        /// <param name="assemblyName">程序集</param>                 
        public static object CreateInstance(string className, string assemblyName)
        {
            Assembly assembly = LoadAssembly(assemblyName);
            object obj = assembly.CreateInstance(className);
            return obj;
        }
        #endregion
        #region 获取类的命名空间
        /// <summary>         
        /// 获取类的命名空间         
        /// </summary>         
        /// <typeparam name="T">类名或接口名</typeparam>                 
        public static string GetNamespace<T>() { return typeof(T).Namespace; }
        #endregion
        #region 设置成员的值
        #region 设置属性值
        /// <summary>         
        /// 将值装载到目标对象的指定属性中         
        /// </summary>         
        /// <param name="target">要装载数据的目标对象</param>         
        /// <param name="propertyName">目标对象的属性名</param>         
        /// <param name="value">要装载的值</param>         
        public static void SetPropertyValue(object target, string propertyName, object value)
        {
            PropertyInfo propertyInfo = target.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            SetValue(target, propertyInfo, value);
        }
        #endregion
        #region 设置成员的值
        /// <summary>        
        /// 设置成员的值         
        /// </summary>         
        /// <param name="target">要装载数据的目标对象</param>         
        /// <param name="memberInfo">目标对象的成员</param>         
        /// <param name="value">要装载的值</param>         
        private static void SetValue(object target, MemberInfo memberInfo, object value)
        {
            if (value != null)
            {
                Type pType; if (memberInfo.MemberType == MemberTypes.Property)
                    pType = ((PropertyInfo)memberInfo).PropertyType;
                else pType = ((FieldInfo)memberInfo).FieldType;
                Type vType = GetPropertyType(value.GetType());
                value = CoerceValue(pType, vType, value);
            } if (memberInfo.MemberType == MemberTypes.Property)
                ((PropertyInfo)memberInfo).SetValue(target, value, null);
            else ((FieldInfo)memberInfo).SetValue(target, value);
        }
        #endregion
        #region 强制将值转换为指定类型
        /// <summary>         
        /// 强制将值转换为指定类型         
        /// </summary>         
        /// <param name="propertyType">目标类型</param>         
        /// <param name="valueType">值的类型</param>         
        /// <param name="value">值</param>         
        public static object CoerceValue(Type propertyType, Type valueType, object value)
        {
            //如果值的类型与目标类型相同则直接返回,否则进行转换             
            if (propertyType.Equals(valueType))
            {
                return value;
            }
            else
            {
                if (propertyType.IsGenericType)
                {
                    if (propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        if (value == null)
                            return null;
                        else if (valueType.Equals(typeof(string)) && (string)value == string.Empty)
                            return null;
                    } propertyType = GetPropertyType(propertyType);
                }
                if (propertyType.IsEnum && valueType.Equals(typeof(string))) return Enum.Parse(propertyType, value.ToString());
                if (propertyType.IsPrimitive && valueType.Equals(typeof(string)) && string.IsNullOrEmpty((string)value)) value = 0;
                try { return Convert.ChangeType(value, GetPropertyType(propertyType)); }
                catch (Exception ex)
                {
                    TypeConverter cnv = TypeDescriptor.GetConverter(GetPropertyType(propertyType));
                    if (cnv != null && cnv.CanConvertFrom(value.GetType())) return cnv.ConvertFrom(value);
                    else
                        throw ex;
                }
            }
        }
        #endregion
        #region 获取类型,如果类型为Nullable<>，则返回Nullable<>的基础类型
        /// <summary>         
        /// 获取类型,如果类型为Nullable(of T)，则返回Nullable(of T)的基础类型         
        /// </summary>         
        /// <param name="propertyType">需要转换的类型</param>         
        private static Type GetPropertyType(Type propertyType)
        {
            Type type = propertyType; if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Nullable<>)))
                return Nullable.GetUnderlyingType(type); return type;
        }
        #endregion
        #endregion
    }
}
