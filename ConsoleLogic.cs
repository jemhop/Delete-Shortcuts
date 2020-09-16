using System;
using System.IO;
using System.Drawing;
using Console = Colorful.Console;
using System.Collections.Generic;
using System.Threading;

namespace DeleteShortcuts
{
    class ConsoleLogic
    {
        bool searchSub;
        bool firstStart = true;
        public void Print()
        {
            if (firstStart == true)
            {
                DisplayLogo();
            }
            firstStart = false;
            Console.WriteLine(@"Enter directory to clear of shortcuts    (e.g C:\Users\User\Desktop)");
            string input = Console.ReadLine();

            bool isValid = ValidateInputs(input);
            if (!isValid)
            {
                Console.WriteLine("Invalid value\n");
                Print();
            }
            else
            {
                Console.WriteLine("Search all subdirectories? (Y/N)");
                string answer = Console.ReadLine();
                if (answer == "Y" || answer == "Yes")
                {
                    searchSub = true;

                }
                else if (answer == "N" || answer == "N")
                {
                    searchSub = false;
                }

                if (input == "exit")
                {
                    Environment.Exit(0);
                }
                GetFiles(input); 
            }

        }
        void DisplayLogo()
        {
            Thread.Sleep(500);
            Console.WriteLine(@" _____       _      _             _____ _                _             _", Color.White);
            Console.WriteLine(@"|  __ \     | |    | |           / ____| |              | |           | |   ", Color.FromArgb(255, 219, 243));
            Console.Beep(987, 500);
            Console.WriteLine(@"| |  | | ___| | ___| |_ ___     | (___ | |__   ___  _ __| |_ ___ _   _| |_ __", Color.FromArgb(255, 183, 231));
            Console.WriteLine(@"| |  | |/ _ \ |/ _ \ __/ _ \     \___ \| '_ \ / _ \| '__| __/ __| | | | __/ __|", Color.FromArgb(255, 148, 219));
            Console.Beep(659, 300);
            Console.WriteLine(@"| |__| |  __/ |  __/ ||  __/     ____) | | | | (_) | |  | || (__| |_| | |_\__ \", Color.FromArgb(255, 112, 207));
            Console.WriteLine(@"|_____/ \___|_|\___|\__\___|    |_____/|_| |_|\___/|_|   \__\___|\__,_|\__|___/", Color.FromArgb(255, 77, 195));
            Console.Beep(783, 300);
            Thread.Sleep(500);

        }

        bool ValidateInputs(string input)
        {
            if (String.IsNullOrEmpty(input))
            {
                return false;
            }
            if (Directory.Exists(input))
            {
                return true;
            }

            return true;
        }

        void GetFiles(string input)
        {

            try
            {
                string[] lnkArray;
                string[] urlArray;
                if (searchSub == true)
                {
                    lnkArray = Directory.GetFiles(input, "*.lnk", SearchOption.AllDirectories);
                    urlArray = Directory.GetFiles(input, "*.url", SearchOption.AllDirectories);
                }
                else
                {
                    lnkArray = Directory.GetFiles(input, "*.lnk");
                    urlArray = Directory.GetFiles(input, "*.url");
                }


                List<string> files = new List<string>(lnkArray);
                files.AddRange(urlArray);
                if (files.Count == 0)
                {
                    Console.WriteLine("No shortcuts in this directory");
                    Print();
                }
                else
                {
                    DisplayFiles(files);
                }
            }

            catch (System.IO.DirectoryNotFoundException)
            {
                Console.WriteLine("Invalid directory!\n");
                Print();

            }
            catch (System.UnauthorizedAccessException)
            {
                Console.WriteLine("Unfortunately, directory includes read only directories or files. Cannot get .lnk files.\n");
                Print();
            }
        }



        void DisplayFiles(List<string> files)
        {
            int currentFile = 0;
            Console.WriteLine();
            foreach (string item in files)
            {
                currentFile++;
                Console.WriteLine($"{currentFile}.) {item}");
            }
            Console.WriteLine("If you do not want to delete any files, enter their index and they will be removed!");
            Console.WriteLine("Delete files?  (Y/N)");

            string deleteS = Console.ReadLine();
            int deleteI;
            if (!Int32.TryParse(deleteS, out deleteI))
            {
                if (deleteS == "Y")
                {
                    Console.WriteLine("\nDeleting Now");
                    if (DeleteFiles(files))
                    {
                        DisplayFinished();
                    }

                }
                else if (deleteS == "N")
                {
                    Print();
                }
                else if (deleteS != "exit") 
                {
                    Console.WriteLine("Not Y or N");
                    DisplayFiles(files);
                }
            }
            else if (Int32.TryParse(deleteS, out deleteI))
            {
                ParseInt(deleteI, files);
            }
            
            
        }
        bool DeleteFiles(List<string> files)
        {
            bool filesDeleted;
            foreach (string current in files)
            {
                File.Delete(current);
                Console.WriteLine($"File {current} deleted succesfully!");
            }
            filesDeleted = true;
            return filesDeleted;
        }

        void DisplayFinished()
        {
            Console.WriteLine("\nPress enter to exit, or uparrow to rerun program!");
            if (Console.ReadKey().Key == ConsoleKey.Enter)
            {
                Environment.Exit(0);
            }
            else if (Console.ReadKey().Key == ConsoleKey.UpArrow)
            {
                Console.Clear();
                Print();
            }
        }

        void ParseInt(int deleteI, List<string> files)
        {
            try
            {
                deleteI--;

                if (deleteI > files.Count)
                {
                    Console.WriteLine("File does not exist in list!");
                    DisplayFiles(files);
                }
                else
                {
                    files.RemoveAt(deleteI--);
                    Console.Clear();
                    Console.WriteLine("Removed from list!");
                    DisplayFiles(files);
                }
                Console.ReadLine();
            }
            catch
            {
                
                Console.Clear();
                Console.WriteLine("Number out of bounds");
                DisplayFiles(files);
            }
            }
        }
        

    }


