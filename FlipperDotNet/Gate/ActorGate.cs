using System;
using System.Collections.Generic;

namespace FlipperDotNet.Gate
{
    public class ActorGate : IGate
    {
        public const string NAME = "actor";
        public const string KEY = "actors";

        public bool IsEnabled(object value)
        {
            return IsEnabled((ISet<string>) value);
        }

        public bool IsEnabled(ISet<string> value)
        {
            return value.Count != 0;
        }

        public bool IsOpen(object thing, object value, string featureName)
        {
            if (thing == null)
            {
                return false;
            }

            var actor = thing as IFlipperActor;
            if (actor != null)
            {
                var enabledActors = (ISet<string>) value;
                return enabledActors.Contains(actor.FlipperId);
            }
            return false;
        }

        public string Name { get { return NAME; } }

        public string Key { get { return KEY; } }

        public Type DataType
        {
            get { return typeof (ISet<string>); }
        }
    }
}