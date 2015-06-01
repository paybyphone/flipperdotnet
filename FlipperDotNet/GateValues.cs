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
            Boolean = ParseBoolean(GetAdapterValue(adapterValues, BooleanGate.KEY));
            Groups = CopySet(GetAdapterValue(adapterValues, GroupGate.KEY));
            Actors = CopySet(GetAdapterValue(adapterValues, ActorGate.KEY));
            PercentageOfActors = ParseInt(GetAdapterValue(adapterValues, PercentageOfActorsGate.KEY));
            PercentageOfTime = ParseInt(GetAdapterValue(adapterValues, PercentageOfTimeGate.KEY));
        }

        private static object GetAdapterValue(IDictionary<string, object> adapterValues, string key)
        {
            object value;
            adapterValues.TryGetValue(key, out value);
            return value;
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

        private static bool ParseBoolean(object adapterValue)
        {
            if (adapterValue is bool && ((bool)adapterValue))
            {
                return true;
            }
            if (adapterValue is int && (int)adapterValue == 1)
            {
                return true;
            }
            var stringValue = adapterValue as string;
            if (stringValue != null)
            {
                if (stringValue == "1" || stringValue.ToLower() == "true")
                {
                    return true;
                }
            }
            return false;
        }

        private static int ParseInt(object adapterValue)
        {
            if (adapterValue is int)
            {
                return (int) adapterValue;
            }
            var stringValue = adapterValue as string;
            if (stringValue != null)
            {
                if (string.IsNullOrEmpty(stringValue))
                {
                    return 0;
                }
                return Convert.ToInt32(adapterValue);
            }
            return 0;
        }

        private static ISet<string> CopySet(object value)
        {
            var set = value as ISet<string>;
            if (set != null)
            {
                return set;
            }
            return new HashSet<string>();
        }
    }
}
