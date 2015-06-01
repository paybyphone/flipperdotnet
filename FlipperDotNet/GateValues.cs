using System;
using System.Collections.Generic;
using FlipperDotNet.Gate;

namespace FlipperDotNet
{
    public class GateValues : IEquatable<GateValues>
    {
        private  HashSet<string> _groups;
        private  HashSet<string> _actors;

        public GateValues(IDictionary<string, object> adapterValues)
        {
            //Boolean = adapterValues.Boolean.HasValue && adapterValues.Boolean.Value;
            //Groups = adapterValues.Groups;
            //Actors = adapterValues.Actors;
            //PercentageOfActors = adapterValues.PercentageOfActors;
            //PercentageOfTime = adapterValues.PercentageOfTime;
        }

        public bool Boolean { get; private set; }

        public ISet<string> Groups
        {
            get { return _groups; }
            private set { _groups = new HashSet<string>(value); }
        }

        public ISet<string> Actors
        {
            get { return _actors; }
            private set { _actors = new HashSet<string>(value); }
        }

        public int PercentageOfActors { get; private set; }

        public int PercentageOfTime { get; private set; }

        public object this[string key]
        {
            get
            {
                switch (key)
                {
                    case BooleanGate.KEY:
                        return Boolean;

                    case ActorGate.KEY:
                        return Actors;

                    case GroupGate.KEY:
                        return Groups;

                    case PercentageOfActorsGate.KEY:
                        return PercentageOfActors;

                    case PercentageOfTimeGate.KEY:
                        return PercentageOfTime;

                    default:
                        return null;
                }
            }
        }

        public bool Equals(GateValues other)
        {
            return Boolean == other.Boolean &&
                   Groups.SetEquals(other.Groups) &&
                   Actors.SetEquals(other.Actors) &&
                   PercentageOfActors == other.PercentageOfActors &&
                   PercentageOfTime == other.PercentageOfTime;
        }
    }
}
