using System;
using System.Collections;
using System.Collections.Generic;

namespace Commons.Collections
{
    public class SquareList<T> : IEnumerable<T> where T : IComparable
    {
        public SquareList(int capacity)
        {
            Capacity = capacity;
            _maxDepth = CalcMaxDepth(capacity);
            _lists = new List<VerticalLinkedList<T>>(_maxDepth + 1);
        }

        public SquareList() : this(10)
        {
        }

        public int Capacity { get; private set; }
        public bool IsEmpty => _size == 0;
        public T Max => IsEmpty ? default(T) : _lastList.Last;
        public T Min => IsEmpty ? default(T) : _firstList.First;

        public int Size
        {
            get { return _size; }
            private set
            {
                _size = value;
                if (_size > Capacity) {
                    Capacity = _size + _maxDepth;
                    _maxDepth = CalcMaxDepth(Capacity);
                }
                if (_lists[_dirty].Depth > _maxDepth)
                    Resquare(_dirty);
            }
        }

        public bool Contains(T value)
        {
            return (!IsEmpty) && (BinarySearch(value) != null);
        }

        public void Delete(T value, bool removeAll = false)
        {
            var emptyLists = new List<VerticalLinkedList<T>>();
            int removed = 0;
            foreach (var verticalList in _lists) {
                removed += verticalList.Remove(value, removeAll);
                if (verticalList.IsEmpty)
                    emptyLists.Add(verticalList);
                if ((!removeAll) && removed > 0)
                    break;
            }
            if (removed > 0)
                Size -= removed;
            foreach (var list in emptyLists)
                _lists.Remove(list);
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
                    if (_lastList.Last.CompareTo(value) >= 0) {
                        var list = WhereToInsert(value);
                        if (list != null) {
                            list.Insert(value);
                            Size++;
                            return;
                        }
                    }
                    if (_lastList.Depth < _maxDepth) {
                        _lastList.Insert(value);
                        Size++;
                        return;
                    }
                }
                AddNewList(value);
            }
        }

        public override string ToString()
        {
            return $"SquareList({Size} ^ {_maxDepth} x {Width}) {Dump(_firstList)} ...  {Dump(_lastList)}";
        }

        private readonly List<VerticalLinkedList<T>> _lists;
        private int _dirty = 0;
        private int _maxDepth = 0;
        private int _size = 0;
        private VerticalLinkedList<T> _firstList => IsEmpty ? null : _lists[0];
        private VerticalLinkedList<T> _lastList => IsEmpty ? null : _lists[_lists.Count - 1];
        private int Width => _lists.Count;

        private void AddNewList(T value)
        {
            _lists.Add(new VerticalLinkedList<T>(value));
            Size++;
        }

        private VerticalLinkedList<T> BinarySearch(T value)
        {
            int i = 0;
            int j = _lists.Count - 1;
            return InternalBinarySearch(value, i, j, true);
        }

        private int CalcMaxDepth(int size)
        {
            return (size > 0) ? Convert.ToInt32(Math.Ceiling(Math.Sqrt(size))) : 0;
        }

        private string Dump(VerticalLinkedList<T> list) => (list == null) ? "!" : list.ToString();

        private VerticalLinkedList<T> FindFirstList(T value, int m, VerticalLinkedList<T> list)
        {
            while (--m >= 0) {
                var l = _lists[m];
                if (l.Contains(value))
                    list = l;
                else
                    break;
            }
            return list;
        }

        private VerticalLinkedList<T> InternalBinarySearch(T value, int i, int j, bool exact)
        {
            if (j < i)
                return null;
            int m = (i + j) / 2;
            var list = _lists[m];
            if (exact) {
                if (list.Contains(value))
                    return FindFirstList(value, m, list);
            } else {
                if (list.Last.CompareTo(value) >= 0) {
                    if ((m <= 0) || (_lists[m - 1].Last.CompareTo(value) < 0)) {
                        _dirty = m;
                        return list;
                    }
                }
            }
            if (list.Last.CompareTo(value) > 0)
                j = m - 1;
            else
                i = m + 1;
            return InternalBinarySearch(value, i, j, exact);
        }

        private void Resquare(int start)
        {
            for (int i = start; i < _lists.Count;) {
                var list = _lists[i];
                var nextList = ((i + 1) < _lists.Count) ? _lists[i + 1] : null;
                var delta = list.Depth - _maxDepth;
                if (delta < -1) {
                    if (nextList != null) {
                        list.MoveToTail(nextList, -delta);
                        if (nextList.Depth == 0)
                            _lists.Remove(nextList);
                    }
                    if (list.Depth == 0) {
                        _lists.Remove(list);
                        continue;
                    }
                } else if (delta > 0) {
                    if (nextList == null) {
                        nextList = new VerticalLinkedList<T>();
                        _lists.Add(nextList);
                    }
                    nextList.MoveToHead(list, delta);
                }
                i++;
            }
            _dirty = 0;
        }

        private VerticalLinkedList<T> WhereToInsert(T value)
        {
            int i = 0;
            int j = _lists.Count - 1;
            return InternalBinarySearch(value, i, j, false);
        }
    }
}
