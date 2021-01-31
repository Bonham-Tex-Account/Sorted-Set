using System.Text;
using System;
using System.Collections.Generic;

namespace Sortedset
{
    public class LeftLeaningRedBlackTree<T> where T : IComparable<T>
    {
        public Node _rootNode;
        public int Count { get; private set; }
        public class Node
        {
            public T Value;
            public Node Left;
            public Node Right;
            public bool IsBlack;
            public Node(T value)
            {
                this.Value = value;
                IsBlack = false;
                Left = null;
                Right = null;
            }
        }
        public LeftLeaningRedBlackTree()
        {
            Count = 0;
            _rootNode = null;
        }
        public void Add(T value)
        {
            _rootNode = Add(_rootNode, value);
            _rootNode.IsBlack = true;
        }
        private Node Add(Node node, T value)
        {
            if (node == null)
            {
                Count++;
                return new Node(value);
            }

            if (IsRed(node.Left) && IsRed(node.Right))
            {
                FlipColor(node);
            }
            if (value.CompareTo(node.Value) < 0)
            {
                node.Left = Add(node.Left, value);
            }
            else if (value.CompareTo(node.Value) > 0)
            {
                node.Right = Add(node.Right, value);
            }
            else
            {
                throw new ArgumentException("An entry with the same value arleady exists");
            }

            if (IsRed(node.Right))
            {
                node = RotateLeft(node);
            }

            if (IsRed(node.Left) && IsRed(node.Left.Left))
            {
                node = RotateRight(node);
            }

            return node;
        }
        public bool Remove(T value)
        {
            int initialCount = Count;
            if (_rootNode != null)
            {
                _rootNode = Remove(_rootNode, value);
                if (_rootNode != null)
                {
                    _rootNode.IsBlack = true;
                }
            }

            return initialCount != Count;
        }
        private Node Remove(Node node, T value)
        {
            if (value.CompareTo(node.Value) < 0)
            {
                if (node.Left != null)
                {
                    if (!IsRed(node.Left) && !IsRed(node.Left.Left))
                    {
                        node = MoveRedLeft(node);
                    }
                    node.Left = Remove(node.Left, value);
                }
            }
            else
            {
                if (IsRed(node.Left))
                {
                    node = RotateRight(node);
                }

                if (value.CompareTo(node.Value) == 0 && node.Right == null)
                {
                    Count--;
                    return null;
                }

                if (node.Right != null)
                {
                    if (!IsRed(node.Right) && !IsRed(node.Right.Left))
                    {
                        node = MoveRedRight(node);
                    }
                    if (value.CompareTo(node.Value) == 0)
                    {
                        Node min = GetMinimum(node.Right);
                        node.Value = min.Value;
                        node.Right = Remove(node.Right, min.Value);
                    }
                    else
                    {
                        node.Right = Remove(node.Right, value);
                    }
                }
            }
            return Fixup(node);
        }
        private Node DeleteMinimum(Node node)
        {
            if (node.Left == null)
            {

                return null;
            }

            if (!IsRed(node.Left) && !IsRed(node.Left.Left))
            {

                node = MoveRedLeft(node);
            }


            node.Left = DeleteMinimum(node.Left);


            return Fixup(node);
        }


        public void Clear()
        {
            _rootNode = null;
            Count = 0;
        }

        private Node RotateLeft(Node node)
        {

            Node temp = node.Right;
            node.Right = temp.Left;
            temp.Left = node;


            temp.IsBlack = node.IsBlack;
            node.IsBlack = false;

            return temp;
        }


        private Node RotateRight(Node node)
        {

            Node temp = node.Left;
            node.Left = temp.Right;
            temp.Right = node;


            temp.IsBlack = node.IsBlack;
            node.IsBlack = false;

            return temp;
        }


        private void FlipColor(Node node)
        {
            node.IsBlack = !node.IsBlack;
            node.Left.IsBlack = !node.Left.IsBlack;
            node.Right.IsBlack = !node.Right.IsBlack;
        }


        private Node MoveRedLeft(Node node)
        {
            FlipColor(node);
            if (IsRed(node.Right.Left))
            {

                node.Right = RotateRight(node.Right);
                node = RotateLeft(node);

                FlipColor(node);

                if (IsRed(node.Right.Right))
                {
                    node.Right = RotateLeft(node.Right);
                }
            }

            return node;
        }


        private Node MoveRedRight(Node node)
        {
            FlipColor(node);
            if (IsRed(node.Left.Left))
            {
                node = RotateRight(node);
                FlipColor(node);
            }

            return node;
        }


        private Node GetMinimum(Node node)
        {
            Node temp = node;
            while (temp.Left != null)
            {
                temp = temp.Left;
            }
            return temp;
        }


        private Node Fixup(Node node)
        {
            if (IsRed(node.Right))
            {

                node = RotateLeft(node);
            }

            if (IsRed(node.Left) && IsRed(node.Left.Left))
            {

                node = RotateRight(node);
            }

            if (IsRed(node.Left) && IsRed(node.Right))
            {
                FlipColor(node);
            }


            if ((node.Left != null) && IsRed(node.Left.Right) && !IsRed(node.Left.Left))
            {
                node.Left = RotateLeft(node.Left);


                if (IsRed(node.Left))
                {

                    node = RotateRight(node);
                }
            }

            return node;
        }


        internal bool IsRed(Node node)
        {
            if (node == null)
            {
                return false;
            }

            return !node.IsBlack;
        }
        public bool Contains(T item)
        {
            Node temp = _rootNode;
            while (temp != null)
            {
                if (item.CompareTo(temp.Value) == 0)
                {
                    return true;
                }
                if (item.CompareTo(temp.Value) < 0)
                {
                    temp = temp.Left;
                }
                if (item.CompareTo(temp.Value) > 0)
                {
                    temp = temp.Right;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        public T Min()
        {
            Node temp = _rootNode;
            while (temp != null)
            {
                if (temp.Left != null)
                {
                    temp = temp.Left;
                }
                else
                {
                    return temp.Value;
                }
            }
            return default(T);
        }
        public T Max()
        {
            Node temp = _rootNode;
            while (temp != null)
            {
                if (temp.Right != null)
                {
                    temp = temp.Right;
                }
                else
                {
                    return temp.Value;
                }
            }
            return default(T);
        }
        public Node Find(T item)
        {
            Node temp = _rootNode;
            while (temp != null)
            {
                if (item.CompareTo(temp.Value) == 0)
                {
                    return temp;
                }
                if (item.CompareTo(temp.Value) < 0)
                {
                    temp = temp.Left;
                }
                if (item.CompareTo(temp.Value) > 0)
                {
                    temp = temp.Right;
                }
                else
                {
                    return temp;
                }
            }
            return temp;
        }
        /* public T FindNearest(T item,bool IsTop)
         {
             Node node= Find(item);
             if(node==null)
             {

             }
             else
             {
                 return node.Value;
             }



             return default(T);
         }*/
        public T FindNear(T item, bool IsTop)
        {
            T thing = default(T);
            Node node = Find(item);
            if (node == null)
            {
                return Near(_rootNode, thing, IsTop, item);
            }
            else
            {
                return node.Value;
            }
        }

        public T Near(Node node, T closestvalue, bool IsTop, T item)
        {
            if (node == null)
            {
                return closestvalue;
            }
            if (IsTop)
            {
                if (node.Value.CompareTo(item) > 0)
                {
                    if (node.Value.CompareTo(closestvalue) < 0)
                    {
                        closestvalue = node.Value;
                    }
                    T a = Near(node.Right, closestvalue, IsTop, item);
                    T b = Near(node.Left, closestvalue, IsTop, item);
                    if(a.CompareTo(b)<0)
                    {
                        return a;
                    }
                    if(b.CompareTo(a)<0)
                    {
                        return b;
                    }
                }
            }
            if (!IsTop)
            {

                if (node.Value.CompareTo(item) < 0)
                {
                    if (node.Value.CompareTo(closestvalue) > 0)
                    {
                        closestvalue = node.Value;
                    }
                    T a = Near(node.Left, closestvalue, IsTop, item);
                    T b = Near(node.Right, closestvalue, IsTop, item);
                    if(a.CompareTo(b)>0)
                    {
                        return a;
                    }
                    if(a.CompareTo(b)<0)
                    {
                        return b;
                    }
                }
            }
            return default(T);
        }
        internal IEnumerable<T> Traverse<T>(Node node, Func<Node, bool> condition, Func<Node, T> selector)
        {

            Stack<Node> stack = new Stack<Node>();
            Node current = node;
            while (null != current)
            {
                if (null != current.Left)
                {

                    stack.Push(current);
                    current = current.Left;
                }
                else
                {
                    do
                    {


                        if (condition(current))
                        {
                            yield return selector(current);
                        }


                        current = current.Right;
                    }
                    while ((null == current) &&
                           (0 < stack.Count) &&
                           (null != (current = stack.Pop())));
                }
            }
        }
    }
}
