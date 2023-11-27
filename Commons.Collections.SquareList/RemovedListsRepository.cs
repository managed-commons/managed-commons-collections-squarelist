// ******************************************************************************************************************************
// ****
// ****      Copyright (c) 2016-2021 Rafael 'Monoman' Teixeira
// ****
// ******************************************************************************************************************************

namespace Commons.Collections;

internal class RemovedListsRepository<T> where T : IComparable
{
    internal void Add(VerticalLinkedList<T> list) {
        list.Clear();
        _lists.Add(list.Id, list);
    }

    internal void Clear() => _lists.Clear();

    internal VerticalLinkedList<T> Recover(int id) {
        if (_lists.TryGetValue(id, out var value)) {
            var unremoved = value;
            _lists.Remove(id);
            return unremoved;
        }

        return null;
    }

    private readonly Dictionary<int, VerticalLinkedList<T>> _lists = [];
}
