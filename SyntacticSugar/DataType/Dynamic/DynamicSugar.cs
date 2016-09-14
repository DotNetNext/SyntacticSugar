using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
using System.Reflection;

namespace SyntacticSugar
{
    public class DynamicSugar
    {
        private class DynamicDictionary : DynamicObject
        {
            private readonly Dictionary<string, object> dictionary;

            public DynamicDictionary(Dictionary<string, object> dictionary)
            {
                this.dictionary = dictionary;
            }

            public override bool TryGetMember(
                GetMemberBinder binder, out object result)
            {
                return dictionary.TryGetValue(binder.Name, out result);
            }

            public override bool TrySetMember(
                SetMemberBinder binder, object value)
            {
                dictionary[binder.Name] = value;

                return true;
            }
        }

        /// <summary>
        /// 将Dictionary转成Dynamic
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static dynamic DictionaryToDynamic(Dictionary<string, object> value)
        {
            return new DynamicDictionary(value);
        }

        /// <summary>
        /// 将Dynamic转成Dictionary
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public static Dictionary<string, object> DynamicToDictionary<T>(T objeto)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();

            Type type = objeto.GetType();
            FieldInfo[] field = type.GetFields();
            PropertyInfo[] myPropertyInfo = type.GetProperties();

            object value = null;

            foreach (var propertyInfo in myPropertyInfo)
            {
                if (propertyInfo.GetIndexParameters().Length == 0)
                {
                    value = propertyInfo.GetValue(objeto, null);
                    value = value == null ? null : value;
                    dictionary.Add(propertyInfo.Name,value);
                }
            }

            return dictionary;
        }
    }
}
