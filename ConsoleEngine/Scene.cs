using System;
using System.Collections.Generic;
using System.Threading;

namespace ConsoleEngine
{
    public class Scene : IDrawable
    {
        const int DEFAULT_SCENE_WIDTH = 120;
        const int DEFAULT_SCENE_HEIGHT = 30;

        private string _name;
        private string _title;
        private int _width;
        private int _height;
        private readonly List<UIComponent> _components;

        public event EventHandler<ConsoleKey> KeyPressed;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        public List<UIComponent> Components
        {
            get { return _components; }
        }

        public Scene(string name, int width = DEFAULT_SCENE_WIDTH, int height = DEFAULT_SCENE_HEIGHT)
        {
            _name = name;
            _title = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            _width = width;
            _height = height;
            _components = new List<UIComponent>();
        }

        public void AddComponent(UIComponent component)
        {
            _components.Add(component);
            component.Scene = this;
        }

        public void HandleInput(ConsoleKey input)
        {
            KeyPressed?.Invoke(this, input);
        }

        public void Render()
        {
            foreach (UIComponent component in _components)
            {
                component.Render();
            }
        }
    }
}
