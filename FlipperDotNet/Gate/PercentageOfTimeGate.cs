using System;

namespace FlipperDotNet.Gate
{
    public class PercentageOfTimeGate : IGate
    {
        public const string NAME = "percentage_of_time";
        public const string KEY = "percentage_of_time";

        private readonly Random _random;

        public PercentageOfTimeGate()
        {
            _random = new Random();
        }

        public PercentageOfTimeGate(Random random)
        {
            _random = random;
        }

        public bool IsEnabled(object value)
        {
            return (int) value > 0;
        }

        public bool IsOpen(object thing, object value, string featureName)
        {
            var percentage = (int) value;
            return _random.NextDouble() < (percentage / 100.0);
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
            get { return typeof (int); }
        }
    }
}
