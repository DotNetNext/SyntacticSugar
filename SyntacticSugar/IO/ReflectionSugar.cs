using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace SyntacticSugar
{
    public class ReflectionSugar
    {
        public static int Minutes = 60;
        public static int Hour = 60 * 60;
        public static int Day = 60 * 60 * 24;
        private int _time = 0;
        private bool _isCache { get { return _time > 0; } }
        /// <summary>
        /// 缓存时间，0为不缓存（默认值：0秒，单位：秒）
        /// </summary>
        public ReflectionSugar(int time = 0)
        {
            _time = time;
        }

        /// <summary>
        /// 创建对象实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fullName">命名空间.类型名</param>
        /// <param name="assemblyName">程序集（dll名称）</param>
        /// <returns></returns>
        public T CreateInstance<T>(string fullName, string assemblyName)
        {
            string key = GetKey("CreateInstance1", fullName, assemblyName);
            if (_isCache)
                if (ContainsKey(key))
                {
                    return Get<T>(key);
                }
            string path = fullName + "," + assemblyName;//命名空间.类型名,程序集
            Type o = Type.GetType(path);//加载类型
            object obj = Activator.CreateInstance(o, true);//根据类型创建实例
            var reval = (T)obj;
            if (_isCache)
                Add<T>(key, reval, _time);
            return reval;//类型转换并返回
        }

        /// <summary>
        /// 创建对象实例
        /// </summary>
        /// <typeparam name="T">要创建对象的类型</typeparam>
        /// <param name="assemblyName">类型所在程序集名称（dll名称）</param>
        /// <param name="nameSpace">类型所在命名空间</param>
        /// <param name="className">类型名</param>
        /// <returns></returns>
        public T CreateInstance<T>(string assemblyName, string nameSpace, string className)
        {
            string key = GetKey("CreateInstance2", assemblyName, nameSpace, className);
            if (_isCache)
                if (ContainsKey(key))
                {
                    return Get<T>(key);
                }
            try
            {
                string fullName = nameSpace + "." + className;//命名空间.类型名
                //此为第一种写法
                object ect = Assembly.Load(assemblyName).CreateInstance(fullName);//加载程序集，创建程序集里面的 命名空间.类型名 实例
                var reval = (T)ect;//类型转换并返回
                if (_isCache)
                    Add<T>(key, reval, _time);
                return reval;
                //下面是第二种写法
                //string path = fullName + "," + assemblyName;//命名空间.类型名,程序集
                //Type o = Type.GetType(path);//加载类型
                //object obj = Activator.CreateInstance(o, true);//根据类型创建实例
                //return (T)obj;//类型转换并返回
            }
            catch
            {
                //发生异常，返回类型的默认值
                var reval = default(T);
                if (_isCache)
                    Add<T>(key, reval, _time);
                return reval;//类型转换
            }
        }
        /// <summary>
        /// 加载程序集
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public Assembly LoadFile(string path)
        {
            string key = GetKey("LoadFile", path);
            if (_isCache)
                if (ContainsKey(key))
                {
                    return Get<Assembly>(key);
                }
            Assembly asm = Assembly.LoadFile(path);
            if (_isCache)
                Add<Assembly>(key, asm, _time);
            return asm;
        }

        /// <summary>
        /// 获取类型根据程序集
        /// </summary>
        /// <param name="asm">Assembly对象</param>
        /// <returns></returns>
        public Type GetTypeByAssembly(Assembly asm, string nameSpace, string className)
        {
            string key = GetKey("GetTypeByAssembly", nameSpace, className);
            if (_isCache)
                if (ContainsKey(key))
                {
                    return Get<Type>(key);
                }
            Type type = asm.GetType(nameSpace + "." + className);
            if (_isCache)
                Add<Type>(key, type, _time);
            return type;
        }

        /// <summary>
        /// 返回当前 System.Type 的所有公共属性。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public PropertyInfo[] GetProperties(Type type)
        {
            string key = GetKey("GetProperties", type.FullName);
            if (_isCache)
                if (ContainsKey(key))
                {
                    return Get<PropertyInfo[]>(key);
                }
            var reval = type.GetProperties();
            if (_isCache)
                Add<PropertyInfo[]>(key, reval, _time);
            return reval;
        }


        /// <summary>
        /// 根据字符执行方法
        /// </summary>
        /// <param name="nameSpace">命名空间</param>
        /// <param name="className">类名</param>
        /// <param name="MethodName">方法名</param>
        /// <param name="parameters">参数</param>
        /// <returns>返回object类型</returns>
        public object ExecuteMethod(string nameSpace, string className, string MethodName, params object[] parameters)
        {
            string key = GetKey("ExecuteMethod", nameSpace, className, MethodName, parameters.Length.ToString());
            MethodInfo methodinfo = null;
            if (_isCache&&ContainsKey(key))
            {
               methodinfo = Get<MethodInfo>(key);
            }
            else
            {
                //动态从程序集中查找所需要的类并使用系统激活器创建实例最后获取它的Type
                Type type = Assembly.Load(nameSpace).CreateInstance(nameSpace + "." + className).GetType();
                //定义参数的个数,顺序以及类型的存储空间;
                Type[] parametersLength;
                if (parameters != null)
                {

                    //如果有参数创建参数存储空间并依次设置类型
                    parametersLength = new Type[parameters.Length];
                    int i = 0;
                    foreach (object obj in parameters)
                    {

                        parametersLength.SetValue(obj.GetType(), i);
                        i++;

                    }

                }
                else
                {

                    //没有参数就为空
                    parametersLength = new Type[0];

                }

                //查找指定的方法
                methodinfo = type.GetMethod(MethodName, parametersLength);
                if (_isCache)
                    Add<MethodInfo>(key, methodinfo, _time);
            }
            //如果是静态方法就执行(非静态我没试过)
            if (methodinfo.IsStatic)
            {

                //调用函数
                return methodinfo.Invoke(null, parameters);

            }

            return null;

        }



        #region helper
        private string GetKey(params string[] keyElementArray)
        {
            return string.Join("", keyElementArray);
        }

        /// <summary>         
        /// 插入缓存        
        /// </summary>         
        /// <param name="key"> key</param>         
        /// <param name="value">value</param>         
        /// <param name="cacheDurationInSeconds">过期时间单位秒</param>         
        private void Add<V>(string key, V value, int cacheDurationInSeconds)
        {
            Add(key, value, cacheDurationInSeconds, CacheItemPriority.Default);
        }

        /// <summary>         
        /// 插入缓存.         
        /// </summary>         
        /// <param name="key">key</param>         
        /// <param name="value">value</param>         
        /// <param name="cacheDurationInSeconds">过期时间单位秒</param>         
        /// <param name="priority">缓存项属性</param>         
        private void Add<V>(string key, V value, int cacheDurationInSeconds, CacheItemPriority priority)
        {
            string keyString = key;
            HttpRuntime.Cache.Insert(keyString, value, null, DateTime.Now.AddSeconds(cacheDurationInSeconds), Cache.NoSlidingExpiration, priority, null);
        }

        /// <summary>         
        /// 插入缓存.         
        /// </summary>         
        /// <param name="key">key</param>         
        /// <param name="value">value</param>         
        /// <param name="cacheDurationInSeconds">过期时间单位秒</param>         
        /// <param name="priority">缓存项属性</param>         
        private void Add<V>(string key, V value, int cacheDurationInSeconds, CacheDependency dependency, CacheItemPriority priority)
        {
            string keyString = key;
            HttpRuntime.Cache.Insert(keyString, value, dependency, DateTime.Now.AddSeconds(cacheDurationInSeconds), Cache.NoSlidingExpiration, priority, null);
        }

        /// <summary>         
        /// key是否存在       
        /// </summary>         
        /// <param name="key">key</param>         
        /// <returns> /// 	存在<c>true</c> 不存在<c>false</c>.        /// /// </returns>         
        private bool ContainsKey(string key)
        {
            return HttpRuntime.Cache[key] != null;
        }

        /// <summary>         
        ///获取Cache根据key  
        /// </summary>                 
        private V Get<V>(string key)
        {
            return (V)HttpRuntime.Cache[key];
        }
        #endregion


        //PropertyInfo GET SET说明
        //T.GetProperty("key").GetValue(obj, null); //read a key value
        //T.GetProperty("key").SetValue(obj, "", null); //write a value to key
        ////注意如果是字典 
        //T.GetProperty("Item").GetValue(obj, new [] {"id"}); //先拿Item 然后才通过 new[] {这里放指定的key}
    }
}
