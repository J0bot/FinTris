using System;
using System.Collections.Generic;

namespace ConsoleEngine
{
    public class Scene : UIContainer<UIComponent>, IDrawable
    {
        const int DEFAULT_SCENE_WIDTH = 120;
        const int DEFAULT_SCENE_HEIGHT = 30;

        private string _name;
        private string _title;

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

        public Scene(string name, int width = DEFAULT_SCENE_WIDTH, int height = DEFAULT_SCENE_HEIGHT) : base()
        {
            _name = name;
            _title = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            _width = width;
            _height = height;
        }

        public override void AddComponent(UIComponent component)
        {
            base.AddComponent(component);
            component.Scene = this;
        }

        public void HandleInput(ConsoleKey input)
        {
            foreach (UIComponent child in _children)
            {
                child.OnKeyPressed(input);
            }
        }

        public override void Render()
        {
            foreach (UIComponent component in _children)
            {
                component.Render();
            }
        }
    }
}
