using System.Collections.Generic;
using System.Linq;
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
        private readonly List<IGate> _gates =
            new List<IGate>(new IGate[]
                {
                    new BooleanGate(),
                    new GroupGate(),
                    new ActorGate(),
                    new PercentageOfActorsGate(),
                    new PercentageOfTimeGate()
                });

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

        public void EnableActor(IFlipperActor actor)
        {
            Adapter.Add(this);
            Adapter.Enable(this, ActorGate, actor.FlipperId);
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

        public void DisableActor(IFlipperActor actor)
        {
            Adapter.Add(this);
            Adapter.Disable(this, ActorGate, actor.FlipperId);
        }

        public void DisablePercentageOfTime()
        {
            Adapter.Add(this);
            Adapter.Disable(this, PercentageOfTimeGate, 0);
        }

        public void DisablePercentageOfActors()
        {
            Adapter.Add(this);
            Adapter.Disable(this, PercentageOfActorsGate, 0);
        }

        public FeatureState State
        {
            get
            {
                if (GateValues.Boolean || GateValues.PercentageOfActors == 100 || GateValues.PercentageOfTime == 100)
                {
                    return FeatureState.On;
                }
                var nonBooleanGates = from gate in Gates where gate.Name != FlipperDotNet.Gate.BooleanGate.NAME select gate;
                if (nonBooleanGates.Any(x => x.IsEnabled(GateValues[x.Key])))
                {
                    return FeatureState.Conditional;
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

        public GateValues GateValues
        {
            get { return new GateValues(Adapter.Get(this)); }
        }

        public bool BooleanValue
        {
            get { return GateValues.Boolean; }
        }

        public ISet<string> ActorsValue
        {
            get { return GateValues.Actors; }
        }

        public int PercentageOfTimeValue
        {
            get { return GateValues.PercentageOfTime; }
        }

        public int PercentageOfActorsValue
        {
            get { return GateValues.PercentageOfActors; }
        }

        public IGate BooleanGate
        {
            get { return Gate(FlipperDotNet.Gate.BooleanGate.NAME); }
        }

        public IGate GroupGate
        {
            get { return Gate(FlipperDotNet.Gate.GroupGate.NAME); }
        }

        public IGate ActorGate
        {
            get { return Gate(FlipperDotNet.Gate.ActorGate.NAME); }
        }

        public IGate PercentageOfActorsGate
        {
            get { return Gate(FlipperDotNet.Gate.PercentageOfActorsGate.NAME); }
        }

        public IGate PercentageOfTimeGate
        {
            get { return Gate(FlipperDotNet.Gate.PercentageOfTimeGate.NAME); }
        }

        public IList<IGate> Gates
        {
            get { return _gates; }
        }

        public IGate Gate(string name)
        {
            return _gates.Find(x => x.Name == name);
        }

        public IEnumerable<IGate> EnabledGates
        {
            get
            {
                var values = GateValues;
                return from gate in Gates
                       where gate.IsEnabled(values[gate.Key])
                       select gate;
            }
        }

        public IEnumerable<IGate> DisabledGates
        {
            get { return Gates.Except(EnabledGates); }
        }

        public bool IsEnabled
        {
            get
            {
                var values = GateValues;
                return Gates.Any(gate => gate.IsOpen(null, values[gate.Key], Name));
            }
        }

        public bool IsEnabledFor(IFlipperActor actor)
        {
            var values = GateValues;
            return Gates.Any(gate => gate.IsOpen(actor, values[gate.Key], Name));
        }
    }
}
