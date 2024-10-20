using KdTree.Structuries;

namespace KdTree.Generator
{
    internal abstract class OperationGenerator<S, T>
        where S : IStructure<T>
        where T : IKdTreeComparable<T>
    {
        protected Random random;
        protected List<T> generatedElements = new List<T>();
        public S Structure { get; private set; }
        public OperationGenerator(S structure, int? seed)
        {
            Structure = structure;
            if(random == null)
                random = new Random();
            else
                random = new Random(seed!.Value);
        }

        public virtual OperationGenerator<S, T> GenerateInsert(int count)
        {
            for (int i = 0; i < count; i++)
            {
                T data = GenerateRadomData();
                generatedElements.Add(data);
                Structure.Insert(data);
            }

            return this;
        }

        public virtual OperationGenerator<S, T> GenerateDelete(int count, Action<IEnumerable<T>, int, int, T> action)
        {
            if (generatedElements.Count == 0)
                return this;

            for (int i = 0; i < count; i++)
            {
                int countBefore = Structure.Count();
                T data = generatedElements[random.Next(0, generatedElements.Count)];
                generatedElements.Remove(data);
                IEnumerable<T> deleted = Structure.Delete(data);
                int countAfter = Structure.Count();
                action(deleted, countBefore, countAfter, data);
            }

            return this;
        }

        public virtual OperationGenerator<S, T> GenerateFind(int count, Action<IEnumerable<T>> action)
        {
            if (generatedElements.Count == 0)
                return this;

            for (int i = 0; i < count; i++)
            {
                T data = generatedElements[random.Next(0, generatedElements.Count)];
                action(Structure.Find(data));
            }
            
            return this;
        }

        public virtual OperationGenerator<S, T> GenerateRandomOperations(int count, Action<IEnumerable<T>> findAction,
                Action<IEnumerable<T>, int, int, T> deleteAction)
        {
            Random random = new Random();
            for (int i = 0; i < count; i++)
            {
                int rnd = random.Next(3);
                switch(rnd)
                {
                    case 1: GenerateInsert(1); break;
                    case 2: GenerateDelete(1, deleteAction); break;
                    case 3: GenerateFind(1, findAction); break;
                }
            }
            return this;
        }

        public abstract T GenerateRadomData();
    }
}
