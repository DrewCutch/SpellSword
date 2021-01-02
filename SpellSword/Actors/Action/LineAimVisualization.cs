using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using GoRogue;
using SpellSword.Render;
using SpellSword.Render.Particles;

namespace SpellSword.Actors.Action
{
    class LineAimVisualization : IAimVisualization
    {
        public bool Changed { get; private set; }
        public bool Finished { get; private set; }

        private readonly Actor _from;

        private Coord _target;

        private readonly Color _color;

        private readonly int _range;

        public LineAimVisualization(Actor from, Coord target, Color color, int maxRange)
        {
            _from = from;
            _target = target;
            _color = color;
            _range = maxRange;
            Changed = true;
        }

        public IEnumerable<Particle> Particles()
        {
            Changed = false;
            return Lines.Get(_from.Parent.Position, _target, Lines.Algorithm.DDA)
                .Where(coOrd => Distance.EUCLIDEAN.Calculate(_from.Parent.Position, coOrd) <= _range)
                .Select(coOrd => new Particle(new Glyph(_color), coOrd));
        }

        public void Update(long deltaTime)
        {
            // TODO: add animation support
        }

        public void UpdateTarget(Coord newTarget)
        {
            _target = newTarget;
            Changed = true;
        }

        public void Stop()
        {
            Finished = true;
        }
    }
}
