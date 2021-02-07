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
        }

        private void _game_PositionChanged(object sender, SquareState[,] board)
        {
            Refresh(board);
        }

        public void Refresh(SquareState[,] board)
        {
            #region tests
            //int startX = 0;
            //int startY = 0;

            //int endX = 0;
            //int endY = 0;

            //switch (rot)
            //{
            //    case RotationState.Rotation0:
            //        startX = 0;
            //        startY = 0;
            //        break;
            //    case RotationState.Rotation1:
            //        startX = 3;
            //        startY = 0;
            //        break;
            //    case RotationState.Rotation2:
            //        startX = 0;
            //        startY = 3;
            //        break;
            //    case RotationState.Rotation3:
            //        startX = 3;
            //        startY = 3;
            //        break;
            //    default:
            //        break;
            //}
            #endregion

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
