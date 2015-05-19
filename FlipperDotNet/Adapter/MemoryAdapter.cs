﻿using System;
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
        private readonly Dictionary<string,string> _dictionary = new Dictionary<string, string>();

        public FeatureResult Get(Feature feature)
        {
            var result = new FeatureResult();

            result.Boolean = ReadBool(Key(feature, feature.BooleanGate));
            result.PercentageOfTime = ReadInt(Key(feature, feature.PercentageOfTimeGate));
            result.PercentageOfActors = ReadInt(Key(feature, feature.PercentageOfActorsGate));

            return result;
        }

        public void Enable(Feature feature, IGate gate, object b)
        {
            if (gate.DataType == typeof (bool) || gate.DataType == typeof (int))
            {
                Write(Key(feature, gate), b.ToString());
            }
            else
            {
                throw new NotSupportedException(string.Format("{0} is not supported by this adapter yet", gate.Name));
            }
        }

        public void Disable(Feature feature, IGate gate, object b)
        {
            if (gate.DataType == typeof (bool))
            {
                Clear(feature);
            }else if (gate.DataType == typeof (int))
            {
                Write(Key(feature, gate), b.ToString());
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
        }

        private void Clear(Feature feature)
        {
            Delete(Key(feature, feature.BooleanGate));
            Delete(Key(feature, feature.PercentageOfTimeGate));
        }

        private void Write(string key, string value)
        {
            _dictionary[key] = value;
        }

        private bool? ReadBool(string key)
        {
            bool? result = null;
            string value;
            if (_dictionary.TryGetValue(key, out value))
            {
                result = Convert.ToBoolean(value);
            }
            return result;
        }

        private int ReadInt(string key)
        {
            var result = 0;
            string value;
            if (_dictionary.TryGetValue(key, out value))
            {
                result = Convert.ToInt32(value);
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
