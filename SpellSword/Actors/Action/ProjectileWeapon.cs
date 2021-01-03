using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using GoRogue;
using GoRogue.GameFramework;
using SpellSword.Engine;
using SpellSword.Engine.Components;
using SpellSword.Render;
using SpellSword.Speech;
using SpellSword.Time;

namespace SpellSword.Actors.Action
{
    class ProjectileWeapon: IUsable
    {
        public Title Title { get; }
        public int Range { get; }
        public Distance RangeDistanceType => Distance.EUCLIDEAN;

        public ProjectileWeapon(int range)
        {
            Range = range;
            Title = new Title("a", "bow");
        }
        
        public void Use(Actor by, Coord target)
        {
            Direction fireDirection = Direction.GetDirection(by.Parent.Position, target);

            GameObject projectile = new UpdatingGameObject(by.Parent.Position + fireDirection, Layers.Main, null);
            projectile.AddComponent(new GlyphComponent(new Glyph('.', Color.Black)));

            ProjectileComponent projectileComponent =
                new ProjectileComponent(fireDirection.DeltaX * 50, fireDirection.DeltaY * 50);
            projectile.AddComponent(projectileComponent);



            by.MainBus.Send(new SpawnEvent(projectile));
            Console.WriteLine("Fired arrow!");
        }

        public bool CanUse(Actor by, Coord target)
        {
            IGameObject gameObject = by.Parent.CurrentMap.GetObject(target);

            Actor actor = gameObject?.GetComponent<Actor>();
            if (actor is null)
                return false;

            if (RangeDistanceType.Calculate(by.Parent.Position, target) > Range)
                return false;

            foreach (Coord point in Lines.Get(by.Parent.Position, target, Lines.Algorithm.ORTHO).Skip(1).SkipLast(1))
            {
                if (!actor.Parent.CurrentMap.WalkabilityView[point])
                    return false;
            }

            return true;
        }

        public EventTimerTiming UseTiming(Actor by)
        {
            return new EventTimerTiming(50, 150);
        }

        public IAimVisualization GetVisualization(Actor by, Coord target)
        {
            return new LineAimVisualization(by, target, Color.FromArgb(100, Color.LawnGreen), Range);
        }
    }
}
