using System;
using System.IO;

class Televizor
{
    public string Proizvoditel;
    public string Model;
    public int God;
    public double Cena;

    public Televizor(string proizvoditel, string model, int god, double cena)
    {
        Proizvoditel = proizvoditel;
        Model = model;
        God = god;
        Cena = cena;
    }

    public string ToFileString()
    {
        return $"Производитель: {Proizvoditel}, Модель: {Model}, Год выпуска: {God}, Цена: {Cena}";
    }
}

class Program
{
    static void Main()
    {
        string fileName = "televizory.txt";
        string answer;

        do
        {
            Console.Write("Введите производителя: ");
            string proizvoditel = Console.ReadLine();

            Console.Write("Введите модель: ");
            string model = Console.ReadLine();

            Console.Write("Введите год выпуска: ");
            int god = int.Parse(Console.ReadLine());

            Console.Write("Введите цену: ");
            double cena = double.Parse(Console.ReadLine());

            Televizor tv = new Televizor(proizvoditel, model, god, cena);

            using (StreamWriter sw = new StreamWriter(fileName, true))
            {
                sw.WriteLine(tv.ToFileString());
            }

            Console.WriteLine("Данные сохранены в файл!");

            Console.Write("Добавить еще один телевизор? (да/нет): ");
            answer = Console.ReadLine();

        } while (answer.ToLower() == "да");

        Console.WriteLine("\nСодержимое файла:");
        Console.WriteLine(File.ReadAllText(fileName));
    }
}