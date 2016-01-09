using System.Linq;
using FlipperDotNet;
using FlipperDotNet.RedisAdapter;
using NUnit.Framework;
using StackExchange.Redis;
using FlipperDotNet.AdapterTests.Interop;

namespace FlipperDotNet.RedisAdapter.Tests.Interop
{
	[TestFixture]
	public class RedisInteropTests : SharedAdapterInteropTests
	{
		private ConnectionMultiplexer Redis { get; set;}

		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			Redis = ConnectionMultiplexer.Connect("localhost,allowAdmin=true");
		}

		[SetUp]
		public void SetUp()
		{
			var endpoint = Redis.GetEndPoints().First();
			var server = Redis.GetServer(endpoint);
			server.FlushDatabase();
			var database = Redis.GetDatabase();
			adapter = new RedisAdapter(database);
			flipper = new Flipper (adapter);

			rubyAdapter = new RedisRubyAdapter();
		}
	}
}

