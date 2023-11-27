using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab10_SQL_ORM
{
    internal class Menu
    {
        public static int ShowMenu(IEnumerable<string> menuOptions, string title, string optionAD, string optionX)
        {

            // menu with options in the list/array
            // you can choose one of the options in the list and get the position back
            // or press A and get max positions +1
            // or press D and get max positions +2
            // or press X and get max positions +3
            Console.Clear();

            Console.WriteLine(title);

            foreach(string option in menuOptions)
            {
                Console.WriteLine($"   {option}");
            }

            Console.CursorVisible = false;
            int startTop = 1;
            int top = startTop;

            int listLength = menuOptions.Count();
            int choice = 0; //börjar med 0 som listor
            
            // moving around the arrow and getting the choice
            while (true)
            {
                Console.SetCursorPosition(0, top);
                Console.WriteLine("=> ");

                ConsoleKeyInfo keyInfo = Console.ReadKey();

                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        choice = choice > 0 ? choice - 1 : listLength - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        choice = choice < listLength - 1 ? choice + 1 : 0;
                        break;
                    case ConsoleKey.Enter:
                        return choice;
                    case ConsoleKey.A:
                        return listLength;
                    case ConsoleKey.D:
                        return listLength + 1;
                    case ConsoleKey.X:
                        return listLength + 2;
                    default:
                        break;
                }
                Console.SetCursorPosition(0, top);
                Console.WriteLine("   ");
                top = startTop + choice;

            }
        }
    }
}
