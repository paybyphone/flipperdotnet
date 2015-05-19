using System;

namespace FlipperDotNet.Gate
{
    public class BooleanGate : IGate
    {
        public const string NAME = "boolean";
        public const string KEY = "boolean";

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
            get { return NAME; }
        }

        public string Key
        {
            get { return KEY; }
        }

        public Type DataType
        {
            get { return typeof (bool); }
        }
    }
}
