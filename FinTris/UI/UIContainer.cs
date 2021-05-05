﻿using System.Collections;
using System.Collections.Generic;

namespace FinTris
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

        public void AddComponent(T child)
        {
            _children.Add(child);
            child.Width = _width;
        }
    }
}