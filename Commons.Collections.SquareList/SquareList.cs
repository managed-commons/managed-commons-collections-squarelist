using System;
using System.Collections;
using System.Collections.Generic;

namespace Commons.Collections
{
    public class SquareList<T> : IEnumerable<T> where T : IComparable
    {
        public SquareList(int capacity)
        {
            _maxDepth = CalcMaxDepth(capacity);
            Capacity = _maxDepth * (_maxDepth + 1);
            _bigArray = new T[Capacity];
            _lists = new List<VerticalLinkedList<T>>();
        }

        public SquareList() : this(9)
        {
        }

        public int Capacity { get; private set; }
        public bool IsEmpty => _size <= 0;
        public T Max => IsEmpty ? default(T) : _lastList.Last;
        public T Min => IsEmpty ? default(T) : _firstList.First;
        public int Size => _size;

        public bool Contains(T value)
        {
            return (!IsEmpty) && (BinarySearch(value) != null);
        }

        public void Delete(T value, bool removeAll = false)
        {
            lock (this) {
                int removed = 0;
                foreach (var verticalList in _lists) {
                    removed += verticalList.Remove(value, removeAll);
                    if ((!removeAll) && removed > 0)
                        break;
                }
                if (removed > 0)
                    _size -= removed;
                while (_lastList != null && _lastList.IsEmpty)
                    _lists.Remove(_lastList);
                if (!IsEmpty && _firstList.IsEmpty) {
                    _firstList.Receive(FindFirstListWithContent());
                    while (_lastList != null && _lastList.IsEmpty)
                        _lists.Remove(_lastList);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var verticalList in _lists)
                foreach (var value in verticalList)
                    yield return value;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            foreach (var verticalList in _lists)
                foreach (var value in verticalList)
                    yield return value;
        }

        public void Insert(T value)
        {
            lock (this) {
                if (!IsEmpty) {
                    if (_size + 1 > Capacity)
                        Enlarge();
                    if (_lastList.Last.CompareTo(value) >= 0) {
                        int listIndex = WhereToInsert(value);
                        if (listIndex >= 0) {
                            var list = _lists[listIndex];
                            while (list.IsFull) {
                                if (list.OpenSpaceThru(FindListWithSpaceAfter(listIndex))) {
                                    if (list.Last.CompareTo(value) < 0)
                                        list = _lists[++listIndex];
                                } else if (list.OpenSpaceBack(FindListWithSpaceBefore(listIndex))) {
                                    if (list.First.CompareTo(value) > 0)
                                        list = _lists[--listIndex];
                                }
                            }
                            list.Insert(value);
                            _size++;
                            return;
                        }
                    }
                    if (!_lastList.IsFull) {
                        _lastList.Insert(value);
                        _size++;
                        return;
                    }
                }
                AddNewList(value);
            }
        }

        public override string ToString()
        {
            return $"SquareList({Size} of {Capacity} as {_maxDepth} x {Width}) [{DumpLists()}]";
        }

        internal T[] _bigArray;

        internal int _maxDepth = 0;

        private List<VerticalLinkedList<T>> _lists;

        private int _size = 0;

        private VerticalLinkedList<T> _firstList => IsEmpty ? null : _lists[0];

        private VerticalLinkedList<T> _lastList => IsEmpty ? null : _lists[_lists.Count - 1];

        private int Width => _lists.Count;

        private void AddNewList(T value)
        {
            _lists.Add(new VerticalLinkedList<T>(_bigArray, _maxDepth, _lists.Count * _maxDepth, value));
            _size++;
        }

        private VerticalLinkedList<T> BinarySearch(T value)
        {
            var result = InternalBinarySearch(value, 0, _lists.Count - 1, true);
            return result >= 0 ? _lists[result] : null;
        }

        private int CalcMaxDepth(int size)
        {
            return (size > 0) ? Convert.ToInt32(Math.Ceiling(Math.Sqrt(size))) : 0;
        }

        private string Dump(VerticalLinkedList<T> list) => (list == null) ? "!" : list.ToString();

        private string DumpLists()
        {
            switch (_lists.Count) {
                case 0:
                    return "";

                case 1:
                    return $"{Dump(_firstList)}";

                default:
                    return $"{Dump(_firstList)} ...  {Dump(_lastList)}";
            }
        }

        private void Enlarge()
        {
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
            Capacity = newCapacity;
        }

        private int FindFirstList(T value, int m)
        {
            while (--m >= 0 && _lists[m].Contains(value)) { }
            return m + 1;
        }

        private VerticalLinkedList<T> FindFirstListWithContent()
        {
            for (int index = 1; index < _lists.Count; index++)
                if (!_lists[index].IsEmpty)
                    return _lists[index];
            return null;
        }

        private VerticalLinkedList<T> FindListWithSpaceAfter(int listIndex)
        {
            for (int index = listIndex + 1; index < _lists.Count; index++)
                if (!_lists[index].IsFull)
                    return _lists[index];
            if (_lists.Count <= _maxDepth) {
                var endList = new VerticalLinkedList<T>(_bigArray, _maxDepth, _lists.Count * _maxDepth);
                _lists.Add(endList);
                return endList;
            }
            return null;
        }

        private VerticalLinkedList<T> FindListWithSpaceBefore(int listIndex)
        {
            for (int index = listIndex - 1; index >= 0; index--)
                if (!_lists[index].IsFull)
                    return _lists[index];
            return null;
        }

        private int InternalBinarySearch(T value, int i, int j, bool exact)
        {
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

        private int WhereToInsert(T value) => InternalBinarySearch(value, 0, _lists.Count - 1, false);
    }
}
