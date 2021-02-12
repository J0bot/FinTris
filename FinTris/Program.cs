using System;
using System.Timers;

namespace FinTris
{
    class Program
    {
        static Game _game;
        static GameRenderer _gameRenderer;

        static void Main(string[] args)
        {
            Console.CursorVisible = false;

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
                
            } while (input != ConsoleKey.Escape);
        }
    }
}
