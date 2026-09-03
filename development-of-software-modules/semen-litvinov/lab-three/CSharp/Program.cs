using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ExcelDataReader;

namespace ExcelReader
{
    class Program
    {
        static void Main(string[] args)
        {
            // Регистрируем кодировку для работы с русским текстом
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            Console.WriteLine("==========================================");
            Console.WriteLine("ЛАБОРАТОРНАЯ РАБОТА №3: ЧТЕНИЕ ДАННЫХ ИЗ EXCEL");
            Console.WriteLine("Язык: C#");
            Console.WriteLine("Студент: ink-666");
            Console.WriteLine("Дата: 17.03.2026");
            Console.WriteLine("==========================================\n");

            // Путь к файлу Excel (поднимаемся на уровень выше)
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "data.xlsx");
            
            // Проверяем существование файла
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Файл не найден: {filePath}");
                Console.WriteLine("Пожалуйста, убедитесь что файл data.xlsx находится в папке LR3_Excel");
                return;
            }

            Console.WriteLine($"Чтение файла: {filePath}\n");

            // Открываем файл и читаем Excel
            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                // Создаём конфигурацию для чтения
                var readerConfig = new ExcelReaderConfiguration
                {
                    // Автоматически определять типы данных
                    FallbackEncoding = Encoding.GetEncoding(1251)
                };

                // Создаём reader для Excel
                using (var reader = ExcelReaderFactory.CreateReader(stream, readerConfig))
                {
                    // ==========================================================
                    // 1. ЧТЕНИЕ ВСЕХ ЛИСТОВ ПОСЛЕДОВАТЕЛЬНО
                    // ==========================================================
                    Console.WriteLine("==========================================");
                    Console.WriteLine("1. ЧТЕНИЕ ВСЕХ ЛИСТОВ");
                    Console.WriteLine("==========================================\n");

                    int sheetIndex = 0;
                    do
                    {
                        Console.WriteLine($"\n--- Лист {sheetIndex + 1}: {reader.Name} ---");
                        
                        // Читаем заголовки (первая строка)
                        reader.Read();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            Console.Write($"{reader.GetValue(i),-15}");
                        }
                        Console.WriteLine();
                        Console.WriteLine(new string('-', 70));

                        // Читаем данные (следующие строки)
                        int rowCount = 0;
                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                Console.Write($"{reader.GetValue(i),-15}");
                            }
                            Console.WriteLine();
                            rowCount++;
                            
                            // Показываем только первые 5 строк для краткости
                            if (rowCount >= 5) break;
                        }
                        
                        Console.WriteLine($"\nВсего строк: {rowCount} (показано первых 5)");
                        
                        sheetIndex++;
                    } while (reader.NextResult());

                    // ==========================================================
                    // 2. ЧТЕНИЕ С ИСПОЛЬЗОВАНИЕМ DATASET
                    // ==========================================================
                    Console.WriteLine("\n\n==========================================");
                    Console.WriteLine("2. ЧТЕНИЕ С ИСПОЛЬЗОВАНИЕМ DataSet");
                    Console.WriteLine("==========================================\n");

                    // Создаём новый reader для Dataset
                    stream.Seek(0, SeekOrigin.Begin);
                    using (var datasetReader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var result = datasetReader.AsDataSet(new ExcelDataSetConfiguration
                        {
                            ConfigureDataTable = _ => new ExcelDataTableConfiguration
                            {
                                UseHeaderRow = true // Использовать первую строку как заголовки
                            }
                        });

                        // Выводим информацию о всех таблицах
                        Console.WriteLine($"Количество листов: {result.Tables.Count}\n");

                        foreach (System.Data.DataTable table in result.Tables)
                        {
                            Console.WriteLine($"\n>>> Лист: {table.TableName}");
                            Console.WriteLine($"Колонки: {table.Columns.Count}, Строки: {table.Rows.Count}");
                            
                            // Выводим названия колонок
                            for (int i = 0; i < table.Columns.Count; i++)
                            {
                                Console.Write($"{table.Columns[i].ColumnName,-15}");
                            }
                            Console.WriteLine();
                            Console.WriteLine(new string('-', 70));

                            // Выводим первые 3 строки
                            for (int row = 0; row < Math.Min(3, table.Rows.Count); row++)
                            {
                                for (int col = 0; col < table.Columns.Count; col++)
                                {
                                    Console.Write($"{table.Rows[row][col],-15}");
                                }
                                Console.WriteLine();
                            }
                        }
                    }

                    // ==========================================================
                    // 3. ВЫБОРОЧНОЕ ЧТЕНИЕ (эмуляция параметров как в Python)
                    // ==========================================================
                    Console.WriteLine("\n\n==========================================");
                    Console.WriteLine("3. ВЫБОРОЧНОЕ ЧТЕНИЕ (как usecols и nrows в Python)");
                    Console.WriteLine("==========================================\n");

                    // Снова создаём reader
                    stream.Seek(0, SeekOrigin.Begin);
                    using (var selectReader = ExcelReaderFactory.CreateReader(stream))
                    {
                        // Читаем второй лист (Предметы)
                        int targetSheet = 1; // 0 - первый, 1 - второй, 2 - третий
                        for (int i = 0; i < targetSheet; i++)
                        {
                            selectReader.NextResult();
                        }

                        Console.WriteLine($"\n>>> Лист: {selectReader.Name} (только первые 2 столбца и 3 строки)");

                        // Читаем заголовки (только первые 2 столбца)
                        selectReader.Read();
                        Console.WriteLine("\nЗаголовки:");
                        for (int i = 0; i < Math.Min(2, selectReader.FieldCount); i++)
                        {
                            Console.Write($"{selectReader.GetValue(i),-15}");
                        }
                        Console.WriteLine();
                        Console.WriteLine(new string('-', 30));

                        // Читаем первые 3 строки (только первые 2 столбца)
                        int rowsRead = 0;
                        while (selectReader.Read() && rowsRead < 3)
                        {
                            for (int i = 0; i < Math.Min(2, selectReader.FieldCount); i++)
                            {
                                Console.Write($"{selectReader.GetValue(i),-15}");
                            }
                            Console.WriteLine();
                            rowsRead++;
                        }
                    }

                    // ==========================================================
                    // 4. СВОИ ТИПЫ ДАННЫХ (эмуляция dtype)
                    // ==========================================================
                    Console.WriteLine("\n\n==========================================");
                    Console.WriteLine("4. ЯВНОЕ ПРЕОБРАЗОВАНИЕ ТИПОВ (как dtype в Python)");
                    Console.WriteLine("==========================================\n");

                    stream.Seek(0, SeekOrigin.Begin);
                    using (var typeReader = ExcelReaderFactory.CreateReader(stream))
                    {
                        // Читаем первый лист (Студенты)
                        typeReader.Read(); // пропускаем заголовки
                        
                        Console.WriteLine("Данные с явным приведением типов:");
                        Console.WriteLine("ID (string) | Имя (string) | Возраст (int) | Балл (double)");
                        Console.WriteLine(new string('-', 60));

                        int count = 0;
                        while (typeReader.Read() && count < 3)
                        {
                            // Явное преобразование типов
                            string id = typeReader.GetValue(0)?.ToString() ?? "";
                            string name = typeReader.GetValue(1)?.ToString() ?? "";
                            int age = Convert.ToInt32(typeReader.GetValue(2) ?? 0);
                            double gpa = Convert.ToDouble(typeReader.GetValue(3) ?? 0.0);

                            Console.WriteLine($"{id,-12} | {name,-10} | {age,-7} | {gpa:F2}");
                            count++;
                        }
                    }
                }
            }

            Console.WriteLine("\n" + "=" * 60);
            Console.WriteLine("ПРОГРАММА C# ЗАВЕРШЕНА");
            Console.WriteLine("=" + "=" * 60);
        }
    }
}