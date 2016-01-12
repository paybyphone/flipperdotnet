using System;
using FlipperDotNet.Gate;
using NUnit.Framework;

namespace FlipperDotNet.Tests.Gate
{
    [TestFixture]
    class PercentageOfTimeGateTests
    {
        [TestCase(0, ExpectedResult = false)]
        [TestCase(1, ExpectedResult = true)]
        public bool IsEnabled(int value)
        {
            var gate = new PercentageOfTimeGate();
            return gate.IsEnabled(value);
        }

        [TestCase(1, 50, ExpectedResult = true)]
        [TestCase(1, 20, ExpectedResult = false)]
        public bool IsOpen(int seed, int percentage)
        {
            var random = new Random(seed);
            var gate = new PercentageOfTimeGate(random);
            return gate.IsOpen(null, percentage, "Feature");
        }

		[TestCase(0, ExpectedResult = 0)]
		[TestCase(1, ExpectedResult = 1)]
		public object WrapValueReturnsTheValue(int value)
		{
			var gate = new PercentageOfTimeGate();
			return gate.WrapValue(value);
		}
    }
}
