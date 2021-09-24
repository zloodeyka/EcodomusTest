using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcodomusTest
{
	public class Database
	{
		public Database()
		{
			_storage = new Dictionary<string, int>();
			_valuesCache = new Dictionary<int, int>();

			_transactions = new Stack<Guid>();
			_history = new Stack<DbOperation>();
			_updatedKeys = new Dictionary<string, int?>();
			_updatedValues = new Dictionary<int, int>();
		}
		public void Begin()
		{
			_transactions.Push(Guid.NewGuid());
		}

		public void Commit()
		{
			if (!_transactions.Any())
			{
				return;
			}

			foreach(var item in _updatedKeys)
			{
				if (item.Value == null) // key is deleted
				{
					if (!_storage.ContainsKey(item.Key))
					{
						continue;
					}

					var value = _storage[item.Key];
					_storage.Remove(item.Key);
					DecValuesCache(value);
				} else
				{
					if (_storage.ContainsKey(item.Key))
					{
						DecValuesCache(_storage[item.Key]);
						_storage[item.Key] = (int)item.Value;
						IncValuesCache((int)item.Value);
					} else
					{
						_storage.Add(item.Key, (int)item.Value);
						IncValuesCache((int)item.Value);
					}
				}
			}

			_transactions.Clear();
			_history.Clear();
			_updatedKeys.Clear();
			_updatedValues.Clear();

		}

		public void Rollback()
		{
			if (!_transactions.Any())
			{
				Console.WriteLine("No Transactions");
				return;
			}

			var lastTransaction = _transactions.Pop();

			while (_history.Any() && _history.Peek().Transasction == lastTransaction)
			{
				var lastOperation = _history.Pop();
				_updatedKeys[lastOperation.Key] = lastOperation.OldValue;

				if (lastOperation.Value != null)
				{
					DecTransactionValuesCache((int)lastOperation.Value);
				}

				if (lastOperation.OldValue != null)
				{
					IncTransactionValuesCache((int)lastOperation.OldValue);
				}
			}
		}

		public void Set(string key, int value)
		{
			if (!_transactions.Any())
			{
				_storage[key] = value;
				IncValuesCache(value);
				return;
			}

			DbOperation operation;

			if (_updatedKeys.ContainsKey(key))
			{
				operation = new DbOperation
				{
					Key = key,
					OldValue = _updatedKeys[key],
					Value = value,
					Transasction = _transactions.Peek()
				};
				_updatedKeys[key] = value;
			} else
			{
				operation = new DbOperation
				{
					Key = key,
					OldValue = _storage.ContainsKey(key) ? _storage[key] : null,
					Value = value,
					Transasction = _transactions.Peek()
				};
				_updatedKeys.Add(key, value);
			}
			_history.Push(operation);

			if (operation.OldValue != null)
			{
				DecTransactionValuesCache((int)operation.OldValue);
			}
			IncTransactionValuesCache(value);
		}

		public int? Get(string key)
		{
			if (!_transactions.Any())
			{
				return _storage.ContainsKey(key) ? _storage.GetValueOrDefault(key) : null;
			}
			if (_updatedKeys.ContainsKey(key))
			{
				return _updatedKeys[key];
			}
			if (!_storage.ContainsKey(key))
			{
				return null;
			}
			return _storage[key];
		}

		public void Delete(string key)
		{
			if (!_storage.ContainsKey(key) && !_updatedKeys.ContainsKey(key))
			{
				return;
			}

			if (!_transactions.Any())
			{
				if (_storage.ContainsKey(key))
				{
					var value = _storage[key];
					_storage.Remove(key);
					DecValuesCache(value);
				}
				return;
			}

			DbOperation operation;
			if (_updatedKeys.ContainsKey(key))
			{
				operation = new DbOperation
				{
					Key = key,
					OldValue = _updatedKeys[key],
					Value = null,
					Transasction = _transactions.Peek()
				};
				_updatedKeys[key] = null;
			}
			else
			{
				operation = new DbOperation
				{
					Key = key,
					OldValue = _storage.ContainsKey(key) ? _storage[key] : null,
					Value = null,
					Transasction = _transactions.Peek()
				};
				_updatedKeys.Add(key, null);
			}

			_history.Push(operation);
			if (operation.OldValue != null)
			{
				DecTransactionValuesCache((int)operation.OldValue);
			}
		}

		public int Count(int value)
		{
			int storageValuesCount = _valuesCache.ContainsKey(value) ? _valuesCache[value] : 0;
			int updatedValuesCount = _updatedValues.ContainsKey(value) ? _updatedValues[value] : 0;
			return storageValuesCount + updatedValuesCount;
		}

		private void IncValuesCache(int key)
		{
			if (_valuesCache.ContainsKey(key))
			{
				_valuesCache[key]++;
			}
			else
			{
				_valuesCache.Add(key, 1);
			}
		}

		private void DecValuesCache(int key)
		{
			if (_valuesCache.ContainsKey(key))
			{
				_valuesCache[key]--;
			}
			else
			{
				_valuesCache.Add(key, -1);
			}
		}

		private void IncTransactionValuesCache(int key)
		{
			if (_updatedValues.ContainsKey(key))
			{
				_updatedValues[key]++;
			}
			else
			{
				_updatedValues.Add(key, 1);
			}
		}

		private void DecTransactionValuesCache(int key)
		{
			if (_updatedValues.ContainsKey(key))
			{
				_updatedValues[key]--;
			}
			else
			{
				_updatedValues.Add(key, -1);
			}
		}

		private Dictionary<string, int> _storage;
		private Dictionary<int, int> _valuesCache;

		private Stack<Guid> _transactions;
		private Stack<DbOperation> _history;
		private static Dictionary<string, int?> _updatedKeys;
		private static Dictionary<int, int> _updatedValues;
	}
}
