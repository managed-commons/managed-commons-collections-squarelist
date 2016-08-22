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
            return FindAndAct(value, (list, i) => true);
        }

        public void Delete(T value)
        {
            FindAndAct(value, (list, i) => {
                list.RemoveAt(i);
                if (list.Count == 0)
                    _lists.Remove(list);
                Size--;
                return true;
            });
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
                            for (int i = 0; i < list.Count; i++)
                                if (list[i].CompareTo(value) > 0) {
                                    list.Insert(i, value);
                                    Size++;
                                    return;
                                }
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

        private bool FindAndAct(T value, Func<VerticalList, int, bool> act)
        {
            lock (this) {
                if (!IsEmpty)
                    foreach (var list in _lists)
                        if (list.Last.CompareTo(value) >= 0)
                            for (int i = 0; i < list.Count; i++)
                                if (list[i].CompareTo(value) == 0)
                                    return act(list, i);

                return false;
            }
        }

        private void Resquare()
        {
            for (int i = 0; i < _lists.Count; i++) {
                var list = _lists[i];
                var nextList = ((i + 1) < _lists.Count) ? _lists[i + 1] : null;
                var delta = list.Count - MaxDepth;
                if (delta < 0) {
                    if (nextList != null) {
                        while (delta++ < 0) {
                            if (nextList.Count > 0) {
                                list.InsertAsLast(nextList.RemoveFirst());
                            }
                        }
                        if (nextList.Count == 0)
                            _lists.Remove(nextList);
                    }
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

        private class VerticalList : List<T>
        {
            public VerticalList()
            {
            }

            public VerticalList(T Value)
            {
                Add(Value);
            }

            public T First => Count == 0 ? default(T) : this[0];
            public T Last => Count == 0 ? default(T) : this[Count - 1];

            public void InsertAsFirst(T value)
            {
                Insert(0, value);
            }

            public void InsertAsLast(T value)
            {
                Insert(Count, value);
            }

            public T RemoveFirst()
            {
                if (Count == 0)
                    return default(T);
                T first = First;
                RemoveAt(0);
                return first;
            }

            public T RemoveLast()
            {
                if (Count == 0)
                    return default(T);
                T last = Last;
                RemoveAt(Count - 1);
                return last;
            }
        }
    }
}
