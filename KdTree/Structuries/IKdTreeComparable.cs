namespace KdTree.Structuries
{
    internal interface IKdTreeComparable<T> 
    {
        public int Compare(T other, int level);
        public bool FullComapre(T other);

    }
}
