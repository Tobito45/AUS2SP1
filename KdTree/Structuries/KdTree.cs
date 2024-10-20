namespace KdTree.Structuries
{
    internal class KdTree<T> : IStructure<T> where T : IKdTreeComparable<T>
    {
        private readonly int maxKeyLevel;

        public Node<T> Root { get; private set; }

        public KdTree(int maxKeyLevel) => this.maxKeyLevel = maxKeyLevel;

        public void Insert(T data)
        {
            Node<T> node = new Node<T>(data, maxKeyLevel);
            Insert(node);
        }

        public void Insert(Node<T> node)
        {
            if (Root == null)
                Root = node;
            else
                Root.AddSon(node);
        }

        public void Insert(List<T> data, Action<List<T>> actn)
        {
            Queue<List<T>> q = new Queue<List<T>>();
            Queue<List<T>> newQ = new Queue<List<T>>();
            q.Enqueue(data);

            int key = 0;
            while (true)
            {
                key++;
                if (key > maxKeyLevel) key = 1;
                while (q.Count > 0)
                {
                    List<T> list = q.Dequeue();
                    if (list.Count == 2)
                    {
                        Insert(list[0]);
                        Insert(list[1]);
                        continue;
                    }

                    data.Sort((x, y) => x.Compare(y, key));
                    actn(list);
                    //Console.WriteLine(string.Join(",", list.Select(n => n.ToString())));
                    List<T> firstList = new List<T>();
                    List<T> secondList = new List<T>();

                    int midl = list.Count / 2;
                    Insert(list[midl]);
                    for (int i = 0; i < midl; i++)
                        firstList.Add(list[i]);

                    if (firstList.Count > 1)
                        newQ.Enqueue(firstList);
                    else
                        Insert(firstList[0]);

                    for (int i = midl + 1; i < list.Count; i++)
                        secondList.Add(list[i]);

                    if (secondList.Count > 1)
                        newQ.Enqueue(secondList);
                    else
                        Insert(secondList[0]);
                }
                if (newQ.Count > 0)
                    q = newQ;

                if (q.Count == 0)
                    break;
            }
        }
        public IEnumerable<T> Find(T element)
        {
            return FindNodes(element).Select(n => n.Data);
        }

        private List<Node<T>> FindNodes(T element)
        {
            List<Node<T>> list = new List<Node<T>>();
            Node<T> nodeSearch = new Node<T>(element, 0);
            Node<T> aktualNode = Root;

            while (true)
            {
                Node<T> findNew = aktualNode.FindNodeWithCompare(nodeSearch);
                if (findNew == aktualNode)
                {
                    list.Add(findNew);
                    aktualNode = findNew.LeftSon;
                }
                else
                    aktualNode = findNew;

                if (aktualNode == null)
                    break;
            }
            return list;
        }

        public IEnumerable<T> Delete(T element)
        {
            List<T> deletedItems = new List<T>();
            Stack<(Node<T> toDelete, Node<T> chanceNode)> stack = new();
            Stack<Node<T>> stackInsert = new();

            foreach (Node<T> findedNodex in FindNodes(element))
            {
                deletedItems.Add(findedNodex.Data);
                stack.Push((findedNodex, null));

                while (stack.Count > 0)
                {
                    (Node<T> delete, Node<T> chanceNodeStack) = stack.Pop();

                    if (delete.LeftSon == null && delete.RightSon == null && chanceNodeStack == null)
                    {
                        if (delete.Parent == null && delete == Root)
                            Root = null;
                        else
                        {
                            if (delete == delete.Parent.RightSon)
                                delete.Parent.DeleteRightSon();
                            else if (delete == delete.Parent.LeftSon)
                                delete.Parent.DeleteLeftSon();
                        }
                    }
                    else
                    {
                        if (chanceNodeStack == null)
                        {
                            Node<T> chanceNode = null;
                            if (delete.RightSon == null)
                            {
                                foreach (Node<T> node in GetLevelOrderNodes(null, delete.LeftSon, false))
                                    if (chanceNode == null || chanceNode.Data.Compare(node.Data, delete.KeyIndex) > 0)
                                        chanceNode = node;
                            }
                            else
                                foreach (Node<T> node in GetLevelOrderNodes(null, delete.RightSon, false))
                                    if (chanceNode == null || chanceNode.Data.Compare(node.Data, delete.KeyIndex) < 0)
                                        chanceNode = node;

                            stack.Push((delete, chanceNode));
                            stack.Push((chanceNode, null));
                        }
                        else
                        {
                            if (delete.Parent != null)
                            {
                                if (delete == delete.Parent.RightSon)
                                {
                                    delete.Parent.DeleteRightSon();
                                    delete.Parent.RightSon = chanceNodeStack;
                                }
                                else if (delete == delete.Parent.LeftSon)
                                {
                                    delete.Parent.DeleteLeftSon();
                                    delete.Parent.LeftSon = chanceNodeStack;
                                }
                            }
                            else
                                Root = chanceNodeStack;

                            delete.TakeFromAllReferences(chanceNodeStack);

                            foreach (Node<T> node in GetLevelOrderNodes(null, chanceNodeStack.RightSon, false))
                            {
                                if (node != chanceNodeStack && chanceNodeStack.Data.Compare(node.Data, chanceNodeStack.KeyIndex) == 0)
                                {
                                    stack.Push((node, null));
                                    stackInsert.Push(node);
                                }
                            }
                        }
                    }
                }
                while (stackInsert.Count > 0)
                {
                    Node<T> getNode = stackInsert.Pop();
                    getNode.ClearNodeReferences();
                    Insert(getNode);
                }
            }

            return deletedItems;
        }
        public int Count() => GetLevelOrder(null).Count();

        public IEnumerable<T> GetLevelOrder(Action<int> OnNextLevel, Node<T> startNode = null, bool startWithRoot = true)
        {
            foreach (Node<T> element in GetLevelOrderNodes(OnNextLevel, startNode, startWithRoot))
                yield return element.Data;
        }

        //Only for debuging
        public IEnumerable<Node<T>> GetLevelOrderNodes(Action<int> OnNextLevel, Node<T> startNode = null, bool startWithRoot = true)
        {
            if (startWithRoot)
                startNode = Root;

            if (startNode != null)
            {
                int index = 1;
                Queue<Node<T>> q = new Queue<Node<T>>();
                q.Enqueue(startNode);

                OnNextLevel?.Invoke(index);
                while (q.Count > 0)
                {
                    Node<T> get = q.Dequeue();
                    if (index != get.Level)
                        OnNextLevel?.Invoke(++index);
                    if (get.LeftSon != null)
                        q.Enqueue(get.LeftSon);

                    if (get.RightSon != null)
                        q.Enqueue(get.RightSon);
                    yield return get;
                }
            }
        }
    }
}
