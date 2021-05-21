using System;
using System.Collections.Generic;
using System.Threading;

namespace ConsoleEngine
{
    public static class ScenesManager
    {
        private static readonly Dictionary<string, Scene> _scenes = new Dictionary<string, Scene>();

        private static Scene activeScene;
        private static Thread threadScene = new Thread(HandleInput);

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
                return;
            }

            while (true)
            {
                ConsoleKey input = Console.ReadKey(false).Key;
                activeScene.HandleInput(input);
            }
        }

        public static void SetActiveScene(string sceneName)
        {
            if (_scenes.ContainsKey(sceneName))
            {
                Scene scene = _scenes[sceneName];
                activeScene = scene;

                Console.Clear();
                Console.WindowWidth = scene.Width;
                Console.BufferWidth = scene.Width;
                Console.WindowHeight = scene.Height;
                Console.BufferHeight = scene.Height;
                Console.Title = scene.Title;
                Console.CursorVisible = scene.IsCursorVisible;

                scene.Render();
            }
            else
            {
                throw new Exception($"Scene with name {sceneName} was not found.");
            }
        }

    }
}
