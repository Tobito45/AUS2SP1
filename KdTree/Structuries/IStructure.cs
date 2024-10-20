using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace KdTree.Structuries
{
    internal interface IStructure<T>
    {
        public void Insert(T node);
        public IEnumerable<T> Delete(T date);
        public IEnumerable<T> Find(T data);
        public int Count();
    }
}
