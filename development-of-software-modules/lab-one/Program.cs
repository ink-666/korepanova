using System;

// ==================== БАЗОВЫЙ КЛАСС PERSON ====================
class Person
{
    // Свойство для имени
    public string Name { get; set; }

    // Конструктор с параметром
    public Person(string name)
    {
        Name = name;
        Console.WriteLine($"   [КОНСТРУКТОР Person] Создан человек с именем: {name}");
    }

    // Виртуальный метод для вывода информации
    public virtual void Print()
    {
        Console.WriteLine($"   [МЕТОД Print] Person: {Name}");
    }
}

// ==================== ПРОИЗВОДНЫЙ КЛАСС EMPLOYEE ====================
class Employee : Person
{
    // Свойство для компании
    public string Company { get; set; }

    // Конструктор Employee ВЫЗЫВАЕТ конструктор Person через base
    public Employee(string name, string company) : base(name)
    {
        Company = company;
        Console.WriteLine($"   [КОНСТРУКТОР Employee] Сотрудник работает в: {company}");
    }

    // Переопределение метода Print
    public override void Print()
    {
        Console.WriteLine($"   [МЕТОД Print] Employee: {Name}, работает в {Company}");
    }
}

// ==================== ОБОБЩЁННЫЙ КЛАСС DEPARTMENT<T> ====================
class Department<T>
{
    // Свойство типа T (универсальный параметр)
    public T Id { get; set; }
    
    // Свойство типа Person (может быть Person или Employee)
    public Person Chief { get; set; }

    // Конструктор
    public Department(T id, Person chief)
    {
        Id = id;
        Chief = chief;
        Console.WriteLine($"   [КОНСТРУКТОР Department] Создан отдел с Id типа {typeof(T).Name} = {id}");
    }

    // Метод для вывода информации о начальнике
    public void ShowChief()
    {
        Console.Write("   [МЕТОД ShowChief] Информация о начальнике отдела: ");
        Chief.Print();
    }
}

// ==================== КЛАСС HELPER С ПЕРЕГРУЗКОЙ МЕТОДОВ ====================
class Helper
{
    // ПЕРЕГРУЗКА МЕТОДОВ - разные версии метода Display
    
    // Версия 1: для целого числа
    public void Display(int number)
    {
        Console.WriteLine($"   [ПЕРЕГРУЗКА 1] Display(int): {number}");
    }

    // Версия 2: для строки
    public void Display(string text)
    {
        Console.WriteLine($"   [ПЕРЕГРУЗКА 2] Display(string): \"{text}\"");
    }

    // Версия 3: для двух целых чисел
    public void Display(int a, int b)
    {
        Console.WriteLine($"   [ПЕРЕГРУЗКА 3] Display(int, int): {a} и {b}");
    }

    // Версия 4: для числа с плавающей точкой
    public void Display(double number)
    {
        Console.WriteLine($"   [ПЕРЕГРУЗКА 4] Display(double): {number}");
    }

    // ОБОБЩЁННЫЙ МЕТОД Swap для обмена двух переменных любого типа
    public void Swap<T>(ref T first, ref T second)
    {
        Console.WriteLine($"   [ОБОБЩЁННЫЙ МЕТОД Swap] Меняем местами значения типа {typeof(T).Name}");
        Console.WriteLine($"      Было: first = {first}, second = {second}");
        
        T temp = first;
        first = second;
        second = temp;
        
        Console.WriteLine($"      Стало: first = {first}, second = {second}");
    }
}

// ==================== ГЛАВНЫЙ КЛАСС PROGRAM ====================
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("==========================================");
        Console.WriteLine("ПРИМЕРЫ ОБЪЕКТНО-ОРИЕНТИРОВАННОГО ПРОГРАММИРОВАНИЯ");
        Console.WriteLine("==========================================\n");

        // ============= ЧАСТЬ 1: НАСЛЕДОВАНИЕ =============
        Console.WriteLine("--- ЧАСТЬ 1: Наследование классов ---");
        
        // Создаём обычного человека
        Console.WriteLine("\n>> Создание объекта Person:");
        Person person = new Person("Семён");
        person.Print();

        // Создаём сотрудника (Employee наследует Person)
        Console.WriteLine("\n>> Создание объекта Employee (обрати внимание на порядок вызова конструкторов):");
        Employee employee = new Employee("Иван", "Microsoft");
        employee.Print();

        // ============= ЧАСТЬ 2: ПОЛИМОРФИЗМ =============
        Console.WriteLine("\n\n--- ЧАСТЬ 2: Полиморфизм (объект Employee через переменную Person) ---");
        Person personAsEmployee = new Employee("Мария", "Google");
        personAsEmployee.Print(); // Вызовется метод Employee.Print()!

        // ============= ЧАСТЬ 3: ОБОБЩЁННЫЕ КЛАССЫ =============
        Console.WriteLine("\n\n--- ЧАСТЬ 3: Обобщённые классы (Generics) ---");
        
        // Отдел с числовым идентификатором
        Console.WriteLine("\n>> Department<int>:");
        Department<int> itDepartment = new Department<int>(101, employee);
        itDepartment.ShowChief();

        // Отдел со строковым идентификатором
        Console.WriteLine("\n>> Department<string>:");
        Department<string> salesDepartment = new Department<string>("A-205", person);
        salesDepartment.ShowChief();

        // ============= ЧАСТЬ 4: ПЕРЕГРУЗКА МЕТОДОВ =============
        Console.WriteLine("\n\n--- ЧАСТЬ 4: Перегрузка методов ---");
        
        Helper helper = new Helper();
        Console.WriteLine("\n>> Вызов разных версий метода Display:");
        helper.Display(42);                // int
        helper.Display("Привет мир");      // string
        helper.Display(7, 14);             // два int
        helper.Display(3.14);              // double

        // ============= ЧАСТЬ 5: ОБОБЩЁННЫЙ МЕТОД =============
        Console.WriteLine("\n\n--- ЧАСТЬ 5: Обобщённый метод Swap<T> ---");
        
        // Обмен целых чисел
        Console.WriteLine("\n>> Swap<int>:");
        int x = 5, y = 10;
        Console.WriteLine($"   До вызова Swap: x = {x}, y = {y}");
        helper.Swap<int>(ref x, ref y);
        Console.WriteLine($"   После Swap: x = {x}, y = {y}");

        // Обмен строк
        Console.WriteLine("\n>> Swap<string> (тип выводится автоматически):");
        string str1 = "Первый", str2 = "Второй";
        Console.WriteLine($"   До вызова Swap: str1 = {str1}, str2 = {str2}");
        helper.Swap(ref str1, ref str2);    // тип <string> можно не указывать
        Console.WriteLine($"   После Swap: str1 = {str1}, str2 = {str2}");

        // ============= ЧАСТЬ 6: ДЕМОНСТРАЦИЯ ПОРЯДКА ВЫЗОВА КОНСТРУКТОРОВ =============
        Console.WriteLine("\n\n--- ЧАСТЬ 6: Демонстрация цепочки вызовов конструкторов ---");
        Console.WriteLine(">> Создаём ещё одного сотрудника, чтобы увидеть порядок:");
        Employee testEmp = new Employee("Пётр", "Amazon");
        
        Console.WriteLine("\n==========================================");
        Console.WriteLine("ПРОГРАММА ЗАВЕРШЕНА");
        Console.WriteLine("==========================================");
    }
}