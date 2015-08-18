using System;
using System.Collections.Generic;
using FlipperDotNet.Adapter;
using FlipperDotNet.Gate;
using StackExchange.Redis;

namespace FlipperDotNet.RedisAdapter
{
	public class RedisAdapter : IAdapter
	{
		public const string FeaturesKey = "flipper_features";

		private IDatabase _database;

		public RedisAdapter(IDatabase database)
		{
			_database = database;
		}

		public IDictionary<string, object> Get(Feature feature)
		{
			return new Dictionary<string,object>();
		}

		public void Enable(Feature feature, IGate gate, object thing)
		{
		}

		public void Disable(Feature feature, IGate gate, object thing)
		{
		}

		public ISet<string> Features {
			get {
				var members = _database.SetMembers(FeaturesKey);
				var result = new HashSet<string>();
				foreach (var member in members)
				{
					result.Add(member);
				}
				return result;
			}
		}

		public void Add(Feature feature)
		{
			_database.SetAdd(FeaturesKey, feature.Key);
		}

		public void Remove(Feature feature)
		{
			_database.SetRemove(FeaturesKey, feature.Key);
		}

		public void Clear(Feature feature)
		{
		}
	}
}

