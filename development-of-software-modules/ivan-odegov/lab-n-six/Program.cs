using System;
using System.Text.RegularExpressions;
using System.Linq;

class Program
{
    static void Main()
    {
        // 1. Исходный текст (отрывок из "У лукоморья дуб зелёный...")
        string text = @"У лукоморья дуб зелёный;
Златая цепь на дубе том:
И днём и ночью кот учёный
Всё ходит по цепи кругом;
Идёт направо — песнь заводит,
Налево — сказку говорит.
Там чудеса: там леший бродит,
Русалка на ветвях сидит;
Там на неведомых дорожках
Следы невиданных зверей;
Избушка там на курьих ножках
Стоит без окон, без дверей;
Там лес и дол видений полны;
Там о заре прихлынут волны
На брег песчаный и пустой,
И тридцать витязей прекрасных
Чредой из вод выходят ясных,
И с ними дядька их морской;
Там королевич мимоходом
Пленяет грозного царя;
Там в облаках перед народом
Через леса, через моря
Колдун несёт богатыря;
В темнице там царевна тужит,
А бурый волк ей верно служит;
Там ступа с Бабою Ягой
Идёт, бредёт сама собой,
Там царь Кащей над златом чахнет;
Там русский дух… там Русью пахнет!
И там я был, и мёд я пил;
У моря видел дуб зелёный;
Под ним сидел, и кот учёный
Свои мне сказки говорил.";

        Console.WriteLine("========== ЛАБОРАТОРНАЯ РАБОТА: РЕГУЛЯРНЫЕ ВЫРАЖЕНИЯ ==========\n");

        // ==================== 1. ПОДСЧЁТ СИМВОЛОВ И СЛОВ ====================
        Console.WriteLine("1. ПОДСЧЁТ СИМВОЛОВ И СЛОВ:");
        Console.WriteLine(new string('-', 50));

        // Подсчёт всех символов (включая пробелы и знаки препинания)
        int totalChars = text.Length;
        Console.WriteLine($"Общее количество символов в тексте: {totalChars}");

        // Подсчёт символов без пробелов
        // Regex.Replace заменяет все пробелы (\s - любой ws символ) на пустую строку, затем считаем длину
        int charsWithoutSpaces = Regex.Replace(text, @"\s", "").Length;
        Console.WriteLine($"Количество символов без пробелов: {charsWithoutSpaces}");

        // Подсчёт слов (последовательности букв, цифр и подчёркиваний)
        // \w+ - одно или более (\W = буквы, цифры, подчёркивание), Matches возвращает коллекцию всех совпадений
        MatchCollection wordMatches = Regex.Matches(text, @"\w+");
        int wordCount = wordMatches.Count;
        Console.WriteLine($"Количество слов: {wordCount}");

        // Подсчёт определённых словосочетаний (например, "там")
        MatchCollection phraseMatches = Regex.Matches(text, @"там", RegexOptions.IgnoreCase);
        int phraseCount = phraseMatches.Count;
        Console.WriteLine($"Количество словосочетаний \"там\": {phraseCount}");

        // Подсчёт слов, начинающихся с определённой буквы (например, "к")
        // \b - граница слова (начало), к - буква 'к', \w* - ноль или более букв/цифр/подчёркиваний после неё
        // RegexOptions.IgnoreCase - игнорировать различия в регистре букв
        MatchCollection wordsWithK = Regex.Matches(text, @"\bк\w*", RegexOptions.IgnoreCase);
        Console.WriteLine($"Количество слов, начинающихся на 'к': {wordsWithK.Count}");

        Console.WriteLine("\n");

        // ==================== 2. СТРОКИ, НАЧИНАЮЩИЕСЯ С ОПРЕДЕЛЁННОГО СИМВОЛА ====================
        Console.WriteLine("2. СТРОКИ, НАЧИНАЮЩИЕСЯ С 'Т':");
        Console.WriteLine(new string('-', 50));

        // Разбиваем текст на строки
        string[] lines = text.Split(new[] { Environment.NewLine, "\n" }, StringSplitOptions.None);

        // Регулярное выражение для строк, начинающихся с 'Т' (с учётом начала строки)
        // ^ - начало строки, \s* - ноль или более пробельных символов (вначале), Т - буква 'Т'
        string startsWithPattern = @"^\s*Т";
        // Where - фильтр LINQ, оставляет только строки, где регулярное выражение совпадает
        var linesStartingWithT = lines.Where(line => Regex.IsMatch(line, startsWithPattern));

        foreach (string line in linesStartingWithT)
        {
            Console.WriteLine(line);
        }

        Console.WriteLine($"\nВсего строк, начинающихся с 'Т': {linesStartingWithT.Count()}\n");

        // ==================== 3. СТРОКИ, ОКАНЧИВАЮЩИЕСЯ ОПРЕДЕЛЁННЫМ СИМВОЛОМ ====================
        Console.WriteLine("3. СТРОКИ, ОКАНЧИВАЮЩИЕСЯ НА ';':");
        Console.WriteLine(new string('-', 50));

        // Регулярное выражение для строк, оканчивающихся на ';'
        // ; - точка с запятой, $ - конец строки
        string endsWithPattern = @";$";
        // Trim() - удаляет пробелы с концов строки перед проверкой регулярным выражением
        var linesEndingWithSemicolon = lines.Where(line => Regex.IsMatch(line.Trim(), endsWithPattern));

        foreach (string line in linesEndingWithSemicolon)
        {
            Console.WriteLine(line);
        }

        Console.WriteLine($"\nВсего строк, оканчивающихся на ';': {linesEndingWithSemicolon.Count()}\n");

        // ==================== 4. ИЗМЕНЕНИЕ ЧАСТИ ТЕКСТА ====================
        Console.WriteLine("4. ИЗМЕНЕНИЕ ЧАСТИ ТЕКСТА:");
        Console.WriteLine(new string('-', 50));

        // Вариант 1: Замена слова "кот" на "КОТИЩЕ" с использованием регулярного выражения
        // \b - границы слова (\bкот\b - точное совпадение слова "кот", а не часть большего слова)
        string modifiedText1 = Regex.Replace(text, @"\bкот\b", "КОТИЩЕ", RegexOptions.IgnoreCase);
        Console.WriteLine("Вариант 1 - Замена 'кот' на 'КОТИЩЕ':");
        Console.WriteLine(modifiedText1.Substring(0, Math.Min(modifiedText1.Length, 300)) + "...\n");

        // Вариант 2: Замена всех слов, начинающихся на "ле", на "***"
        // \bле\w* - начало слова, "ле", затем ноль или более букв (\w*)
        string modifiedText2 = Regex.Replace(text, @"\bле\w*", "***", RegexOptions.IgnoreCase);
        Console.WriteLine("Вариант 2 - Замена слов на 'ле' на '***':");
        Console.WriteLine(modifiedText2.Substring(0, Math.Min(modifiedText2.Length, 300)) + "...\n");

        // Вариант 3: Удаление всех знаков препинания
        // [^\w\s] - НЕ буква/цифра/подчёркивание И НЕ пробел (всё остальное - знаки препинания)
        // ^ внутри [ ] означает отрицание
        string modifiedText3 = Regex.Replace(text, @"[^\w\s]", "");
        Console.WriteLine("Вариант 3 - Удаление всех знаков препинания (первые 300 символов):");
        Console.WriteLine(modifiedText3.Substring(0, Math.Min(modifiedText3.Length, 300)) + "...\n");

        // Вариант 4: Замена окончаний строк на специальный маркер
        // (?<=[а-яё]) - положительный lookbehind, проверяет что ДО этого место есть русская буква [а-яё]
        // \s*$ - ноль или более пробелов в конце строки
        // RegexOptions.Multiline - ^ и $ работают для начала/конца каждой строки, не только всего текста
        string modifiedText4 = Regex.Replace(text, @"(?<=[а-яё])\s*$", " [КОНЕЦ_СТРОКИ]", RegexOptions.Multiline);
        Console.WriteLine("Вариант 4 - Добавление маркера в конец каждой строки:");
        string[] previewLines = modifiedText4.Split('\n').Take(5).ToArray();
        foreach (string line in previewLines)
        {
            Console.WriteLine(line);
        }
        Console.WriteLine("...\n");

        // ==================== ДОПОЛНИТЕЛЬНЫЙ ПРИМЕР: ПОИСК ПО ШАБЛОНУ ====================
        Console.WriteLine("5. ДОПОЛНИТЕЛЬНО: ПОИСК ВСЕХ СЛОВ, СОДЕРЖАЩИХ КОРЕНЬ 'мор':");
        Console.WriteLine(new string('-', 50));

        // \w*мор\w* - ноль или более букв, потом "мор", потом ноль или более букв (корень "мор" в любом месте слова)
        Regex rootRegex = new Regex(@"\w*мор\w*", RegexOptions.IgnoreCase);
        MatchCollection matchesWithRoot = rootRegex.Matches(text);

        foreach (Match match in matchesWithRoot)
        {
            Console.WriteLine($"Найдено: {match.Value}");
        }

        Console.WriteLine($"\nВсего найдено слов с корнем 'мор': {matchesWithRoot.Count}\n");

        // ==================== ПРОВЕРКА СООТВЕТСТВИЯ ФОРМАТУ ====================
        Console.WriteLine("6. ПРОВЕРКА СООТВЕТСТВИЯ СТРОК ФОРМАТУ:");
        Console.WriteLine(new string('-', 50));

        string[] testStrings =
        {
            "У лукоморья дуб зелёный;",
            "123-456-7890",
            "Там чудеса: там леший бродит",
            "user@example.com"
        };

        // Проверка на наличие цифр в строке
        // \d - любая цифра (0-9)
        string digitPattern = @"\d";
        foreach (string test in testStrings)
        {
            // IsMatch - возвращает true если в строке найдено хоть одно совпадение с шаблоном
            bool hasDigits = Regex.IsMatch(test, digitPattern);
            Console.WriteLine($"Строка: \"{test}\" -> содержит цифры: {(hasDigits ? "Да" : "Нет")}");
        }

        Console.WriteLine("\n========== КОНЕЦ ЛАБОРАТОРНОЙ РАБОТЫ ==========");
    }
}