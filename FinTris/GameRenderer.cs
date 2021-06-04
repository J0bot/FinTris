/// ETML
/// Auteur   	 : José Carlos Gasser, Ahmad Jano, Maxime Andrieux, Maxence Weyermann, Larissa Debarros
/// Date     	 : 09.03.2021
/// Description  : Fintris

using System;
using System.Diagnostics;
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

        private readonly Vector2 _nextTetroPos;
        private readonly Vector2 _position;

        /// <summary>
        /// La matrice scale du console.
        /// </summary>
        private static readonly Vector2 MAT_SCALE = new Vector2(2, 1);

        /// <summary>
        /// Constructor renseigné de la classe GameRenderer.
        /// </summary>
        /// <param name="game">La référence de l'instance de Game. Sert à récupérer certaines informations importantes par rapport à l'affichage.</param>
        public GameRenderer(Game game)
        {
            _game = game;
            _position = new Vector2(30, 2);
            _nextTetroPos = new Vector2(60, 4);

            _game.BoardChanged += _game_PositionChanged;
            _game.IsDead += _game_IsDed;
            _game.NextTetroSpawned += OnNextTetroSpawned;

            Console.Clear();
            RenderGameBorder();
            RenderNextTetro();
            RenderScore();
        }

        private void OnNextTetroSpawned(object sender, EventArgs e)
        {
            RenderNextTetro();
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
                    for (int x = 0; x < _game.Columns; x++)
                    {
                        Console.ForegroundColor = board[x, y].Color;
                        Console.SetCursorPosition(x * 2 + _position.x + 2, y + _position.y + 1);
                        Console.Write(board[x, y].State == SquareState.Empty ? "  " : "██");
                    }
                }
                Console.ResetColor();
                RenderScore();
            }
        }

        private void DrawTile(Vector2 position)
        {
            Console.SetCursorPosition(position.x, position.y);
            Console.Write("██");
        }

        private void DrawTile(Vector2 position, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.SetCursorPosition(position.x, position.y);
            Console.Write("██");
        }

        private void DrawTile(Vector2 position, Vector2 shift, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.SetCursorPosition(shift.x + position.x, shift.y + position.y);
            Console.Write("██");
        }

        private void DrawBorder(Vector2 position, int width, int height, ConsoleColor color)
        {
            for (int j = 0; j < height; j++)
            {
                bool middle = j > 0 && j < height - 1;
                for (int i = 0; i < (middle ? 2 : width); i++)
                {
                    i = middle && i > 0 ? width - 1 : i;
                    Vector2 pos = new Vector2(i * MAT_SCALE.x, j);
                    DrawTile(pos, position, color);
                }
            }
        }

        /// <summary>
        /// Permet de créer la bordure du jeu.
        /// </summary>
        public void RenderGameBorder()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(_position.x, _position.y);
            Console.Write(new string('█', 26));

            for (int i = 0; i < 22; i++)
            {
                Console.SetCursorPosition(_position.x, i + _position.y+1);
                Console.Write("██"+ new string(' ', 22) + "██");
            }
            Console.SetCursorPosition(_position.x, 22 + _position.y + 1);
            Console.Write(new string('█', 26));
            
            Console.ResetColor();

            // Prochain Tetromino.
            Console.SetCursorPosition(_nextTetroPos.x, _nextTetroPos.y - 2);
            Console.Write("NEXT TETROMINO");
            Console.ResetColor();
        }

        /// <summary>
        /// Fonction qui s'occupe de dessiner le score.
        /// </summary>
        private void RenderScore()
        {
            // Affichage du score.
            Console.SetCursorPosition(60, 15);
            Console.WriteLine($"Score : {_game.Score} pts");

            // Affichage des lignes supprimées.
            Console.SetCursorPosition(60, 17);
            Console.WriteLine($"Lignes : {_game.RowsCleared}");

            // Affichage du niveau.
            Console.SetCursorPosition(60, 19);
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

                Console.ForegroundColor = ConsoleColor.Blue;
                for (int y = _game.Rows - 1; y >= 0; y--)
                {
                    for (int x = _game.Columns - 1; x >= 0; x--)
                    {
                        WriteAt("██", x * 2 + _position.x + 2, y + _position.y + 1);
                    }
                    Thread.Sleep(100);
                }
                
                Console.ForegroundColor = ConsoleColor.Gray;
                for (int y = _game.Rows; y > 0; y--)
                {
                    for (int x = 0; x < _game.Columns; x++)
                    {
                        WriteAt("██", x * 2 + _position.x + 2, y + _position.y);
                    }
                    Thread.Sleep(10);
                }


                Thread.Sleep(1200);

                Console.ResetColor();

                RenderGameBorder();

                int cursorX = _position.x + _game.Columns / 2;
                int cursorY = _position.y + _game.Rows / 4;

                foreach (string line in Resources.game_over.Split('\n'))
                {
                    WriteAt(line, cursorX, ++cursorY);
                }

                cursorY += 5;
                cursorX += 2;

                foreach (string line in Resources.try_again.Split('\n'))
                {
                    WriteAt(line, cursorX, cursorY);
                    cursorX += 2;
                    cursorY += 1;
                }

                Thread.Sleep(1500);
            }
        }

        /// <summary>
        /// Fonction qui va s'occuper d'afficher le prochain Tetromino.
        /// </summary>
        private void RenderNextTetro()
        {
            int max = 10; // 10 parceque le tetromino le plus large est de 6 caractères + 4 de bordures.
            int width = _game.NextTetromino.Width + 4;
            int height = _game.NextTetromino.Height + 4;

            // Effacer l'ancienne représentation du next tetromino.
            for (int x = 0; x < max; x++)
            {
                for (int y = 0; y < max; y++)
                {
                    Console.SetCursorPosition(_nextTetroPos.x + x * 2, _nextTetroPos.y + y);
                    Console.Write("  ");
                }
            }

            // Dessiner la bordure.
            DrawBorder(_nextTetroPos, width, height, ConsoleColor.Red);

            // Dessiner la forme du next tetromino.
            foreach (Vector2 posBlock in _game.NextTetromino.Blocks)
            {
                Vector2 pos = posBlock + (Vector2.One * 2);
                Vector2 scaled = new Vector2(pos.x * MAT_SCALE.x, pos.y);
                DrawTile(scaled, _nextTetroPos, _game.NextTetromino.TetrominoColor);
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

            if (!GameManager.Muted)
            {
                bowserSound2.Play();
            }

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

            if (!GameManager.Muted)
            {
                bowserSound.Play();
            }         

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
            RenderGameBorder();

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
            Console.SetCursorPosition(x, y);
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
