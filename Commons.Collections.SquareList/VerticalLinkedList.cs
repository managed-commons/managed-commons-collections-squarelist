using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commons.Collections
{
    internal class VerticalLinkedList<T> : IEnumerable<T> where T : IComparable
    {
        public VerticalLinkedList(T[] bigArray, int maxDepth, int firstIndex)
        {
            _bigArray = bigArray;
            _maxDepth = maxDepth;
            _firstIndex = firstIndex;
            _count = 0;
        }

        public VerticalLinkedList(T[] bigArray, int maxDepth, int firstIndex, T Value) : this(bigArray, maxDepth, firstIndex)
        {
            InsertAsFirst(Value);
        }

        public int Depth => _count;
        public T First => IsEmpty ? default(T) : _bigArray[_firstIndex];
        public bool IsEmpty => _count < 1;
        public bool IsFull => _count >= _maxDepth;
        public T Last => IsEmpty ? default(T) : _bigArray[_lastIndex];

        public bool Contains(T value)
        {
            if (!InRange(value))
                return false;
            var up = _firstIndex;
            var down = _lastIndex;
            for (int i = _count; i > 0; i--) {
                if (_bigArray[up].CompareTo(value) == 0 || _bigArray[down].CompareTo(value) == 0)
                    return true;
                up++;
                down--;
            }
            return false;
        }

        public int Count(T value)
        {
            lock (this) {
                if (!InRange(value))
                    return 0;
                var up = _firstIndex;
                var down = _lastIndex;
                int count = 0;
                for (int i = _count; i > 0; i--) {
                    var upCompare = _bigArray[up].CompareTo(value);
                    if (upCompare > 0)
                        return 0;
                    if (upCompare == 0) {
                        while (upCompare == 0) {
                            count++;
                            up++;
                            if (--i <= 0)
                                return count;
                            upCompare = _bigArray[up].CompareTo(value);
                        }
                        return count;
                    }
                    var downCompare = _bigArray[down].CompareTo(value);
                    if (downCompare < 0)
                        return 0;
                    if (downCompare == 0) {
                        while (downCompare == 0) {
                            count++;
                            down--;
                            if (--i <= 0)
                                return count;
                            downCompare = _bigArray[down].CompareTo(value);
                        }
                        return count;
                    }
                    up++;
                    down--;
                }
                return count;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ImplementedEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ImplementedEnumerator();
        }

        public bool InRange(T value) => (!IsEmpty) && First.CompareTo(value) <= 0 && Last.CompareTo(value) >= 0;

        public void Insert(T value)
        {
            if (IsEmpty) {
                InsertAsFirst(value);
                return;
            }
            if (IsFull)
                throw new ArgumentOutOfRangeException();
            var up = _firstIndex;
            var down = _lastIndex;
            for (int i = _count; i > 0; i--) {
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

        public void InsertAsFirst(T value)
        {
            if (IsFull)
                throw new ArgumentOutOfRangeException();
            AddBefore(_firstIndex, value);
        }

        public void InsertAsLast(T value)
        {
            if (IsFull)
                throw new ArgumentOutOfRangeException();
            _count++;
            _bigArray[_lastIndex] = value;
        }

        public int Remove(T value, bool removeAll)
        {
            lock (this) {
                if (!Contains(value))
                    return 0;
                var up = _firstIndex;
                int removed = 0;
                for (int i = _count; i > 0; i--) {
                    var upCompare = _bigArray[up].CompareTo(value);
                    if (upCompare > 0)
                        break;
                    if (upCompare < 0)
                        up++;
                    else {
                        var start = up;
                        while (upCompare == 0) {
                            removed++;
                            InnerRemove(up);
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

        public T RemoveFirst()
        {
            return InnerRemove(_firstIndex);
        }

        public T RemoveLast()
        {
            return InnerRemove(_lastIndex);
        }

        public override string ToString()
        {
            return IsEmpty ? "[]" : $"<{Depth}> [{Concat(this.Take(10))}{(Depth > 10 ? " ..." : "")}]";
        }

        internal VerticalLinkedList<T> CopyTo(T[] newArray, int newMaxDepth, int firstIndex)
        {
            var newList = new VerticalLinkedList<T>(newArray, newMaxDepth, firstIndex);
            var index = _firstIndex;
            for (int i = _count; i > 0; i--)
                newArray[firstIndex++] = _bigArray[index++];
            newList._count = _count;
            return newList;
        }

        internal bool OpenSpaceBack(VerticalLinkedList<T> startList)
        {
            if (startList == null)
                return false;
            int delta = startList._slack;
            var bottom = _lastIndex - delta;
            for (int i = startList._lastIndex; i <= bottom; i++)
                _bigArray[i] = _bigArray[i + delta];
            _count -= delta;
            startList._count += delta;
            return true;
        }

        internal bool OpenSpaceThru(VerticalLinkedList<T> endList)
        {
            if (endList == null)
                return false;
            int delta = endList._slack / 2;
            if (delta < 1)
                delta = 1;
            var bottom = _lastIndex + 1 - delta;
            for (int i = endList._lastIndex; i >= bottom; i--)
                _bigArray[i + delta] = _bigArray[i];
            _count -= delta;
            endList._count += delta;
            return true;
        }

        internal void Receive(VerticalLinkedList<T> from)
        {
            if (from == null)
                throw new ArgumentNullException(nameof(from));
            int delta = from._firstIndex;
            for (int i = from._count - 1; i >= 0; i--)
                _bigArray[i] = _bigArray[i + delta];
            _count = from._count;
            from._count = 0;
        }

        private T[] _bigArray;
        private int _count;
        private int _firstIndex;
        private int _maxDepth;
        private int _lastIndex => _firstIndex + _count - 1;
        private int _slack => _maxDepth - _count;

        private void AddAfter(int index, T value)
        {
            AddBefore(index + 1, value);
        }

        private void AddBefore(int index, T value)
        {
            for (int i = _lastIndex; i >= index; i--)
                _bigArray[i + 1] = _bigArray[i];
            _bigArray[index] = value;
            _count++;
        }

        private string Concat(IEnumerable things)
        {
            var sb = new StringBuilder();
            foreach (var thing in things) {
                sb.Append(thing);
                sb.Append(' ');
            }
            if (sb.Length > 0)
                sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        private IEnumerator<T> ImplementedEnumerator()
        {
            if (IsEmpty)
                yield break;
            var next = _firstIndex;
            for (int i = _count; i > 0; i--)
                yield return _bigArray[next++];
        }

        private T InnerRemove(int index)
        {
            if (IsEmpty || index < _firstIndex || index > _lastIndex)
                return default(T);
            var value = _bigArray[index];
            for (int i = index; i <= _lastIndex; i++)
                _bigArray[i] = _bigArray[i + 1];
            _count--;
            return value;
        }
    }
}
