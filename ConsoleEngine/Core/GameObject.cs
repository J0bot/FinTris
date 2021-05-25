using System;
using System.Collections.Generic;

namespace ConsoleEngine
{
    public abstract class GameObject : IGameObject
    {
        protected internal string _id;
        protected internal int _width;
        protected internal int _height;
        protected internal GameObject _parent;
        protected internal Vector2 _position;
        protected internal HorizontalAlignment _hAlignment;
        protected internal VerticalAlignment _vAlignment;
        protected internal SizingMode _sizingMode;
        protected internal readonly List<IGameObject> _children;
        protected internal Scene _scene;

        public event EventHandler<ConsoleKey> KeyPressed;

        public string Id
        {
            get { return _id; }
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

        public Scene Scene
        {
            get { return _scene; }
        }

        public GameObject Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        public GameObject()
        {
            _id = Guid.NewGuid().ToString();
            _children = new List<IGameObject>();
        }

        public virtual void OnKeyPressed(ConsoleKey input)
        {
            KeyPressed?.Invoke(this, input);
        }

        public virtual HorizontalAlignment HorizontalAlignment
        {
            get { return _hAlignment; }
            set { _hAlignment = value; }
        }

        public VerticalAlignment VerticalAlignment
        {
            get { return _vAlignment; }
            set { _vAlignment = value; }
        }

        public SizingMode SizingMode
        {
            get { return _sizingMode; }
            set { _sizingMode = value; }
        }

        public List<IGameObject> Children
        {
            get => _children;
        }

        public virtual void AddComponent(GameObject child)
        {
            _children.Add(child);
            child._parent = this;
            child._scene = _scene;

            if (child._sizingMode == SizingMode.ParentWidth)
            {
                child._width = _width;
            }
        }

        protected void UpdateChildren(string propertyName, object value)
        {
            foreach (GameObject child in _children)
            {
                child.GetType().GetProperty(propertyName).SetValue(child, value);
            }
        }
    }
}
