using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections;

namespace Sortedset
{
    public class SortedSet<T> where T : IComparable<T>
    {
        private LeftLeaningRedBlackTree<T> tree = new LeftLeaningRedBlackTree<T>();
        public void Insert(T item)
        {
            tree.Add(item);
        }
        public void Clear()
        {
            tree.Clear();
        }
        public bool Contains(T item)
        {
            return tree.Contains(item);
        }
        public bool Remove(T item)
        {
            return tree.Remove(item);
        }
        public T Min()
        {
            return tree.Min();
        }
        public T Max()
        {
            return tree.Max();

        }
        public  T TopofHouse(T item)
        {

            return tree.FindNear(item, true);
        }
        public T BottomofHouse(T item)
        {
            return tree.FindNear(item, false);
        }
        public Sor
    }
}
