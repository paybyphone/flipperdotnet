namespace FlipperDotNet.Gate
{
    public class BooleanGate : IGate
    {
        public bool IsEnabled(bool value)
        {
            return value;
        }

        public bool IsOpen(object thing, bool value)
        {
            return value;
        }

        public string Key
        {
            get { return "boolean"; }
        }
    }
}
