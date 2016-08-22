using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons.Collections
{
    public class SquareList<T> where T : IComparable
    {
        private class Node
        {
            public T Value;
            public Node Parent;
            public Node Child;
            public Node Left;
            public Node Right;
            public Node Bottom;
            public int Depth;

            public Node(T value)
            {
                Value = value;
                Bottom = Left = Right = this;
                Parent = Child = null;
                Depth = 1;
            }

            public void InsertBefore(Node node)
            {
                Parent = node.Parent;
                Child = node;
                node.Parent = this;
                if (Parent != null) {
                    Parent.Child = this;
                    Right = Left = null;
                    Depth = 0;
                } else {
                    Depth = node.Depth + 1;
                    Bottom = node.Bottom;
                    if (node != node.Right) {
                        Right = node.Right;
                        Left = node.Left;
                        Right.Left = this;
                        Left.Right = this;
                    } else {
                        Right = Left = this;
                    }
                    node.Bottom = node.Right = node.Left = null;
                    node.Depth = 0;
                }
            }

            public override string ToString()
            {
                return $"{Left.Value} < [{Value}] ({Depth}) > {Right.Value} >> {Bottom.Value}";
            }
        }

        private Node Head;
        public int Size { get; private set; }
        public int MaxDepth { get; private set; }

        public T Min => Head == null ? default(T) : Head.Value;
        public T Max => Head == null ? default(T) : Head.Left.Bottom.Value;

        public void Insert(T value)
        {
            Node toAdd = new Node(value);
            lock (this) {
                Size++;
                MaxDepth = Convert.ToInt32(Math.Ceiling(Math.Sqrt(Size)));
                if (Size == 1)
                    Head = toAdd;
                else {
                    Node vertList = FindVertList(toAdd);
                    PutInVertList(vertList, toAdd);
                }
            }
        }

        private void PutInVertList(Node vertList, Node toAdd)
        {
            var it = vertList;
            while (it != null && it.Value.CompareTo(toAdd.Value) < 0)
                it = it.Child;
            if (it == null) {
                AddToBottom(vertList, toAdd);
            } else {
                toAdd.InsertBefore(it);
            }
            if (it == vertList) {
                if (vertList == Head)
                    Head = toAdd;
                vertList = toAdd;
            } else
                vertList.Depth++;
            ShiftRight(vertList);
        }

        private static void AddToBottom(Node vertList, Node toAdd)
        {
            vertList.Bottom.Child = toAdd;
            toAdd.Parent = vertList.Bottom;
            vertList.Bottom = toAdd;
        }

        private void ShiftRight(Node vertList)
        {
            if (vertList.Depth <= MaxDepth)
                return;
            var newTop = vertList.Bottom;
            var newBottom = newTop.Parent;
            var oldTop = vertList.Right;
            vertList.Bottom = newBottom;
            newBottom.Child = null;
            vertList.Depth--;
            if (oldTop != Head) {
                newTop.InsertBefore(oldTop);
            } else {
                newTop.Depth = 1;
                newTop.Left = vertList;
                newTop.Right = Head;
                newTop.Parent = newTop.Child = null;
                newTop.Bottom = newTop;
                vertList.Right = Head.Left = newTop;
            }
        }

        private Node FindVertList(Node toAdd)
        {
            var vertList = Head;
            while (vertList.Bottom.Value.CompareTo(toAdd.Value) < 0 && vertList.Right != Head)
                vertList = vertList.Right;
            return vertList;
        }

        public void Delete(T value)
        {
            lock (this) {
                if (Head == null)
                    return;
                var vertList = Head;
                while (vertList.Bottom.Value.CompareTo(value) < 0 && vertList.Right != Head)
                    vertList = vertList.Right;
                var item = vertList;
                while (item != null && item.Value.CompareTo(value) != 0)
                    item = item.Child;
                if (item == null)
                    return;
                Size--;
                MaxDepth = Convert.ToInt32(Math.Ceiling(Math.Sqrt(Size)));
                vertList.Depth--;
                if (Size == 0)
                    Head = null;
                else
                    RemoveItem(vertList, item);
            }
        }

        private void RemoveItem(Node vertList, Node item)
        {
            if (item == vertList) {
                RemoveFirst(vertList, item);
            } else if (item == vertList.Bottom) {
                vertList.Bottom = item.Parent;
                item.Parent.Child = null;
            } else {
                item.Parent.Child = item.Child;
                item.Child.Parent = item.Parent;
            }
        }

        private void RemoveFirst(Node vertList, Node item)
        {
            var newTop = item.Child;
            if (newTop != null) {
                newTop.Parent = null;
                if (vertList.Left != vertList) {
                    newTop.Left = vertList.Left;
                    newTop.Left.Right = newTop;
                    newTop.Right = vertList.Right;
                    newTop.Right.Left = newTop;
                } else {
                    newTop.Left = newTop;
                    newTop.Right = newTop;
                }
                newTop.Bottom = vertList.Bottom;
                newTop.Depth = vertList.Depth;
                if (vertList == Head)
                    Head = newTop;
            } else {
                if (vertList.Right != vertList) {
                    vertList.Right.Left = vertList.Left;
                    vertList.Left.Right = vertList.Right;
                    Head = vertList.Right;
                }
            }
        }

 
        public override string ToString()
        {
            return $"[{Head}] ({Size} ^ {MaxDepth})";
        }
    }
}
