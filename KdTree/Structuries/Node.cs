using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KdTree.Structuries
{
    internal class Node<T> where T : IKdTreeComparable<T>
    {
        public T Data { get; private set; }
        public Node<T> LeftSon { get;  set; } //private???
        public Node<T> RightSon { get;  set; } //private??
        public Node<T> Parent { get; private set; }
        public int KeyIndex { get; private set; }
        public int Level { get; private set; }
        private int keyMaxIndex;


        public Node(T data, int keyMaxIndex)
        {
            Data = data;
            this.keyMaxIndex = keyMaxIndex;
            KeyIndex = 1;
            Level = 1;
        }

        public void AddSon(Node<T> son)
        {
            son.Level++;
            son.KeyIndex = KeyIndex + 1;
            if (son.KeyIndex > keyMaxIndex) son.KeyIndex = 1;

            son.Parent = this;
            if (Data.Compare(son.Data, KeyIndex) > 0)
            {
                if (RightSon == null)
                    RightSon = son;
                else
                    RightSon.AddSon(son);
            }
            else
            {
                if (LeftSon == null)
                    LeftSon = son;
                else
                    LeftSon.AddSon(son);
            }
        }

        public Node<T> FindNodeWithCompare(Node<T> compare)
        {
            switch(Data.Compare(compare.Data, KeyIndex))
            {
                case 0:
                    if (Data.FullComapre(compare.Data))
                        return this;
                    else
                        return LeftSon;

                case 1: return RightSon;
                case -1: return LeftSon;
                default: return null;
            }
        }
        public IEnumerable<Node<T>> GetInOrder()
        {
            yield return LeftSon;//.GetInOrder();
            yield return this;
            yield return RightSon;
        }

        public void DeleteRightSon() => RightSon = null;
        public void DeleteLeftSon() => LeftSon = null;
        public void DeleteParent() => Parent = null;

        public void TakeFromAllReferences(Node<T> node)
        {
            node.Parent = Parent;
            node.LeftSon = LeftSon;
            node.RightSon = RightSon;
            node.Level = Level;
            node.KeyIndex = KeyIndex;
            if(RightSon != null)
                RightSon.Parent = node;
            if(LeftSon != null)
                LeftSon.Parent = node;
        }

        public void ClearNodeReferences()
        {
            DeleteLeftSon();
            DeleteRightSon();
            DeleteParent();
            Level = 1;
            KeyIndex = 1;
        }
    }
}
