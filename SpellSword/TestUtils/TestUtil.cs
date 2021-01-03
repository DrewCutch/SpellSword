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
using SpellSword.Time;

namespace SpellSword.TestUtils
{
    static class TestUtil
    {
        private static Alignment goblins = new Alignment("goblins", AlignmentRelation.Enemy);

        public static GameObject CreateGoblin(Coord pos, Actor player, Timeline timeline, GoalMapStore goalMapStore, MessageBus mainBus) { 
            GameObject goblin = new GameObject(pos, Layers.Main, null, isWalkable:true);

            Item mainWeapon = new MeleeWeapon(new Damage(1)); // new ProjectileWeapon(3);

            Being goblinBeing = new Being(new SelectedAttributes(new AttributeSet(6, 15, 4, 4, 3)), goblins, new EquipmentSlotSet(), 4, "goblin");

            goblinBeing.Equipment.Equip(mainWeapon, EquipmentSlot.RightHandEquip);

            Actor goblinActor = new Actor(goblinBeing, new BasicAgent(player, goalMapStore), mainBus);
            goblin.AddComponent(goblinActor);
            goblin.AddComponent(new GlyphComponent(new Glyph('g', Color.ForestGreen)));

            goblin.AddComponent(new NameComponent("goblin"));

            timeline.OnAdvance += goblinActor.Update;

            return goblin;
        }
    }
}
