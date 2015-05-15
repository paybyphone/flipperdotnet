using FlipperDotNet.Gate;

namespace FlipperDotNet.Adapter
{
    public interface IAdapter
    {
        FeatureResult Get(Feature feature);
        void Enable(Feature feature, IGate gate, bool b);
        void Disable(Feature feature, IGate booleanGate, bool b);
    }
}
