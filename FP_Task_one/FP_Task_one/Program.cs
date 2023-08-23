using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FP_Task_one
{
    internal class Program
    {

        static private uint _floors;
        static private uint _entrances;
        static private uint _room;
        static String[] flats = new String[] { "Ближняя слева", " Дальняя слева", "Дальняя справа", "Ближняя справа" };

        static void Main(string[] args)
        {
            bool isValid = false;
            bool isRoomExists = false;

            do
            {
                Console.Write("Введите кол-во этажей: ");
                isValid = UInt32.TryParse(Console.ReadLine(), out _floors);

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
                isValid = UInt32.TryParse(Console.ReadLine(), out _entrances);

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
                isValid = UInt32.TryParse(Console.ReadLine(), out _room);

                if (isValid && _room > 0)
                {
                    break;
                }
                Console.WriteLine("Неккоректный ввод, введите номер квартиры, используя только цифры начиная с 1.");
                isValid = false;

            } while (!isValid);

            isRoomExists = _room > _floors * _entrances * 4 ? false : true;

            if (!isRoomExists)
            {
                Console.Write("Такой квартиры не существует в этом доме.");
                Console.ReadKey();
                return;
            }

            int room = (int)_room;
            uint floor = 0, entrance = 0;

            while (room > 0)
            {
                room -= (int)(4 * _entrances);
                entrance++;
            }

            room = (int)_room;

            while (room > 0)
            {
                room -= 4;
                floor++;
            }

            floor -= (entrance - 1) * _floors;

            Console.WriteLine($"{flats[room + 3]} квартира находится в {entrance} подъезде, на {floor} этаже");
            Console.ReadKey();
        }
    }
}
