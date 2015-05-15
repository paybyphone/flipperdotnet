using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipperDotNet.Adapter
{
    public class FeatureResult:IEquatable<FeatureResult>
    {
        private readonly HashSet<string> _groups = new HashSet<string>();
        private readonly HashSet<string> _actors = new HashSet<string>();

        public bool? Boolean { get; set; }
        public ISet<string> Groups
        {
            get { return _groups; }
        }
        public ISet<string> Actors
        {
            get { return _actors; }
        }
        public int? PercentageOfActors { get; set; }
        public int? PercentageOfTime { get; set; }

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
