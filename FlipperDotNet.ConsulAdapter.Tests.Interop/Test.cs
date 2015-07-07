using NUnit.Framework;
using FlipperDotNet;
using Consul;
using FlipperDotNet.ConsulAdapter;
using Rhino.Mocks;
using System;

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

		[Test]
		public void ShouldReadAPercentageOfActorsGate()
		{
			const string stats = "Stats";
			const int percentage = 10;

			rubyAdapter.EnablePercentageOfActors(stats, percentage);

			Assert.That(flipper.Feature(stats).PercentageOfActorsValue, Is.EqualTo(percentage));
		}

		[Test]
		public void ShouldEnablePercentageOfActorsForRuby()
		{
			const string stats = "Stats";
			const int percentage = 10;

			flipper.Feature(stats).EnablePercentageOfActors(percentage);

			Assert.That(rubyAdapter.PercentageOfActorsValue(stats), Is.EqualTo(percentage));
		}

		[Test]
		public void ShouldDisablePercentageOfActorsForRuby()
		{
			const string stats = "Stats";
			const int percentage = 10;

			rubyAdapter.EnablePercentageOfActors(stats, percentage);

			flipper.Feature(stats).DisablePercentageOfActors();

			Assert.That(rubyAdapter.PercentageOfActorsValue(stats), Is.EqualTo(0));
		}

		[Test]
		public void ShouldReadAPercentageOfTimeGate()
		{
			const string stats = "Stats";
			const int percentage = 10;

			rubyAdapter.EnablePercentageOfTime(stats, percentage);

			Assert.That(flipper.Feature(stats).PercentageOfTimeValue, Is.EqualTo(percentage));
		}

		[Test]
		public void ShouldEnablePercentageOfTimeForRuby()
		{
			const string stats = "Stats";
			const int percentage = 10;

			flipper.Feature(stats).EnablePercentageOfTime(percentage);

			Assert.That(rubyAdapter.PercentageOfTimeValue(stats), Is.EqualTo(percentage));
		}

		[Test]
		public void ShouldDisablePercentageOfTimeForRuby()
		{
			const string stats = "Stats";
			const int percentage = 10;

			rubyAdapter.EnablePercentageOfTime(stats, percentage);

			flipper.Feature(stats).DisablePercentageOfTime();

			Assert.That(rubyAdapter.PercentageOfTimeValue(stats), Is.EqualTo(0));
		}
	}
}

