using System;
using Klinkby.Checkum;

namespace FlipperDotNet.Gate
{
    public class PercentageOfActorsGate : IGate
    {
        public const string NAME = "percentage_of_actors";
        public const string KEY = "percentage_of_actors";

        public bool IsEnabled(object value)
        {
            return (int) value > 0;
        }

        public bool IsOpen(object thing, object value, string featureName)
        {
            var actor = thing as IFlipperActor;
            if (actor != null)
            {
                var percentage = (int) value;
                var key = featureName + actor.FlipperId;
                var crc = Crc32.ComputeChecksum(key);
                return crc % 100 < percentage;
            }
            else
            {
                return false;
            }
        }

        public string Name
        {
            get { return NAME; }
        }

        public string Key
        {
            get { return KEY; }
        }

        public Type DataType
        {
            get { return typeof (int); }
        }
    }
}
