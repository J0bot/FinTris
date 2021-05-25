using System;
using System.Collections.Generic;

namespace ConsoleEngine
{
    public class Scene : IDrawable
    {
        const int DEFAULT_SCENE_WIDTH = 120;
        const int DEFAULT_SCENE_HEIGHT = 30;

        internal string _name;
        internal string _title;
        internal bool _isCursorVisible;
        internal ConsoleColor _backgroundColor;
        internal ConsoleBackground _backgroundImage;
        internal int _width;
        internal int _height;
        private readonly List<GameObject> _objects;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int Width
        {
            get { return _width; }
        }

        public int Height
        {
            get { return _height; }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public bool IsCursorVisible
        {
            get { return _isCursorVisible; }
            set { _isCursorVisible = value; }
        }

        public ConsoleColor BackgroundColor
        {
            get { return _backgroundColor; }
            set { _backgroundColor = value; }
        }

        public ConsoleBackground BackgroundImage
        {
            get { return _backgroundImage; }
            set {
                if (value.Width == _width && value.Height == _height)
                {
                    _backgroundImage = value;
                }
                else
                {
                    throw new Exception("The scene's dimensions do not match the dimensions of the image provided.");
                }
            }
        }

        public Scene(string name, string title, int width = DEFAULT_SCENE_WIDTH, int height = DEFAULT_SCENE_HEIGHT)
        {
            _name = name;
            _title = title;
            _width = width;
            _height = height;
            _objects = new List<GameObject>();
        }

        public void HandleInput(ConsoleKey input)
        {
            foreach (GameObject child in _objects)
            {
                child.OnKeyPressed(input);
            }
        }

        public void Render()
        {
            foreach (GameObject child in _objects)
            {
                if (child is IDrawable)
                {
                    (child as IDrawable).Render();
                }
            }
        }

        public void AddObject(GameObject gameObj)
        {
            _objects.Add(gameObj);
            gameObj._scene = this;

            foreach (GameObject child in gameObj._children)
            {
                child._scene = this;
            }
        }
    }
}
