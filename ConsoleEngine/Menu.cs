﻿using System;

namespace ConsoleEngine
{
    public class Menu : UIContainer<Button>
    {
        private int selectedIndex;

        public new HorizontalAlignment HorizontalAlignment
        {
            get => _hAlignment;
            set { _hAlignment = value; UpdateChildren(nameof(HorizontalAlignment), value); }
        }

        public Menu()
        {
            selectedIndex = 0;
            UpdateButtons();
        }

        public override void OnKeyPressed(ConsoleKey input)
        {
            base.OnKeyPressed(input);

            if (input == ConsoleKey.DownArrow)
            {
                selectedIndex = (selectedIndex + 1) % _children.Count;
                UpdateButtons();
            }
            else if (input == ConsoleKey.UpArrow)
            {
                selectedIndex = mod((selectedIndex - 1), _children.Count); // Negative numbers aren't supported so i made something up
                UpdateButtons();
            }
            else if (input == ConsoleKey.Enter)
            {
                _children[selectedIndex].OnClicked();
            }
        }

        private void UpdateButtons()
        {
            foreach (Button child in _children)
            {
                child.IsSelected = child == _children[selectedIndex];
                child.Render();
            }
        }

        public void Select(Button button)
        {
            foreach (Button btn in _children)
            {
                btn.IsSelected = btn == button;                
            }
        }

        public override void AddComponent(Button child)
        {
            if (_sizingMode == SizingMode.AutoResize && child.Width > _width)
            {
                _width = child.Width;
            }

            child.Width = _width;
            child.Parent = this;

            if (_children.Count > 1)
            {
                //child.Position += new Vector2(child.Position.x, _children[-1].Height);
            }

            Children.Add(child);

            _height += child.Height;
        }

        public new int Width 
        {
            set
            {
                _width = value;
                foreach (UIComponent child in _children)
                {
                    child.Width = _width;
                }
            }
            get => _width;
        }

        public override void Render()
        {
            int i = 0;
            foreach (UIComponent component in _children)
            {
                //component
                component.Position += Vector2.Up * (component.Height * i);
                component.Render();
                i++;
            }
        }

        private int mod(int x, int m)
        {
            return (x % m + m) % m;
        }

    }
}
