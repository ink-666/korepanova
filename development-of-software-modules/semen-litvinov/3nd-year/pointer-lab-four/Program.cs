using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace SnakeGame
{
    // Узел двусвязного списка (тело змейки). Размещается в "сырой" памяти
    // через Marshal.AllocHGlobal и управляется только через указатели.
    internal unsafe struct SnakeNode
    {
        public int X;
        public int Y;
        public SnakeNode* Prev;
        public SnakeNode* Next;
    }

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

    // Двусвязный список узлов SnakeNode, реализованный вручную через
    // указатели (без List<T>/LinkedList<T>).
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

        private static SnakeNode* AllocateNode(int x, int y)
        {
            SnakeNode* node = (SnakeNode*)Marshal.AllocHGlobal(sizeof(SnakeNode));
            node->X = x;
            node->Y = y;
            node->Prev = null;
            node->Next = null;
            return node;
        }

        public void AddHead(int x, int y)
        {
            SnakeNode* node = AllocateNode(x, y);
            node->Next = Head;
            Head->Prev = node;
            Head = node;
            Length++;
        }

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

        // skipHead = true исключает саму голову из проверки.
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
        private const int Width = 30;
        private const int Height = 20;
        private const int TickMs = 130;

        private static void Main()
        {
            Console.CursorVisible = false;
            Console.Title = "Snake (указатели, двусвязный список)";
            // ANSI-очистка экрана: SetCursorPosition на Unix "уезжает" после
            // прокрутки терминала, из-за этого кадры не перезаписываются.
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
                    if (Console.KeyAvailable)
                    {
                        ConsoleKey key = Console.ReadKey(intercept: true).Key;
                        direction = UpdateDirection(direction, key);
                    }

                    int newX = snake.Head->X;
                    int newY = snake.Head->Y;
                    switch (direction)
                    {
                        case Direction.Up: newY--; break;
                        case Direction.Down: newY++; break;
                        case Direction.Left: newX--; break;
                        case Direction.Right: newX++; break;
                    }

                    if (newX < 0 || newX >= Width || newY < 0 || newY >= Height)
                    {
                        gameOver = true;
                        break;
                    }

                    bool willEat = (newX == food->X && newY == food->Y);

                    // Если еда не съедена, хвост в этом кадре уйдёт — не считаем
                    // столкновением попадание головы в текущую клетку хвоста.
                    bool bodyToCheck = snake.Collides(newX, newY);
                    if (bodyToCheck && !(!willEat && newX == snake.Tail->X && newY == snake.Tail->Y && snake.Length > 1))
                    {
                        gameOver = true;
                        break;
                    }

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

        // Запрещает разворот на 180 градусов.
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
            sb.Append("\u001b[H"); // ANSI: курсор в левый верхний угол видимой области
            sb.Append('#', Width + 2).Append('\n');
            for (int y = 0; y < Height; y++)
            {
                sb.Append('#');
                for (int x = 0; x < Width; x++)
                    sb.Append(buffer[y, x]);
                sb.Append('#').Append('\n');
            }
            sb.Append('#', Width + 2).Append('\n');
            // PadRight — чтобы короткая строка затирала более длинную предыдущую.
            string status = $"Счёт: {score}".PadRight(Width + 2 + 30);
            sb.Append(status);

            Console.Write(sb.ToString());
        }
    }
}