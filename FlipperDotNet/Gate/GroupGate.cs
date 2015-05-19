using System;

namespace FlipperDotNet.Gate
{
    class GroupGate : IGate
    {
        public bool IsEnabled(object value)
        {
            throw new NotImplementedException();
        }

        public bool IsOpen(object thing, object value, string featureName)
        {
            throw new NotImplementedException();
        }

        public string Name { get { return "group"; } }

        public string Key { get { return "groups"; } }

        public Type DataType { get; private set; }
    }
}