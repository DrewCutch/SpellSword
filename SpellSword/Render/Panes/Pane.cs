using GoRogue;
using SpellSword.Input;

namespace SpellSword.Render.Panes
{
    abstract class Pane: IInputListener
    {
        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public bool HasFocus { get; private set; }

        public bool Dirty { get; protected set; }

        public Pane()
        {
        }

        public virtual void SuggestWidth(int width)
        {
            Width = width;
        }

        public virtual void SuggestHeight(int height)
        {
            Height = height;
        }

        public abstract bool Paint(IWriteable writeContext);
        public virtual void OnMouseClick(Coord pos) { }

        public virtual void OnMouseMove(Coord last, Coord current) { }

        public virtual void OnKeyDown(int keyCode) { }

        public virtual void OnKeyUp(int keyCode) { }

        public virtual IInputListener Focus(Coord pos)
        {
            HasFocus = true;
            return this;
        }

        public virtual void LoseFocus()
        {
            HasFocus = false;
        }
    }
}
