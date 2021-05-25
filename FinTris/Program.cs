/// ETML
/// Auteur   	 : José Carlos Gasser, Ahmad Jano, Maxime Andrieux, Maxence Weyermann, Larissa Debarros
/// Date     	 : 09.03.2021
/// Description  : Fintris

using System;
using ConsoleEngine;

namespace FinTris
{
    /// <summary> 
    /// Classe principle du programme/jeu.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Le point d'entrée du programme/jeu.
        /// </summary>
        /// <param name="args">Les paramètres passés après l'exécution du programme/jeu.</param>
        static void Main(string[] args)
        {
            // SCENE 1 : (menu)
            Scene sceneMenu = new Scene("Menu", "Fintris", 60)
            {
                IsCursorVisible = false
            };

            StackPanel<Button> menu = new StackPanel<Button>()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                Width = sceneMenu.Width
            };
            TextBlock title = new TextBlock(Resources.fintris_title)
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                Width = sceneMenu.Width
            };
            menu.Position += Vector2.Up * (title.Height + 3);

            sceneMenu.AddObject(menu);
            sceneMenu.AddObject(title);

            Button play = new Button("Play") { Height = 3 };
            menu.AddComponent(play);
            menu.AddComponent(new Button("Options") { Height = 3 });
            menu.AddComponent(new Button("Player name:") { Height = 3 });
            menu.AddComponent(new Button("Quit") { Height = 3 });

            // SCENE 2 (main) :
            Scene sceneMain = new Scene("Main", "Fintris", 100, 40)
            {
                BackgroundColor = ConsoleColor.DarkGray,
                BackgroundImage = new ConsoleBackground(Resources.fintris_background),
                IsCursorVisible = false,
            };

            Game game = new Game();

            GameRenderer renderer = new GameRenderer(game)
            {
                Position = new Vector2(38, 13)
            };
            sceneMain.AddObject(renderer);


            play.Clicked += (_, __) =>
            {
                ScenesManager.LoadScene("Main");
                game.Start();
            };


            ScenesManager.Add(sceneMenu);
            ScenesManager.Add(sceneMain);
            ScenesManager.LoadScene("Menu");

        }
    }
}
