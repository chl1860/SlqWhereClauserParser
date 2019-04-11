using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreDParser
{
    public static class ClsExtension
    {
        public static T Reduce<T>(this List<T> list, Func<T, T, T> func, dynamic acc = null)
        {
            var startIndex = 0;
            if (acc == null)
            {
                startIndex = 1;
                acc = list[0];
            }
            for (; startIndex < list.Count(); startIndex++)
            {
                acc = func(acc, list[startIndex]);
            }
            return acc;
        }
    }

}
