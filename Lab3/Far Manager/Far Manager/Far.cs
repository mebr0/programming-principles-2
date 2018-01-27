﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Far_Manager
{
    enum FarMode //enum for Far modes
    {
        Explorer,
        FileReader
    }
    class Far
    {
        Stack<Layer> History = new Stack<Layer>(); //history of directories
        Layer current;                             //current location
        FarMode mode = FarMode.Explorer;           //mode for Far

        public Far(string path)
        {
            this.current = new Layer(path, 0);
        }

        public void Draw() //main function for drawing
        {
            switch (mode)
            {
                case FarMode.Explorer:
                    DrawExplorer();
                    break;
                case FarMode.FileReader:
                    DrawFileReader();
                    break;
            }
            DrawStatus();
            Console.SetCursorPosition(0, current.items.Count);
        }

        void DrawExplorer() //Draw in Explorer Mode
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Clear();

            for(int i=0; i<current.items.Count; ++i)
            {
                if(i == current.ind && current.items[i].GetType() == typeof(DirectoryInfo))
                {
                    Console.BackgroundColor = ConsoleColor.Cyan;
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                }
                else if (i == current.ind && current.items[i].GetType() == typeof(FileInfo))
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                }
                else if (i != current.ind && current.items[i].GetType() == typeof(DirectoryInfo))
                {
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.ForegroundColor = ConsoleColor.Cyan;
                }
                else if (i != current.ind && current.items[i].GetType() == typeof(FileInfo))
                {
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                Console.WriteLine(current.items[i].Name);
            }
        }

        void DrawFileReader() //Draw in FileReader Mode
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Clear();
            FileStream fs = null;
            StreamReader sr = null;
            try //try to open
            {
                fs = new FileStream(current.GetSelectedItemInfo(), FileMode.Open, FileAccess.Read);
                sr = new StreamReader(fs);

                Console.WriteLine(sr.ReadToEnd());
            }
            catch (Exception e) //if not, write an error
            {
                Console.WriteLine("cannot open file!");
            }
            finally //close files if open
            {
                if (sr != null)
                {
                    sr.Close();
                }
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }

        void DrawStatus()
        {
            Console.SetCursorPosition(32, 1);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" ");
            if (mode == FarMode.Explorer) Console.Write("dir");
            else Console.Write("file");
            Console.WriteLine("      ");
        }

        public void Process(ConsoleKeyInfo pressed)
        {
            switch (pressed.Key)
            {
                case ConsoleKey.UpArrow:   //ind up
                    current.Process(-1);
                    break;
                case ConsoleKey.DownArrow: //ind down
                    current.Process(1);
                    break;
                case ConsoleKey.Enter:
                    try
                    {
                        if (current.items[current.ind].GetType() == typeof(DirectoryInfo)) //open directory
                        {
                            mode = FarMode.Explorer;
                            History.Push(current);
                            current = new Layer(current.GetSelectedItemInfo(), 0);
                        }
                        else                                                               //open file
                        {
                            mode = FarMode.FileReader;
                        }
                    }
                    catch(Exception e)
                    {
                        current = History.Pop();
                    }
                    break;
                case ConsoleKey.Backspace:                           
                    if (mode == FarMode.Explorer) current = History.Pop();             //back to directory
                    else mode = FarMode.Explorer;                                      //back to directory from file
                    break;
            }
        }
    }
}
