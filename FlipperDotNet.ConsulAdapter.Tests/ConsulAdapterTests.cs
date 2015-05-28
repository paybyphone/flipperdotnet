using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace FlipperDotNet.ConsulAdapter.Tests
{
    [TestFixture]
    class ConsulAdapterTests : AdapterTests.SharedAdapterTests
    {
        [SetUp]
        public new void SetUp()
        {
            var client = new Consul.Client();
            Adapter = new ConsulAdapter(client);
            client.KV.DeleteTree("/");
        }
    }
}
