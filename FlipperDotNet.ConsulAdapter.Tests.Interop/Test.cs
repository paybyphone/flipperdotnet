using NUnit.Framework;
using FlipperDotNet;
using Consul;
using FlipperDotNet.ConsulAdapter;
using Rhino.Mocks;

namespace FlipperDotNet.ConsulAdapter.Tests.Interop
{
	[TestFixture]
	public class Test
	{
		private Client client;
		private ConsulAdapter adapter;
		private RubyAdapter rubyAdapter;
		private Flipper flipper;

		[SetUp]
		public void SetUp()
		{
			client = new Client();
			client.KV.DeleteTree("/");
			adapter = new ConsulAdapter(client);
			flipper = new Flipper (adapter);

			rubyAdapter = new RubyAdapter();
		}

		[Test]
		public void ShouldReadABooleanGate()
		{
			const string stats = "Stats";

			rubyAdapter.Enable(stats);

			Assert.That(flipper.Feature (stats).IsEnabled);
		}

		[Test]
		public void ShouldEnableABooleanGateForRuby()
		{
			const string stats = "Stats";

			flipper.Enable(stats);

			Assert.That(rubyAdapter.IsEnabled(stats), Is.True);
		}

		[Test]
		public void ShouldDisableABooleanGateForRuby()
		{
			const string stats = "Stats";

			rubyAdapter.Enable(stats);

			flipper.Disable(stats);

			Assert.That(rubyAdapter.IsEnabled(stats), Is.False);
		}

		[Test]
		public void ShouldReadAnActorGate()
		{
			const string stats = "Stats";
			const string actorId1 = "22";
			const string actorId2 = "asdf";

			rubyAdapter.EnableActor(stats, actorId1);
			rubyAdapter.EnableActor(stats, actorId2);

			Assert.That(flipper.Feature(stats).ActorsValue, Is.EquivalentTo(new[] { actorId1, actorId2 }));
		}

		[Test]
		public void ShouldEnableAnActorForRuby()
		{
			const string stats = "Stats";
			const string actorId1 = "22";
			const string actorId2 = "asdf";

			var actor1 = MockRepository.GenerateStub<IFlipperActor>();
			actor1.Stub(x => x.FlipperId).Return(actorId1);
			flipper.Feature(stats).EnableActor(actor1);

			var actor2 = MockRepository.GenerateStub<IFlipperActor>();
			actor2.Stub(x => x.FlipperId).Return(actorId2);
			flipper.Feature(stats).EnableActor(actor2);

			Assert.That(rubyAdapter.ActorsValue(stats), Is.EquivalentTo(new[] { actorId1, actorId2 }));
		}

		[Test]
		public void ShouldDisableAnActorForRuby()
		{
			const string stats = "Stats";
			const string actorId1 = "22";
			const string actorId2 = "asdf";

			rubyAdapter.EnableActor(stats, actorId1);
			rubyAdapter.EnableActor(stats, actorId2);

			var actor1 = MockRepository.GenerateStub<IFlipperActor>();
			actor1.Stub(x => x.FlipperId).Return(actorId1);
			flipper.Feature(stats).DisableActor(actor1);

			Assert.That(rubyAdapter.ActorsValue(stats), Is.EquivalentTo(new[] { actorId2 }));
		}
	}
}

