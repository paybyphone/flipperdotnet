using System;

namespace FlipperDotNet.Gate
{
    class GroupGate : IGate
    {
        public const string NAME = "groups";
        public const string KEY = "group";

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