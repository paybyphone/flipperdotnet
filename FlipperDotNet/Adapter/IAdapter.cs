using System.Collections.Generic;
using FlipperDotNet.Gate;

namespace FlipperDotNet.Adapter
{
    public interface IAdapter
    {
        FeatureResult Get(Feature feature);
        void Enable(Feature feature, IGate gate, object b);
        void Disable(Feature feature, IGate gate, object b);
        ISet<string> Features { get; }
        void Add(Feature feature);
        void Remove(Feature feature);
        void Clear(Feature feature);
    }
}
