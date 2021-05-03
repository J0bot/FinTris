using System.Collections.Generic;

namespace FinTris
{
    public class ButtonsContainer : UIContainer<Button>
    {
        public void Add(Button child)
        {
            if (child.Width > _width)
            {
                _width = child.Width;
            }

            child.Parent = this;
            child.Position += Vector2.Up * Children.Count;

            Children.Add(child);

            _height = Children.Count;
        }

        public override void Render()
        {
            foreach (UIComponent component in Children)
            {
                component.Render();
            }
        }
    }
}
