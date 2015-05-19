using System;

namespace FlipperDotNet.Gate
{
    class ActorGate : IGate
    {
        public bool IsEnabled(object value)
        {
            throw new NotImplementedException();
        }

        public bool IsOpen(object thing, object value, string featureName)
        {
            throw new NotImplementedException();
        }

        public string Name { get { return "actor"; } }

        public string Key { get { return "actors"; } }

        public Type DataType { get; private set; }
    }
}