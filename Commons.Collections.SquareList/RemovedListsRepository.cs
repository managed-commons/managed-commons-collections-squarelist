using System;
using System.Collections.Generic;
using System.Text;

namespace Commons.Collections
{
    internal class RemovedListsRepository<T> where T : IComparable
    {
        public void Add(VerticalLinkedList<T> list)
        {
            list.Clear();
            _lists.Add(list.Id, list);
        }

        public void Clear()
        {
            _lists.Clear();
        }

        public VerticalLinkedList<T> Recover(int id)
        {
            if (!_lists.ContainsKey(id))
                return null;
            var unremoved = _lists[id];
            _lists.Remove(id);
            return unremoved;
        }

        public override string ToString()
        {
            var sb = new StringBuilder("{");
            foreach (var key in _lists.Keys)
                sb.Append($" {key}");
            sb.Append(" }");
            return sb.ToString();
        }

        private readonly Dictionary<int, VerticalLinkedList<T>> _lists = new Dictionary<int, VerticalLinkedList<T>>();
    }
}
