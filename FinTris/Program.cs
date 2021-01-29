using System;
using System.Timers;

namespace FinTris
{
    class Program
    {
        static Tetromino tet = new Tetromino(TetrominoType.BLUE, 0, 0);
        static Timer timer;
        static int y;
        static void Main(string[] args)
        {
            Console.CursorVisible = false;

            timer = new Timer(500);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();

            y = 0;
            ConsoleKey input;

            do
            {
                input = Console.ReadKey(true).Key;
                if (input == ConsoleKey.RightArrow)
                {
                    tet.X++;
                }
                else if (input == ConsoleKey.LeftArrow)
                {
                    tet.X--;
                }
            } while (input != ConsoleKey.Escape);
        }

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Console.Clear();
            y++;
            for (int y = 0; y < tet.blocks.GetLength(1); y++)
            {
                for (int x = 0; x < tet.blocks.GetLength(0); x++)
                {
                    Console.SetCursorPosition(tet.X + x, tet.Y + y);
                    Console.Write(tet.blocks[x, y] == 1 ? "#" : " ");
                }
            }
            tet.Y++;
        }
    }
}
