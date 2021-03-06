﻿using GoRogue;
using SpellSword.Input;

namespace SpellSword.Render.Panes
{
    abstract class Pane: IInputListener
    {
        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public bool HasFocus { get; private set; }

        public virtual bool Dirty { get; protected set; }

        public Pane()
        {
        }

        public virtual void SuggestWidth(int width)
        {
            Width = width;
            Dirty = true;
        }

        public virtual void SuggestHeight(int height)
        {
            Height = height;
            Dirty = true;
        }

        public abstract bool Paint(Writeable writeContext);
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
