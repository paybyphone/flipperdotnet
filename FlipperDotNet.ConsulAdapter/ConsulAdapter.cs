using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using FlipperDotNet.Adapter;
using FlipperDotNet.Gate;

namespace FlipperDotNet.ConsulAdapter
{
    public class ConsulAdapter : IAdapter
    {
        public const string FeaturesKey = "flipper_features";

        private readonly Consul.Client _client;

        public ConsulAdapter(Consul.Client client)
        {
            _client = client;
        }

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
            if (gate.DataType == typeof(bool))
            {
                WriteBool(Key(feature, gate), (bool) thing);
            }
            else if (gate.DataType == typeof(int))
            {
                WriteInt(Key(feature, gate), (int) thing);
            }
            else if (gate.DataType == typeof(ISet<string>))
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
            if (gate.DataType == typeof(bool))
            {
                Clear(feature);
            }
            else if (gate.DataType == typeof(int))
            {
                WriteInt(Key(feature, gate), (int)thing);
            }
            else if (gate.DataType == typeof(ISet<string>))
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
            get
            {
                var keyPath = string.Format("{0}/features", FeaturesKey);

                var features = new HashSet<string>();
                var featuresResult = _client.KV.List(keyPath);
                if (featuresResult.Response != null)
                {
                    foreach (var feature in featuresResult.Response)
                    {
                        features.Add(feature.Key.Replace(keyPath + "/", ""));
                    }
                }
                return features;
            }
        }

        public void Add(Feature feature)
        {
            var pair = new Consul.KVPair(string.Format("{0}/features/{1}", FeaturesKey, feature.Key))
                {
                    Value = Encoding.UTF8.GetBytes("1")
                };
            _client.KV.Put(pair);
        }

        public void Remove(Feature feature)
        {
            _client.KV.DeleteTree(string.Format("{0}/features/{1}", FeaturesKey, feature.Key));
            Clear(feature);
        }

        public void Clear(Feature feature)
        {
            _client.KV.DeleteTree(feature.Key);
        }

        private string Key(Feature feature, IGate gate)
        {
            return feature.Key + "/" + gate.Key;
        }

        private bool? ReadBool(string key)
        {
            bool? result = null;
            var value = _client.KV.Get(key);
            if (value.Response != null)
            {
                result = bool.Parse(Encoding.UTF8.GetString(value.Response.Value));
            }
            return result;
        }

        private int ReadInt(string key)
        {
            var result = 0;
            var value = _client.KV.Get(key);
            if (value.Response != null)
            {
                result = Convert.ToInt32(Encoding.UTF8.GetString(value.Response.Value));
            }
            return result;
        }

        private ISet<string> ReadSet(string keyPath)
        {
            var values = new HashSet<string>();
            var valuesResult = _client.KV.List(keyPath);
            if (valuesResult.Response != null)
            {
                foreach (var member in valuesResult.Response)
                {
                    values.Add(member.Key.Replace(keyPath + "/", ""));
                }
            }
            return values;
        }

        private void WriteBool(string key, bool value)
        {
            WriteValue(key, value.ToString().ToLower());
        }

        private void WriteValue(string key, string value)
        {
            var pair = new Consul.KVPair(key)
                {
                    Value = Encoding.UTF8.GetBytes(value),
                };
            _client.KV.Put(pair);
        }

        private void WriteInt(string key, int value)
        {
            WriteValue(key, value.ToString(CultureInfo.InvariantCulture).ToLower());
        }

        private void AddToSet(string key, string value)
        {
            WriteValue(SetMemberKey(key, value), "1");
        }

        private void RemoveFromSet(string key, string value)
        {
            _client.KV.Delete(SetMemberKey(key, value));
        }

        private static string SetMemberKey(string key, string value)
        {
            return string.Format("{0}/{1}", key, value);
        }
    }
}
