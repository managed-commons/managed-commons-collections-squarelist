// ******************************************************************************************************************************
// ****
// ****      Copyright (c) 2016-2021 Rafael 'Monoman' Teixeira
// ****
// ******************************************************************************************************************************

namespace Commons.Collections;

internal class VerticalLinkedList<T> : IEnumerable<T> where T : IComparable
{
    public IEnumerator<T> GetEnumerator() => ImplementedEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ImplementedEnumerator();

    internal VerticalLinkedList(T[] bigArray, int maxDepth, int firstIndex) {
        _bigArray = bigArray;
        _maxDepth = maxDepth;
        _firstIndex = firstIndex;
        Depth = 0;
    }

    internal VerticalLinkedList(T[] bigArray, int maxDepth, int firstIndex, T Value) : this(bigArray, maxDepth, firstIndex) {
        Depth = 1;
        _bigArray[_firstIndex] = Value;
    }

    internal int Depth { get; private set; }

    internal T First => IsEmpty ? default : _bigArray[_firstIndex];

    internal int Id => _firstIndex / _maxDepth;

    internal bool IsEmpty => Depth < 1;

    internal bool IsFull => Depth >= _maxDepth;

    internal T Last => IsEmpty ? default : _bigArray[_lastIndex];

    internal void Clear() => Depth = 0;

    internal bool Contains(T value) => InRange(value) && BinarySearch(value) >= 0;

    internal VerticalLinkedList<T> CopyTo(T[] newArray, int newMaxDepth, int firstIndex) {
        var newList = new VerticalLinkedList<T>(newArray, newMaxDepth, firstIndex);
        var index = _firstIndex;
        for (int i = Depth; i > 0; i--)
            newArray[firstIndex++] = _bigArray[index++];
        newList.Depth = Depth;
        return newList;
    }

    internal int Count(T value) {
        lock (this) {
            if (!InRange(value))
                return 0;
            var foundAt = BinarySearch(value);
            if (foundAt < 0)
                return 0;
            int count = 1;
            var up = foundAt + 1;
            var top = _lastIndex;
            while (up++ <= top && _bigArray[up].CompareTo(value) == 0)
                count++;
            var down = foundAt - 1;
            while (down-- >= _firstIndex && _bigArray[down].CompareTo(value) == 0)
                count++;
            return count;
        }
    }

    internal int DeleteWhere(Func<T, bool> predicate) {
        int removed = 0;
        var next = _firstIndex;
        for (int i = Depth; i > 0; i--) {
            var value = _bigArray[next];
            if (predicate(value)) {
                _ = InnerRemove(next);
                removed++;
            } else
                next++;
        }
        return removed;
    }

    internal bool InRange(T value) => (!IsEmpty) && First!.CompareTo(value) <= 0 && Last!.CompareTo(value) >= 0;

    internal void Insert(T value) {
        CheckCapacity();
        if (IsEmpty) {
            _bigArray[_firstIndex] = value;
            Depth = 1;
            return;
        }
        var up = _firstIndex;
        var down = _lastIndex;
        for (int i = Depth; i > 0; i--) {
            if (_bigArray[up].CompareTo(value) > 0) {
                AddBefore(up, value);
                return;
            }
            if (_bigArray[down].CompareTo(value) <= 0) {
                AddAfter(down, value);
                return;
            }
            up++;
            down--;
        }
    }

    internal void InsertAsLast(T value) {
        CheckCapacity();
        Depth++;
        _bigArray[_lastIndex] = value;
    }

    private void CheckCapacity() {
        if (IsFull)
            throw new InvalidOperationException("Can't add any more values!");
    }

    internal void OpenSpaceAndInsert(T value, VerticalLinkedList<T> listAfter, VerticalLinkedList<T> listBefore) {
        if (!IsFull)
            throw new InvalidOperationException("It is full");
        var slot = WhereToInsert(value);
        if (listAfter != null && listBefore != null && listAfter._lastIndex - slot < slot - listBefore._lastIndex)
            listBefore = null;
        if (listBefore is null) {
            if (listAfter is null)
                throw new InvalidOperationException("Both lists are null");
            for (int i = listAfter._lastIndex; i >= slot; i--)
                _bigArray[i + 1] = _bigArray[i];
            listAfter.Depth += 1;
        } else {
            var nextFirst = listBefore._firstIndex + _maxDepth;
            listBefore.Depth += 1;
            _bigArray[listBefore._lastIndex] = _bigArray[nextFirst];
            slot--;
            for (int i = nextFirst; i < slot; i++)
                _bigArray[i] = _bigArray[i + 1];
        }
        _bigArray[slot] = value;
    }

    internal int Remove(T value, bool removeAll) {
        lock (this) {
            if (!Contains(value))
                return 0;
            var up = _firstIndex;
            int removed = 0;
            for (int i = Depth; i > 0; i--) {
                var upCompare = _bigArray[up].CompareTo(value);
                if (upCompare > 0)
                    break;
                if (upCompare < 0)
                    up++;
                else {
                    var start = up;
                    while (upCompare == 0) {
                        removed++;
                        _ = InnerRemove(up);
                        if ((!removeAll) || (--i <= 0))
                            break;
                        upCompare = _bigArray[up].CompareTo(value);
                    }
                    break;
                }
            }
            return removed;
        }
    }

    internal T RemoveFirst() => InnerRemove(_firstIndex);

    private readonly T[] _bigArray;
    private readonly int _firstIndex;
    private readonly int _maxDepth;
    private int _lastIndex => _firstIndex + Depth - 1;

    private void AddAfter(int index, T value) => AddBefore(index + 1, value);

    private void AddBefore(int index, T value) {
        for (int i = _lastIndex; i >= index; i--)
            _bigArray[i + 1] = _bigArray[i];
        _bigArray[index] = value;
        Depth++;
    }

    private int BinarySearch(T value) {
        int foundAt = -1;
        var bottom = _firstIndex;
        var top = _lastIndex;
        while (bottom <= top) {
            var mid = (bottom + top) / 2;
            var compare = _bigArray[mid].CompareTo(value);
            if (compare == 0) {
                foundAt = mid;
                break;
            } else if (compare > 0) {
                top = mid - 1;
            } else {
                bottom = mid + 1;
            }
        }

        return foundAt;
    }

    private IEnumerator<T> ImplementedEnumerator() {
        if (IsEmpty)
            yield break;
        var next = _firstIndex;
        for (int i = Depth; i > 0; i--)
            yield return _bigArray[next++];
    }

    private T InnerRemove(int index) {
        if (IsEmpty || index < _firstIndex || index > _lastIndex)
            return default;
        var value = _bigArray[index];
        for (int i = index; i <= _lastIndex; i++)
            _bigArray[i] = _bigArray[i + 1];
        Depth--;
        return value;
    }

    private int WhereToInsert(T value) {
        var up = _firstIndex;
        var down = _lastIndex;
        for (int i = Depth; i > 0; i--) {
            if (_bigArray[up].CompareTo(value) > 0)
                return up;
            if (_bigArray[down].CompareTo(value) <= 0)
                return down + 1;
            up++;
            down--;
        }
        throw new ArgumentOutOfRangeException(nameof(value));
    }

    public override string ToString() => GetType().FullName ?? nameof(VerticalLinkedList<T>);
}