using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace RegexLab
{
    // class — ссылочный тип, хранит методы и логику программы
    class Program
    {
        // static — метод вызывается без создания объекта, void — ничего не возвращает
        static void Main(string[] args)
        {
            Console.WriteLine("==========================================");
            Console.WriteLine("ЛАБОРАТОРНАЯ РАБОТА №6: РЕГУЛЯРНЫЕ ВЫРАЖЕНИЯ");
            Console.WriteLine("==========================================\n");

            Console.WriteLine("📁 ЧАСТЬ 1: ЗАГРУЗКА ТЕКСТА");
            Console.WriteLine("==========================================\n");

            // string — тип для хранения текстовых данных
            string filePath = "text.txt";

            // File.Exists — проверяет, существует ли файл по указанному пути
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"❌ Файл {filePath} не найден!");
                Console.WriteLine("Создайте файл text.txt с текстом для анализа.");
                return;
            }

            // File.ReadAllText — читает весь файл как одну строку
            string text = File.ReadAllText(filePath);
            // File.ReadAllLines — читает файл построчно в массив строк
            string[] lines = File.ReadAllLines(filePath);

            Console.WriteLine($"📄 Загружен текст из файла: {filePath}");
            Console.WriteLine($"📊 Всего строк: {lines.Length}");
            Console.WriteLine($"📊 Всего символов: {text.Length}");
            Console.WriteLine("\n📝 Первые 100 символов текста:");
            // Math.Min — если текст короче 100 символов, берёт сколько есть
            Console.WriteLine(text.Substring(0, Math.Min(100, text.Length)) + "...\n");

            Console.WriteLine("\n🔍 ЧАСТЬ 2: ПОДСЧЁТ КОЛИЧЕСТВА СЛОВ");
            Console.WriteLine("==========================================");

            // ё/Ё вынесены отдельно — они не входят в диапазоны а-я и А-Я в Unicode
            string wordPattern = @"\b[а-яА-ЯёЁa-zA-Z]+\b";

            // MatchCollection — коллекция всех совпадений, найденных Regex.Matches
            MatchCollection wordMatches = Regex.Matches(text, wordPattern);

            Console.WriteLine($"\n📌 Регулярное выражение: {wordPattern}");
            Console.WriteLine($"📊 Всего слов в тексте: {wordMatches.Count}");
            Console.WriteLine("\n📝 Первые 10 слов:");

            for (int i = 0; i < Math.Min(10, wordMatches.Count); i++)
                Console.WriteLine($"   {i + 1}. {wordMatches[i].Value}");

            Console.WriteLine("\n\n🔍 ЧАСТЬ 3: ПОИСК СЛОВОСОЧЕТАНИЙ");
            Console.WriteLine("==========================================");

            string phrasePattern = @"\bне\s+[а-яА-ЯёЁ]+\b";
            // RegexOptions.IgnoreCase — находит "Не" в начале предложений, не только "не"
            MatchCollection phraseMatches = Regex.Matches(text, phrasePattern, RegexOptions.IgnoreCase);

            Console.WriteLine($"\n📌 Поиск словосочетаний с 'не': {phrasePattern}");
            Console.WriteLine($"📊 Найдено: {phraseMatches.Count}");

            // Match — одно совпадение из коллекции, .Value содержит найденный текст
            foreach (Match match in phraseMatches)
                Console.WriteLine($"   ➡️ {match.Value}");

            string andPattern = @"\b[а-яА-ЯёЁ]+\s+и\s+[а-яА-ЯёЁ]+\b";
            MatchCollection andMatches = Regex.Matches(text, andPattern, RegexOptions.IgnoreCase);

            Console.WriteLine($"\n📌 Поиск сочетаний 'слово и слово': {andPattern}");
            Console.WriteLine($"📊 Найдено: {andMatches.Count}");

            foreach (Match match in andMatches)
                Console.WriteLine($"   ➡️ {match.Value}");

            Console.WriteLine("\n\n🔍 ЧАСТЬ 4: ПОИСК ОПРЕДЕЛЁННЫХ СИМВОЛОВ");
            Console.WriteLine("==========================================");

            // Дефис стоит последним в классе [...], иначе трактуется как диапазон
            string punctuationPattern = @"[.,;:!?-]";
            MatchCollection punctuationMatches = Regex.Matches(text, punctuationPattern);

            Console.WriteLine($"\n📌 Знаки препинания: {punctuationPattern}");
            Console.WriteLine($"📊 Всего знаков препинания: {punctuationMatches.Count}");

            // ё/Ё перечислены явно — они вне диапазона а-я в Unicode
            string vowelsPattern = @"[аеёиоуыэюяАЕЁИОУЫЭЮЯ]";
            MatchCollection vowelsMatches = Regex.Matches(text, vowelsPattern);

            Console.WriteLine($"\n📌 Гласные буквы: {vowelsPattern}");
            Console.WriteLine($"📊 Всего гласных: {vowelsMatches.Count}");

            string consonantsPattern = @"[бвгджзйклмнпрстфхцчшщБВГДЖЗЙКЛМНПРСТФХЦЧШЩ]";
            MatchCollection consonantsMatches = Regex.Matches(text, consonantsPattern);

            Console.WriteLine($"\n📌 Согласные буквы: {consonantsPattern}");
            Console.WriteLine($"📊 Всего согласных: {consonantsMatches.Count}");

            Console.WriteLine("\n\n🔍 ЧАСТЬ 5: СТРОКИ, НАЧИНАЮЩИЕСЯ С 'Мой'");
            Console.WriteLine("==========================================");

            // ^ без флага Multiline работает на начало каждой строки в lines[], а не всего текста
            string startPattern = @"^Мой.*";

            Console.WriteLine($"📌 Регулярное выражение: {startPattern}\n");

            // int — целочисленный тип для счётчика
            int foundStartLines = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                // Regex.IsMatch — возвращает true, если строка соответствует шаблону
                if (Regex.IsMatch(lines[i], startPattern, RegexOptions.IgnoreCase))
                {
                    Console.WriteLine($"   Строка {i + 1}: {lines[i]}");
                    foundStartLines++;
                }
            }

            if (foundStartLines == 0)
                Console.WriteLine("   Строк, начинающихся с 'Мой', не найдено.");

            Console.WriteLine("\n\n🔍 ЧАСТЬ 6: СТРОКИ, ОКАНЧИВАЮЩИЕСЯ НА ';' ИЛИ ','");
            Console.WriteLine("==========================================");

            string endPattern = @"[;,]";

            Console.WriteLine($"📌 Регулярное выражение: {endPattern}$\n");

            int foundEndLines = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                // Trim() — убирает пробелы и \r в конце строки, иначе $ не сработает
                if (Regex.IsMatch(lines[i].Trim(), endPattern + "$"))
                {
                    Console.WriteLine($"   Строка {i + 1}: {lines[i]}");
                    foundEndLines++;
                }
            }

            if (foundEndLines == 0)
                Console.WriteLine("   Строк, оканчивающихся на ';' или ',', не найдено.");

            Console.WriteLine("\n\n✏️ ЧАСТЬ 7: ЗАМЕНА ЧАСТИ ТЕКСТА");
            Console.WriteLine("==========================================");

            // \b с обеих сторон — замена не затронет "нет", "нельзя", "нечто"
            string oldPattern = @"\bне\b";
            string replacement = "НИ";

            // Regex.Replace — заменяет все совпадения в тексте на указанную строку
            string modifiedText = Regex.Replace(text, oldPattern, replacement, RegexOptions.IgnoreCase);

            Console.WriteLine($"📌 Замена: '{oldPattern}' → '{replacement}'\n");
            Console.WriteLine("📝 Текст после замены (первые 200 символов):");
            Console.WriteLine(modifiedText.Substring(0, Math.Min(200, modifiedText.Length)) + "...");

            Console.WriteLine("\n\n✨ ЧАСТЬ 8: ДОПОЛНИТЕЛЬНЫЕ ПРИМЕРЫ");
            Console.WriteLine("==========================================");

            // [А-ЯЁ] — первая заглавная буква, [а-яё]* — остальные строчные
            string capitalPattern = @"\b[А-ЯЁ][а-яё]*\b";
            MatchCollection capitalMatches = Regex.Matches(text, capitalPattern);

            Console.WriteLine($"\n📌 Слова с большой буквы: {capitalPattern}");
            Console.WriteLine($"📊 Найдено: {capitalMatches.Count}");

            for (int i = 0; i < Math.Min(5, capitalMatches.Count); i++)
                Console.WriteLine($"   {i + 1}. {capitalMatches[i].Value}");

            // {5} — ровно 5 символов; \b по краям исключает слова длиннее 5 букв
            string fiveLetterPattern = @"\b[а-яА-ЯёЁ]{5}\b";
            MatchCollection fiveLetterMatches = Regex.Matches(text, fiveLetterPattern);

            Console.WriteLine($"\n📌 Слова из 5 букв: {fiveLetterPattern}");
            Console.WriteLine($"📊 Найдено: {fiveLetterMatches.Count}");

            foreach (Match match in fiveLetterMatches)
                Console.WriteLine($"   ➡️ {match.Value}");

            // PadRight — дополняет строку символом '=' до длины 60
            Console.WriteLine("\n" + "=".PadRight(60, '='));
            Console.WriteLine("📊 ИТОГОВАЯ СТАТИСТИКА");
            Console.WriteLine("=".PadRight(60, '='));

            Console.WriteLine($"\n📄 Всего строк в тексте: {lines.Length}");
            Console.WriteLine($"📝 Всего символов: {text.Length}");
            Console.WriteLine($"🔤 Всего слов: {wordMatches.Count}");
            Console.WriteLine($"🔣 Знаков препинания: {punctuationMatches.Count}");
            Console.WriteLine($"🅰️ Гласных букв: {vowelsMatches.Count}");
            Console.WriteLine($"🅱️ Согласных букв: {consonantsMatches.Count}");

            Console.WriteLine("\n" + "=".PadRight(60, '='));
            Console.WriteLine("✅ ПРОГРАММА ЗАВЕРШЕНА");
            Console.WriteLine("=".PadRight(60, '='));
        }
    }
}