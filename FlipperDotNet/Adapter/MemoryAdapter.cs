using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlipperDotNet.Adapter;
using FlipperDotNet.Gate;

namespace FlipperDotNet.Adapter
{
    public class MemoryAdapter : IAdapter
    {
        private readonly HashSet<string> _features = new HashSet<string>();
        private readonly Dictionary<string, object> _dictionary = new Dictionary<string, object>();

        public FeatureResult Get(Feature feature)
        {
            var result = new FeatureResult();

            result.Boolean = ReadBool(Key(feature, feature.BooleanGate));
            result.Groups = ReadSet(Key(feature, feature.GroupGate));
            result.Actors = ReadSet(Key(feature, feature.ActorGate));
            result.PercentageOfTime = ReadInt(Key(feature, feature.PercentageOfTimeGate));
            result.PercentageOfActors = ReadInt(Key(feature, feature.PercentageOfActorsGate));

            return result;
        }

        public void Enable(Feature feature, IGate gate, object thing)
        {
            if (gate.DataType == typeof (bool) || gate.DataType == typeof (int))
            {
                Write(Key(feature, gate), thing.ToString());
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

        private bool? ReadBool(string key)
        {
            bool? result = null;
            object value;
            if (_dictionary.TryGetValue(key, out value))
            {
                result = Convert.ToBoolean(value);
            }
            return result;
        }

        private int ReadInt(string key)
        {
            var result = 0;
            object value;
            if (_dictionary.TryGetValue(key, out value))
            {
                result = Convert.ToInt32(value);
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
