using System; // Подключает базовые классы C#, например Console
using System.Text; // Нужен для Encoding.UTF8, чтобы русский текст корректно выводился в консоль
using System.Text.RegularExpressions; // Подключает Regex для работы с регулярными выражениями

// Пространство имён объединяет связанные классы программы
namespace RegexTextAnalyzer
{
    // Основной класс программы
    class Program
    {
        // unsafe разрешает использовать указатели в C#
        // Main — главный метод, с которого начинается выполнение программы
        unsafe static void Main()
        {
            // Устанавливаем кодировку UTF-8 для правильного отображения русского текста
            Console.OutputEncoding = Encoding.UTF8;

            // Выводим заголовок программы
            Console.WriteLine("=== Работа с регулярными выражениями и указателями ===\n");

            // ========== ЧАСТЬ 1: Регулярные выражения ==========

            // Создаём строку с текстом, который будем анализировать регулярными выражениями
            string text = "Бык тупогуб, тупогубенький бычок, у быка губа бела была тупа. " +
                          "Мама мыла раму. Привет мир! Это тестовый текст. " +
                          "123-456-7890 - номер телефона. " +
                          "Москва, Санкт-Петербург, Новосибирск. " +
                          "Регулярные выражения в C# очень полезны.";

            // Выводим подпись перед исходным текстом
            Console.WriteLine("Исходный текст:");

            // Выводим сам исходный текст
            Console.WriteLine(text);

            // Добавляем пустую строку для удобного чтения результата
            Console.WriteLine();

            // Вызываем метод подсчёта букв, цифр, символов и слов
            CountCharactersAndWords(text);

            // Вызываем метод поиска предложений, начинающихся со слова "Бык"
            LinesStartingWith(text, "Бык");

            // Вызываем метод поиска предложений, заканчивающихся точкой
            LinesEndingWith(text, ".");

            // Вызываем метод с примерами замены текста через регулярные выражения
            ReplaceTextExample(text);

            // ========== ЧАСТЬ 2: Указатели (небезопасный код) ==========

            // Выводим заголовок второй части программы
            Console.WriteLine("\n=== РАБОТА С УКАЗАТЕЛЯМИ ===");

            // Сообщаем, что ниже используется unsafe-код
            Console.WriteLine("(Небезопасный код - unsafe)\n");

            // Объявляем три целочисленные переменные
            int value1 = 100;
            int value2 = 200;
            int value3 = 300;

            // Объявляем указатели на int
            // Указатель хранит адрес переменной в памяти
            int* ptr1;
            int* ptr2;
            int* ptr3;

            // Записываем в указатели адреса соответствующих переменных
            ptr1 = &value1;
            ptr2 = &value2;
            ptr3 = &value3;

            // Выводим значения и адреса переменных до изменения
            Console.WriteLine("=== ДО ИЗМЕНЕНИЯ ===");

            // (ulong)ptr1:X переводит адрес в шестнадцатеричный формат
            Console.WriteLine($"Переменная value1 = {value1}, Адрес: {(ulong)ptr1:X}");
            Console.WriteLine($"Переменная value2 = {value2}, Адрес: {(ulong)ptr2:X}");
            Console.WriteLine($"Переменная value3 = {value3}, Адрес: {(ulong)ptr3:X}");

            // Изменяем значения переменных через указатели
            // *ptr1 означает "значение по адресу, который хранится в ptr1"
            *ptr1 = 999;
            *ptr2 = 888;
            *ptr3 = 777;

            // Выводим значения после изменения через указатели
            Console.WriteLine("\n=== ПОСЛЕ ИЗМЕНЕНИЯ ЧЕРЕЗ УКАЗАТЕЛИ ===");
            Console.WriteLine($"Переменная value1 = {value1} (было 100)");
            Console.WriteLine($"Переменная value2 = {value2} (было 200)");
            Console.WriteLine($"Переменная value3 = {value3} (было 300)");

            // Работа с указателем на указатель
            Console.WriteLine("\n=== УКАЗАТЕЛЬ НА УКАЗАТЕЛЬ ===");

            // ptrMain хранит адрес переменной value1
            int* ptrMain = &value1;

            // ptrToPtr хранит адрес самого указателя ptrMain
            int** ptrToPtr = &ptrMain;

            // Выводим адрес указателя ptrMain
            Console.WriteLine($"Адрес указателя ptrMain: {(ulong)ptrToPtr:X}");

            // *ptrToPtr даёт значение ptrMain, то есть адрес value1
            Console.WriteLine($"Значение по адресу ptrMain (адрес value1): {(ulong)*ptrToPtr:X}");

            // **ptrToPtr сначала переходит к ptrMain, потом к value1
            Console.WriteLine($"Значение value1 через двойной указатель: {**ptrToPtr}");

            // Меняем value1 через двойной указатель
            **ptrToPtr = 555;

            // Выводим новое значение value1
            Console.WriteLine($"\nПосле изменения через двойной указатель:");
            Console.WriteLine($"value1 = {value1}");

            // Демонстрация арифметики указателей
            Console.WriteLine("\n=== АРИФМЕТИКА УКАЗАТЕЛЕЙ ===");

            // Создаём массив целых чисел
            int[] numbers = { 10, 20, 30, 40, 50 };

            // fixed закрепляет массив в памяти
            // Это нужно, чтобы сборщик мусора не переместил массив во время работы с указателем
            fixed (int* arrPtr = numbers)
            {
                // Выводим массив в привычном виде
                Console.WriteLine("Массив numbers: [10, 20, 30, 40, 50]");

                // Выводим адрес первого элемента массива
                Console.WriteLine($"Адрес первого элемента: {(ulong)arrPtr:X}");

                // Проходим по всем элементам массива
                for (int i = 0; i < numbers.Length; i++)
                {
                    // arrPtr + i переходит к следующему элементу массива
                    // *(arrPtr + i) получает значение элемента по этому адресу
                    Console.WriteLine($"Элемент [{i}] = {*(arrPtr + i)}, Адрес: {(ulong)(arrPtr + i):X}");
                }
            }

            // Сообщаем пользователю, что программа ждёт нажатия клавиши
            Console.WriteLine("\nНажмите любую клавишу для завершения...");

            // Ожидаем нажатия любой клавиши, чтобы окно консоли не закрылось сразу
            Console.ReadKey();
        }

        // Метод считает символы, слова и слова с определённым корнем
        static void CountCharactersAndWords(string text)
        {
            // Заголовок раздела
            Console.WriteLine("=== 2. Подсчёт символов и слов ===");

            // Regex ищет английские буквы, русские буквы и цифры
            Regex lettersRegex = new Regex(@"[A-Za-zА-Яа-я0-9]");

            // Matches находит все совпадения, Count считает их количество
            int letterCount = lettersRegex.Matches(text).Count;

            // Выводим количество букв и цифр
            Console.WriteLine($"Количество букв и цифр: {letterCount}");

            // text.Length считает все символы: буквы, пробелы, знаки препинания и цифры
            Console.WriteLine($"Общее количество символов: {text.Length}");

            // Regex для поиска слов
            // \b — граница слова, \w+ — один или больше символов слова
            Regex wordRegex = new Regex(@"\b\w+\b");

            // Считаем количество найденных слов
            int wordCount = wordRegex.Matches(text).Count;

            // Выводим количество слов
            Console.WriteLine($"Количество слов: {wordCount}");

            // Regex ищет слова, начинающиеся с "тупогуб"
            // \w* означает любое продолжение слова или его отсутствие
            Regex phraseRegex = new Regex(@"тупогуб\w*");

            // Считаем количество таких слов
            int phraseCount = phraseRegex.Matches(text).Count;

            // Выводим количество слов с нужным корнем
            Console.WriteLine($"Количество слов с корнем 'тупогуб': {phraseCount}");

            // Пустая строка для разделения вывода
            Console.WriteLine();
        }

        // Метод ищет части текста, которые начинаются с заданного слова
        static void LinesStartingWith(string text, string startWord)
        {
            // Выводим заголовок с искомым начальным словом
            Console.WriteLine($"=== 3. Строки, начинающиеся с '{startWord}' ===");

            // Делим текст на части по точкам
            // RemoveEmptyEntries удаляет пустые элементы после разделения
            string[] lines = text.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

            // ^ означает начало строки
            // IgnoreCase позволяет не учитывать регистр букв
            Regex regex = new Regex($"^{startWord}", RegexOptions.IgnoreCase);

            // Счётчик найденных строк
            int found = 0;

            // Перебираем все получившиеся части текста
            foreach (string line in lines)
            {
                // Убираем лишние пробелы в начале и конце
                string trimmed = line.Trim();

                // Проверяем, начинается ли строка с нужного слова
                if (regex.IsMatch(trimmed))
                {
                    // Выводим найденную строку
                    Console.WriteLine(trimmed);

                    // Увеличиваем счётчик найденных строк
                    found++;
                }
            }

            // Если ничего не найдено, выводим сообщение
            if (found == 0)
                Console.WriteLine("Не найдено.");
            else
                // Иначе выводим количество найденных строк
                Console.WriteLine($"Найдено строк: {found}");

            // Пустая строка для разделения вывода
            Console.WriteLine();
        }

        // Метод ищет части текста, которые заканчиваются заданным символом
        static void LinesEndingWith(string text, string endSymbol)
        {
            // Выводим заголовок с символом окончания
            Console.WriteLine($"=== 4. Строки, оканчивающиеся на '{endSymbol}' ===");

            // Делим текст на части по точкам
            string[] lines = text.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

            // Экранируем символ, чтобы Regex воспринимал его буквально
            // Например, точка в Regex имеет особое значение, поэтому её нужно экранировать
            string escaped = Regex.Escape(endSymbol);

            // $ означает конец строки
            Regex regex = new Regex($"{escaped}$");

            // Счётчик найденных строк
            int found = 0;

            // Перебираем все части текста
            foreach (string line in lines)
            {
                // Возвращаем точку в конец строки, потому что Split её убрал
                string trimmed = line.Trim() + ".";

                // Проверяем, заканчивается ли строка нужным символом
                if (regex.IsMatch(trimmed))
                {
                    // Выводим найденную строку
                    Console.WriteLine(trimmed);

                    // Увеличиваем счётчик
                    found++;
                }
            }

            // Если ничего не найдено, выводим сообщение
            if (found == 0)
                Console.WriteLine("Не найдено.");
            else
                // Иначе выводим количество найденных строк
                Console.WriteLine($"Найдено строк: {found}");

            // Пустая строка для разделения вывода
            Console.WriteLine();
        }

        // Метод показывает примеры замены текста через Regex
        static void ReplaceTextExample(string text)
        {
            // Заголовок раздела
            Console.WriteLine("=== 5. Замена части текста ===");

            // Regex ищет один или несколько пробельных символов
            // \s — пробел, табуляция или перенос строки
            // + — один или больше таких символов
            Regex spaceRegex = new Regex(@"\s+");

            // Заменяем все последовательности пробелов на один обычный пробел
            string result1 = spaceRegex.Replace(text, " ");

            // Выводим результат замены пробелов
            Console.WriteLine("После замены нескольких пробелов на один:");
            Console.WriteLine(result1);
            Console.WriteLine();

            // Regex ищет любую цифру
            // \d — цифра от 0 до 9
            Regex digitRegex = new Regex(@"\d");

            // Заменяем каждую цифру на символ #
            string result2 = digitRegex.Replace(text, "#");

            // Выводим результат замены цифр
            Console.WriteLine("После замены цифр на #:");
            Console.WriteLine(result2);
            Console.WriteLine();

            // Regex ищет слова, которые начинаются с "тупогуб"
            Regex wordRegex = new Regex(@"тупогуб\w*");

            // Заменяем найденные слова на ****
            string result3 = wordRegex.Replace(text, "****");

            // Выводим результат замены слов
            Console.WriteLine("После замены 'тупогуб' на '****':");
            Console.WriteLine(result3);
        }
    }
}
