using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Text;
using GoRogue;
using GoRogue.GameFramework;
using SpellSword.Engine.Components;
using SpellSword.Logging;
using SpellSword.Render;
using SpellSword.Render.Particles;
using SpellSword.RPG;
using SpellSword.Time;

namespace SpellSword.Actors.Action
{
    class MeleeWeapon: IUsable, IEquippable
    {
        public EquipmentSlotKind SlotKind => EquipmentSlotKind.Hand;
        public string Name => "sword";

        public int Range { get; }
        public Distance RangeDistanceType => Distance.MANHATTAN;
        private Damage _damage { get; }

        public MeleeWeapon(Damage damage, int range = 1)
        {
            _damage = damage;
            Range = range;
        }

        public void Use(Actor by, Coord target)
        {
            IGameObject gameObject = by.Parent.CurrentMap.GetObject(target);

            IDamagable damagable = gameObject?.GetComponent<IDamagable>();

            damagable?.DoDamage(_damage);

            if (damagable != null)
            {
                by.MainBus.Send(new ParticleEvent(new GlyphFlash(new Glyph('/', Color.Red), 200,
                    target)));

                by.MainBus.Send(new LogMessage($"{{0}} attacked with their {Name}", new LogLink(by.Being.Name, Color.Aquamarine, by)));
            }
            else
            {
                by.MainBus.Send(new ParticleEvent(new GlyphFlash(new Glyph('█', Color.FromArgb(100, Color.Gray)), 200,
                    target)));
                by.MainBus.Send(new LogMessage($"{{0}} missed with their {Name}", new LogLink(by.Being.Name, Color.Aquamarine, by)));
            }
        }

        public bool CanUse(Actor by, Coord target)
        {
            IGameObject gameObject = by.Parent.CurrentMap.GetObject(target);

            Actor actor = gameObject?.GetComponent<Actor>();

            return actor != null && Distance.MANHATTAN.Calculate(by.Parent.Position, target) <= Range;
        }

        public EventTimerTiming UseTiming(Actor by)
        {
            return new EventTimerTiming(5, 100);
        }

        public IAimVisualization GetVisualization(Actor @by, Coord target)
        {
            throw new NotImplementedException();
        }
    }
}
