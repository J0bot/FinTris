using System.Timers;
using System;

namespace FinTris
{
    public class GameRenderer
    {        
        private Game _game;

        public GameRenderer(Game game)
        {
            _game = game;

            _game.BoardChanged += _game_PositionChanged;
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
                    int x = i;
                    int y = j;

                    Console.SetCursorPosition(x * 2, y);
                    Console.Write(board[i,j] ==0 ? "  " : "██");
                }
            }
        }

        private void BorderStyle()
        {

        }
    }
}
