using System;
using System.Collections.Generic;

namespace FlipperDotNet.Adapter
{
    public class FeatureResult:IEquatable<FeatureResult>
    {
        public FeatureResult()
        {
            Groups = new HashSet<string>();
            Actors = new HashSet<string>();
        }
        public bool? Boolean { get; set; }
        public ISet<string> Groups { get; set; }
        public ISet<string> Actors { get; set; }
        public int PercentageOfActors { get; set; }
        public int PercentageOfTime { get; set; }

        public bool Equals(FeatureResult other)
        {
            return Boolean == other.Boolean &&
                   Groups.SetEquals(other.Groups) &&
                   Actors.SetEquals(other.Actors) &&
                   PercentageOfActors == other.PercentageOfActors &&
                   PercentageOfTime == other.PercentageOfTime;
        }
    }
}
