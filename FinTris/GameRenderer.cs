///ETML
///Auteur   	: José Carlos Gasser, Ahmad Jano, Maxime Andrieux, Maxence Weyermann, Larissa Debarros
///Date     	: 09.03.2021
///Description  : Fintris

using System.Timers;
using System;

namespace FinTris
{
    /// <summary>
    /// Classe qui s'occupe d'afficher le jeu
    /// </summary>
    public class GameRenderer
    {        
        private readonly Game _game;
        private const int SHIFT_X = 30;
        private const int SHIFT_Y = 2;

        public GameRenderer(Game game)
        {
            _game = game;

            _game.BoardChanged += _game_PositionChanged;


            BorderStyle();
        }

        #region Ahmad way
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
                    Console.Write("██");
                    Console.SetCursorPosition(SHIFT_X + width - 1, SHIFT_Y + y);
                    Console.Write("██");
                }
                else
                {
                    Console.SetCursorPosition(SHIFT_X, SHIFT_Y + y);
                    Console.Write((y == 0 ? "╔" : "╚") + new string('═', width - 2) + (y == 0 ? "╗" : "╝"));
                }
            }
            Console.ResetColor();
        }
        #endregion

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
            lock (this)
            {

                for (int y = 0; y < _game.Rows; y++)
                {
                    for (int x = 0; x < _game.Cols; x++)
                    {
                        #region ora Ahmad wey
                        //SquareState color = ((SquareState)((byte)board[x, y] & 0b11111100));

                        //int diff = (int)color - 4;
                        //diff += 12;
                        //if (color == SquareState.SolidRed)
                        //{
                        //    Console.ForegroundColor = (ConsoleColor)diff;
                        //}$
                        #endregion
                        Console.SetCursorPosition(x * 2 + SHIFT_X + 2, y + SHIFT_Y + 1);
                        Console.Write(board[x, y] == SquareState.Empty ? "  " : "██");

                    }
                }
                Console.ResetColor();
            }
            
        }

        //C'est la fonction qui permet de créer le tour du jeu
        private void BorderStyle()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(SHIFT_X, SHIFT_Y);
            Console.Write(new string('█', 26));

            for (int i = 0; i < 22; i++)
            {
                Console.SetCursorPosition(SHIFT_X, i + SHIFT_Y+1);
                Console.Write("██"+ new string(' ', 22) + "██");
            }
            Console.SetCursorPosition(SHIFT_X, 22 + SHIFT_Y + 1);
            Console.Write(new string('█', 26));
             
            Console.ResetColor();   
        }

        private void NextTetrominoRender()
        {

        }

    }
}
