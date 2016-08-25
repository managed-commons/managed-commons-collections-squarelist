using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commons.Collections
{
    internal class VerticalLinkedList<T> : IEnumerable<T> where T : IComparable
    {
        public VerticalLinkedList()
        {
        }

        public VerticalLinkedList(T Value)
        {
            InsertAsFirst(Value);
        }

        public int Depth { get; private set; }

        public T First => _first == null ? default(T) : _first.Value;

        public bool IsEmpty => _first == null;

        public T Last => _last == null ? default(T) : _last.Value;

        public bool Contains(T value) => Count(value) > 0;

        public int Count(T value)
        {
            lock (this) {
                if (IsEmpty || !InRange(value))
                    return 0;
                var up = _first;
                var down = _last;
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
            return ImplementedEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ImplementedEnumerator();
        }

        public bool InRange(T value) => (!IsEmpty) && First.CompareTo(value) <= 0 && Last.CompareTo(value) >= 0;

        public void Insert(T value)
        {
            lock (this) {
                if (IsEmpty) {
                    InsertAsFirst(value);
                    return;
                }
                var up = _first;
                var down = _last;
                while (up != null && down != null) {
                    if (up.Value.CompareTo(value) > 0) {
                        AddBefore(up, value);
                        return;
                    }
                    if (down.Value.CompareTo(value) <= 0) {
                        AddAfter(down, value);
                        return;
                    }
                    up = up.Next;
                    down = down.Previous;
                }
            }
        }

        public void InsertAsFirst(T value)
        {
            lock (this) {
                var node = new Node(value);
                node.Next = _first;
                if (_first == null)
                    _last = node;
                else
                    _first.Previous = node;
                _first = node;
                Depth++;
            }
        }

        public void InsertAsLast(T value)
        {
            lock (this) {
                var node = new Node(value);
                node.Previous = _last;
                if (_last == null)
                    _first = node;
                else
                    _last.Next = node;
                _last = node;
                Depth++;
            }
        }
        public void MoveToTail(VerticalLinkedList<T> from, int delta)
        {
            Node first = from._first;
            if (first == null)
                return;
            Node last = null;
            Node cut = first;
            int size = 0;
            while (delta++ < 0 && cut != null) {
                last = cut;
                cut = cut.Next;
                size++;
            }
            if (_last == null)
                _first = first;
            else {
                _last.Next = first;
                first.Previous = _last;
            }
            _last = last;
            Depth += size;
            from._first = cut;
            from.Depth -= size;
            if (cut == null)
                from._last = null;
        }

        public void MoveToHead(VerticalLinkedList<T> from, int delta)
        {
            while (delta-- > 0) {
                this.InsertAsFirst(from.RemoveLast());
            }
        }

        public int Remove(T value, bool removeAll)
        {
            lock (this) {
                if (IsEmpty || !InRange(value))
                    return 0;
                var up = _first;
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
                            InnerRemove(up);
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
            return InnerRemove(_first);
        }

        public T RemoveLast()
        {
            return InnerRemove(_last);
        }

        public override string ToString()
        {
            return IsEmpty ? "[]" : $"[{Concat(this.Take(10))}{(Depth > 10 ? " ..." : "")}]";
        }

        private Node _first;

        private Node _last;

        private void AddAfter(Node node, T value)
        {
            var newNode = new Node(value);
            newNode.Previous = node;
            newNode.Next = node.Next;
            node.Next = newNode;
            if (newNode.Next != null)
                newNode.Next.Previous = newNode;
            else
                _last = newNode;
            Depth++;
        }

        private void AddBefore(Node node, T value)
        {
            var newNode = new Node(value);
            newNode.Next = node;
            newNode.Previous = node.Previous;
            node.Previous = newNode;
            if (newNode.Previous != null)
                newNode.Previous.Next = newNode;
            else
                _first = newNode;
            Depth++;
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
            var next = _first;
            while (next != null) {
                yield return next.Value;
                next = next.Next;
            }
        }

        private T InnerRemove(Node node)
        {
            if (node == null)
                return default(T);
            if (node.Previous == null)
                _first = node.Next;
            else
                node.Previous.Next = node.Next;
            if (node.Next == null)
                _last = node.Previous;
            else
                node.Next.Previous = node.Previous;
            Depth--;
            return node.Value;
        }

        private class Node
        {
            public Node Next;

            public Node Previous;

            public T Value;

            public Node(T value)
            {
                Value = value;
            }
        }
    }
}
