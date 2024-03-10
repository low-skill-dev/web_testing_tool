using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Threading.Interlocked;

namespace SharedServices;

public class ObjectStorage<K, V>
{
	private readonly ConcurrentDictionary<K, (V Obj, DateTime Exp)> _dict;

	/* StrongBox и Interlocked не используются сознательно
	 * в целях увеличения производительности.
	 */
	private int _addedSinceLastCleanup;
	private DateTime _lastCleanup;

	private const int _cleanupAfterCount = 16384;
	private const int _cleanupAfterSeconds = 0;

	public ObjectStorage()
	{
		_dict = new(Environment.ProcessorCount, 16);

		_addedSinceLastCleanup = 0;
		_lastCleanup = DateTime.UtcNow;
	}

	private void CleanupIfNeeded()
	{
		if(
			(_cleanupAfterCount > 0 && _addedSinceLastCleanup > _cleanupAfterCount)
			||
			(_cleanupAfterSeconds > 0 && DateTime.UtcNow.Subtract(_lastCleanup).TotalSeconds > _cleanupAfterSeconds))
		{
			lock(_dict)
			{
				// Если поток ждал и зашел сюда сразу после предыдущей очистки.
				if(DateTime.UtcNow.Subtract(_lastCleanup).TotalSeconds < 3) return;

				var toBePersisted = _dict.Where(x => x.Value.Exp > DateTime.UtcNow).ToList();
				_dict.Clear();
				toBePersisted.ForEach(x => _dict.TryAdd(x.Key, x.Value));

				_addedSinceLastCleanup = 0;
				_lastCleanup = DateTime.UtcNow;
			}
		}
	}

	public void SetObject(K id, V item, TimeSpan lifespan)
	{
		var tuple = (item, DateTime.UtcNow.Add(lifespan));

		if(!_dict.TryAdd(id, tuple)) _dict[id] = tuple;
		else _addedSinceLastCleanup++;

		CleanupIfNeeded();
	}

	/// <returns>
	/// <typeparamref name="V"/> if <paramref name="id"/> was found and object is 
	/// not expired, <see langword="default"/>(<typeparamref name="V"/>) otherwise.
	/// </returns>
	public V? GetObject(K id, bool remove = false)
	{
		return _dict.TryGetValue(id, out var val) 
			&& val.Exp > DateTime.UtcNow ? val.Obj : default;
	}
}
