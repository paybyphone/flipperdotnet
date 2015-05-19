using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlipperDotNet.Adapter;
using FlipperDotNet.Gate;

namespace FlipperDotNet
{
    public class GateValues
    {
        public GateValues(FeatureResult adapterValues)
        {
            Boolean = adapterValues.Boolean.HasValue && adapterValues.Boolean.Value;
            PercentageOfActors = adapterValues.PercentageOfActors;
            PercentageOfTime = adapterValues.PercentageOfTime;
        }

        public bool Boolean { get; private set; }

        public int PercentageOfActors { get; private set; }

        public int PercentageOfTime { get; private set; }

        public object this[string name]
        {
            get
            {
                switch (name)
                {
                    case BooleanGate.KEY:
                        return Boolean;

                    case PercentageOfActorsGate.KEY:
                        return PercentageOfActors;

                    case PercentageOfTimeGate.KEY:
                        return PercentageOfTime;

                    default:
                        return null;
                }
            }
        }
    }
}
