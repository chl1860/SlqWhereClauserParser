using System.Collections.Generic;
using System.Linq;

namespace DParser
{
    public class SimpleStack<T>
    {
        List<T> _array = null;
        int len = 0;
        public SimpleStack()
        {
            _array = new List<T>();
            len = 0;
        }

        public void Push(T item)
        {
            _array.Add(item);
            len++;
        }

        public T Pop()
        {
            if(len > 0)
            {
                len--;
            }
            return _array.Last();
        }

        /// <summary>
        /// 清空栈并返回被清空的元素
        /// </summary>
        public List<T> Clear()
        {
            var result = new List<T>();
            while(len > 0)
            {
                result.Add(this.Pop());
            }
            return result;
        }

        public int Len
        {
            get
            {
                return len;
            }
        }
    }
}
