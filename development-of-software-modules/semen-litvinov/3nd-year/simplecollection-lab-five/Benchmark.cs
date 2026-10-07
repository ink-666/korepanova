using System;
using System.Diagnostics;

namespace SimpleCollections
{
    public static class Benchmark
    {
        // Замер времени вставки n элементов, миллисекунды
        public static double MeasureInsert(int n, Action<int> insert)
        {
            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < n; i++)
                insert(i);
            sw.Stop();
            return sw.Elapsed.TotalMilliseconds;
        }
    }
}
