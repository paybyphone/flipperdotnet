using System;

namespace FlipperDotNet.Gate
{
    public class BooleanGate : IGate
    {
        public bool IsEnabled(object value)
        {
            return IsEnabled((bool) value);
        }

        public bool IsEnabled(bool value)
        {
            return value;
        }

        public bool IsOpen(object thing, object value, string featureName)
        {
            return IsOpen(thing, (bool) value);
        }

        public bool IsOpen(object thing, bool value)
        {
            return value;
        }

        public string Name
        {
            get { return Key; }
        }

        public string Key
        {
            get { return "boolean"; }
        }

        public Type DataType
        {
            get { return typeof (bool); }
        }
    }
}
