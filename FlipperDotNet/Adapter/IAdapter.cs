using System.Collections.Generic;
using FlipperDotNet.Gate;

namespace FlipperDotNet.Adapter
{
    public interface IAdapter
    {
        IDictionary<string, object> Get(Feature feature);
        void Enable(Feature feature, IGate gate, object thing);
        void Disable(Feature feature, IGate gate, object thing);
        ISet<string> Features { get; }
        void Add(Feature feature);
        void Remove(Feature feature);
        void Clear(Feature feature);
		string Name { get; }
    }
}
