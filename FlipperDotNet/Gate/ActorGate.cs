using System;

namespace FlipperDotNet.Gate
{
    class ActorGate : IGate
    {
        public const string NAME = "actor";
        public const string KEY = "actors";

        public bool IsEnabled(object value)
        {
            throw new NotImplementedException();
        }

        public bool IsOpen(object thing, object value, string featureName)
        {
            throw new NotImplementedException();
        }

        public string Name { get { return NAME; } }

        public string Key { get { return KEY; } }

        public Type DataType { get; private set; }
    }
}