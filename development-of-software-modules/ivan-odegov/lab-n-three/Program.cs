using System;
using System.IO;
using ClosedXML.Excel;

class Program
{
    static void Main()
    {
        string path = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "test_file.xlsx"
        );

        if (!File.Exists(path))
        {
            Console.WriteLine("Файл не найден: " + path);
            return;
        }

        using (var workbook = new XLWorkbook(path))
        {
            foreach (var sheet in workbook.Worksheets)
            {
                Console.WriteLine($"\nЛист: {sheet.Name}");

                var range = sheet.RangeUsed();

                foreach (var row in range.Rows())
                {
                    foreach (var cell in row.Cells())
                    {
                        Console.Write(cell.Value + "\t");
                    }
                    Console.WriteLine();
                }
            }
        }

        Console.WriteLine("\nГотово!");
    }
}