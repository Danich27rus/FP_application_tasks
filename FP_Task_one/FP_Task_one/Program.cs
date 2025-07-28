using System;

namespace FP_Task_one
{
    internal class Program
    {
        static private int _floors;
        static private int _entrances;
        static private int _room;

        static void Main(string[] args)
        {
            var isValid = false;

            do
            {
                Console.Write("Введите кол-во этажей: ");
                isValid = Int32.TryParse(Console.ReadLine(), out _floors);

                if (isValid && _floors > 0)
                {
                    break;
                }
                Console.WriteLine("Неккоректный ввод, введите кол-во этажей, используя только цифры начиная с 1.");
                isValid = false;

            } while (!isValid);

            do
            {
                Console.Write("Введите кол-во подъездов: ");
                isValid = Int32.TryParse(Console.ReadLine(), out _entrances);

                if (isValid && _entrances > 0)
                {
                    break;
                }
                Console.WriteLine("Неккоректный ввод, введите кол-во подъездов, используя только цифры начиная с 1.");
                isValid = false;

            } while (!isValid);

            do
            {
                Console.Write("Введите номер квартиры: ");
                isValid = Int32.TryParse(Console.ReadLine(), out _room);

                if (isValid && _room > 0)
                {
                    break;
                }
                Console.WriteLine("Неккоректный ввод, введите номер квартиры, используя только цифры начиная с 1.");
                isValid = false;

            } while (!isValid);

            int apartmentsPerFloor = 4;
            int floorsPerEntrance = _floors;
            int apartmentsPerEntrance = floorsPerEntrance * apartmentsPerFloor;
            int totalApartments = apartmentsPerEntrance * _entrances;

            if (_room < 1 || _room > totalApartments)
            {
                Console.WriteLine("Квартиры с таким номером нет в доме.");
                return;
            }

            int entrance = (_room - 1) / apartmentsPerEntrance + 1;
            int localApartmentNumber = (_room - 1) % apartmentsPerEntrance;

            int floor = localApartmentNumber / apartmentsPerFloor + 1;

            string[] positions = { "ближняя слева (1)", "дальняя слева (2)", "дальняя справа (3)", "ближняя справа (4)" };
            int positionIndex = localApartmentNumber % apartmentsPerFloor;

            Console.WriteLine($"Квартира №{_room}:");
            Console.WriteLine($"Подъезд: {entrance}");
            Console.WriteLine($"Этаж: {floor}");
            Console.WriteLine($"Расположение: {positions[positionIndex]}");
        }
    }
}
