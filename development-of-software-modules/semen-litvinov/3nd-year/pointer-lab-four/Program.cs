using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace SnakeGame
{
    // ------------------------------------------------------------------
    //  Узел двусвязного списка, представляющего тело змейки.
    //  Структура неуправляемая (unmanaged) — это позволяет размещать её
    //  в "сырой" памяти через Marshal.AllocHGlobal и работать с ней
    //  исключительно через указатели, без участия сборщика мусора.
    // ------------------------------------------------------------------
    internal unsafe struct SnakeNode
    {
        public int X;
        public int Y;
        public SnakeNode* Prev;
        public SnakeNode* Next;
    }

    // ------------------------------------------------------------------
    //  Еда — тоже хранится в динамической памяти и доступна только
    //  через указатель Food*.
    // ------------------------------------------------------------------
    internal struct Food
    {
        public int X;
        public int Y;
    }

    internal enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    // ------------------------------------------------------------------
    //  Класс "Змейка" — реализует двусвязный список узлов SnakeNode
    //  вручную, через указатели (без List<T>, LinkedList<T> и т.п.).
    // ------------------------------------------------------------------
    internal unsafe class Snake
    {
        public SnakeNode* Head;
        public SnakeNode* Tail;
        public int Length { get; private set; }

        public Snake(int startX, int startY)
        {
            Head = Tail = AllocateNode(startX, startY);
            Length = 1;
        }

        // Выделение памяти под новый узел напрямую в куче процесса.
        private static SnakeNode* AllocateNode(int x, int y)
        {
            SnakeNode* node = (SnakeNode*)Marshal.AllocHGlobal(sizeof(SnakeNode));
            node->X = x;
            node->Y = y;
            node->Prev = null;
            node->Next = null;
            return node;
        }

        // Добавление новой головы (двусвязный список: Head <-> ... <-> Tail).
        public void AddHead(int x, int y)
        {
            SnakeNode* node = AllocateNode(x, y);
            node->Next = Head;
            Head->Prev = node;
            Head = node;
            Length++;
        }

        // Удаление хвоста с освобождением динамической памяти.
        public void RemoveTail()
        {
            if (Tail == null) return;

            SnakeNode* prev = Tail->Prev;
            Marshal.FreeHGlobal((IntPtr)Tail);

            Tail = prev;
            if (Tail != null)
                Tail->Next = null;
            else
                Head = null;

            Length--;
        }

        // Проверка столкновения точки (x, y) с телом змейки.
        // skipHead = true позволяет не учитывать саму голову при проверке.
        public bool Collides(int x, int y, bool skipHead = false)
        {
            SnakeNode* current = Head;
            if (skipHead && current != null) current = current->Next;

            while (current != null)
            {
                if (current->X == x && current->Y == y)
                    return true;
                current = current->Next;
            }
            return false;
        }

        // Полное освобождение всех узлов списка (вызывается при выходе из игры).
        public void FreeAll()
        {
            SnakeNode* current = Head;
            while (current != null)
            {
                SnakeNode* next = current->Next;
                Marshal.FreeHGlobal((IntPtr)current);
                current = next;
            }
            Head = Tail = null;
            Length = 0;
        }
    }

    internal static unsafe class Program
    {
        private const int Width = 30;   // ширина игрового поля (в символах)
        private const int Height = 20;  // высота игрового поля (в символах)
        private const int TickMs = 130; // задержка между кадрами, мс

        private static void Main()
        {
            Console.CursorVisible = false;
            Console.Title = "Snake (указатели, двусвязный список)";
            // Полная очистка экрана один раз в начале.
            // Дальше используем ANSI "курсор домой" вместо SetCursorPosition,
            // т.к. на macOS/Linux SetCursorPosition работает по координатам
            // буфера, а не видимой области, и после прокрутки терминала
            // "уезжает" — из-за этого кадры не перезаписываются, а копятся.
            Console.Write("\u001b[2J");

            var snake = new Snake(Width / 2, Height / 2);
            Direction direction = Direction.Right;

            Food* food = (Food*)Marshal.AllocHGlobal(sizeof(Food));
            SpawnFood(food, snake);

            int score = 0;
            bool gameOver = false;

            try
            {
                while (!gameOver)
                {
                    // ---- неблокирующее чтение клавиатуры ----
                    if (Console.KeyAvailable)
                    {
                        ConsoleKey key = Console.ReadKey(intercept: true).Key;
                        direction = UpdateDirection(direction, key);
                    }

                    // ---- вычисление новой позиции головы ----
                    int newX = snake.Head->X;
                    int newY = snake.Head->Y;
                    switch (direction)
                    {
                        case Direction.Up: newY--; break;
                        case Direction.Down: newY++; break;
                        case Direction.Left: newX--; break;
                        case Direction.Right: newX++; break;
                    }

                    // ---- проверка столкновения со стеной ----
                    if (newX < 0 || newX >= Width || newY < 0 || newY >= Height)
                    {
                        gameOver = true;
                        break;
                    }

                    bool willEat = (newX == food->X && newY == food->Y);

                    // ---- проверка столкновения с собственным телом ----
                    // Если еда не съедена, хвост в этом кадре уйдёт,
                    // поэтому саму последнюю клетку хвоста не считаем столкновением.
                    bool bodyToCheck = snake.Collides(newX, newY);
                    if (bodyToCheck && !(!willEat && newX == snake.Tail->X && newY == snake.Tail->Y && snake.Length > 1))
                    {
                        gameOver = true;
                        break;
                    }

                    // ---- перемещение змейки ----
                    snake.AddHead(newX, newY);
                    if (willEat)
                    {
                        score++;
                        SpawnFood(food, snake);
                    }
                    else
                    {
                        snake.RemoveTail();
                    }

                    Render(snake, food, score);
                    Thread.Sleep(TickMs);
                }
            }
            finally
            {
                snake.FreeAll();
                Marshal.FreeHGlobal((IntPtr)food);
            }

            Console.SetCursorPosition(0, Height + 2);
            Console.WriteLine($"Игра окончена! Счёт: {score}");
            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey(true);
        }

        // Запрещаем разворот змейки на 180 градусов "в лоб".
        private static Direction UpdateDirection(Direction current, ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.UpArrow when current != Direction.Down:
                    return Direction.Up;
                case ConsoleKey.DownArrow when current != Direction.Up:
                    return Direction.Down;
                case ConsoleKey.LeftArrow when current != Direction.Right:
                    return Direction.Left;
                case ConsoleKey.RightArrow when current != Direction.Left:
                    return Direction.Right;
                default:
                    return current;
            }
        }

        // Размещение еды на случайной свободной клетке поля.
        private static readonly Random Rng = new Random();

        private static void SpawnFood(Food* food, Snake snake)
        {
            int x, y;
            do
            {
                x = Rng.Next(0, Width);
                y = Rng.Next(0, Height);
            } while (snake.Collides(x, y));

            food->X = x;
            food->Y = y;
        }

        // Отрисовка поля в консоли (перерисовка каждый кадр).
        private static void Render(Snake snake, Food* food, int score)
        {
            char[,] buffer = new char[Height, Width];
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    buffer[y, x] = '.';

            buffer[food->Y, food->X] = '*';

            SnakeNode* current = snake.Head;
            bool isHead = true;
            while (current != null)
            {
                buffer[current->Y, current->X] = isHead ? 'O' : 'o';
                isHead = false;
                current = current->Next;
            }

            var sb = new System.Text.StringBuilder();
            sb.Append("\u001b[H"); // курсор в левый верхний угол видимой области (ANSI)
            sb.Append('#', Width + 2).Append('\n');
            for (int y = 0; y < Height; y++)
            {
                sb.Append('#');
                for (int x = 0; x < Width; x++)
                    sb.Append(buffer[y, x]);
                sb.Append('#').Append('\n');
            }
            sb.Append('#', Width + 2).Append('\n');
            // PadRight гарантирует фиксированную ширину строки статуса,
            // чтобы более короткая строка полностью затирала предыдущую,
            // более длинную (иначе остаются "хвосты" старого текста).
            string status = $"Счёт: {score}".PadRight(Width + 2 + 30);
            sb.Append(status);

            Console.Write(sb.ToString());
        }
    }
}
