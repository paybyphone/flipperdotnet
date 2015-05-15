using FlipperDotNet.Adapter;
using NUnit.Framework;

namespace FlipperDotNet.Tests.Adapter
{
    [TestFixture]
    class MemoryAdapterTests : AdapterTests.SharedAdapterTests
    {
        [SetUp]
        public new void SetUp()
        {
            Adapter = new MemoryAdapter();
        }
    }
}
