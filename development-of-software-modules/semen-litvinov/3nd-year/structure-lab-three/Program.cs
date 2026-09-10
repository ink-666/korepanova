using System;
using System.Collections.Generic;
using System.IO;

struct Flight
{
    public string Number;      // номер рейса
    public string Destination; // пункт назначения
    public string Time;        // время вылета
    public string Date;        // дата вылета
    public double Price;       // стоимость билета
}

class Program
{
    static List<Flight> flights = new List<Flight>();
    static string fileName = "flights.csv";

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Load();

        int choice = -1;
        while (choice != 0)
        {
            Console.WriteLine();
            Console.WriteLine("1 - Ввод рейсов");
            Console.WriteLine("2 - Сортировка по номеру рейса");
            Console.WriteLine("3 - Поиск по номеру рейса");
            Console.WriteLine("4 - Изменение рейса");
            Console.WriteLine("5 - Удаление рейса");
            Console.WriteLine("6 - Показать все рейсы");
            Console.WriteLine("0 - Выход");
            Console.Write("Выбор: ");
            choice = ReadInt();

            if (choice == 1) InputFlights();
            else if (choice == 2) SortFlights();
            else if (choice == 3) SearchFlight();
            else if (choice == 4) EditFlight();
            else if (choice == 5) DeleteFlight();
            else if (choice == 6) ShowAll();
            else if (choice != 0) Console.WriteLine("Нет такого пункта меню.");
        }

        Save();
    }

    // Ввод
    static void InputFlights()
    {
        Console.Write("Сколько рейсов ввести? ");
        int n = ReadInt();

        for (int i = 0; i < n; i++)
        {
            Flight f = new Flight();

            Console.Write("Номер рейса: ");
            f.Number = ReadString();

            Console.Write("Пункт назначения: ");
            f.Destination = ReadString();

            Console.Write("Время вылета: ");
            f.Time = ReadString();

            Console.Write("Дата вылета: ");
            f.Date = ReadString();

            Console.Write("Цена билета: ");
            f.Price = ReadDouble();

            flights.Add(f);
        }
    }

    // Сортировка пузырьком по номеру рейса
    static void SortFlights()
    {
        for (int i = 0; i < flights.Count - 1; i++)
        {
            for (int j = 0; j < flights.Count - 1 - i; j++)
            {
                if (string.Compare(flights[j].Number, flights[j + 1].Number) > 0)
                {
                    Flight temp = flights[j];
                    flights[j] = flights[j + 1];
                    flights[j + 1] = temp;
                }
            }
        }
        Console.WriteLine("Готово.");
    }

    // Поиск
    static void SearchFlight()
    {
        Console.Write("Введите номер рейса: ");
        string num = Console.ReadLine();

        bool found = false;
        for (int i = 0; i < flights.Count; i++)
        {
            if (flights[i].Number == num)
            {
                PrintFlight(flights[i]);
                found = true;
            }
        }
        if (!found) Console.WriteLine("Не найдено.");
    }

    // Изменение
    static void EditFlight()
    {
        Console.Write("Введите номер рейса для изменения: ");
        string num = Console.ReadLine();

        int index = -1;
        for (int i = 0; i < flights.Count; i++)
        {
            if (flights[i].Number == num) { index = i; break; }
        }

        if (index == -1)
        {
            Console.WriteLine("Не найдено.");
            return;
        }

        Flight f = flights[index];

        Console.Write("Новый пункт назначения: ");
        f.Destination = ReadString();

        Console.Write("Новое время вылета: ");
        f.Time = ReadString();

        Console.Write("Новая дата вылета: ");
        f.Date = ReadString();

        Console.Write("Новая цена: ");
        f.Price = ReadDouble();

        flights[index] = f;
        Console.WriteLine("Изменено.");
    }

    // Удаление
    static void DeleteFlight()
    {
        Console.Write("Введите номер рейса для удаления: ");
        string num = Console.ReadLine();

        int index = -1;
        for (int i = 0; i < flights.Count; i++)
        {
            if (flights[i].Number == num) { index = i; break; }
        }

        if (index == -1)
            Console.WriteLine("Не найдено.");
        else
        {
            flights.RemoveAt(index);
            Console.WriteLine("Удалено.");
        }
    }

    // Вывод
    static void ShowAll()
    {
        if (flights.Count == 0)
        {
            Console.WriteLine("Список пуст.");
            return;
        }
        for (int i = 0; i < flights.Count; i++)
            PrintFlight(flights[i]);
    }

    static void PrintFlight(Flight f)
    {
        Console.WriteLine(f.Number + " | " + f.Destination + " | " + f.Time + " | " + f.Date + " | " + f.Price);
    }

    // Сохранение в файл
    static void Save()
    {
        StreamWriter sw = new StreamWriter(fileName, false, System.Text.Encoding.UTF8);
        for (int i = 0; i < flights.Count; i++)
        {
            Flight f = flights[i];
            sw.WriteLine(f.Number + ";" + f.Destination + ";" + f.Time + ";" + f.Date + ";" + f.Price);
        }
        sw.Close();
    }

    // Загрузка из файла
    static void Load()
    {
        if (!File.Exists(fileName)) return;

        string[] lines = File.ReadAllLines(fileName, System.Text.Encoding.UTF8);
        foreach (string line in lines)
        {
            if (line == "") continue;
            string[] p = line.Split(';');
            Flight f = new Flight();
            f.Number = p[0];
            f.Destination = p[1];
            f.Time = p[2];
            f.Date = p[3];
            f.Price = double.Parse(p[4]);
            flights.Add(f);
        }
    }

    // Ввод целого числа с проверкой
    static int ReadInt()
    {
        int result;
        while (!int.TryParse(Console.ReadLine(), out result))
        {
            Console.Write("Введите целое число: ");
        }
        return result;
    }

    // Ввод дробного числа с проверкой
    static double ReadDouble()
    {
        double result;
        while (!double.TryParse(Console.ReadLine(), out result) || result < 0)
        {
            Console.Write("Введите корректное положительное число: ");
        }
        return result;
    }

    // Ввод непустой строки с проверкой
    static string ReadString()
    {
        string s = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(s))
        {
            Console.Write("Поле не может быть пустым, повторите: ");
            s = Console.ReadLine();
        }
        return s;
    }
}