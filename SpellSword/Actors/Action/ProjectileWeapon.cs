using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using GoRogue;
using GoRogue.GameFramework;
using SpellSword.Actors.Effects;
using SpellSword.Engine;
using SpellSword.Engine.Components;
using SpellSword.Logging;
using SpellSword.Render;
using SpellSword.Render.Particles;
using SpellSword.RPG;
using SpellSword.RPG.Items;
using SpellSword.Speech;
using SpellSword.Time;

namespace SpellSword.Actors.Action
{
    class ProjectileWeapon: Item, IUsable
    {
        public int Range { get; }
        public Distance RangeDistanceType => Distance.EUCLIDEAN;

        private Damage _damage { get; }

        private readonly int _stamina;

        public ProjectileWeapon(Damage damage, int range) : base(new Title("a", "bow"), "a short wooden bow", new Glyph(Characters.RIGHT_PARENTHESIS, Color.SaddleBrown), EquipmentSlotKind.Hand, 1)
        {
            Range = range;
            _damage = damage;

            _stamina = 3;
        }
        
        public void Use(Actor by, Coord target)
        {
            Direction fireDirection = Direction.GetDirection(by.Parent.Position, target);

            Coord spawnPos = by.Parent.Position + fireDirection;

            GameObject projectile = new UpdatingGameObject(spawnPos, Layers.Main, null, by.GameObject.Timeline);

            Characters[] alignChars = new[] {Characters.VERTICAL_LINE, Characters.SLASH, Characters.HYPHEN, Characters.BACK_SLASH, Characters.VERTICAL_LINE, Characters.SLASH, Characters.HYPHEN, Characters.BACK_SLASH};

            int dX = target.X - spawnPos.X;
            int dY = target.Y - spawnPos.Y;

            int dist = (int) Distance.MANHATTAN.Calculate(dX, dY);
            int speed = 30;

            int xVel = dX == 0 ? 0 : speed * dist / dX;
            int yVel = dY == 0 ? 0 : speed * dist / dY;


            Damagable damagable = new Damagable(1);
            projectile.AddComponent(damagable);
            projectile.AddComponent(new GlyphComponent(new Glyph(alignChars[(int) fireDirection.Type], Color.SaddleBrown))); 
            projectile.AddComponent(new NameComponent(new Title("an", "arrow")));

            ProjectileComponent projectileComponent = new ProjectileComponent(xVel, yVel, damagable);

            projectileComponent.OnCollide += (gameObject) =>
            {
                EffectTarget effectTarget = gameObject?.GetComponent<EffectTargetComponent>()?.EffectTarget;

                effectTarget?.ApplyEffect(new DamageEffect(_damage));

                by.MainBus.Send(new ParticleEvent(new GlyphFlash(new Glyph(Characters.ASTERISK, Color.Red), 200,
                    gameObject.Position)));

                
                if(gameObject.GetComponent<NameComponent>() is NameComponent nameComponent)
                    by.MainBus.Send(new LogMessage($"{{0}} was hit by {{1}}'s arrow", new LogLink(nameComponent.Title.ToString(), Color.Aquamarine, by), new LogLink(by.Being.Name, Color.Aquamarine, by)));
            };


            projectile.AddComponent(projectileComponent);

            by.MainBus.Send(new SpawnEvent(projectile));
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
            return new EventTimerTiming(100, 400);
        }

        public IAimVisualization GetVisualization(Actor by, Coord target)
        {
            return new LineAimVisualization(by, target, Color.FromArgb(100, Color.LawnGreen), Range);
        }
    }
}
