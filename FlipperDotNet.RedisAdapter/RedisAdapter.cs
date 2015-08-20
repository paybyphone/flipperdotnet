using System;
using System.Collections.Generic;
using System.Linq;
using FlipperDotNet.Adapter;
using FlipperDotNet.Gate;
using StackExchange.Redis;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
			var fields = from value in values
			             select value.Name.ToString();

			foreach (var gate in feature.Gates)
			{
				if (gate.DataType == typeof(bool) || gate.DataType == typeof(int))
				{
					var value = values.SingleOrDefault(x => x.Name.ToString() == gate.Key);
					result[gate.Key] = value.Value.ToString();
				}
				else if (gate.DataType == typeof(ISet<string>))
				{
					result[gate.Key] = ValuesForSetGate(fields, gate);
				}
				else
				{
					throw new NotSupportedException(string.Format("{0} is not supported by this adapter yet", gate.Name));
				}
			}

			return result;
		}

		public void Enable(Feature feature, IGate gate, object thing)
		{
			if (gate.DataType == typeof(bool) || gate.DataType == typeof(int))
			{
				_database.HashSet(feature.Key, gate.Key, thing.ToString().ToLower());
			}
			else if (gate.DataType == typeof(ISet<string>))
			{
				_database.HashSet(feature.Key, ToField(gate, thing), 1);
			}
			else
			{
				throw new NotSupportedException(string.Format("{0} is not supported by this adapter yet", gate.Name));
			}
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
			else if (gate.DataType == typeof(ISet<string>))
			{
				_database.HashDelete(feature.Key, ToField(gate, thing));
			}
			else
			{
				throw new NotSupportedException(string.Format("{0} is not supported by this adapter yet", gate.Name));
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
			var transaction = _database.CreateTransaction();
			transaction.SetRemoveAsync(FeaturesKey, feature.Key);
			Clear(transaction, feature);
			transaction.Execute();
		}

		public void Clear(Feature feature)
		{
			var task = Clear(_database, feature);
			task.Wait();
		}

		private Task<bool> Clear(IDatabaseAsync database, Feature feature)
		{
			return database.KeyDeleteAsync(feature.Key);
		}

		private string ToField(IGate gate, object thing)
		{
			return String.Format("{0}/{1}", gate.Key, thing.ToString());
		}

		private ISet<string> ValuesForSetGate(IEnumerable<string> fields, IGate gate)
		{
			var regex = String.Format("^{0}\\/", gate.Key);
			var keys = from field in fields
			           where Regex.IsMatch(field, regex)
			           select field;
			var values = from key in keys select key.Split(new[]{'/'},2).Last();
			return new HashSet<string>(values);
		}
	}
}

