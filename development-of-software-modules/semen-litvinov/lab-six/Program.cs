using System;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        // Исходный текст
        string text = @"
Программирование делает жизнь проще.
Регулярные выражения помогают искать текст.
Работа со строками очень важна.
Программисты часто используют Regex.
Регулярные выражения экономят время.
";

        Console.WriteLine("Исходный текст:");
        Console.WriteLine(text);

        // -------------------------------------------------
        // 1. Подсчет количества символов
        // -------------------------------------------------

        // Считаем количество букв 'о'
        Regex symbolRegex = new Regex(@"о", RegexOptions.IgnoreCase);

        MatchCollection symbolMatches = symbolRegex.Matches(text);

        Console.WriteLine($"Количество букв 'о': {symbolMatches.Count}");

        // -------------------------------------------------
        // 2. Подсчет количества слов
        // -------------------------------------------------

        // \b\w+\b — поиск отдельных слов
        Regex wordRegex = new Regex(@"\b\w+\b");

        MatchCollection wordMatches = wordRegex.Matches(text);

        Console.WriteLine($"Количество слов: {wordMatches.Count}");

        // -------------------------------------------------
        // 3. Подсчет словосочетания
        // -------------------------------------------------

        // Ищем словосочетание "Регулярные выражения"
        Regex phraseRegex = new Regex(@"Регулярные выражения",
            RegexOptions.IgnoreCase);

        MatchCollection phraseMatches = phraseRegex.Matches(text);

        Console.WriteLine(
            $"Количество словосочетаний 'Регулярные выражения': " +
            $"{phraseMatches.Count}");

        // -------------------------------------------------
        // 4. Строки, начинающиеся с определенного слова
        // -------------------------------------------------

        Console.WriteLine("\nСтроки, начинающиеся со слова 'Регулярные':");

        // ^ — начало строки
        Regex startRegex = new Regex(
            @"^Регулярные.*",
            RegexOptions.Multiline);

        MatchCollection startMatches = startRegex.Matches(text);

        foreach (Match match in startMatches)
        {
            Console.WriteLine(match.Value);
        }

        // -------------------------------------------------
        // 5. Строки, оканчивающиеся определенным словом
        // -------------------------------------------------

        Console.WriteLine("\nСтроки, оканчивающиеся словом 'время.':");

        // $ — конец строки
        Regex endRegex = new Regex(
            @".*время\.$",
            RegexOptions.Multiline);

        MatchCollection endMatches = endRegex.Matches(text);

        foreach (Match match in endMatches)
        {
            Console.WriteLine(match.Value);
        }

        // -------------------------------------------------
        // 6. Замена части текста
        // -------------------------------------------------

        Console.WriteLine("\nТекст после замены:");

        // Заменяем слово Regex на C#
        Regex replaceRegex = new Regex(@"Regex");

        string newText = replaceRegex.Replace(text, "C#");

        Console.WriteLine(newText);
    }
}