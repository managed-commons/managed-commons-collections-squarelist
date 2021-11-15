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
        if (!_lists.ContainsKey(id))
            return null;
        var unremoved = _lists[id];
        _lists.Remove(id);
        return unremoved;
    }

    private readonly Dictionary<int, VerticalLinkedList<T>> _lists = new();
}
