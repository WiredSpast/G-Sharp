using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Extensions.parsers.stuffdata
{
    // Todo
    public class MapStuffData // : StuffDataBase, IDictionary<string, string>
    {
        public const int IDENTIFIER = 1;

        private IDictionary<string, string> map = new Dictionary<string, string>();

        protected MapStuffData() { }

        public MapStuffData(Dictionary<string, string> map)
        {
            this.map = map == null ? new Dictionary<string, string>() : map
                .Where(e => e.Value != null)
                .ToDictionary(e => e.Key, e => e.Value);
        }

        // TODO
        //public MapStuffData(int uniqueSerialNumber, int uniqueSerialSize, Dictionary<string, string> map)
        //    : base(uniqueSerialNumber, uniqueSerialSize)
        //{
        //    this.map = map == null ? new Dictionary<string, string>() : map
        //        .Where(e => e.Value != null)
        //        .ToDictionary(e => e.Key, e => e.Value);
        //}
        
        // TODO
        //protected override void Initialize(HPacket packet)
        //{
        //    int size = packet.ReadInteger();
        //    Clear();
        //    for (int i = 0; i < size; i++)
        //    {
        //        string key = packet.ReadString();
        //        string value = packet.ReadString();
        //        Add(key, value);
        //    }
        //    base.Initialize(packet);
        //}

        // TODO
        //public override void AppendToPacket(HPacket packet)
        //{
        //    packet.AppendInt(IDENTIFIER | GetFlags());
        //    packet.AppendInt(Count);
        //    foreach (var entry in this)
        //    {
        //        packet.AppendObjects(entry.Key, entry.Value ?? "");
        //    }
        //    base.AppendToPacket(packet);
        //}

        // TODO
        //public override string GetLegacyString()
        //{
        //    return GetOrDefault("state", "");
        //}

        //public override void SetLegacyString(string legacyString)
        //{
        //    this["state"] = legacyString ?? "";
        //}

        #region IDictionary<string, string> Members

        public int Count => map.Count;

        public bool IsReadOnly => map.IsReadOnly;

        public ICollection<string> Keys => map.Keys;

        public ICollection<string> Values => map.Values;

        public string this[string key]
        {
            get => map[key];
            set => map[key] = value;
        }

        public void Add(string key, string value)
        {
            map.Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return map.ContainsKey(key);
        }

        public bool Remove(string key)
        {
            return map.Remove(key);
        }

        public bool TryGetValue(string key, out string value)
        {
            return map.TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<string, string> item)
        {
            ((ICollection<KeyValuePair<string, string>>)map).Add(item);
        }

        public void Clear()
        {
            map.Clear();
        }

        public bool Contains(KeyValuePair<string, string> item)
        {
            return ((ICollection<KeyValuePair<string, string>>)map).Contains(item);
        }

        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<string, string>>)map).CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, string> item)
        {
            return ((ICollection<KeyValuePair<string, string>>)map).Remove(item);
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return map.GetEnumerator();
        }
        
        // TODO
        //IEnumerator IEnumerable.GetEnumerator()
        //{
        //    return map.GetEnumerator();
        //}

        #endregion

        #region Additional Dictionary methods

        public string GetOrDefault(string key, string defaultValue)
        {
            if (TryGetValue(key, out string value))
                return value;
            return defaultValue;
        }

        public void ForEach(Action<string, string> action)
        {
            foreach (var entry in map)
            {
                action(entry.Key, entry.Value);
            }
        }

        public void ReplaceAll(Func<string, string, string> function)
        {
            var entries = map.ToList();
            map.Clear();
            foreach (var entry in entries)
            {
                map.Add(entry.Key, function(entry.Key, entry.Value));
            }
        }

        // TODO
        //public string PutIfAbsent(string key, string value)
        //{
        //    if (!ContainsKey(key))
        //    {
        //        Add(key, value);
        //        return null;
        //    }
        //    return Get(key);
        //}

        public bool Remove(string key, string value)
        {
            return map.Remove(new KeyValuePair<string, string>(key, value));
        }

        public bool Replace(string key, string oldValue, string newValue)
        {
            if (ContainsKey(key) && this[key] == oldValue)
            {
                this[key] = newValue;
                return true;
            }
            return false;
        }

        public string Replace(string key, string value)
        {
            if (ContainsKey(key))
            {
                string oldValue = this[key];
                this[key] = value;
                return oldValue;
            }
            return null;
        }

        // TODO
        //public string ComputeIfAbsent(string key, Func<string, string> mappingFunction)
        //{
        //    if (!ContainsKey(key))
        //    {
        //        string value = mappingFunction(key);
        //        Add(key, value);
        //        return value;
        //    }
        //    return Get(key);
        //}

        // TODO
        //public string ComputeIfPresent(string key, Func<string, string, string> remappingFunction)
        //{
        //    if (ContainsKey(key))
        //    {
        //        string value = remappingFunction(key, Get(key));
        //        if (value != null)
        //        {
        //            this[key] = value;
        //            return value;
        //        }
        //        else
        //        {
        //            Remove(key);
        //        }
        //    }
        //    return null;
        //}

        // TODO
        //public string Compute(string key, Func<string, string, string> remappingFunction)
        //{
        //    if (ContainsKey(key))
        //    {
        //        string oldValue = Get(key);
        //        string newValue = remappingFunction(key, oldValue);
        //        if (newValue != null)
        //        {
        //            this[key] = newValue;
        //            return newValue;
        //        }
        //        else
        //        {
        //            Remove(key);
        //        }
        //    }
        //    return null;
        //}

        // TODO
        //public string Merge(string key, string value, Func<string, string, string, string> remappingFunction)
        //{
        //    if (ContainsKey(key))
        //    {
        //        string oldValue = Get(key);
        //        string newValue = remappingFunction(key, oldValue, value);
        //        if (newValue != null)
        //        {
        //            this[key] = newValue;
        //            return newValue;
        //        }
        //        else
        //        {
        //            Remove(key);
        //        }
        //    }
        //    else if (value != null)
        //    {
        //        Add(key, value);
        //        return value;
        //    }
        //    return null;
        //}

        #endregion
    }
}
