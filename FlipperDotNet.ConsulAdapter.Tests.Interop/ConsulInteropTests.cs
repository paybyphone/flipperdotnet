using FlipperDotNet;
using FlipperDotNet.ConsulAdapter;
using Consul;
using NUnit.Framework;
using FlipperDotNet.AdapterTests.Interop;

namespace FlipperDotNet.ConsulAdapter.Tests.Interop
{
	[TestFixture]
	public class ConsulInteropTests : SharedAdapterInteropTests
	{
		private IConsulClient client;

		[SetUp]
		public void SetUp()
		{
			client = new Client();
			client.KV.DeleteTree("/");
			adapter = new ConsulAdapter(client);
			flipper = new Flipper(adapter);

			rubyAdapter = new ConsulRubyAdapter();
		}
	}
}

