using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Unblocker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            ShowWindow(ThisConsole, MAXIMIZE);
            UnblockPath(Directory.GetCurrentDirectory());
            Console.ReadKey();
        }
        [DllImport("kernel32.dll", ExactSpelling = true)]

        private static extern IntPtr GetConsoleWindow();
        private static IntPtr ThisConsole = GetConsoleWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]

        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private const int HIDE = 0;
        private const int MAXIMIZE = 3;
        private const int MINIMIZE = 6;
        private const int RESTORE = 9;

        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteFile(string name);

        public static void UnblockPath(string path)
        {
            string[] files = Directory.GetFiles(path);
            string[] dirs = Directory.GetDirectories(path);

            foreach (string file in files)
            {
                try
                {
                    UnblockFile(file);
                    Console.WriteLine(file);
                }
                catch (IOException)
                {
                    continue;
                }
            }

            foreach (string dir in dirs)
            {
                try
                {
                    UnblockPath(dir);
                    Console.WriteLine(dir);
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }

        public static bool UnblockFile(string fileName)
        {
            return DeleteFile(fileName + ":Zone.Identifier");
        }
    }
}
