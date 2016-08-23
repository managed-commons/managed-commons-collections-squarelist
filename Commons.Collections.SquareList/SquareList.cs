using System;
using System.Collections;
using System.Collections.Generic;

namespace Commons.Collections
{
    public class SquareList<T> : IEnumerable<T> where T : IComparable
    {
        public bool IsEmpty => _size == 0;
        public T Max => IsEmpty ? default(T) : _lastList.Last;
        public int MaxDepth => Convert.ToInt32(Math.Ceiling(Math.Sqrt(_size)));
        public T Min => IsEmpty ? default(T) : _lists[0].First;

        public int Size
        {
            get { return _size; }
            private set { _size = value; Resquare(); }
        }

        public int Width => _lists.Count;

        public bool Contains(T value)
        {
            foreach (var verticalList in _lists)
                if (verticalList.Contains(value))
                    return true;
            return false;
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
                    foreach (var list in _lists) {
                        if (list.Last.CompareTo(value) > 0) {
                            list.Insert(value);
                            Size++;
                            return;
                        }
                    }
                }
                AddNewList(value);
            }
        }

        public override string ToString()
        {
            return $"SquareList({Size} ^ {MaxDepth}) ";
        }

        private readonly List<VerticalList> _lists = new List<VerticalList>();
        private int _size = 0;
        private VerticalList _lastList => _lists[_lists.Count - 1];

        private void AddNewList(T value)
        {
            _lists.Add(new VerticalList(value));
            Size++;
        }

        private void Resquare()
        {
            for (int i = 0; i < _lists.Count; i++) {
                var list = _lists[i];
                var nextList = ((i + 1) < _lists.Count) ? _lists[i + 1] : null;
                var delta = list.Depth - MaxDepth;
                if (delta < 0) {
                    if (nextList != null) {
                        while (delta++ < 0) {
                            if (nextList.Depth > 0) {
                                list.InsertAsLast(nextList.RemoveFirst());
                            }
                        }
                        if (nextList.Depth == 0)
                            _lists.Remove(nextList);
                    }
                    if (list.Depth == 0)
                        _lists.Remove(list);
                } else if (delta > 0) {
                    if (nextList == null) {
                        nextList = new VerticalList();
                        _lists.Add(nextList);
                    }
                    while (delta-- > 0) {
                        nextList.InsertAsFirst(list.RemoveLast());
                    }
                }
            }
        }

        private class VerticalList : IEnumerable<T>
        {
            public VerticalList()
            {
            }

            public VerticalList(T Value)
            {
                InsertAsFirst(Value);
            }

            public int Depth => _list.Count;
            public T First => IsEmpty ? default(T) : _list.First.Value;
            public bool IsEmpty => Depth == 0;
            public T Last => IsEmpty ? default(T) : _list.Last.Value;

            public bool Contains(T value) => Count(value) > 0;

            public int Count(T value)
            {
                lock (_list) {
                    if (IsEmpty || !InRange(value))
                        return 0;
                    var up = _list.First;
                    var down = _list.Last;
                    int count = 0;
                    while (up != null && down != null) {
                        var upCompare = up.Value.CompareTo(value);
                        if (upCompare > 0)
                            return 0;
                        if (upCompare == 0) {
                            while (upCompare == 0) {
                                count++;
                                up = up.Next;
                                if (up == null)
                                    return count;
                                upCompare = up.Value.CompareTo(value);
                            }
                            return count;
                        }
                        var downCompare = down.Value.CompareTo(value);
                        if (downCompare < 0)
                            return 0;
                        if (downCompare == 0) {
                            while (downCompare == 0) {
                                count++;
                                down = down.Previous;
                                if (down == null)
                                    return count;
                                downCompare = down.Value.CompareTo(value);
                            }
                            return count;
                        }
                        up = up.Next;
                        down = down.Previous;
                    }
                    return count;
                }
            }

            public IEnumerator<T> GetEnumerator()
            {
                return ((IEnumerable<T>)_list).GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IEnumerable<T>)_list).GetEnumerator();
            }

            public void Insert(T value)
            {
                lock (_list) {
                    if (IsEmpty) {
                        _list.AddFirst(value);
                        return;
                    }
                    var up = _list.First;
                    var down = _list.Last;
                    while (up != null && down != null) {
                        if (up.Value.CompareTo(value) > 0) {
                            _list.AddBefore(up, value);
                            return;
                        }
                        if (down.Value.CompareTo(value) <= 0) {
                            _list.AddAfter(down, value);
                        }
                        up = up.Next;
                        down = down.Previous;
                    }
                }
            }

            public void InsertAsFirst(T value)
            {
                _list.AddFirst(value);
            }

            public void InsertAsLast(T value)
            {
                _list.AddLast(value);
            }

            public int Remove(T value, bool removeAll)
            {
                lock (_list) {
                    if (IsEmpty || !InRange(value))
                        return 0;
                    var up = _list.First;
                    int removed = 0;
                    while (up != null) {
                        var upCompare = up.Value.CompareTo(value);
                        if (upCompare > 0)
                            break;
                        if (upCompare < 0)
                            up = up.Next;
                        else {
                            while (upCompare == 0) {
                                var next = up.Next;
                                _list.Remove(up);
                                removed++;
                                if (!removeAll)
                                    break;
                                up = next;
                                if (up == null)
                                    break;
                                upCompare = up.Value.CompareTo(value);
                            }
                            break;
                        }
                    }
                    return removed;
                }
            }

            public T RemoveFirst()
            {
                if (IsEmpty)
                    return default(T);
                T first = First;
                _list.RemoveFirst();
                return first;
            }

            public T RemoveLast()
            {
                if (IsEmpty)
                    return default(T);
                T last = Last;
                _list.RemoveLast();
                return last;
            }

            private readonly LinkedList<T> _list = new LinkedList<T>();

            private bool InRange(T value) => First.CompareTo(value) <= 0 && Last.CompareTo(value) >= 0;
        }
    }
}
