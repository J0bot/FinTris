using System;
using System.Timers;
using Figgle;

namespace FinTris
{
    class Program
    {
        static Game _game;
        static GameRenderer _gameRenderer;


        static void Main(string[] args)
        {
            Console.CursorVisible = false;

            #region Max Way Menu
            Console.Title = "FinTris";



            //Menu
            Menu menu = new Menu(FiggleFonts.Starwars.Render("FinTris"));

            MenuEntry play = new MenuEntry("Play");
            MenuEntry quit = new MenuEntry("Quit");

            menu.Add(play);
            menu.Add(quit);

            MenuEntry choice = null;

            do
            {
                choice = menu.ShowMenu();

                if (choice == play)
                {
                    Play();
                }

            } while (choice != quit);

            Environment.Exit(0);

            #endregion

            
        }

        public static void Play()
        {
            Console.Clear();

            _game = new Game();
            _gameRenderer = new GameRenderer(_game);

            _game.Start();

            ConsoleKey input;
            do
            {
                input = Console.ReadKey(true).Key;
                if (input == ConsoleKey.RightArrow)
                {
                    _game.MoveRight();
                }
                else if (input == ConsoleKey.LeftArrow)
                {
                    _game.MoveLeft();
                }
                else if (input == ConsoleKey.Spacebar)
                {
                    _game.Rotate();
                }
                else if(input == ConsoleKey.DownArrow)
                {
                    _game.MoveDown();
                }

            } while (input != ConsoleKey.Escape);
        }

        #region tests
        //private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        //{
        //    Console.Clear();
        //    y++;

        //    //int startX;
        //    //int startY;

        //    //int endX;
        //    //int endY;

        //    //switch (tet.Rotation)
        //    //{
        //    //    default:
        //    //    case RotationState.Rotation0:
        //    //        startX = 0;
        //    //        startY = 0;
        //    //        endX = 4;
        //    //        endY = 4;
        //    //        break;
        //    //    case RotationState.Rotation1:
        //    //        startX = 4;
        //    //        startY = 0;
        //    //        endX = 0;
        //    //        endY = 4;
        //    //        break;
        //    //    case RotationState.Rotation2:
        //    //        startX = 0;
        //    //        startY = 4;
        //    //        endX = 4;
        //    //        endY = 0;
        //    //        break;
        //    //    case RotationState.Rotation3:
        //    //        startX = 4;
        //    //        startY = 4;
        //    //        endX = 0;
        //    //        endY = 0;
        //    //        break;
        //    //}


        //    //int stepX = tet.Rotation == Rotat ionState.Rotation1 ? -1 : 1;
        //    //int stepY= tet.Rotation == RotationState.Rotation2 ? -1 : 1;

        //    //int maxX = 0;
        //    ////int maxY = 0;

        //    //for (int j = 0; j < tet.Blocks.GetLength(1); j++)
        //    //{
        //    //    for (int i = 0; i < tet.Blocks.GetLength(0); i++)
        //    //    {
        //    //        Console.SetCursorPosition(tet.X + i * 2, tet.Y + j);
        //    //        Console.Write(tet.Blocks[i, j] == 1 ? "██" : "  ");
        //    //    }

        //    //}
        //    //tet.Y++;
        //}

        #endregion
    }
}
