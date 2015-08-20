using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using FlipperDotNet.Adapter;
using FlipperDotNet.Gate;

namespace FlipperDotNet.ConsulAdapter
{
    public class ConsulAdapter : IAdapter
    {
        public const string FeaturesKey = "flipper_features";

        private readonly Consul.Client _client;
        private readonly string _namespace;

        public ConsulAdapter(Consul.Client client, string rootNamespace)
        {
            _client = client;
            _namespace = rootNamespace.TrimStart('/');
        }

        public ConsulAdapter(Consul.Client client)
        {
            _client = client;
            _namespace = "";
        }

        public string Namespace
        {
            get { return _namespace; }
        }

        public IDictionary<string, object> Get(Feature feature)
        {
            var result = new Dictionary<string, object>();

            var values = GetFeatureValues(feature);

            foreach (var gate in feature.Gates)
            {
				if (gate.DataType == typeof(bool) || gate.DataType == typeof(int))
				{
					result[gate.Key] = ReadValue(values, gate);
				}
                else if (gate.DataType == typeof (ISet<string>))
                {
                    result[gate.Key] = ReadSet(values, gate);
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
            if (gate.DataType == typeof (bool))
            {
                WriteBool(Key(feature, gate), (bool) thing);
            }
            else if (gate.DataType == typeof (int))
            {
                WriteInt(Key(feature, gate), (int) thing);
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
                WriteInt(Key(feature, gate), (int) thing);
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
            get
            {
                var keyPath = BuildPath(string.Format("{0}/features", FeaturesKey));
                return ReadSet(keyPath);
            }
        }

        public void Add(Feature feature)
        {
            var pair = new Consul.KVPair(BuildPath(string.Format("{0}/features/{1}", FeaturesKey, feature.Key)))
                {
                    Value = Encoding.UTF8.GetBytes("1")
                };
            _client.KV.Put(pair);
        }

        public void Remove(Feature feature)
        {
            _client.KV.DeleteTree(BuildPath(string.Format("{0}/features/{1}", FeaturesKey, feature.Key)));
            Clear(feature);
        }

        public void Clear(Feature feature)
        {
            _client.KV.DeleteTree(BuildPath(feature.Key));
        }

        private string Key(Feature feature, IGate gate)
        {
            return BuildPath(feature.Key + "/" + gate.Key);
        }

        private IDictionary<string, object> GetFeatureValues(Feature feature)
        {

            var result = new Dictionary<string, object>();
            var keyPath = BuildPath(feature.Key);
            var values = _client.KV.List(keyPath);
            if (values.Response != null)
            {
                foreach (var member in values.Response)
                {
                    result.Add(member.Key.Replace(keyPath + "/", ""), Encoding.UTF8.GetString(member.Value));
                }
            }
            return result;
        }

        private static object ReadValue(IDictionary<string, object> values, IGate gate)
        {
            object value;
            if (values.TryGetValue(gate.Key, out value))
            {
                return value;
            }
            return null;
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

        private static ISet<string> ReadSet(IDictionary<string, object> values, IGate gate)
        {
            var keysFromSet = from key in values.Keys
                              where key.StartsWith(gate.Key)
                              select key.Split('/')[1];
            var value = new HashSet<string>(keysFromSet);
            return value;
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

        private string BuildPath(string key)
        {
            if (Namespace != string.Empty)
            {
                return string.Join("/", Namespace, key);
            }
            return key;
        }
    }
}
