using System.Collections.Generic;
using System.Linq;
using FlipperDotNet.Adapter;
using FlipperDotNet.Gate;
using FlipperDotNet.Instrumenter;
using System;

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
		public const string InstrumentationName = "feature_operation" + "." + Flipper.InstrumentationNamespace;
		public const string GateInstrumentationName = "gate_operation" + "." + Flipper.InstrumentationNamespace;

        private readonly List<IGate> _gates =
            new List<IGate>(new IGate[]
                {
                    new BooleanGate(),
                    new GroupGate(),
                    new ActorGate(),
                    new PercentageOfActorsGate(),
                    new PercentageOfTimeGate()
                });

		public Feature(string name, IAdapter adapter) : this(name, adapter, new NoOpInstrumenter())
        { }

		public Feature(string name, IAdapter adapter, IInstrumenter instrumenter)
		{
			if (instrumenter == null)
			{
				throw new ArgumentNullException("instrumenter");
			}
			Name = name;
			Adapter = adapter;
			Instrumenter = instrumenter;
		}

        public string Name { get; private set; }

        public string Key
        {
            get { return Name; }
        }

        public IAdapter Adapter { get; private set; }

		public IInstrumenter Instrumenter { get; private set; }

        public void Enable()
		{
			Enable(BooleanGate, true);
		}

        public void EnableActor(IFlipperActor actor)
		{
			Enable(ActorGate, actor);
		}

        public void EnablePercentageOfTime(int percentage)
        {
			ValidatePercentage(percentage);
			Enable(PercentageOfTimeGate, percentage);
        }

        public void EnablePercentageOfActors(int percentage)
        {
			ValidatePercentage(percentage);
			Enable(PercentageOfActorsGate, percentage);
        }

		private void Enable(IGate gate, object value)
		{
			var payload = new InstrumentationPayload {
				FeatureName = Name,
				GateName = gate.Name,
				Operation = "enable",
				Thing = value,
			};
			using (Instrumenter.Instrument(InstrumentationName, payload))
			{
				try
				{
					Adapter.Add(this);
					Adapter.Enable(this, gate, gate.WrapValue(value));
				} catch (Exception e)
				{
					throw new AdapterRequestException(string.Format("Failed to enable feature {0}", Name), e);
				}
			}
		}

		static void ValidatePercentage(int percentage)
		{
			if (percentage < 0 || percentage > 100)
			{
				throw new ArgumentException(string.Format("Value must be a positive number less than or equal to 100, but was {0}", percentage));
			}
		}

        public void Disable()
        {
            Disable(BooleanGate, false);
        }

        public void DisableActor(IFlipperActor actor)
        {
            Disable(ActorGate, actor);
        }

        public void DisablePercentageOfTime()
        {
            Disable(PercentageOfTimeGate, 0);
        }

        public void DisablePercentageOfActors()
        {
            Disable(PercentageOfActorsGate, 0);
        }

		private void Disable(IGate gate, object value)
		{
			var payload = new InstrumentationPayload {
				FeatureName = Name,
				GateName = gate.Name,
				Operation = "disable",
				Thing = value,
			};
			using (Instrumenter.Instrument(InstrumentationName, payload))
			{
				try
				{
					Adapter.Add(this);
					Adapter.Disable(this, gate, gate.WrapValue(value));
				} catch (Exception e)
				{
					throw new AdapterRequestException(string.Format("Failed to disable feature {0}", Name), e);
				}
			}
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
			get
			{ 
				try
				{
					return new GateValues(Adapter.Get(this));
				} catch (Exception e)
				{
					throw new AdapterRequestException(string.Format("Unable to retrieve feature values for {0}", Name), e);
				}
			}
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

		public bool IsEnabled()
		{
			return IsEnabled(null);
		}

        public bool IsEnabledFor(IFlipperActor actor)
        {
			return IsEnabled(actor);
        }

		private bool IsEnabled(object thing)
		{
			var payload = new InstrumentationPayload {
				FeatureName = Name,
				Operation = "enabled?",
			};
			if (thing != null)
			{
				payload.Thing = thing;
			}
			using (Instrumenter.Instrument(InstrumentationName, payload))
			{
				var values = GateValues;
				var openGate = Gates.FirstOrDefault(gate => InstrumentGate(gate, "open?", thing, x => x.IsOpen(thing, values[x.Key], Name)));
				bool result;
				if (openGate != null)
				{
					payload.GateName = openGate.Name;
					result = true;
				} else
				{
					result = false;
				}
				payload.Result = result;
				return result;
			}
		}

		private bool InstrumentGate(IGate gate, string operation, object thing, Func<IGate,bool> function)
		{
			var payload = new InstrumentationPayload {
				FeatureName = Name,
				GateName = gate.Name,
				Operation = operation,
				Thing = thing,
			};
			using(Instrumenter.Instrument(GateInstrumentationName, payload))
			{
				return function(gate);
			}
		}
    }
}
