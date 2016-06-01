using Consul;
using NUnit.Framework;
using FlipperDotNet.AdapterTests.Interop;

namespace FlipperDotNet.ConsulAdapter.Tests.Interop
{
    [TestFixture]
    public class ConsulInteropTests : SharedAdapterInteropTests
    {
        private IConsulClient _client;

        [SetUp]
        public void SetUp()
        {
            _client = new ConsulClient();
            var pairs = _client.KV.Keys("/", "/").Result;
            foreach (var key in pairs.Response)
            {
                _client.KV.DeleteTree(key).Wait();
            }
            adapter = new ConsulAdapter(_client);
            flipper = new Flipper(adapter);

            rubyAdapter = new ConsulRubyAdapter();
        }
    }
}

