using FlipperDotNet.Adapter;

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
        private bool _enabled;

        public Feature(string name, IAdapter adapter)
        {
            Name = name;
            Adapter = adapter;
        }

        public string Name { get; private set; }

        public IAdapter Adapter { get; private set; }

        public void Enable()
        {
            _enabled = true;
        }

        public void Disable()
        {
            _enabled = false;
        }

        public FeatureState State
        {
            get { return _enabled ? FeatureState.On : FeatureState.Off; }
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

        public object BooleanValue
        {
            get { return _enabled; }
        }
    }
}
