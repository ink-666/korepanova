using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

class Program
{
    // Для быстрой проверки можно временно уменьшить n (например, до 10_000_000)
    static int n = 250_000_000;

    static int[] a = Array.Empty<int>();
    static int[] b = Array.Empty<int>();
    static long[] result = Array.Empty<long>();

    static void Main()
    {
        a = new int[n];
        b = new int[n];
        result = new long[n];

        Console.WriteLine("Заполнение массивов...");
        Random rnd = new Random();
        for (int i = 0; i < n; i++)
        {
            a[i] = rnd.Next(1, 100);
            b[i] = rnd.Next(1, 100);
        }

        Stopwatch sw = new Stopwatch();

        // Без потоков
        sw.Start();
        MultiplyWithoutThreads();
        sw.Stop();
        Console.WriteLine("Время без потоков: " + sw.ElapsedMilliseconds + " мс");

        // С потоками (5 потоков)
        sw.Restart();
        MultiplyWithThreads(5);
        sw.Stop();
        Console.WriteLine("Время с потоками: " + sw.ElapsedMilliseconds + " мс");

        Console.WriteLine("Сохранение результата в файл...");
        SaveToFile("result.txt");
        Console.WriteLine("Готово.");
    }

    // Умножение без потоков
    static void MultiplyWithoutThreads()
    {
        for (int i = 0; i < n; i++)
            result[i] = (long)a[i] * b[i];
    }

    // Умножение с использованием нескольких потоков
    static void MultiplyWithThreads(int threadCount)
    {
        Thread[] threads = new Thread[threadCount];
        int chunk = n / threadCount;

        for (int t = 0; t < threadCount; t++)
        {
            int start = t * chunk;
            int end = (t == threadCount - 1) ? n : start + chunk;

            threads[t] = new Thread(() => MultiplyPart(start, end));
            threads[t].Start();
        }

        // ждем завершения всех потоков
        for (int t = 0; t < threadCount; t++)
            threads[t].Join();
    }

    // Умножение части массива (для одного потока)
    static void MultiplyPart(int start, int end)
    {
        for (int i = start; i < end; i++)
            result[i] = (long)a[i] * b[i];
    }

    // Сохранение результата в текстовый файл
    static void SaveToFile(string fileName)
    {
        using (StreamWriter sw = new StreamWriter(fileName))
        {
            for (int i = 0; i < n; i++)
                sw.WriteLine(result[i]);
        }
    }
}