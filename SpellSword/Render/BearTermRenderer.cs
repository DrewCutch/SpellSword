using System;
using System.Collections.Generic;
using System.Drawing;
using BearLib;
using GoRogue.Messaging;
using SpellSword.Engine;
using SpellSword.Render.Panes;
using SpellSword.Render.Particles;

namespace SpellSword.Render
{
    class BearTermRenderer: ISubscriber<ParticleEvent>
    {
        private Pane _root;
        private TerminalWritable _texture;
        private TerminalWritable _particleWritable;

        private HashSet<IParticleEffect> _particleEffects;

        private long _lastFrame;

        private bool _particleEffectRemoved;

        public BearTermRenderer(Pane root, string options = "")
        {
            _root = root;

            if(!Terminal.Open())
                throw new Exception("Terminal instance is already open!");

            Terminal.Set(options);

            _texture = new TerminalWritable(Terminal.State(Terminal.TK_WIDTH), Terminal.State(Terminal.TK_HEIGHT), 0);
            _particleWritable = new TerminalWritable(Terminal.State(Terminal.TK_WIDTH), Terminal.State(Terminal.TK_HEIGHT), 1);

            _lastFrame = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            _particleEffects = new HashSet<IParticleEffect>();
            _particleEffectRemoved = false;

            Terminal.Refresh();
        }

        public void Refresh()
        {
            bool particlesUpdated = false;

            long now = Environment.TickCount;
            long deltaTime = now - _lastFrame;

            _root.SuggestWidth(_texture.Width);
            _root.SuggestHeight(_texture.Height);

            _root.Paint(_texture);

            foreach (IParticleEffect particleEffect in _particleEffects)
            {
                particleEffect.Update(deltaTime);
                particlesUpdated |= particleEffect.Changed;
            }

            _particleEffects.RemoveWhere(effect => effect.Finished);

            if (particlesUpdated || _particleEffectRemoved)
                PaintParticles();

            if (_particleWritable.Dirty)
            {
                Terminal.Refresh();
                _particleWritable.Clean();
            }

            if (_texture.Dirty)
            {
                Terminal.Refresh();
                _texture.Clean();
            }

            _particleEffectRemoved = false;
            _lastFrame = now;
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

        private void ClearCurrentLayer()
        {
            for (int x = 0; x < _texture.Width; x++)
            {
                for (int y = 0; y < _texture.Height; y++)
                {
                    Terminal.Put(x, y, ' ');
                }
            }
        }

        /*public void Paint(TextTexture texture)
        {
            bool dirty = false;
            bool compositing = false;

            foreach (var (layerType, layer) in texture.DirtyLayers())
            {
                Terminal.Layer(layerType.Num);

                for (int x = 0; x < texture.Width; x++)
                {
                    for (int y = 0; y < texture.Height; y++)
                    {
                        if (layer[y][x] == null)
                            continue;

                        if (layer[y][x].BackgroundColor is Color backgroundColor)
                        {
                            Terminal.Color(backgroundColor);
                            Terminal.Put(x, y, '█');
                            Terminal.Composition(true);
                            compositing = true;
                        }

                        Terminal.Color(layer[y][x].Color);

                        Terminal.Put(x, y, layer[y][x].Character);
                        if(compositing)
                            Terminal.Composition(false);
                    }
                }

                dirty = true;
            }

            if(dirty)
                Terminal.Refresh();

            texture.Clean();
        }*/

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
