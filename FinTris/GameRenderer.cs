///ETML
///Auteur   	: José Carlos Gasser, Ahmad Jano, Maxime Andrieux, Maxence Weyermann, Larissa Debarros
///Date     	: 09.03.2021
///Description  : Fintris

using System;

namespace FinTris
{
    /// <summary>
    /// Classe qui s'occupe d'afficher le jeu
    /// </summary>
    public class GameRenderer
    {        
        /// <summary>
        /// Attribut _game pour récuperer les infos de game
        /// </summary>
        private readonly Game _game;

        /// <summary>
        /// position de l'affichage du plateau en x
        /// </summary>
        private const int SHIFT_X = 30;

        /// <summary>
        /// position de l'affichage du plateau en y
        /// </summary>
        private const int SHIFT_Y = 2;

        /// <summary>
        /// Attribut Random pour effectuer des fonctions Random
        /// </summary>
        private static Random _random;

        /// <summary>
        /// Constructor renseigné de la classe GameRenderer
        /// </summary>
        /// <param name="game">paramètre game pour récup les infos de game</param>
        public GameRenderer(Game game)
        {
            _game = game;

            _game.BoardChanged += _game_PositionChanged;

            _random = new Random();

            BorderStyle();
        }

        /// <summary>
        /// Fonction qui s'active quand il y a eu un changememt dans la classe Game, elle va lancer la fonction Refresh()
        /// </summary>
        /// <param name="sender">c'est les données reçues</param>
        /// <param name="board">c'est le tableau contenant les informations du jeu</param>
        private void _game_PositionChanged(object sender, Case[,] board)
        {
            Refresh(board);
        }

        /// <summary>
        /// La fonction Refresh va s'occuper d'afficher les données du tableau de SquareState envoyé par la classe Game.
        /// 
        /// Cette fonction fonctionnne indépendamment du temps pour assurer que dès qu'on bouge quelque chose, tout s'affiche directement
        /// </summary>
        /// <param name="board">paramètre du tableau de SquarState</param>
        private void Refresh(Case[,] board)
        {
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
                NextTetrominoRender();
            }
        }

        /// <summary>
        /// C'est la fonction qui permet de créer la bordure du jeu
        /// </summary>
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

            //Bordure du prochain Tetromino
            Console.SetCursorPosition(58, 2);
            Console.Write("NEXT TETROMINO");

            byte min = 4;
            byte max = 10;
            byte decal = 59;

            Console.ForegroundColor = ConsoleColor.Green;
            for (int l = min; l < max; l++)
            {
                if (l == min || l == max - 1)
                {
                    Console.SetCursorPosition(decal, l);
                    Console.Write(new string('█', 12));
                }
                else
                {
                    Console.SetCursorPosition(decal, l);
                    Console.Write("██        ██");
                }

            }

            Console.ResetColor();
        }

        /// <summary>
        /// Fonction qui s'occupe de dessiner le score
        /// </summary>
        private void DrawScore()
        {
            //Affichage du score
            Console.SetCursorPosition(60, 15);
            Console.WriteLine($"Score : {_game.Score} pts");

            //Affichage du niveau
            Console.SetCursorPosition(60, 18);
            Console.WriteLine($"Niveau : {_game.Level}");
        }

        /// <summary>
        /// Animation quand le jeu finit qui permet de remplir l'écran avec des blocs
        /// </summary>
        public void DeathAnim()
        {
            lock(this)
            {
                for (int y = _game.Rows-1; y >=0 ; y--)
                {
                    for (int x = _game.Cols-1; x >= 0; x--)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.SetCursorPosition(x * 2 + SHIFT_X + 2, y + SHIFT_Y + 1);
                        Console.Write("██");
                    }
                    System.Threading.Thread.Sleep(100);
                }
            }
        }

        /// <summary>
        /// Fonction qui va s'occuper de render le prochain Tetromino
        /// </summary>
        private void NextTetrominoRender()
        {
            if (_game.CurrentTetromino.Shape == TetrominoType.Lawlet)
            {

            }
            

        }
    }
}
