using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace LinkedListsLab
{
    internal class Program
    {
        private const int ElementsCount = 1000000; // размер каждого списка
        private const int MaxValue = 1000;         // числа от 0 до 1000
        private const int FastRepeats = 100000;    // повторов для быстрых операций (O(1))
        private const int SlowRepeats = 200;       // повторов для вставки в конец односвязного списка (O(n))

        // Результат измерений: среднее время трёх операций, нс
        private class Result
        {
            public double AddFirst;
            public double RemoveFirst;
            public double AddLast;
        }

        private static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Random random = new Random(12345);

            Console.WriteLine("Практическая работа №6. Односвязный и двусвязный списки");
            Console.WriteLine();

            // 1. Создаём два контейнера
            SinglyLinkedList<int> singly = new SinglyLinkedList<int>();
            DoublyLinkedList<int> doubly = new DoublyLinkedList<int>();

            // 2. Заполняем случайными числами (0..1000)
            // Заполнение через AddFirst: для односвязного списка AddLast на 1 000 000
            // элементов потребовал бы около 5*10^11 переходов по узлам.
            Fill(singly, random);
            Fill(doubly, random);
            Console.WriteLine("Односвязный список заполнен, элементов: " + singly.Count);
            Console.WriteLine("Двусвязный список заполнен, элементов:  " + doubly.Count);
            Console.WriteLine("Повторов операции: " + FastRepeats + " (вставка в конец односвязного списка: " + SlowRepeats + ")");
            Console.WriteLine();

            // Прогрев JIT-компилятора, чтобы он не попал в замеры
            Measure(new SinglyLinkedList<int>(), 100, 100);
            Measure(new DoublyLinkedList<int>(), 100, 100);

            // 3. Измеряем время операций
            Console.WriteLine("Измерение односвязного списка...");
            Result rSingly = Measure(singly, FastRepeats, SlowRepeats);
            Console.WriteLine("Измерение двусвязного списка...");
            Result rDoubly = Measure(doubly, FastRepeats, FastRepeats);
            Console.WriteLine();

            PrintTable(rSingly, rDoubly);
            SaveCsv(rSingly, rDoubly, "results.csv");
            Console.WriteLine();
            Console.WriteLine("Результаты сохранены в файл results.csv");
            Console.WriteLine("Нажмите любую клавишу для выхода...");
            if (!Console.IsInputRedirected) Console.ReadKey();
        }

        private static void Fill(ILinkedContainer<int> list, Random random)
        {
            for (int i = 0; i < ElementsCount; i++)
                list.AddFirst(random.Next(0, MaxValue + 1));
        }

        // Время каждой операции измеряется отдельно (Stopwatch), затем берётся среднее.
        // Операции выполняются сериями: сначала все вставки в начало, затем все
        // удаления первого элемента, затем все вставки в конец. Так долгий проход
        // по списку при вставке в конец не портит кэш для соседних замеров.
        private static Result Measure(ILinkedContainer<int> list, int repeats, int repeatsLast)
        {
            Random random = new Random(777);
            double sumFirst = 0, sumRemove = 0, sumLast = 0;

            // Чистим память и запрещаем сборку мусора на время замера,
            // чтобы редкие паузы GC не искажали среднее значение.
            GC.Collect();
            GC.WaitForPendingFinalizers();
            bool noGc = GC.TryStartNoGCRegion(20000000);

            for (int i = 0; i < repeats; i++)
            {
                int value = random.Next(0, MaxValue + 1);
                long t = Stopwatch.GetTimestamp();
                list.AddFirst(value);
                sumFirst += ToNs(Stopwatch.GetTimestamp() - t);
            }

            for (int i = 0; i < repeats; i++)
            {
                long t = Stopwatch.GetTimestamp();
                list.RemoveFirst();
                sumRemove += ToNs(Stopwatch.GetTimestamp() - t);
            }

            for (int i = 0; i < repeatsLast; i++)
            {
                int value = random.Next(0, MaxValue + 1);
                long t = Stopwatch.GetTimestamp();
                list.AddLast(value);
                sumLast += ToNs(Stopwatch.GetTimestamp() - t);
            }

            if (noGc && System.Runtime.GCSettings.LatencyMode == System.Runtime.GCLatencyMode.NoGCRegion)
                GC.EndNoGCRegion();

            Result result = new Result();
            result.AddFirst = sumFirst / repeats;
            result.RemoveFirst = sumRemove / repeats;
            result.AddLast = sumLast / repeatsLast;
            return result;
        }

        private static double ToNs(long ticks)
        {
            return ticks * 1e9 / Stopwatch.Frequency;
        }

        private static void PrintTable(Result a, Result b)
        {
            Console.WriteLine("Среднее время выполнения операции, нс");
            Console.WriteLine("+---------------------------+----------------+----------------+");
            Console.WriteLine("| Операция                  | Односвязный    | Двусвязный     |");
            Console.WriteLine("+---------------------------+----------------+----------------+");
            PrintRow("Вставка в начало", a.AddFirst, b.AddFirst);
            PrintRow("Удаление первого элемента", a.RemoveFirst, b.RemoveFirst);
            PrintRow("Вставка в конец", a.AddLast, b.AddLast);
            Console.WriteLine("+---------------------------+----------------+----------------+");
        }

        private static void PrintRow(string name, double a, double b)
        {
            Console.WriteLine("| {0,-25} | {1,14:F1} | {2,14:F1} |", name, a, b);
        }

        private static void SaveCsv(Result a, Result b, string path)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("operation;singly_ns;doubly_ns");
            sb.AppendLine(string.Format(System.Globalization.CultureInfo.InvariantCulture, "AddFirst;{0:F1};{1:F1}", a.AddFirst, b.AddFirst));
            sb.AppendLine(string.Format(System.Globalization.CultureInfo.InvariantCulture, "RemoveFirst;{0:F1};{1:F1}", a.RemoveFirst, b.RemoveFirst));
            sb.AppendLine(string.Format(System.Globalization.CultureInfo.InvariantCulture, "AddLast;{0:F1};{1:F1}", a.AddLast, b.AddLast));
            File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
        }
    }
}