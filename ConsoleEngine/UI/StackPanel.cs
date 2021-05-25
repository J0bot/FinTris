using System;

namespace ConsoleEngine
{
    public class StackPanel<T> : GameObject, IDrawable where T : GameObject
    {
        private int selectedIndex;

        public StackPanel()
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
                (_children[selectedIndex] as Button).OnClicked();
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

        public void Select(T button)
        {
            foreach (Button btn in _children)
            {
                btn.IsSelected = btn == button;                
            }
        }

        public override void AddComponent(GameObject component)
        {
            base.AddComponent(component);

            component.HorizontalAlignment = _hAlignment;

            if (component.Width > _width)
            {
                _width = component.Width;
            }

            if (_children.Count > 1)
            {
                IGameObject last = _children[_children.Count - 2];
                //child.Position = new Vector2(child.Position.x, last.Position.y + last.Height);
            }

            _height += component._height;
        }

        public void Render()
        {
            int i = 0;
            foreach (T child in _children)
            {
                //component
                child._position = (Vector2.Up * (child._height * i));
                ((IDrawable)child).Render();
                i++;
            }
        }

        private int mod(int x, int m)
        {
            return (x % m + m) % m;
        }

    }
}
