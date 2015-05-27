using System;
using System.Collections.Generic;

namespace FlipperDotNet.Gate
{
    public class GroupGate : IGate
    {
        public const string NAME = "groups";
        public const string KEY = "group";

        public bool IsEnabled(object value)
        {
            return IsEnabled((ISet<string>)value);
        }

        public bool IsEnabled(ISet<string> value)
        {
            return value.Count != 0;
        }

        public bool IsOpen(object thing, object value, string featureName)
        {
            return false;
        }

        public string Name { get { return NAME; } }

        public string Key { get { return KEY; } }

        public Type DataType
        {
            get { return typeof(ISet<string>); }
        }
    }
}