using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SyntacticSugar
{
    public static class DictionaryExtensions
    {
        public static string GetTryValue(this Dictionary<string, string> thisObj, string key)
        {
            if (thisObj == null) return null;
            if (!thisObj.ContainsKey(key)) return null;
            return thisObj[key]+"";
        }
    }
}
