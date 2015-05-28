using System;
using System.Collections.Generic;
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
            throw new NotImplementedException();
        }

        public void Enable(Feature feature, IGate gate, object b)
        {
            throw new NotImplementedException();
        }

        public void Disable(Feature feature, IGate booleanGate, object b)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public void Clear(Feature feature)
        {
            throw new NotImplementedException();
        }
    }
}
