namespace TransportExample
{
    // Структура для хранения характеристик транспорта
    public struct VehicleSpec
    {
        public int Weight;
        public int Year;

        public void PrintSpec()
        {
            Console.WriteLine($"Вес: {Weight} кг, Год выпуска: {Year}");
        }
    }

    // Базовый класс
    public class Vehicle
    {
        public string Brand { get; set; }
        public int Speed { get; set; }

        // Используем структуру внутри класса
        public VehicleSpec Spec;

        public Vehicle(string brand, int speed, VehicleSpec spec)
        {
            Brand = brand;
            Speed = speed;
            Spec = spec;
        }

        public virtual void DisplayInfo()
        {
            Console.WriteLine($"Транспорт: {Brand}, Скорость: {Speed} км/ч");
            Spec.PrintSpec();
        }

        // Перегрузка методов
        public void Move()
        {
            Console.WriteLine($"{Brand} начинает движение.");
        }

        public void Move(string destination)
        {
            Console.WriteLine($"{Brand} движется в {destination}.");
        }

        public void Move(int distance)
        {
            Console.WriteLine($"{Brand} проедет {distance} км.");
        }
    }

    public class Car : Vehicle
    {
        public int DoorsCount { get; set; }

        public Car(string brand, int speed, int doors, VehicleSpec spec)
            : base(brand, speed, spec)
        {
            DoorsCount = doors;
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"Автомобиль: {Brand}, Скорость: {Speed} км/ч, Дверей: {DoorsCount}");
            Spec.PrintSpec();
        }
    }

    public class Motorcycle : Vehicle
    {
        public bool HasSidecar { get; set; }

        public Motorcycle(string brand, int speed, bool sidecar, VehicleSpec spec)
            : base(brand, speed, spec)
        {
            HasSidecar = sidecar;
        }

        public override void DisplayInfo()
        {
            string sidecarInfo = HasSidecar ? "с коляской" : "без коляски";
            Console.WriteLine($"Мотоцикл: {Brand}, Скорость: {Speed} км/ч, {sidecarInfo}");
            Spec.PrintSpec();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Демонстрация работы ===\n");

            // создаём структуры
            VehicleSpec carSpec = new VehicleSpec { Weight = 1500, Year = 2022 };
            VehicleSpec bikeSpec = new VehicleSpec { Weight = 300, Year = 2021 };

            Vehicle vehicle = new Vehicle("Обычный транспорт", 30, carSpec);
            Car car = new Car("Toyota", 180, 4, carSpec);
            Motorcycle bike = new Motorcycle("Harley-Davidson", 200, false, bikeSpec);

            Console.WriteLine("--- Перегрузка методов ---");
            vehicle.Move();
            vehicle.Move("Москва");
            vehicle.Move(100);

            Console.WriteLine("\n--- Информация ---");
            vehicle.DisplayInfo();
            car.DisplayInfo();
            bike.DisplayInfo();

            Console.ReadKey();
        }
    }
}