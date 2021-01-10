using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Text;
using GoRogue;
using GoRogue.GameFramework;
using SpellSword.Actors.Effects;
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
    class MeleeWeapon: Item, IUsable
    {
        public EquipmentSlotKind SlotKind => EquipmentSlotKind.Hand;

        public int Range { get; }
        public Distance RangeDistanceType => Distance.MANHATTAN;
        private Damage _damage { get; }

        private readonly int _stamina;

        public MeleeWeapon(Damage damage, int range = 1) : base(new Title("a", "sword"), "a dull blade", new Glyph('|', Color.Silver), EquipmentSlotKind.Hand, 1)
        {
            _damage = damage;
            Range = range;

            _stamina = 5;
        }

        public void Use(Actor by, Coord target)
        {
            IGameObject gameObject = by.Parent.CurrentMap.GetObject(target);

            EffectTarget effectTarget = gameObject?.GetComponent<EffectTargetComponent>()?.EffectTarget;

            effectTarget?.ApplyEffect(new DamageEffect(_damage));
            by.Parent.GetComponent<EffectTargetComponent>()?.EffectTarget.ApplyEffect(new StaminaEffect(-_stamina));

            if (effectTarget != null)
            {
                by.MainBus.Send(new ParticleEvent(new GlyphFlash(new Glyph('/', Color.Red), 200,
                    target)));

                by.MainBus.Send(new LogMessage($"{{0}} attacked with their {Title.Name}", new LogLink(by.Being.Name, Color.Aquamarine, by)));
            }
            else
            {
                by.MainBus.Send(new ParticleEvent(new GlyphFlash(new Glyph('█', Color.FromArgb(100, Color.Gray)), 200,
                    target)));
                by.MainBus.Send(new LogMessage($"{{0}} missed with their {Title.Name}", new LogLink(by.Being.Name, Color.Aquamarine, by)));
            }
        }

        public bool CanUse(Actor by, Coord target)
        {
            if (by.Being.Stamina.CurrentValue < _stamina)
                return false;

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
