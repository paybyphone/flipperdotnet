using System;
using System.Collections.Generic;
using FlipperDotNet.Gate;

namespace FlipperDotNet.Adapter
{
    public class MemoryAdapter : IAdapter
    {
        private readonly HashSet<string> _features = new HashSet<string>();
        private readonly Dictionary<string, object> _dictionary = new Dictionary<string, object>();

        public IDictionary<string, object> Get(Feature feature)
        {
            var result = new Dictionary<string, object>();

            foreach (var gate in feature.Gates)
            {
                if (gate.DataType == typeof (bool))
                {
                    result[gate.Key] = ReadBool(Key(feature, gate));
                }
                else if (gate.DataType == typeof (int))
                {
                    result[gate.Key] = ReadInt(Key(feature, gate));
                }
                else if (gate.DataType == typeof (ISet<string>))
                {
                    result[gate.Key] = ReadSet(Key(feature, gate));
                }
                else
                {
                    throw new NotSupportedException(string.Format("{0} is not supported by this adapter yet", gate.Name));
                }
            }

            return result;
        }

        public void Enable(Feature feature, IGate gate, object thing)
        {
            if (gate.DataType == typeof (bool) || gate.DataType == typeof (int))
            {
                Write(Key(feature, gate), thing.ToString().ToLower());
            }
            else if (gate.DataType == typeof (ISet<string>))
            {
                AddToSet(Key(feature, gate), thing.ToString());
            }
            else
            {
                throw new NotSupportedException(string.Format("{0} is not supported by this adapter yet", gate.Name));
            }
        }

        public void Disable(Feature feature, IGate gate, object thing)
        {
            if (gate.DataType == typeof (bool))
            {
                Clear(feature);
            }
            else if (gate.DataType == typeof (int))
            {
                Write(Key(feature, gate), thing.ToString());
            }
            else if (gate.DataType == typeof (ISet<string>))
            {
                RemoveFromSet(Key(feature, gate), thing.ToString());
            }
            else
            {
                throw new NotSupportedException(string.Format("{0} is not supported by this adapter yet", gate.Name));
            }
        }

        public ISet<string> Features
        {
            get { return _features; }
        }

        public void Add(Feature feature)
        {
            _features.Add(feature.Key);
        }

        public void Remove(Feature feature)
        {
            _features.Remove(feature.Key);
            Clear(feature);
        }

        public void Clear(Feature feature)
        {
            foreach (var gate in feature.Gates)
            {
                Delete(Key(feature, gate));
            }
        }

        private void Write(string key, string value)
        {
            _dictionary[key] = value;
        }

        private void AddToSet(string key, string value)
        {
            EnsureSetExists(key);
            ((ISet<string>) _dictionary[key]).Add(value);
        }

        private void RemoveFromSet(string key, string value)
        {
            EnsureSetExists(key);
            ((ISet<string>)_dictionary[key]).Remove(value);
        }

        private void EnsureSetExists(string key)
        {
            if (! _dictionary.ContainsKey(key))
            {
                _dictionary[key] = new HashSet<string>();
            }
        }

        private string ReadBool(string key)
        {
            string result = null;
            object value;
            if (_dictionary.TryGetValue(key, out value))
            {
                result = (string) value;
            }
            return result;
        }

        private string ReadInt(string key)
        {
            string result = null;
            object value;
            if (_dictionary.TryGetValue(key, out value))
            {
                result = (string)value;
            }
            return result;
        }

        private ISet<string> ReadSet(string key)
        {
            EnsureSetExists(key);
            return new HashSet<string>(_dictionary[key] as IEnumerable<string>);
        }

        private void Delete(string key)
        {
            _dictionary.Remove(key);
        }

        private string Key(Feature feature, IGate gate)
        {
            return feature.Key + "/" + gate.Key;
        }
    }
}
