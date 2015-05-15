using FlipperDotNet.Adapter;
using FlipperDotNet.Gate;

namespace FlipperDotNet
{
    public enum FeatureState
    {
        On,
        Off,
        Conditional
    }

    public class Feature
    {
        private BooleanGate _booleanGate = new BooleanGate();

        public Feature(string name, IAdapter adapter)
        {
            Name = name;
            Adapter = adapter;
        }

        public string Name { get; private set; }

        public object Key
        {
            get { return Name; }
        }

        public IAdapter Adapter { get; private set; }

        public void Enable()
        {
            Adapter.Enable(this, BooleanGate, true);
        }

        public void Disable()
        {
            Adapter.Disable(this, BooleanGate, false);
        }

        public FeatureState State
        {
            get
            {
                return GateValues.Boolean.HasValue
                           ? GateValues.Boolean.Value ? FeatureState.On : FeatureState.Off
                           : FeatureState.Off;
            }
        }

        public bool IsOn
        {
            get { return State == FeatureState.On; }
        }

        public bool IsOff
        {
            get { return State == FeatureState.Off; }
        }

        public bool IsConditional
        {
            get { return State == FeatureState.Conditional; }
        }

        public FeatureResult GateValues
        {
            get { return Adapter.Get(this); }
        }

        public bool BooleanValue
        {
            get { return GateValues.Boolean.HasValue && GateValues.Boolean.Value; }
        }

        public IGate BooleanGate
        {
            get { return _booleanGate; }
        }
    }
}
