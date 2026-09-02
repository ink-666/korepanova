using System;

namespace ZooLab
{
    // ==========================================================
    // Базовый класс Animal описывает произвольное животное.
    // Он содержит общие поля/свойства, которые есть у ЛЮБОГО
    // животного: кличка и возраст.
    // ==========================================================
    class Animal
    {
        // Свойства класса (автосвойства с get/set)
        public string Name { get; set; }
        public int Age { get; set; }

        // Конструктор базового класса без параметров.
        // Он нужен на случай, если производный класс не передаст
        // данные явно через base(...) — тогда используются
        // значения по умолчанию.
        public Animal()
        {
            Name = "Без имени";
            Age = 0;
            Console.WriteLine("--> Вызван Animal() — конструктор по умолчанию");
        }

        // Конструктор с параметрами. Данные (name, age) приходят
        // сюда либо напрямую при создании Animal, либо из
        // конструктора производного класса через ключевое слово base(...)
        public Animal(string name, int age)
        {
            Name = name;
            Age = age;
            Console.WriteLine($"--> Вызван Animal(string, int): Name={Name}, Age={Age}");
        }

        // Обычный (не перегруженный) метод — общий для всех животных
        public virtual void MakeSound()
        {
            Console.WriteLine($"{Name} издаёт какой-то звук.");
        }

        public void PrintInfo()
        {
            Console.WriteLine($"Животное: {Name}, возраст: {Age}");
        }
    }

    // ==========================================================
    // Производный класс Predator (Хищник) наследует Animal.
    // Данные полей Name и Age Predator НЕ дублирует — они уже
    // есть в базовом классе и достаются "по наследству".
    // Дополнительно добавляется своё поле Prey (жертва/добыча).
    // ==========================================================
    class Predator : Animal
    {
        public string Prey { get; set; }

        // Конструктор Predator принимает 3 параметра.
        // Часть данных (name, age) он НЕ обрабатывает сам,
        // а передаёт их "наверх", в конструктор базового класса
        // Animal, с помощью выражения ": base(name, age)".
        // Именно там name и age будут записаны в свойства Name и Age.
        // Сам конструктор Predator обрабатывает только свой параметр prey.
        public Predator(string name, int age, string prey) : base(name, age)
        {
            Prey = prey;
            Console.WriteLine($"--> Вызван Predator(...): Prey={Prey}");
        }

        // Переопределение метода базового класса (полиморфизм)
        public override void MakeSound()
        {
            Console.WriteLine($"{Name} рычит, охотясь на {Prey}!");
        }
    }

    class Program
    {
        // ==========================================================
        // ОБОБЩЁННЫЙ (GENERIC) МЕТОД.
        // Метод PrintPair<T> может печатать пару значений ЛЮБОГО
        // одинакового типа T — тип подставляется в момент вызова,
        // поэтому один и тот же код работает и для int, и для
        // string, и для Animal, без переписывания.
        // Данные приходят в метод через параметры first и second,
        // тип T определяется автоматически по переданным аргументам
        // (или указывается явно PrintPair<Тип>(...)).
        // ==========================================================
        static void PrintPair<T>(T first, T second)
        {
            Console.WriteLine($"Обобщённый метод получил данные типа {typeof(T).Name}: [{first}] и [{second}]");
        }

        // ==========================================================
        // ПЕРЕГРУЗКА МЕТОДОВ (method overloading).
        // Три метода с одинаковым именем Feed, но разной сигнатурой:
        // отличаются количеством и типами параметров.
        // ==========================================================

        // Версия 1: кормим животное едой по умолчанию
        static void Feed(Animal animal)
        {
            Console.WriteLine($"{animal.Name} покормили стандартной едой.");
        }

        // Версия 2: кормим животное конкретной едой (строка передаётся явно)
        static void Feed(Animal animal, string food)
        {
            Console.WriteLine($"{animal.Name} покормили едой: {food}.");
        }

        // Версия 3: кормим животное несколько раз (int задаёт количество раз,
        // данные "раз" используются в цикле для повторного вывода)
        static void Feed(Animal animal, string food, int times)
        {
            for (int i = 1; i <= times; i++)
            {
                Console.WriteLine($"[{i}/{times}] {animal.Name} получил порцию: {food}.");
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("=== Создание объекта базового класса Animal ===");
            // Данные "Кеша", 2 передаются напрямую в конструктор Animal(string, int)
            Animal simpleAnimal = new Animal("Кеша", 2);
            simpleAnimal.PrintInfo();
            simpleAnimal.MakeSound();

            Console.WriteLine();
            Console.WriteLine("=== Создание объекта производного класса Predator ===");
            // Данные "Лео", 5, "Зебра" передаются в Predator(...),
            // откуда "Лео" и 5 уходят дальше в Animal через base(name, age),
            // а "Зебра" остаётся и обрабатывается в самом Predator
            Predator lion = new Predator("Лео", 5, "Зебра");
            lion.PrintInfo();   // унаследованный метод из Animal
            lion.MakeSound();   // переопределённый метод

            Console.WriteLine();
            Console.WriteLine("=== Использование обобщённого метода PrintPair<T> ===");
            PrintPair<int>(10, 20);          // T = int
            PrintPair<string>("Кот", "Пёс"); // T = string
            PrintPair(simpleAnimal, lion);   // T выводится автоматически как Animal

            Console.WriteLine();
            Console.WriteLine("=== Использование перегруженных методов Feed ===");
            Feed(simpleAnimal);                     // Feed(Animal)
            Feed(lion, "Мясо");                      // Feed(Animal, string)
            Feed(lion, "Мясо", 3);                   // Feed(Animal, string, int)

            Console.WriteLine();
            Console.WriteLine("Программа завершена. Итоговый вывод выполнен.");
        }
    }
}