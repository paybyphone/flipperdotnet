using System;

namespace FlipperDotNet.Gate
{
    public interface IGate
    {
        bool IsEnabled(object value);
        bool IsOpen(object thing, object value, string featureName);
        string Name { get; }
        string Key { get; }
        Type DataType { get; }
    }
}