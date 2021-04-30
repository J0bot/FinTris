/// ETML
/// Auteur   	 : José Carlos Gasser, Ahmad Jano, Maxime Andrieux, Maxence Weyermann, Larissa Debarros
/// Date     	 : 09.03.2021
/// Description  : Fintris

using FinTris.Properties;
using System;
using System.IO;
using System.Media;
using System.Threading;

namespace FinTris
{
    /// <summary>
    /// Classe qui s'occupe d'afficher le jeu.
    /// </summary>
    public class GameRenderer
    {        
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

        /// <summary>
        /// Constructor renseigné de la classe GameRenderer.
        /// </summary>
        /// <param name="game">La référence de l'instance de Game. Sert à récupérer certaines informations importantes par rapport à l'affichage.</param>
        public GameRenderer(Game game)
        {
            _game = game;

            _game.BoardChanged += _game_PositionChanged;
            _game.IsDead += _game_IsDed;

            BorderStyle();
        }

        /// <summary>
        /// Fonction qui se déclenche quand il y a eu un changememt dans le plateau du jeu.
        /// </summary>
        /// <param name="sender">Le déclencheur de l'événement.</param>
        /// <param name="board">Le plateau du jeu contenant les état de chaque case.</param>
        private void _game_PositionChanged(object sender, Case[,] board)
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
                NextTetrominoRender();
            }
        }

        /// <summary>
        /// Permet de créer la bordure du jeu.
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

            // Bordure du prochain Tetromino.
            Console.SetCursorPosition(58, 2);
            Console.Write("NEXT TETROMINO");

            byte min = 4;
            byte max = 9;
            byte decal = 60;
            byte length = 10;

            Console.ForegroundColor = ConsoleColor.Gray;
            for (int l = min; l < max; l++)
            {
                if (l == min || l == max - 1)
                {
                    Console.SetCursorPosition(decal, l);

                    Console.Write(new string('█', length));
                }
                else
                {
                    Console.SetCursorPosition(decal, l);
                    Console.Write("██"+new string(' ', length-4)+"██");
                }

            }

            Console.ResetColor();
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
        /// La fonction callback de l'événement IsDead. Déclenché par la Game quand le jeu est terminé.
        /// </summary>
        /// <param name="sender">Le déclencheur de l'événement.</param>
        /// <param name="e">If set to <c>true</c> e.</param>
        private void _game_IsDed(object sender, bool e)
        {
            if (e == true)
            {
                DeathAnim();
                GameManager.Play();
            }
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

                SoundPlayer koSound = new SoundPlayer(Resources.TetrisSoundKo);
                koSound.Play();

                for (int y = _game.Rows - 1; y >= 0; y--)
                {
                    for (int x = _game.Cols - 1; x >= 0; x--)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.SetCursorPosition(x * 2 + SHIFT_X + 2, y + SHIFT_Y + 1);
                        Console.Write("██");
                    }
                    System.Threading.Thread.Sleep(100);
                }


                for (int y = _game.Rows; y > 0; y--)
                {
                    for (int x = 0; x < _game.Cols; x++)
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.SetCursorPosition(x * 2 + SHIFT_X + 2, y + SHIFT_Y);
                        Console.Write("██");
                    }

                    System.Threading.Thread.Sleep(8);
                }


                System.Threading.Thread.Sleep(1200);

                Console.ResetColor();

                BorderStyle();

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

                System.Threading.Thread.Sleep(1500);
            }
        }

        /// <summary>
        /// Fonction qui va s'occuper de render le prochain Tetromino.
        /// </summary>
        private void NextTetrominoRender()
        {

            int initPosX =62;
            int initPosY =5;

            Console.SetCursorPosition(initPosX, initPosY);

            Console.ForegroundColor = _game.NextTetromino.TetrominoColor;

            if (_game.NextTetromino.Shape == TetrominoType.ILawlet)
            {
                Console.Write(  "██    ");
                WriteAt(        "██    ", initPosX, initPosY + 1);
                WriteAt(        "████  ", initPosX, initPosY + 2);
            }
            else if (_game.NextTetromino.Shape == TetrominoType.Lawlet)
            {
                Console.Write(  "    ██");
                WriteAt(        "    ██", initPosX, initPosY + 1);
                WriteAt(        "  ████", initPosX, initPosY + 2);
            }
            else if (_game.NextTetromino.Shape == TetrominoType.Pyramid)
            {
                Console.Write(  "      ");
                WriteAt(        "  ██  ", initPosX, initPosY + 1);
                WriteAt(        "██████", initPosX, initPosY + 2);
            }
            else if (_game.NextTetromino.Shape == TetrominoType.Snake)
            {
                Console.Write(  "  ██  ");
                WriteAt(        "████  ", initPosX, initPosY + 1);
                WriteAt(        "██    ", initPosX, initPosY + 2);

            }
            else if (_game.NextTetromino.Shape == TetrominoType.ISnake)
            {
                Console.Write(  "  ██  ");
                WriteAt(        "  ████", initPosX, initPosY + 1);
                WriteAt(        "    ██", initPosX, initPosY + 2);
            }
            else if (_game.NextTetromino.Shape == TetrominoType.Squarie)
            {
                Console.Write(  "██████");
                WriteAt(        "██████", initPosX, initPosY + 1);
                WriteAt(        "██████", initPosX, initPosY + 2);
            }
            else if (_game.NextTetromino.Shape == TetrominoType.Malong)
            {
                Console.Write(  "  ██  ");
                WriteAt(        "  ██  ", initPosX, initPosY+1);
                WriteAt(        "  ██  ", initPosX, initPosY+2);
            }



            Console.ResetColor();
        }

#if DEBUG
        /// <summary>
        /// Si le joueur appuie sur A, il entre dans une zone interdite.
        /// </summary>
        public void CheatCode()
        {
            // Lancement de la première voix.
            SoundPlayer bowserSound2 = new SoundPlayer(Resources.bowserSound2);
            bowserSound2.Play();

            Console.Clear();
            Console.SetCursorPosition(50, 14);
            TypewriterEffect("??? : Tricheur !");
            Thread.Sleep(200);

            Console.SetCursorPosition(35, 16);
            TypewriterEffect("??? : Tu ne devais pas avoir accès à cette zone !");

            Thread.Sleep(200);


            Console.SetCursorPosition(39, 18);
            TypewriterEffect("??? : Maintenant il va falloir...");
            Thread.Sleep(100);
            TypewriterEffect(" payer !", 100);
            Thread.Sleep(1000);
            Console.Clear();

            // Lancement de la deuxième voix.
            SoundPlayer bowserSound = new SoundPlayer(Resources.bowserSound);
            bowserSound.Play();

            string[] bowserString = new string[] {
                "                                   @                                  ",
                "                                 @@@@@                                ",
                "                          *@@@&@@@@@@@@##@@@@                         ",
                "                         @@@@@@@@@@@@@@@@@@@@@.                       ",
                "         @             @@@@@@@@@@@@@@@@@@@@@@@@@            @@        ",
                "       @@@@           @@@@@@@@@@@@@@@@@@@@@@@@@@@@          @@@@      ",
                "     /@@@@@%        @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@       @@@@@@     ",
                "     @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@    ",
                "     @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@    ",
                "      @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@    ",
                "        @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@      ",
                "          @@@@@@@@   @@@@@@@@@@@@@@@@@@@@@@@@@@@@@   @@@@@@@@         ",
                "          @@@@@@@@       &@@@@@@@@@@@@@@@@@@@@.      &@@@@@@@         ",
                "       #@@@@@@@@@@,         @@@@@@@@@@@@@@@          @@@@@@@@@@,      ",
                "    @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@   ",
                "  @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ ",
                " @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@",
                ".@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@",
                " @@@@@@@@@@@   @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@   @@@@@@@@@@@",
                " @@@@@@@@@@@@       @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@       ,@@@@@@@@@@@",
                "  @@@@@@@@@@@*         @@@@@@,           ,@@@@@@          @@@@@@@@@@@ ",
                "   @@@@@@@@@@@           @@                 @@.          @@@@@@@@@@@  ",
                "     @@@@@@@@@@           @                 @           @@@@@@@@@@    ",
                "        @@@@@@@@                                       @@@@@@@@       ",
                "          @@@@@@@                                     @@@@@@@         ",
                "            @@@@@@                                   @@@@@@           ",
                "              @@@@@@     @@@              ,@@@      @@@@@.            ",
                "               @@@@@@  @@@@@@@@@@@@@@@@@@@@@@@@@  @@@@@@              ",
                "                 @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@                ",
                "                 (@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@(                ",
                "                  @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@                 ",
                "                   @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@                  ",
                "                    @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@                   ",
                "                      &@@@@@@@@        #@@@@@@@@                      "
            };

            // Affichage du monstre.
            // 
            for (int i = 0; i < 5; i++)
            {

                Console.ForegroundColor = ConsoleColor.Red;

                //string[] bowser = File.ReadAllLines(Resources.Bowser);

                //for (int w = 0; w < bowser.Length; w++)
                //{
                //    Console.WriteLine(bowser[w]);
                //}
                for (int j = 0; j < bowserString.Length; j++)
                {
                    Console.WriteLine(bowserString[j]);
                }


                Thread.Sleep(100);
                Console.Clear();
                Thread.Sleep(100);

            }
            Console.ResetColor();
            BorderStyle();

            _game.CheatCode();

        }
#endif

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
