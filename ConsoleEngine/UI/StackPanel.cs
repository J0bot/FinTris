using System;

namespace ConsoleEngine
{
    public class StackPanel<T> : GameObject where T : GameObject
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

        public void Select(Button button)
        {
            foreach (Button btn in _children)
            {
                btn.IsSelected = btn == button;                
            }
        }

        public override void AddComponent(GameObject child)
        {
            base.AddComponent(child);

            child.HorizontalAlignment = _hAlignment;

            if (child.Width > _width)
            {
                _width = child.Width;
            }

            if (_children.Count > 1)
            {
                IGameObject last = _children[_children.Count - 2];
                child.Position = new Vector2(child.Position.x, last.Position.y + last.Height);
            }

            _height += child.Height;
        }

        public void Render()
        {
            int i = 0;
            foreach (Button child in _children)
            {
                //component
                child.Position += Vector2.Up * (child.Height * i);
                child.Render();
                i++;
            }
        }

        private int mod(int x, int m)
        {
            return (x % m + m) % m;
        }

    }
}
