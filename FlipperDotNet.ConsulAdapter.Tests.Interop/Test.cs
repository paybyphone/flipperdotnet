using NUnit.Framework;
using FlipperDotNet;
using Consul;
using FlipperDotNet.ConsulAdapter;

namespace FlipperDotNet.ConsulAdapter.Tests.Interop
{
	[TestFixture]
	public class Test
	{
		private Client client;
		private ConsulAdapter adapter;
		private RubyImpl rubyImpl;

		[SetUp]
		public void SetUp()
		{
			client = new Client();
			client.KV.DeleteTree("/");

			rubyImpl = new RubyImpl();
		}

		[Test]
		public void ShouldReadABooleanGate()
		{
			const string stats = "Stats";

			rubyImpl.Enable(stats);

			adapter = new ConsulAdapter(client);
			var flipper = new Flipper(adapter);

			Assert.That(flipper.Feature (stats).IsEnabled);
		}

		[Test]
		public void ShouldEnableABooleanGateForRuby()
		{
			const string stats = "Stats";

			adapter = new ConsulAdapter(client);
			var flipper = new Flipper(adapter);

			flipper.Enable(stats);

			Assert.That(rubyImpl.IsEnabled(stats), Is.True);
		}

		[Test]
		public void ShouldDisableABooleanGateForRuby()
		{
			const string stats = "Stats";

			adapter = new ConsulAdapter(client);
			var flipper = new Flipper(adapter);

			rubyImpl.Enable(stats);

			flipper.Disable(stats);

			Assert.That(rubyImpl.IsEnabled(stats), Is.False);
		}
	}

}

