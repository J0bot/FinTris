﻿using System;
using System.Collections.Generic;

namespace FinTris
{
    public static class ScenesManager
    {
        private static readonly Dictionary<string, Scene> _scenes = new Dictionary<string, Scene>();

        public static void Add(Scene scene)
        {
            if (!_scenes.ContainsKey(scene.Name))
            {
                _scenes.Add(scene.Name, scene);
            }
            else
            {
                throw new Exception("Attempted to add an already-existing scene.");
            }
        }

        public static void SetActiveScene(string sceneName)
        {
            if (_scenes.ContainsKey(sceneName))
            {
                Scene scene = _scenes[sceneName];

                Console.Clear();
                Console.WindowWidth = scene.Width;
                Console.BufferWidth = scene.Width;
                Console.WindowHeight = scene.Height;
                Console.BufferHeight = scene.Height;
                Console.Title = scene.Title;

                scene.Render();
            }
            else
            {
                throw new Exception($"Scene with name {sceneName} was not found.");
            }
        }

    }
}
