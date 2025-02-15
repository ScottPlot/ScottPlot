// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace ScottPlot.Collections;

/// <summary>
/// The circular buffer starts with an empty list and grows to a maximum size.
/// When the buffer is full, adding or inserting a new item removes the first item in the buffer.
/// </summary>
[DebuggerDisplay("Count = {Count}")]
[DebuggerTypeProxy(typeof(CircularBuffer<>.CircularBufferDebugView))]
public sealed class CircularBuffer<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
{
    // Internal for testing.
    internal readonly List<T> _buffer;
    internal int _start;
    internal int _end;

    public CircularBuffer(int capacity) : this(new List<T>(), capacity, start: 0, end: 0)
    {
    }

    internal CircularBuffer(List<T> buffer, int capacity, int start, int end)
    {
        if (capacity < 1)
        {
            throw new ArgumentException("Circular buffer must have a capacity greater than 0.", nameof(capacity));
        }

        _buffer = buffer;
        Capacity = capacity;
        _start = start;
        _end = end;
    }

    public int Capacity { get; }

    public bool IsFull => Count == Capacity;

    public bool IsEmpty => Count == 0;

    public int Count => _buffer.Count;

    public bool IsReadOnly { get; }

    public bool IsFixedSize { get; } = true;

    public object SyncRoot { get; } = new object();

    public bool IsSynchronized { get; }

    public int IndexOf(T item)
    {
        for (var index = 0; index < Count; ++index)
        {
            if (Equals(this[index], item))
            {
                return index;
            }
        }
        return -1;
    }

    public void Insert(int index, T item)
    {
        // Can be implemented, but necessary to drag along System.Memory.
        throw new NotImplementedException();
    }

    public void RemoveAt(int index)
    {
        ValidateIndexInRange(index);

        var internalIndex = InternalIndex(index);
        _buffer.RemoveAt(internalIndex);
        if (internalIndex < _end)
        {
            Decrement(ref _end);
            if (_start > 0)
            {
                _start = _end;
            }
        }
    }

    private void ValidateIndexInRange(int index)
    {
        if (index >= Count)
        {
            throw new InvalidOperationException($"Cannot access index {index}. Buffer size is {Count}");
        }
    }

    public bool Remove(T item)
    {
        var index = IndexOf(item);
        if (index == -1)
        {
            return false;
        }

        RemoveAt(index);
        return true;
    }

    public T this[int index]
    {
        get
        {
            ValidateIndexInRange(index);
            return _buffer[InternalIndex(index)];
        }
        set
        {
            ValidateIndexInRange(index);
            _buffer[InternalIndex(index)] = value;
        }
    }

    public void Add(T item)
    {
        if (IsFull)
        {
            _buffer[_end] = item;
            Increment(ref _end);
            _start = _end;
        }
        else
        {
            _buffer.Insert(_end, item);
            Increment(ref _end);
            if (_end != _buffer.Count)
            {
                _start = _end;
            }
        }
    }

    public void Clear()
    {
        _start = 0;
        _end = 0;
        _buffer.Clear();
    }

    public bool Contains(T item) => IndexOf(item) != -1;

    public void CopyTo(T[] array, int arrayIndex)
    {
        if (array.Length - arrayIndex < Count)
        {
            throw new ArgumentException("Array does not contain enough space for items");
        }

        for (var index = 0; index < Count; ++index)
        {
            array[index + arrayIndex] = this[index];
        }
    }

    public T[] ToArray()
    {
        if (IsEmpty)
        {
            return [];
        }

        var array = new T[Count];
        for (var index = 0; index < Count; ++index)
        {
            array[index] = this[index];
        }

        return array;
    }

    public IEnumerator<T> GetEnumerator()
    {
        for (var i = 0; i < Count; ++i)
        {
            yield return this[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private int InternalIndex(int index)
    {
        return (_start + index) % _buffer.Count;
    }

    private void Increment(ref int index)
    {
        if (++index < Capacity)
        {
            return;
        }

        index = 0;
    }

    private void Decrement(ref int index)
    {
        if (index <= 0)
        {
            index = Capacity - 1;
        }

        --index;
    }

    private sealed class CircularBufferDebugView(CircularBuffer<T> collection)
    {
        private readonly CircularBuffer<T> _collection = collection;

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] Items => _collection.ToArray();
    }

    public int BinarySearch(int index, int count, T item, IComparer<T> comparer)
    {
        if (index < 0 || count < 0)
        {
            throw new ArgumentOutOfRangeException(index < 0 ? nameof(index) : nameof(count), "Non-negative number required.");
        }

        if (Count - index < count)
        {
            throw new ArgumentException("Invalid offset length.");
        }

        int lo = index;
        int hi = index + count - 1;

        while (lo <= hi)
        {
            int i = lo + ((hi - lo) >> 1);
            int order = comparer.Compare(_buffer[InternalIndex(i)], item);

            if (order == 0) return i;
            if (order < 0)
            {
                lo = i + 1;
            }
            else
            {
                hi = i - 1;
            }
        }

        return ~lo;
    }
}
