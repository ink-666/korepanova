using System;

class Program
{
    static void Main()
    {
        try
        {
            Console.Write("Введите минимальное значение диапазона: ");
            int min = Convert.ToInt32(Console.ReadLine());

            Console.Write("Введите максимальное значение диапазона: ");
            int max = Convert.ToInt32(Console.ReadLine());

            // Исключение 1: min > max
            if (min > max)
            {
                throw new ArgumentException("Минимальное значение не может быть больше максимального!");
            }

            Random random = new Random();
            int secretNumber = random.Next(min, max + 1);
            bool guessed = false;

            Console.WriteLine($"Угадайте число от {min} до {max}");

            while (!guessed)
            {
                Console.Write("Ваше число: ");
                // Исключение 2: некорректный ввод (не число) обрабатывается ниже в catch (FormatException)
                int attempt = Convert.ToInt32(Console.ReadLine());

                if (attempt == secretNumber)
                {
                    Console.WriteLine("Поздравляем, вы угадали число!");
                    guessed = true;
                }
                else if (attempt < secretNumber)
                {
                    Console.WriteLine("Загаданное число больше.");
                }
                else
                {
                    Console.WriteLine("Загаданное число меньше.");
                }
            }
        }
        catch (FormatException)
        {
            Console.WriteLine("Ошибка: введены некорректные данные (ожидалось целое число)!");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Непредвиденная ошибка: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Работа программы завершена.");
        }
    }
}