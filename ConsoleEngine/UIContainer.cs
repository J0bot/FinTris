using System.Collections;
using System.Collections.Generic;

namespace ConsoleEngine
{
    public abstract class UIContainer<T> : UIComponent where T : UIComponent
    {
        protected readonly List<T> _children;

        public List<T> Children
        {
            get => _children;
        }

        public UIContainer()
        {
            _children = new List<T>();
        }

        public virtual void AddComponent(T child)
        {
            _children.Add(child);
            if (child.SizeingMode == SizingMode.AutoResize)
            {
                child.Width = _width;
            }
        }

        protected void UpdateChildren(string propertyName, object value)
        {
            foreach (UIComponent child in _children)
            {
                child.GetType().GetProperty(propertyName).SetValue(child, value);
            }
        }
    }
}
