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
        private readonly Dictionary<string,string> _dictionary = new Dictionary<string, string>();

        public FeatureResult Get(Feature feature)
        {
            var result = new FeatureResult();

            result.Boolean = Read(Key(feature, feature.BooleanGate));

            return result;
        }

        public void Enable(Feature feature, IGate gate, bool b)
        {
            Write(Key(feature, gate), b.ToString());
        }

        public void Disable(Feature feature, IGate booleanGate, bool b)
        {
            Clear(feature);
        }

        private void Clear(Feature feature)
        {
            Delete(Key(feature, feature.BooleanGate));
        }

        private void Write(string key, string value)
        {
            _dictionary[key] = value;
        }

        private bool? Read(string key)
        {
            bool? result = null;
            string value;
            if (_dictionary.TryGetValue(key, out value))
            {
                result = Convert.ToBoolean(value);
            }
            return result;
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
