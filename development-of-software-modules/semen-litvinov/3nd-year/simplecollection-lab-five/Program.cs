using System;
using System.Text;

namespace SimpleCollections
{
    internal static class Program
    {
        private static void PrintList<T>(string name, List<T> list)
        {
            Console.Write(name + " (size = " + list.Size() + "): ");
            foreach (T x in list)
                Console.Write(x + " ");
            Console.WriteLine();
        }

        private static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("=== List<T> ===");
            List<int> a = new List<int>();
            for (int i = 1; i <= 5; i++) a.Add(i * 10);
            PrintList("a", a);

            List<int> b = new List<int>(a);               // копирующий конструктор
            b.Add(99);
            PrintList("b (копия a + 99)", b);
            PrintList("a (не изменился)", a);

            List<int> c = new List<int>(b, true);         // перемещающий конструктор
            PrintList("c (перемещён из b)", c);
            Console.WriteLine("b.Size() после перемещения = " + b.Size());

            List<int> d = new List<int>();
            d.Assign(a);                                  // копирующее присваивание
            d.Add(7);
            PrintList("d (= a + 7)", d);

            List<int> e = new List<int>();
            e.MoveFrom(d);                                // перемещающее присваивание
            PrintList("e (перемещён из d)", e);

            List<string> words = new List<string>();
            words.Add("шаблон");
            words.Add("класс");
            words.Add("T");
            PrintList("words", words);

            Console.WriteLine();
            Console.WriteLine("=== Stack<T> ===");
            Stack<int> s = new Stack<int>();
            for (int i = 1; i <= 5; i++) s.Push(i);
            Console.WriteLine("Top() = " + s.Top() + ", Size() = " + s.Size());

            Stack<int> s2 = new Stack<int>(s);            // копирование
            s.Pop();
            Console.WriteLine("после Pop(): Top() = " + s.Top() + ", копия s2.Top() = " + s2.Top());

            s2.Top() = 50;                                // Top() возвращает ссылку
            Console.WriteLine("после s2.Top() = 50: s2.Top() = " + s2.Top());

            Stack<int> s3 = new Stack<int>(s2, true);     // перемещение
            Console.WriteLine("s3.Top() = " + s3.Top() + ", s2.Empty() = " + s2.Empty());

            Console.Write("Извлекаем из s3: ");
            while (!s3.Empty())
            {
                Console.Write(s3.Top() + " ");
                s3.Pop();
            }
            Console.WriteLine();
            Console.WriteLine("s3.Empty() = " + s3.Empty());

            try
            {
                s3.Pop();
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("Исключение: " + ex.Message);
            }

            Console.WriteLine();
            Console.WriteLine("=== Сравнение скорости вставки (мс) ===");
            Console.WriteLine("{0,-12}{1,-22}{2,-16}{3,-16}", "N", "System List<int>", "Stack<int>", "List<int>");

            // прогрев JIT-компилятора
            Run(1000);

            foreach (int n in new int[] { 100000, 1000000, 5000000 })
                Run(n);
        }

        private static void Run(int n)
        {
            System.Collections.Generic.List<int> lib = new System.Collections.Generic.List<int>();
            Stack<int> stack = new Stack<int>();
            List<int> mine = new List<int>();

            double tLib = Benchmark.MeasureInsert(n, i => lib.Add(i));
            double tStack = Benchmark.MeasureInsert(n, i => stack.Push(i));
            double tMine = Benchmark.MeasureInsert(n, i => mine.Add(i));

            if (n >= 100000)
                Console.WriteLine("{0,-12}{1,-22:F2}{2,-16:F2}{3,-16:F2}", n, tLib, tStack, tMine);
        }
    }
}
