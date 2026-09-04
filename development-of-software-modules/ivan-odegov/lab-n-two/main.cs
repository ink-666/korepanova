namespace TransportExample
{
    // Базовый класс (родительский)
    public class Vehicle
    {
        public string Brand { get; set; }
        public int Speed { get; set; }

        public Vehicle(string brand, int speed)
        {
            Brand = brand;
            Speed = speed;
        }

        // Виртуальный метод для демонстрации полиморфизма
        public virtual void DisplayInfo()
        {
            Console.WriteLine($"Транспорт: {Brand}, Скорость: {Speed} км/ч");
        }

        // Перегрузка метода Move (разные способы движения)
        public void Move()
        {
            // Перегрузка 1: без параметров (стандартное движение)
            Console.WriteLine($"{Brand} начинает движение.");
        }

        public void Move(string destination)
        {
            // Перегрузка 2: с указанием пункта назначения
            // Данные передаются через параметр destination
            Console.WriteLine($"{Brand} движется в {destination}.");
        }

        public void Move(int distance)
        {
            // Перегрузка 3: с указанием дистанции
            // Данные передаются через параметр distance
            Console.WriteLine($"{Brand} проедет {distance} км.");
        }
    }

    // Подкласс (дочерний класс) - Легковой автомобиль
    public class Car : Vehicle
    {
        public int DoorsCount { get; set; }

        public Car(string brand, int speed, int doors) : base(brand, speed)
        {
            // Данные для базового класса передаются через base(brand, speed)
            DoorsCount = doors;
        }

        public override void DisplayInfo()
        {
            // Переопределение метода базового класса
            Console.WriteLine($"Автомобиль: {Brand}, Скорость: {Speed} км/ч, Дверей: {DoorsCount}");
        }
    }

    // Подкласс - Мотоцикл
    public class Motorcycle : Vehicle
    {
        public bool HasSidecar { get; set; }

        public Motorcycle(string brand, int speed, bool sidecar) : base(brand, speed)
        {
            HasSidecar = sidecar;
        }

        public override void DisplayInfo()
        {
            string sidecarInfo = HasSidecar ? "с коляской" : "без коляски";
            Console.WriteLine($"Мотоцикл: {Brand}, Скорость: {Speed} км/ч, {sidecarInfo}");
        }
    }

    // Класс с обобщенным методом
    public class TransportManager
    {
        // Обобщенный метод (Generic method)
        // T - тип параметра, который будет определен при вызове
        public void PrintVehicleInfo<T>(T vehicle) where T : Vehicle
        {
            // Данные передаются через параметр vehicle типа T
            // Ограничение where T : Vehicle гарантирует, что T - это Vehicle или его наследник
            Console.WriteLine("\n--- Обобщенный метод выводит информацию ---");
            vehicle.DisplayInfo(); // Полиморфный вызов
        }

        // Перегрузка обобщенного метода для списка
        public void PrintVehicleInfo<T>(List<T> vehicles) where T : Vehicle
        {
            // Данные передаются через список vehicles
            Console.WriteLine("\n--- Обобщенный метод для списка ---");
            foreach (var v in vehicles)
            {
                v.DisplayInfo();
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Демонстрация работы классов ===\n");

            // Создаем объекты
            // Данные передаются через конструкторы
            Vehicle genericVehicle = new Vehicle("Обычный транспорт", 30);
            Car car = new Car("Toyota", 180, 4);
            Motorcycle bike = new Motorcycle("Harley-Davidson", 200, false);

            // Демонстрация перегрузки методов Move
            Console.WriteLine("--- Перегрузка методов Move ---");
            genericVehicle.Move();                    // Вызов без параметров
            genericVehicle.Move("Москва");            // Вызов с одним параметром (string)
            genericVehicle.Move(100);                  // Вызов с одним параметром (int)

            Console.WriteLine("\n--- Перегрузка методов Move для автомобиля ---");
            car.Move();                                // Вызов метода базового класса
            car.Move("Санкт-Петербург");
            car.Move(500);

            // Демонстрация работы подклассов и полиморфизма
            Console.WriteLine("\n--- Информация о транспорте ---");
            genericVehicle.DisplayInfo();  // Вызов метода базового класса
            car.DisplayInfo();              // Вызов переопределенного метода
            bike.DisplayInfo();             // Вызов переопределенного метода

            // Работа с обобщенным методом
            Console.WriteLine("\n--- Использование обобщенного метода ---");
            TransportManager manager = new TransportManager();

            // Вызов обобщенного метода с разными типами
            // Данные передаются из объектов car и bike
            manager.PrintVehicleInfo<Car>(car);              // Явное указание типа
            manager.PrintVehicleInfo(bike);                   // Неявное определение типа

            // Создаем список транспортных средств
            List<Vehicle> vehicles = new List<Vehicle>
            {
                // Данные передаются через конструкторы при создании объектов
                new Car("Lada", 150, 4),
                new Motorcycle("Suzuki", 170, true),
                genericVehicle  // Добавляем ранее созданный объект
            };

            // Вызов перегруженной версии обобщенного метода для списка
            // Данные передаются через список vehicles
            manager.PrintVehicleInfo(vehicles);

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}