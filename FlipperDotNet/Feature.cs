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
        private PercentageOfActorsGate _percentageOfActorsGate = new PercentageOfActorsGate();
        private PercentageOfTimeGate _percentageOfTimeGate = new PercentageOfTimeGate();

        public Feature(string name, IAdapter adapter)
        {
            Name = name;
            Adapter = adapter;
        }

        public string Name { get; private set; }

        public string Key
        {
            get { return Name; }
        }

        public IAdapter Adapter { get; private set; }

        public void Enable()
        {
            Adapter.Add(this);
            Adapter.Enable(this, BooleanGate, true);
        }

        public void EnablePercentageOfTime(int percentage)
        {
            Adapter.Add(this);
            Adapter.Enable(this, PercentageOfTimeGate, percentage);
        }

        public void EnablePercentageOfActors(int percentage)
        {
            Adapter.Add(this);
            Adapter.Enable(this, PercentageOfActorsGate, percentage);
        }

        //private void Enable()

        public void Disable()
        {
            Adapter.Add(this);
            Adapter.Disable(this, BooleanGate, false);
        }

        public FeatureState State
        {
            get
            {
                if (GateValues.Boolean.HasValue && GateValues.Boolean.Value || GateValues.PercentageOfActors == 100 || GateValues.PercentageOfTime == 100)
                {
                    return FeatureState.On;
                }
                return FeatureState.Off;
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

        public object PercentageOfTimeValue
        {
            get { return GateValues.PercentageOfTime; }
        }

        public IGate BooleanGate
        {
            get { return _booleanGate; }
        }

        public IGate PercentageOfActorsGate
        {
            get { return _percentageOfActorsGate; }
        }

        public IGate PercentageOfTimeGate
        {
            get { return _percentageOfTimeGate; }
        }
    }
}
