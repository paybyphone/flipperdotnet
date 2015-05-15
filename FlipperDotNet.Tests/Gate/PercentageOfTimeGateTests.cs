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

    }
}
