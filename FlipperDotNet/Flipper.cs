using System.Collections.Generic;
using FlipperDotNet.Adapter;

namespace FlipperDotNet
{
    public class Flipper
    {
        private readonly Dictionary<string, Feature> _features = new Dictionary<string, Feature>();

        public Flipper(IAdapter adapter)
        {
            Adapter = adapter;
        }

        public IAdapter Adapter { get; private set; }

        public Feature Feature(string name)
        {
            Feature feature;
            if (!_features.TryGetValue(name, out feature))
            {
                feature = new Feature(name, Adapter);
                _features.Add(name, feature);
            }
            return feature;
        }

        public void Enable(string name)
        {
            Feature(name).Enable();
        }

        public void Disable(string name)
        {
            Feature(name).Disable();
        }
    }
}
