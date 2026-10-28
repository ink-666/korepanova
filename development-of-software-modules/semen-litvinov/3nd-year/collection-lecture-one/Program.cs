using System;
using System.Collections.Generic;
using System.Linq;

// Класс "Товар" для задания 13: у товара есть Id, название и цена
class Product
{
    public string Id;
    public string Name;
    public decimal Price;
}

class Program
{
    // ================= Задание 9. Менеджер контактов =================

    // Словарь: ключ - номер телефона, значение - имя
    static Dictionary<string, string> contacts = new Dictionary<string, string>();

    // Добавить контакт
    static void AddContact(string phone, string name)
    {
        // Если такой номер уже есть, второй раз не добавляем (иначе будет ошибка)
        if (contacts.ContainsKey(phone))
            Console.WriteLine("Номер уже есть: " + phone);
        else
            contacts.Add(phone, name);
    }

    // Удалить контакт по номеру (если номера нет, Remove ничего не делает)
    static void RemoveContact(string phone)
    {
        contacts.Remove(phone);
    }

    // Найти имя по номеру
    static string GetNameByPhone(string phone)
    {
        // Сначала проверяем, есть ли номер, чтобы не было исключения
        if (contacts.ContainsKey(phone))
            return contacts[phone];

        // Если номера нет, возвращаем null ("пусто")
        return null;
    }

    // Вернуть все контакты списком строк
    static List<string> ListAll()
    {
        // Select превращает каждую пару (номер, имя) в строку "номер - имя"
        // ToList собирает результат в список
        return contacts.Select(c => c.Key + " - " + c.Value).ToList();
    }

    // ================= Задание 10. Очередь задач =================

    // Очередь: первым пришёл, первым вышел
    static Queue<string> tasks = new Queue<string>();

    // Обработать первые n задач
    static void ProcessTasks(int n)
    {
        // Если просят больше, чем есть в очереди, берём только то, что есть
        if (n > tasks.Count)
        {
            Console.WriteLine("В очереди осталось задач: " + tasks.Count);
            n = tasks.Count;
        }

        for (int i = 0; i < n; i++)
        {
            // Dequeue достаёт первую задачу и сразу удаляет её из очереди
            Console.WriteLine("Выполнено: " + tasks.Dequeue());
        }
    }

    // Оставить только задачи со словом "urgent"
    static List<string> FilterUrgent(List<string> list)
    {
        // Where оставляет только те строки, где есть слово "urgent"
        return list.Where(t => t.Contains("urgent")).ToList();
    }

    // ================= Задание 11. Статистика слов =================

    // Посчитать, сколько раз встречается каждое слово
    static Dictionary<string, int> CountWords(string text)
    {
        // Символы, по которым режем текст на слова: пробел и знаки препинания
        char[] separators = { ' ', '.', ',', '!', '?', ';', ':', '-' };

        // ToLower - всё в нижний регистр, чтобы "Мама" и "мама" считались одним словом
        // Split - разбить на слова; RemoveEmptyEntries - выбросить пустые части
        string[] words = text.ToLower().Split(separators, StringSplitOptions.RemoveEmptyEntries);

        // Словарь: ключ - слово, значение - сколько раз оно встретилось
        Dictionary<string, int> freq = new Dictionary<string, int>();
        foreach (string w in words)
        {
            if (freq.ContainsKey(w))
                freq[w]++;      // слово уже было: прибавляем 1
            else
                freq[w] = 1;    // слово новое: ставим 1
        }
        return freq;
    }

    // ================= Задание 12. История действий (отмена) =================

    // Стек: последним пришёл, первым вышел
    static Stack<string> history = new Stack<string>();

    // Запомнить действие
    static void Do(string action)
    {
        history.Push(action);   // положить наверх стека
    }

    // Отменить последнее действие
    static string Undo()
    {
        // Если стек пуст, отменять нечего
        if (history.Count == 0)
            return null;

        // Pop достаёт верхнее действие и удаляет его из стека
        return history.Pop();
    }

    // Есть ли что отменять
    static bool CanUndo()
    {
        return history.Count > 0;
    }

    // ================= Задание 13. Каталог товаров =================

    // Основной список: тут хранятся сами товары
    static List<Product> products = new List<Product>();

    // Словарь для быстрого поиска товара по Id
    // (хранит ссылки на те же товары, копий нет)
    static Dictionary<string, Product> byId = new Dictionary<string, Product>();

    // Множество первых букв названий (HashSet не хранит повторы)
    static HashSet<char> categories = new HashSet<char>();

    // Добавить товар сразу в список, словарь и множество
    static void AddProduct(string id, string name, decimal price)
    {
        Product p = new Product { Id = id, Name = name, Price = price };
        products.Add(p);
        byId[id] = p;
        categories.Add(name[0]);   // name[0] - первая буква названия
    }

    // Товары дешевле заданной цены
    static List<Product> FindCheaperThan(decimal maxPrice)
    {
        return products.Where(p => p.Price < maxPrice).ToList();
    }

    // N самых дорогих товаров
    static List<Product> GetTopNByPrice(int n)
    {
        // OrderByDescending - сортировка от дорогих к дешёвым
        // Take(n) - взять первые n
        return products.OrderByDescending(p => p.Price).Take(n).ToList();
    }

    // ================= Проверка всех заданий =================
    static void Main()
    {
        // Чтобы русские буквы нормально выводились в консоли
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("--- Задание 9 ---");
        AddContact("111", "Анна");
        AddContact("222", "Борис");
        AddContact("111", "Дубликат");                        // такой номер уже есть
        Console.WriteLine(GetNameByPhone("111") ?? "не найден"); // Анна
        Console.WriteLine(GetNameByPhone("999") ?? "не найден"); // нет такого номера
        RemoveContact("222");
        foreach (string s in ListAll())
            Console.WriteLine(s);

        Console.WriteLine("--- Задание 10 ---");
        tasks.Enqueue("Написать отчёт");                      // Enqueue - добавить в конец очереди
        tasks.Enqueue("urgent: починить сервер");
        tasks.Enqueue("Купить кофе");
        ProcessTasks(2);                                      // обработать 2 задачи
        ProcessTasks(5);                                      // просим больше, чем осталось
        List<string> all = new List<string> { "обычная", "urgent: сдать лабу", "ещё одна" };
        foreach (string t in FilterUrgent(all))
            Console.WriteLine(t);

        Console.WriteLine("--- Задание 11 ---");
        string text = "Мама мыла раму. Мама мыла окно, и рама была чистая!";
        Dictionary<string, int> freq = CountWords(text);

        Console.WriteLine("Топ-5:");
        // Сортируем по числу повторов (от большего) и берём первые 5
        foreach (var p in freq.OrderByDescending(x => x.Value).Take(5))
            Console.WriteLine(p.Key + ": " + p.Value);

        Console.WriteLine("Встретились 1 раз:");
        // Where - оставить слова с количеством 1, Select - взять только само слово
        var once = freq.Where(x => x.Value == 1).Select(x => x.Key);
        Console.WriteLine(string.Join(", ", once));

        Console.WriteLine("--- Задание 12 ---");
        Do("Type");
        Do("Delete");
        Console.WriteLine("Undo: " + Undo());                 // Delete (последнее)
        Console.WriteLine("Undo: " + Undo());                 // Type
        Console.WriteLine("Undo: " + (Undo() ?? "null"));     // стек пуст, будет null
        Console.WriteLine("CanUndo: " + CanUndo());           // False

        Console.WriteLine("--- Задание 13 ---");
        AddProduct("p1", "Яблоко", 50);
        AddProduct("p2", "Арбуз", 300);
        AddProduct("p3", "Банан", 90);
        AddProduct("p4", "Ананас", 250);
        Console.WriteLine("p3: " + byId["p3"].Name);          // быстрый поиск по Id
        Console.WriteLine("Категории: " + string.Join(", ", categories));
        Console.WriteLine("Дешевле 100:");
        foreach (Product p in FindCheaperThan(100))
            Console.WriteLine(p.Name + " " + p.Price);
        Console.WriteLine("Топ-2 по цене:");
        foreach (Product p in GetTopNByPrice(2))
            Console.WriteLine(p.Name + " " + p.Price);
    }
}