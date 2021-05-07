using System;

namespace ConsoleEngine
{
    public abstract class UIComponent : IDrawable
    {
        protected string _id;
        protected int _width;
        protected int _height;
        protected Vector2 _position;
        protected UIComponent _parent;
        protected HorizontalAlignment _hAlignment;
        protected VerticalAlignment _vAlignment;
        protected SizingMode _sizingMode;
        protected Scene _scene;

        public event EventHandler<ConsoleKey> KeyPressed;

        public string Id
        {
            get { return _id; }
            protected set { _id = value; }
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

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public HorizontalAlignment HorizontalAlignment
        {
            get { return _hAlignment; }
            set { _hAlignment = value; }
        }

        public VerticalAlignment VerticalAlignment
        {
            get { return _vAlignment; }
            set { _vAlignment = value; }
        }

        public SizingMode SizeingMode
        {
            get { return _sizingMode; }
            set { _sizingMode = value; }
        }

        public Scene Scene
        {
            get { return _scene; }
            set { _scene = value; }
        }

        public UIComponent Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        public UIComponent()
        {
            _id = Guid.NewGuid().ToString();
        }

        public virtual void OnKeyPressed(ConsoleKey input)
        {
            KeyPressed?.Invoke(this, input);
        }

        public abstract void Render();
    }
}
