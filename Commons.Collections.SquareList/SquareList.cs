using System;
using System.Collections;
using System.Collections.Generic;

namespace Commons.Collections
{
    public class SquareList<T> : IEnumerable<T> where T : IComparable
    {
        public bool IsEmpty => _size == 0;
        public T Max => IsEmpty ? default(T) : _lastList.Last;
        public T Min => IsEmpty ? default(T) : _lists[0].First;

        public int Size
        {
            get { return _size; }
            private set
            {
                _size = value;
                var oldMaxDepth = _maxDepth;
                _maxDepth = (_size > 0) ? Convert.ToInt32(Math.Ceiling(Math.Sqrt(_size))) : 0;
                if (oldMaxDepth != _maxDepth)
                    Resquare(0);
            }
        }

        public int Width => _lists.Count;

        public bool Contains(T value)
        {
            return (!IsEmpty) && (BinarySearch(value) != null);
        }

        public void Delete(T value, bool removeAll = false)
        {
            int removed = 0;
            foreach (var verticalList in _lists) {
                removed += verticalList.Remove(value, removeAll);
                if ((!removeAll) && removed > 0)
                    break;
            }
            if (removed > 0)
                Size -= removed;
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
            return $"SquareList({Size} ^ {_maxDepth}) ";
        }

        private readonly List<VerticalLinkedList<T>> _lists = new List<VerticalLinkedList<T>>();

        private int _maxDepth = 0;

        private int _size = 0;
        private int _dirty;

        private VerticalLinkedList<T> _lastList => _lists[_lists.Count - 1];

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

        private VerticalLinkedList<T> WhereToInsert(T value)
        {
            int i = 0;
            int j = _lists.Count - 1;
            return InternalBinarySearch(value, i, j, false);
        }

        private VerticalLinkedList<T> FindFirstList(T value, int m, VerticalLinkedList<T> list)
        {
            while (--m >= 0) {
                var l = _lists[m];
                if (l.InRange(value))
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
                if (list.InRange(value))
                    return FindFirstList(value, m, list);
            } else {
                if (list.Last.CompareTo(value) >= 0) {
                    if ((m <= 0) || (_lists[m - 1].Last.CompareTo(value) < 0))
                        return list;
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
            for (int i = start; i < _lists.Count; i++) {
                var list = _lists[i];
                var nextList = ((i + 1) < _lists.Count) ? _lists[i + 1] : null;
                var delta = list.Depth - _maxDepth;
                if (delta < 0) {
                    if (nextList != null) {
                        list.MoveToTail(nextList, delta);
                        if (nextList.Depth == 0)
                            _lists.Remove(nextList);
                    }
                    if (list.Depth == 0)
                        _lists.Remove(list);
                } else if (delta > 0) {
                    if (nextList == null) {
                        nextList = new VerticalLinkedList<T>();
                        _lists.Add(nextList);
                    }
                    nextList.MoveToHead(list, delta);
                }
            }
        }

    }
}
