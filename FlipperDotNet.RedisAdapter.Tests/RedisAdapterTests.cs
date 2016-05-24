using System.Linq;
using NUnit.Framework;
using StackExchange.Redis;

namespace FlipperDotNet.RedisAdapter.Tests
{

	[TestFixture]
	public class RedisAdapterTests : AdapterTests.SharedAdapterTests
	{
		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			Redis = ConnectionMultiplexer.Connect("localhost,allowAdmin=true");
		}

		[SetUp]
		public new void SetUp()
		{
			var endpoint = Redis.GetEndPoints().First();
			var server = Redis.GetServer(endpoint);
			server.FlushDatabase();
			Database = Redis.GetDatabase();
			Adapter = new RedisAdapter(Database);
		}

		private ConnectionMultiplexer Redis { get; set;}
		private IDatabase Database { get; set; }
	}
}
	