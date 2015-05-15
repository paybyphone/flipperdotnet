namespace FlipperDotNet.Gate
{
    public interface IGate
    {
        bool IsEnabled(bool value);
        bool IsOpen(object thing, bool value);
        string Key { get; }
    }
}