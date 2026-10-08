using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace ThreadAverage
{
    /// <summary>Результат обработки одного файла.</summary>
    public class FileResult
    {
        public string FileName { get; init; } = "";
        public long Count { get; init; }
        public long Skipped { get; init; }
        public double Average { get; init; }
        public long ElapsedMs { get; init; }
        public int ThreadId { get; init; }
        public string? Error { get; init; }
    }

    /// <summary>Потокобезопасная запись результатов в общий выходной файл
    /// (общий ресурс -> критическая секция через lock).</summary>
    public class ResultWriter
    {
        private readonly object _lock = new object();
        private readonly string _path;

        public ResultWriter(string path)
        {
            _path = path;
            File.WriteAllText(_path, "Результаты расчёта средних значений" + Environment.NewLine,
                Encoding.UTF8);
        }

        public void Write(FileResult r)
        {
            string line = r.Error == null
                ? string.Format(CultureInfo.InvariantCulture,
                    "{0}: чисел = {1}, пропущено строк = {2}, среднее = {3:F6}",
                    r.FileName, r.Count, r.Skipped, r.Average)
                : $"{r.FileName}: ОШИБКА - {r.Error}";

            lock (_lock) // критическая секция
            {
                File.AppendAllText(_path, line + Environment.NewLine, Encoding.UTF8);
            }
        }
    }

    /// <summary>Рабочий объект: читает числа из одного файла и считает среднее.</summary>
    public class AverageWorker
    {
        private readonly string _file;
        private readonly ResultWriter _writer;
        public FileResult? Result { get; private set; }

        public AverageWorker(string file, ResultWriter writer)
        {
            _file = file;
            _writer = writer;
        }

        public void Run()
        {
            var sw = Stopwatch.StartNew();
            try
            {
                double sum = 0;
                long count = 0, skipped = 0;

                using (var reader = new StreamReader(_file))
                {
                    string? line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (double.TryParse(line.Trim(), NumberStyles.Float,
                                CultureInfo.InvariantCulture, out double value))
                        {
                            sum += value;
                            count++;
                        }
                        else if (line.Trim().Length > 0)
                        {
                            skipped++;
                        }
                    }
                }

                if (count == 0) throw new InvalidDataException("в файле нет числовых данных");

                Result = new FileResult
                {
                    FileName = Path.GetFileName(_file),
                    Count = count,
                    Skipped = skipped,
                    Average = sum / count,
                    ElapsedMs = sw.ElapsedMilliseconds,
                    ThreadId = Environment.CurrentManagedThreadId
                };
            }
            catch (Exception ex) // исключения потока не должны «убивать» процесс
            {
                Result = new FileResult
                {
                    FileName = Path.GetFileName(_file),
                    Error = ex.Message,
                    ElapsedMs = sw.ElapsedMilliseconds,
                    ThreadId = Environment.CurrentManagedThreadId
                };
            }
            _writer.Write(Result);
        }
    }

    public static class Program
    {
        private const int FilesCount = 5;
        private const int NumbersPerFile = 2_000_000;

        private static void GenerateData(string dir)
        {
            Directory.CreateDirectory(dir);
            var rnd = new Random(42);
            for (int i = 1; i <= FilesCount; i++)
            {
                string path = Path.Combine(dir, $"data{i}.txt");
                // у каждого файла свой диапазон, чтобы средние отличались
                double min = i * 10, max = i * 10 + 100;
                using var w = new StreamWriter(path);
                for (int j = 0; j < NumbersPerFile; j++)
                    w.WriteLine((min + rnd.NextDouble() * (max - min)).ToString("F4",
                        CultureInfo.InvariantCulture));
            }
        }

        public static void Main()
        {
            string dir = "data";
            string outFile = "results.txt";

            Console.WriteLine("Генерация тестовых файлов...");
            GenerateData(dir);
            // файл с некорректными строками и несуществующий файл - проверка обработки ошибок
            File.WriteAllLines(Path.Combine(dir, "data6.txt"),
                new[] { "10", "20", "abc", "30", "" , "x1"});
            var files = Directory.GetFiles(dir, "data*.txt").OrderBy(f => f).ToList();
            files.Add(Path.Combine(dir, "missing.txt"));

            // ---------- многопоточный расчёт ----------
            var writer = new ResultWriter(outFile);
            var workers = files.Select(f => new AverageWorker(f, writer)).ToList();
            var threads = workers.Select(w => new Thread(w.Run)).ToList();

            var sw = Stopwatch.StartNew();
            threads.ForEach(t => t.Start());
            threads.ForEach(t => t.Join());
            long parallelMs = sw.ElapsedMilliseconds;

            // ---------- последовательный расчёт для сравнения ----------
            var seqWriter = new ResultWriter("results_sequential.txt");
            sw.Restart();
            foreach (var f in files) new AverageWorker(f, seqWriter).Run();
            long seqMs = sw.ElapsedMilliseconds;

            Console.WriteLine();
            Console.WriteLine("{0,-14}{1,10}{2,10}{3,14}{4,10}{5,8}",
                "Файл", "Чисел", "Пропущ.", "Среднее", "Время,мс", "Поток");
            foreach (var r in workers.Select(w => w.Result!))
            {
                if (r.Error == null)
                    Console.WriteLine("{0,-14}{1,10}{2,10}{3,14:F6}{4,10}{5,8}",
                        r.FileName, r.Count, r.Skipped, r.Average, r.ElapsedMs, r.ThreadId);
                else
                    Console.WriteLine("{0,-14}ОШИБКА: {1}", r.FileName, r.Error);
            }
            Console.WriteLine();
            Console.WriteLine($"Логических процессоров: {Environment.ProcessorCount}");
            Console.WriteLine($"Многопоточно:     {parallelMs} мс");
            Console.WriteLine($"Последовательно:  {seqMs} мс");
            Console.WriteLine("Результаты записаны в файл " + Path.GetFullPath(outFile));
        }
    }
}
