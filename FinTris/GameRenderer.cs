using System.Timers;
using System;

namespace FinTris
{
    public class GameRenderer
    {
        const int SHIFT_X = 5;
        const int SHIFT_Y = 3;
        private Game _game;        

        public GameRenderer(Game game)
        {
            _game = game;

            _game.BoardChanged += _game_PositionChanged;

            // TODO: dessiner la bordure.
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
                    int x = SHIFT_X + i * 2;
                    int y = SHIFT_Y + j;

                    Console.SetCursorPosition(x, y);
                    Console.Write(board[i,j] == 0 ? "  " : "██");
                }
            }

        }
    }
}
