/// ETML
/// Auteur   	 : José Carlos Gasser, Ahmad Jano, Maxime Andrieux, Maxence Weyermann, Larissa Debarros
/// Date     	 : 09.03.2021
/// Description  : Fintris

using System;
using System.Threading;

namespace FinTris
{
    /// <summary>
    /// Classe qui s'occupe d'afficher le jeu.
    /// </summary>
    public class GameRenderer
    {
        private const int BORDER_THICKNESS = 2;
        /// <summary>
        /// Attribut _game représentant la référance de l'instance de Game.
        /// </summary>
        private readonly Game _game;

        /// <summary>
        /// Le déplacement horizontal du plateau.
        /// </summary>
        private const int SHIFT_X = 30;

        /// <summary>
        /// Le déplacement vertical du plateau.
        /// </summary>
        private const int SHIFT_Y = 2;

        private Rect _rectGame;
        private Rect _rectNextTetro;

        /// <summary>
        /// Constructor renseigné de la classe GameRenderer.
        /// </summary>
        /// <param name="game">La référence de l'instance de Game. Sert à récupérer certaines informations importantes par rapport à l'affichage.</param>
        public GameRenderer(Game game)
        {
            _game = game;

            _game.BoardChanged += game_PositionChanged;
            _game.StateChanged += game_StateChanged;
            _game.TetrominoChanged += _game_TetrominoChanged;

            int width = (_game.Cols + 2) * 2;

            _rectGame = new Rect(SHIFT_X, SHIFT_Y, width, _game.Rows + 2, ConsoleColor.DarkRed);
            _rectGame.Draw();

            _rectNextTetro = new Rect(SHIFT_X + width + 4 , SHIFT_Y + 2, 12, 6);
            _rectNextTetro.Draw();

            RenderNextTetromino();
        }

        private void _game_TetrominoChanged(object sender, EventArgs e)
        {
            RenderNextTetromino();
        }

        /// <summary>
        /// La fonction callback de l'événement StateChanged. Déclenché par la Game quand le jeu est terminé.
        /// </summary>
        /// <param name="sender">Le déclencheur de l'événement.</param>
        /// <param name="newState">Le nouveau état du jeu.</param>
        private void game_StateChanged(object sender, GameState newState)
        {
            if (newState == GameState.Finished)
            {
                DeathAnim();
                GameManager.Play();
            }
        }

        /// <summary>
        /// Fonction qui se déclenche quand il y a eu un changememt dans le plateau du jeu.
        /// </summary>
        /// <param name="sender">Le déclencheur de l'événement.</param>
        /// <param name="board">Le plateau du jeu contenant les état de chaque case.</param>
        private void game_PositionChanged(object sender, Case[,] board)
        {
            // Mettre à jour l'affichage du plateau après les nouveaux changements.
            Refresh(board);
        }

        /// <summary>
        /// Cette méthode s'occupe d'afficher le plateau du jeu en passant le tableau des états des cases en paramètre.
        /// </summary>
        /// <param name="board">Le tableau contenant les informations des cases.</param>
        private void Refresh(Case[,] board)
        {
            // Cette fonction fonctionnne indépendamment du temps pour assurer que dès qu'on bouge quelque chose, tout s'affiche directement.
            lock (this)
            {
                for (int y = 0; y < _game.Rows; y++)
                {
                    for (int x = 0; x < _game.Cols; x++)
                    {
                        Console.ForegroundColor = board[x, y].Color;
                        Console.SetCursorPosition(x * 2 + SHIFT_X + 2, y + SHIFT_Y + 1);
                        Console.Write(board[x, y].State == SquareState.Empty ? "  " : "██");
                    }
                }
                Console.ResetColor();
                DrawScore();
            }
        }

        /// <summary>
        /// Fonction qui s'occupe de dessiner le score.
        /// </summary>
        private void DrawScore()
        {
            // Affichage du score.
            Console.SetCursorPosition(60, 15);
            Console.WriteLine($"Score : {_game.Score} pts");

            // Affichage du niveau.
            Console.SetCursorPosition(60, 18);
            Console.WriteLine($"Niveau : {_game.Level}");
        }


        /// <summary>
        /// Animation quand le jeu finit qui permet de remplir l'écran avec des blocs.
        /// </summary>
        public void DeathAnim()
        {
            lock (this)
            {
                Config.SaveScore();
                _game.Stop();

                for (int y = _game.Rows - 1; y >= 0; y--)
                {
                    for (int x = _game.Cols - 1; x >= 0; x--)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.SetCursorPosition(x * 2 + SHIFT_X + 2, y + SHIFT_Y + 1);
                        Console.Write("██");
                    }
                    Thread.Sleep(100);
                }

                for (int y = _game.Rows; y > 0; y--)
                {
                    for (int x = 0; x < _game.Cols; x++)
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.SetCursorPosition(x * 2 + SHIFT_X + 2, y + SHIFT_Y);
                        Console.Write("██");
                    }

                    Thread.Sleep(8);
                }


                Thread.Sleep(1200);

                Console.ResetColor();

                int cursorX = SHIFT_X + _game.Cols / 2;
                int cursorY = SHIFT_Y + _game.Rows / 4;

                WriteAt("╔═════════════╗", cursorX, ++cursorY);
                WriteAt("║             ║", cursorX, ++cursorY);
                WriteAt("║  Game Over  ║", cursorX, ++cursorY);
                WriteAt("║             ║", cursorX, ++cursorY);
                WriteAt("╚═════════════╝", cursorX, ++cursorY);
                cursorY += 5;
                WriteAt("Please", cursorX += 2, ++cursorY);
                WriteAt("Try", cursorX += 2, ++cursorY);
                WriteAt("Again❤", cursorX += 2, ++cursorY);

                Thread.Sleep(1500);
            }
        }

        /// <summary>
        /// Fonction qui va s'occuper de render le prochain Tetromino.
        /// </summary>
        private void RenderNextTetromino()
        {
            int initPosX = 62;
            int initPosY = 5;

            _rectNextTetro.Draw();

            Console.SetCursorPosition(initPosX, initPosY);

            Console.ForegroundColor = _game.NextTetromino.TetrominoColor;

            int posx = SHIFT_X + ((_game.Cols + 2) * BORDER_THICKNESS) + 4;
            int posy = SHIFT_Y + 2;

            foreach (Vector2 blockDir in _game.NextTetromino.Blocks)
            {
                Vector2 blockPos = new Vector2(posx + BORDER_THICKNESS, posy + 1) + new Vector2(blockDir.x * BORDER_THICKNESS, blockDir.y);
                Console.SetCursorPosition(blockPos.x, blockPos.y);
                Console.Write("██");
            }

            Console.ResetColor();
        }


        /// <summary>
        /// Ecris ue texte à une position donnée. (repris de la documentation de Microsoft)
        /// </summary>
        /// <param name="s">Le texte à afficher.</param>
        /// <param name="x">La position X.</param>
        /// <param name="y">La position Y.</param>
        public static void WriteAt(string s, int x, int y)
        {
            int origRow = 0;
            int origCol = 0;

            Console.SetCursorPosition(origCol + x, origRow + y);
            Console.Write(s);
        }

        /// <summary>
        /// Effect de machine à ecrire.
        /// </summary>
        /// <param name="text">Le texte à afficher.</param>
        /// <param name="time">Le temps de l'affichage.</param>
        public void TypewriterEffect(string text, int time = 50)
        {
            // On va parcourir le string et écrire lettre par lettre.
            for (int i = 0; i < text.Length; i++) 
            {
                Console.Write(text.Substring(i, 1));
                Thread.Sleep(time);
            }
        }

    }
}
