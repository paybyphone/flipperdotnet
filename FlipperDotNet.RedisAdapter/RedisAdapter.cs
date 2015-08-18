using System;
using System.Collections.Generic;
using System.Linq;
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
			var result = new Dictionary<string, object>();

			var values = _database.HashGetAll(feature.Key);

			foreach (var gate in feature.Gates)
			{
				if (gate.DataType == typeof(bool) || gate.DataType == typeof(int))
				{
					var value = values.SingleOrDefault(x => x.Name.ToString() == gate.Key);
					result[gate.Key] = value.Value.ToString();
				}
			}

			return result;
		}

		public void Enable(Feature feature, IGate gate, object thing)
		{
			_database.HashSet(feature.Key, gate.Key, thing.ToString().ToLower());
		}

		public void Disable(Feature feature, IGate gate, object thing)
		{
			if (gate.DataType == typeof(bool))
			{
				_database.KeyDelete(feature.Key);
			}
			else if (gate.DataType == typeof(int))
			{
				_database.HashSet(feature.Key, gate.Key, thing.ToString());
			}
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

