using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.Messaging;
using SpellSword.Actors;
using SpellSword.Actors.Action;
using SpellSword.Actors.Pathing;
using SpellSword.Engine;
using SpellSword.Engine.Components;
using SpellSword.Render;
using SpellSword.RPG;
using SpellSword.RPG.Alignment;
using SpellSword.RPG.Items;
using SpellSword.Speech;
using SpellSword.Time;

namespace SpellSword.TestUtils
{
    static class TestUtil
    {
        private static Alignment goblins = new Alignment("goblins", AlignmentRelation.Enemy);

        private static Item gold = new Item(new Title("", "gold coin"), "shiny gold coin", new Glyph(Characters.DOLLAR_SIGN, Color.Gold),
            EquipmentSlotKind.None, 1);

        public static GameObject CreateGoblin(Coord pos, Timeline timeline, GoalMapStore goalMapStore, MessageBus mainBus) { 
            GameObject goblin = new GameObject(pos, Layers.Main, null, isWalkable:true);

            Item mainWeapon = new MeleeWeapon(new Damage(2)); // new ProjectileWeapon(3);

            Being goblinBeing = new Being(new SelectedAttributes(new AttributeSet(4, 4, 4, 4, 3)), goblins, new EquipmentSlotSet(), 2, "goblin");

            goblinBeing.Equipment.Equip(mainWeapon, EquipmentSlot.RightHandEquip);
            goblinBeing.Inventory.Add(gold, 10);

            Actor goblinActor = new Actor(goblinBeing, new BasicAgent(goalMapStore), mainBus);
            goblin.AddComponent(goblinActor);
            goblin.AddComponent(new GlyphComponent(new Glyph(Characters.g, Color.ForestGreen)));
            goblin.AddComponent(new NameComponent(new Title("a","goblin")));

            EffectTargetComponent effectTarget = new EffectTargetComponent();
            effectTarget.EffectTarget.AddEffectReceiver(goblinActor);
            goblin.AddComponent(effectTarget);

            timeline.OnAdvance += goblinActor.Update;

            return goblin;
        }

        
        public static GameObject CreateGoblinArcher(Coord pos, Timeline timeline, GoalMapStore goalMapStore, MessageBus mainBus)
        {
            GameObject goblin = new UpdatingGameObject(pos, Layers.Main, null, timeline, isWalkable: true);

            Item mainWeapon = new ProjectileWeapon(new Damage(1), 5);

            Being goblinBeing = new Being(new SelectedAttributes(new AttributeSet(4, 5, 4, 4, 3)), goblins, new EquipmentSlotSet(), 2, "goblin");

            goblinBeing.Equipment.Equip(mainWeapon, EquipmentSlot.RightHandEquip);
            goblinBeing.Inventory.Add(gold, 10);

            Actor goblinActor = new Actor(goblinBeing, new BasicAgent(goalMapStore), mainBus);
            goblin.AddComponent(goblinActor);
            goblin.AddComponent(new GlyphComponent(new Glyph(Characters.g, Color.GreenYellow)));
            goblin.AddComponent(new NameComponent(new Title("a", "goblin")));

            EffectTargetComponent effectTarget = new EffectTargetComponent();
            effectTarget.EffectTarget.AddEffectReceiver(goblinActor);
            goblin.AddComponent(effectTarget);

            timeline.OnAdvance += goblinActor.Update;

            return goblin;
        }
        
    }
}
