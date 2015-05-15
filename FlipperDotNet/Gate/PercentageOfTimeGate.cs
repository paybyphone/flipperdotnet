using System;

namespace FlipperDotNet.Gate
{
    public class PercentageOfTimeGate : IGate
    {
        private readonly Random _random = new Random();

        public bool IsEnabled(object value)
        {
            return (int) value > 0;
        }

        public bool IsOpen(object thing, object value)
        {
            var percentage = (int) value;
            return _random.NextDouble() < (percentage/100.0);
        }

        public string Name { get; private set; }

        public string Key
        {
            get { return "percentage_of_time"; }
        }

        public Type DataType
        {
            get { return typeof (int); }
        }
    }
}
