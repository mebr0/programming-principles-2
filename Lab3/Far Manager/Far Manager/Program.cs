﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Far_Manager
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(40, 40);
            Far FAR = new Far(@"C:\Users\Lenovo\Desktop\Study\1 semestr\ICT"); //root directory
            bool q = false;            //boolean for quiting

            while (!q) //repeat until pressed Escape
            {
                FAR.Draw();
                ConsoleKeyInfo pressed = Console.ReadKey();

                switch (pressed.Key)
                {
                    case ConsoleKey.Escape:
                        q = true;
                        break;
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.Enter:
                    case ConsoleKey.Backspace:
                        FAR.Process(pressed); //command to Far
                        break;
                }
            }
        }
    }
}