using System;

class Program
{
    // 1. Определение делегата
    delegate int Operation(int a, int b);

    static void Main()
    {
        Calculator calc = new Calculator();

        // 2. Присвоение метода делегату
        Operation op1 = calc.Add;
        Operation op2 = calc.Subtract;

        Console.WriteLine(op1(5, 3));
        Console.WriteLine(op2(5, 3));

        // 3. Объединение делегатов
        Operation op3 = op1 + op2;
        op3(10, 5);

        // 4. Делегат как параметр
        DoOperation(7, 4, calc.Add);
        DoOperation(7, 4, calc.Subtract);
    }

    static void DoOperation(int a, int b, Operation op)
    {
        Console.WriteLine("Результат: " + op(a, b));
    }
}

class Calculator
{
    // Перегрузка методов
    public int Add(int a, int b)
    {
        Console.WriteLine("Метод Add(int,int)");
        return a + b;
    }

    public int Add(int a, int b, int c)
    {
        Console.WriteLine("Метод Add(int,int,int)");
        return a + b + c;
    }

    public int Subtract(int a, int b)
    {
        Console.WriteLine("Метод Subtract");
        return a - b;
    }
}