using FlipperDotNet.AdapterTests.Interop;

namespace FlipperDotNet.RedisAdapter.Tests.Interop
{
	public class RedisRubyAdapter : RubyAdapter
	{
		protected override string BuildScript(string command)
		{
			return @"
require 'flipper-redis'
client = Redis.new
adapter = Flipper::Adapters::Redis.new(client)
flipper = Flipper.new(adapter)" + "\n" +
			command;
		}
	}
}
