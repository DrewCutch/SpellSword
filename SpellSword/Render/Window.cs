using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using BearLib;
using SpellSword.Input;
using SpellSword.Render.Panes;
using SpellSword.Render.Particles;
using Rectangle = GoRogue.Rectangle;

namespace SpellSword.Render
{
    class Window
    {
        public Pane Root { get; }
        public int ZIndex { get; }
        public bool Dirty { get; private set; }

        private Color _background;
        public Color Background
        {
            get => _background;
            set
            {
                _background = value;

                for (int i = 0; i < _backgroundWritable.Width; i++)
                    for (int j = 0; j < _backgroundWritable.Height; j++)
                    {
                        _backgroundWritable.SetGlyph(j, i, new Glyph(_background));
                    }
            }
        }

        private IWriteable _backgroundWritable;
        private IWriteable _writable;
        private IWriteable _particleWritable;

        private HashSet<IParticleEffect> _particleEffects;

        private long _lastFrame;
        private bool _particleEffectRemoved;

        public Window(Pane root, Rectangle bounds, int zIndex)
        {
            Root = root;
            ZIndex = zIndex;

            _backgroundWritable = new TextWriteContext(new TerminalWritable(bounds.Width, bounds.Height, zIndex * 3), bounds.Width, bounds.Height, bounds.MinExtentX, bounds.MinExtentY);
            _writable = new TextWriteContext(new TerminalWritable(bounds.Width, bounds.Height, zIndex * 3 + 1), bounds.Width, bounds.Height, bounds.MinExtentX, bounds.MinExtentY);
            _particleWritable = new TextWriteContext(new TerminalWritable(bounds.Width, bounds.Height, zIndex * 3 + 2), bounds.Width, bounds.Height, bounds.MinExtentX, bounds.MinExtentY);

            _lastFrame = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            _particleEffects = new HashSet<IParticleEffect>();
            _particleEffectRemoved = false;

            

            Dirty = true;
        }

        public void Refresh()
        {
            bool particlesUpdated = false;

            long now = Environment.TickCount;
            long deltaTime = now - _lastFrame;

            Root.SuggestWidth(_writable.Width);
            Root.SuggestHeight(_writable.Height);

            Root.Paint(_writable);

            foreach (IParticleEffect particleEffect in _particleEffects)
            {
                particleEffect.Update(deltaTime);
                particlesUpdated |= particleEffect.Changed;
            }

            _particleEffects.RemoveWhere(effect => effect.Finished);

            if (particlesUpdated || _particleEffectRemoved)
                PaintParticles();

            Dirty = _particleWritable.Dirty || _writable.Dirty;

            _particleEffectRemoved = false;
            _lastFrame = now;
        }

        public void Hide()
        {
            _writable.Clear();
            _backgroundWritable.Clear();
            _particleWritable.Clear();
        }

        private void PaintParticles()
        {
            ((IWriteable)_particleWritable).Clear();

            foreach (IParticleEffect effect in _particleEffects)
            {
                foreach (Particle particle in effect.Particles())
                {
                    _particleWritable.WriteGlyph(particle.Pos.Y + 1, particle.Pos.X, particle.Glyph);
                }
            }
        }

        public void Clean()
        {
            _writable.Dirty = false;
            _particleWritable.Dirty = false;
            Dirty = false;
        }

        public void AddEffect(IParticleEffect effect)
        {
            _particleEffects.Add(effect);
        }

        public void RemoveEffect(IParticleEffect effect)
        {
            _particleEffects.Remove(effect);
            _particleEffectRemoved = true;
        }

        public void Handle(ParticleEvent message)
        {
            if (message.Start)
                AddEffect(message.Effect);
            else
                RemoveEffect(message.Effect);
        }
    }
}
