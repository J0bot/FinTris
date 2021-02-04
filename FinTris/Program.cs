using System;
using System.Timers;

namespace FinTris
{
    class Program
    {
        static Tetromino tet = new Tetromino(TetrominoType.Snake, 0, 0);
        static Timer timer;
        static int y;
        static void Main(string[] args)
        {
            Console.CursorVisible = false;

            timer = new Timer(500);
            timer.Elapsed += Timer_Elapsed;
            //timer.Start();

            tet.Rotation = RotationState.Rotation1;
            Timer_Elapsed(null, null);

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

            int startX;
            int startY;

            int endX;
            int endY;

            switch (tet.Rotation)
            {
                default:
                case RotationState.Rotation0:
                    startX = 0;
                    startY = 0;
                    endX = 4;
                    endY = 4;
                    break;
                case RotationState.Rotation1:
                    startX = 4;
                    startY = 0;
                    endX = 0;
                    endY = 4;
                    break;
                case RotationState.Rotation2:
                    startX = 0;
                    startY = 4;
                    endX = 4;
                    endY = 0;
                    break;
                case RotationState.Rotation3:
                    startX = 4;
                    startY = 4;
                    endX = 0;
                    endY = 0;
                    break;
            }


            int stepX = tet.Rotation == RotationState.Rotation1 ? -1 : 1;
            int stepY= tet.Rotation == RotationState.Rotation2 ? -1 : 1;

            //int maxX = 0;
            //int maxY = 0;

            for (int j = 0; j < tet.Blocks.GetLength(1); j++)
            {
                for (int i = 0; i < tet.Blocks.GetLength(0); i++)
                {
                    if (tet.Blocks[i, j] != 0)
                    {
                        Console.SetCursorPosition(i, j);
                        Console.Write(tet.Blocks[i, j] == 1 ? "#" : ".");
                    }
                }
            }
            tet.Y++;
        }
    }
}
