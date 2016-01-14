using FlipperDotNet.Gate;
using NUnit.Framework;

namespace FlipperDotNet.Tests.Gate
{
    [TestFixture]
    class BooleanGateTests
    {
        [TestCase(true, ExpectedResult = true)]
        [TestCase(false, ExpectedResult = false)]
        public bool IsEnabled(bool value)
        {
            var gate = new BooleanGate();
            return gate.IsEnabled(value);
        }

        [TestCase(true, ExpectedResult = true)]
        [TestCase(false, ExpectedResult = false)]
        public bool IsOpen(bool value)
        {
            var gate = new BooleanGate();
            return gate.IsOpen(new object(), value);
        }

		[TestCase(true, ExpectedResult = true)]
		[TestCase(false, ExpectedResult = false)]
		public object WrapValueReturnsTheValue(bool value)
		{
			var gate = new BooleanGate();
			return gate.WrapValue(value);
		}
    }
}
