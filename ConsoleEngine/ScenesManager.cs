using System;
using System.Collections.Generic;
using System.Threading;

namespace ConsoleEngine
{
    public static class ScenesManager
    {
        private static readonly Dictionary<string, Scene> _scenes = new Dictionary<string, Scene>();
        private static readonly Thread threadScene = new Thread(HandleInput);

        private static Scene activeScene;

        public static Scene ActiveScene
        {
            get => activeScene;
        }

        public static void Add(Scene scene)
        {
            if (!_scenes.ContainsKey(scene.Name))
            {
                _scenes.Add(scene.Name, scene);

                if (!threadScene.IsAlive)
                {
                    threadScene.Start();
                }
            }
            else
            {
                throw new Exception("Attempted to add an already-existing scene.");
            }
        }

        private static void HandleInput()
        {
            if (activeScene == null)
            {
                throw new Exception("Attempted to handle user input on an empty scene.");
            }

            while (true)
            {
                ConsoleKey input = Console.ReadKey(false).Key;
                activeScene.HandleInput(input);
            }
        }

        public static void LoadScene(string sceneName)
        {
            if (_scenes.ContainsKey(sceneName))
            {
                Scene scene = _scenes[sceneName];
                activeScene = scene;

                Console.Clear();
                Console.WindowWidth = scene._width;
                Console.WindowHeight = scene._height;
                Console.BufferWidth = scene._width;
                Console.Title = scene._title;
                Console.CursorVisible = scene._isCursorVisible;

                // Render the background image if assigned.
                activeScene.BackgroundImage?.Render();

                // Render the scene components.
                scene.Render();

                // Fix the auto scroll issue.
                Console.CursorTop = 0;
            }
            else
            {
                throw new Exception($"Scene with name `{sceneName}` was not found.");
            }
        }

    }
}
