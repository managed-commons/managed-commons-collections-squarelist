// ******************************************************************************************************************************
// ****
// ****      Copyright (c) 2016-2021 Rafael 'Monoman' Teixeira
// ****
// ******************************************************************************************************************************

namespace Commons.Collections;

public class SquareList<T> : IEnumerable<T> where T : IComparable
{
    public SquareList(IEnumerable<T> source) : this(source.Count(), source) {
    }

    public SquareList(int capacity, IEnumerable<T> source) : this(capacity) {
        int count = _maxDepth;
        var newList = AddList(_lists, _bigArray, _maxDepth, 0);
        T lastValue = default;
        foreach (var value in source) {
            if (Size > 0 && value.CompareTo(lastValue) < 0)
                throw new ArgumentException("Values aren't in ascending order", nameof(source));
            lastValue = value;
            newList.InsertAsLast(value);
            Size++;
            if (Size > capacity)
                throw new ArgumentException("More values than the specified capacity allows", nameof(source));
            if (--count <= 0) {
                newList = AddList(_lists, _bigArray, _maxDepth, Size);
                count = _maxDepth;
            }
        }
        while (RemoveListIfEmpty(_lastList)) ;
    }

    public SquareList(int capacity) {
        _maxDepth = CalcMaxDepth(capacity);
        Capacity = _maxDepth * (_maxDepth + 1);
        _bigArray = new T[Capacity];
        _lists = new List<VerticalLinkedList<T>>();
        _removedLists = new RemovedListsRepository<T>();
    }

    public SquareList() : this(16) {
    }

    public int Capacity { get; private set; }
    public bool IsEmpty => Size <= 0;
    public T Max => IsEmpty ? default : _lastList.Last;
    public T Min => IsEmpty ? default : _firstList.First;
    public int Ratio => (int)(Size * 100L / Capacity);
    public int Size { get; private set; }

    public bool Contains(T value) => (!IsEmpty) && (BinarySearch(value) != null);

    public void Delete(T value, bool removeAll = false) {
        lock (this) {
            int removed = 0;
            while (true) {
                var list = BinarySearch(value);
                if (list == null)
                    break;
                removed += list.Remove(value, removeAll);
                RemoveListIfEmpty(list);
                if ((!removeAll) && removed > 0)
                    break;
            }
            if (removed > 0)
                Size -= removed;
            while (RemoveListIfEmpty(_lastList)) ;
        }
    }

    public void DeleteBelow(T value) {
        lock (this) {
            int removed = 0;
            var index = WhereToInsert(value);
            while (--index >= 0) {
                var list = _lists[index];
                removed += list.Depth;
                RemoveList(list);
            }
            if (removed > 0)
                Size -= removed;
            if (IsEmpty)
                return;
            while (!_firstList.IsEmpty && _firstList.First.CompareTo(value) < 0) {
                _firstList.RemoveFirst();
                Size--;
            }
            while (RemoveListIfEmpty(_firstList)) ;
        }
    }

    public void DeleteWhere(Func<T, bool> predicate) {
        lock (this) {
            foreach (var list in _lists) {
                Size -= list.DeleteWhere(predicate);
                RemoveListIfEmpty(list);
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() {
        foreach (var verticalList in _lists)
            foreach (var value in verticalList)
                yield return value;
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator() {
        foreach (var verticalList in _lists)
            foreach (var value in verticalList)
                yield return value;
    }

    public void Insert(T value) {
        lock (this) {
            FindListToInsertInto(value)?.Insert(value);
            Size++;
            while (RemoveListIfEmpty(_lastList)) ;
        }
    }

    public void ShrinkWithSlackOf(int slack) {
        if (slack < 0)
            throw new ArgumentOutOfRangeException(nameof(slack));
        var newMaxDepth = CalcMaxDepth(Size) + slack;
        if (newMaxDepth != _maxDepth) {
            var newCapacity = newMaxDepth * (newMaxDepth + 1);
            var newArray = new T[newCapacity];
            var newLists = new List<VerticalLinkedList<T>>();
            int index = 0;
            var newList = AddList(newLists, newArray, newMaxDepth, index);
            int columnCount = newMaxDepth - slack;
            int count = columnCount;
            foreach (var value in this) {
                if (count-- == 0) {
                    index += newMaxDepth;
                    newList = AddList(newLists, newArray, newMaxDepth, index);
                    count = columnCount - 1;
                }
                newList.InsertAsLast(value);
            }
            _maxDepth = newMaxDepth;
            _lists = newLists;
            _bigArray = newArray;
            _removedLists.Clear();
            Capacity = newCapacity;
        }
    }

    public override string ToString() => $"SquareList({Size} of {Capacity} as {_maxDepth} x {_width}) [{DumpLists()}]";

    private readonly RemovedListsRepository<T> _removedLists;
    private T[] _bigArray;
    private List<VerticalLinkedList<T>> _lists;
    private int _maxDepth;
    private VerticalLinkedList<T> _firstList => IsEmpty ? null : _lists[0];

#if NETSTANDARD1_0
    private VerticalLinkedList<T> _lastList => IsEmpty ? null : _lists[_lists.Count - 1];
#else
    private VerticalLinkedList<T> _lastList => IsEmpty ? null : _lists[^1];
#endif

    private int _width => _lists.Count;

    private static VerticalLinkedList<T> AddList(List<VerticalLinkedList<T>> newLists, T[] newArray, int newMaxDepth, int index) {
        var newList = new VerticalLinkedList<T>(newArray, newMaxDepth, index);
        newLists.Add(newList);
        return newList;
    }

    private static int CalcMaxDepth(int size) => (size > 0) ? Convert.ToInt32(Math.Ceiling(Math.Sqrt(size))) : 0;

    private static string Dump(VerticalLinkedList<T> list) => (list == null) ? "!" : list.ToString();

    private VerticalLinkedList<T> AddListAtHead() {
        var endList = new VerticalLinkedList<T>(_bigArray, _maxDepth, 0);
        _lists.Add(endList);
        return endList;
    }

    private VerticalLinkedList<T> AddListAtTail() {
        if (_lastList.Id < _maxDepth) {
            var endList = new VerticalLinkedList<T>(_bigArray, _maxDepth, (_lastList.Id + 1) * _maxDepth);
            _lists.Add(endList);
            return endList;
        }
        return null;
    }

    private void AddNewList(T value) {
        _lists.Add(new VerticalLinkedList<T>(_bigArray, _maxDepth, _lists.Count * _maxDepth, value));
        Size++;
    }

    private VerticalLinkedList<T> BinarySearch(T value) {
        var result = InternalBinarySearch(value, 0, _lists.Count - 1, true);
        return result >= 0 ? _lists[result] : null;
    }

    private string DumpLists() => _lists.Count switch {
        0 => "",
        1 => $"{Dump(_firstList)}",
        _ => $"{Dump(_firstList)} ...  {Dump(_lastList)}",
    };

    private void Enlarge() {
        var newMaxDepth = _maxDepth + 1;
        var newCapacity = newMaxDepth * (newMaxDepth + 1);
        var newArray = new T[newCapacity];
        var newLists = new List<VerticalLinkedList<T>>();
        int firstIndex = 0;
        foreach (var list in _lists) {
            newLists.Add(list.CopyTo(newArray, newMaxDepth, firstIndex));
            firstIndex += newMaxDepth;
        }
        _maxDepth = newMaxDepth;
        _lists = newLists;
        _bigArray = newArray;
        _removedLists.Clear();
        Capacity = newCapacity;
    }

    private int FindFirstList(T value, int m) {
        while (--m >= 0 && _lists[m].Contains(value)) { }
        return m + 1;
    }

    private VerticalLinkedList<T> FindFirstListWithContent() {
        for (int index = 1; index < _lists.Count; index++)
            if (!_lists[index].IsEmpty)
                return _lists[index];
        return null;
    }

    private VerticalLinkedList<T> FindListToInsertInto(T value) {
        if (IsEmpty)
            return UnremoveList(0, 0) ?? AddListAtHead();
        if (Size + 1 > Capacity)
            Enlarge();
        if (_lastList.Last.CompareTo(value) >= 0) {
            int listIndex = WhereToInsert(value);
            if (listIndex >= 0) {
                var list = _lists[listIndex];
                return list.IsFull ? MakeSpaceAndInsert(value, listIndex, list) : list;
            }
        }
        return _lastList.IsFull ? AddListAtTail() : _lastList;
    }

    private VerticalLinkedList<T> FindListWithSpaceAfter(int listIndex) {
        var list = _lists[listIndex];
        var id = list.Id;
        for (int index = listIndex + 1; index < _lists.Count; index++) {
            list = _lists[index];
            if (list.Id != id + 1)
                return UnremoveList(id + 1, index);
            if (!list.IsFull)
                return list;
            id = list.Id;
        }
        return UnremoveList(_lastList.Id + 1, _lists.Count) ?? AddListAtTail();
    }

    private VerticalLinkedList<T> FindListWithSpaceBefore(int listIndex) {
        var list = _lists[listIndex];
        var id = list.Id;
        for (int index = listIndex - 1; index >= 0; index--) {
            list = _lists[index];
            if (list.Id != id - 1)
                return UnremoveList(id - 1, index + 1);
            if (!list.IsFull)
                return list;
            id = list.Id;
        }
        return UnremoveList(_firstList.Id - 1, 0);
    }

    private int InternalBinarySearch(T value, int i, int j, bool exact) {
        if (j < i)
            return -1;
        int m = (i + j) / 2;
        var list = _lists[m];
        if (exact) {
            if (list.Contains(value))
                return FindFirstList(value, m);
        } else {
            if (list.Last.CompareTo(value) >= 0) {
                if ((m <= 0) || (_lists[m - 1].Last.CompareTo(value) < 0)) {
                    return m;
                }
            }
        }
        if (list.Last.CompareTo(value) > 0)
            j = m - 1;
        else
            i = m + 1;
        return InternalBinarySearch(value, i, j, exact);
    }

    private VerticalLinkedList<T> MakeSpaceAndInsert(T value, int listIndex, VerticalLinkedList<T> list) {
        var listAfter = FindListWithSpaceAfter(listIndex);
        var listBefore = FindListWithSpaceBefore(listIndex);
        list.OpenSpaceAndInsert(value, listAfter, listBefore);
        return null;
    }

    private void RemoveList(VerticalLinkedList<T> list) {
        _removedLists.Add(list);
        _lists.Remove(list);
    }

    private bool RemoveListIfEmpty(VerticalLinkedList<T> list) {
        if (list != null && list.IsEmpty) {
            RemoveList(list);
            return true;
        }
        return false;
    }

    private VerticalLinkedList<T> UnremoveList(int id, int at) {
        var list = _removedLists.Recover(id);
        if (list != null)
            _lists.Insert(at, list);
        return list;
    }

    private int WhereToInsert(T value) => InternalBinarySearch(value, 0, _lists.Count - 1, false);
}
