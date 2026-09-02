using System.Text.RegularExpressions;

string text = @"Белеет парус одинокий
В тумане моря голубом!
Что ищет он в стране далёкой?
Что кинул он в краю родном?
Играют волны — ветер свищет,
И мачта гнётся и скрипит...
Увы! он счастия не ищет
И не от счастия бежит!
Под ним струя светлей лазури,
Над ним луч солнца золотой...
А он, мятежный, просит бури,
Как будто в буре есть покой!";

Console.WriteLine("=== Исходный текст ===");
Console.WriteLine(text);

// 1. Подсчёт символов и слов
Console.WriteLine("\n=== Подсчёт ===");
int letterCount = Regex.Matches(text, @"[а-яёА-ЯЁ]").Count;
int wordCount = Regex.Matches(text, @"\b[а-яёА-ЯЁ]+\b").Count;
int heCount = Regex.Matches(text, @"\bон\b", RegexOptions.IgnoreCase).Count;

Console.WriteLine($"Количество букв: {letterCount}");
Console.WriteLine($"Количество слов: {wordCount}");
Console.WriteLine($"Вхождений слова 'он': {heCount}");

// 2. Строки, начинающиеся с "И"
Console.WriteLine("\n=== Строки, начинающиеся с 'И' ===");
var startsWithI = Regex.Matches(text, @"^И.+$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
foreach (Match m in startsWithI)
    Console.WriteLine(m.Value);

// 3. Строки, оканчивающиеся на "!"
Console.WriteLine("\n=== Строки, оканчивающиеся на '!' ===");
var endsWithExcl = Regex.Matches(text, @"^.+!$", RegexOptions.Multiline);
foreach (Match m in endsWithExcl)
    Console.WriteLine(m.Value);

// 4. Замена части текста
Console.WriteLine("\n=== После замены 'он' на 'ОН' ===");
string modified = Regex.Replace(text, @"\bон\b", "ОН", RegexOptions.IgnoreCase);
Console.WriteLine(modified);