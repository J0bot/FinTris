using System.Timers;
using System;

namespace FinTris
{
    public class GameRenderer
    {
        const int SHIFT_X = 5;
        const int SHIFT_Y = 3;

        private readonly Game _game;        

        public GameRenderer(Game game)
        {
            _game = game;

            _game.BoardChanged += _game_PositionChanged;

            DrawBorders();
        }

        private void DrawBorders()
        {
            // On ajoute des colonnes/lignes parce qu'on ne veut pas les dimensions exactes du plateau sinon
            // le plateau va ecraser les bordures.

            int width = (_game.Cols + 1) * 2;
            int height = _game.Rows + 2;

            Console.ForegroundColor = ConsoleColor.Blue;

            for (int y = 0; y < height; y++)
            {
                if (y > 0 && y < height - 1)
                {
                    Console.SetCursorPosition(SHIFT_X, SHIFT_Y + y);
                    Console.Write('║');
                    Console.SetCursorPosition(SHIFT_X + width - 1, SHIFT_Y + y);
                    Console.Write('║');
                }
                else
                {
                    Console.SetCursorPosition(SHIFT_X, SHIFT_Y + y);
                    Console.Write((y == 0 ? "╔" : "╚") + new string('═', width - 2) + (y == 0 ? "╗" : "╝"));
                }
            }
            Console.ResetColor();
        }

        private void _game_PositionChanged(object sender, SquareState[,] board)
        {
            Refresh(board);
        }

        public void Refresh(SquareState[,] board)
        {
            for (int j = 0; j < _game.Rows; j++)
            {
                for (int i = 0; i < _game.Cols; i++)
                {
                    int x = (SHIFT_X + 1) + (i * 2);
                    int y = (SHIFT_Y + 1) + j;

                    Console.SetCursorPosition(x, y);
                    Console.Write(board[i,j] == 0 ? "  " : "██");
                }
            }
        }
    }
}
