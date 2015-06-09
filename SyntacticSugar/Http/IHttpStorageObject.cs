using System;
namespace SyntacticSugar
{
    public abstract class IHttpStorageObject<V>
    {

        public int Minutes = 60;
        public int Hour = 60 * 60;
        public int Day = 60 * 60 * 24;
        public abstract void Add(string key, V value);
        public abstract void Add(string key, V value, int cacheDurationInSeconds);
        public abstract bool ContainsKey(string key);
        public abstract V Get(string key);
        public abstract global::System.Collections.Generic.IEnumerable<string> GetAllKey();
        public abstract void Remove(string key);
        public abstract void RemoveAll();
        public abstract void RemoveAll(Func<string, bool> removeExpression);
        public abstract V this[string key] { get; }
    }
}
