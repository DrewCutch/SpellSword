using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using GoRogue;
using GoRogue.GameFramework;
using SpellSword.Actors;
using SpellSword.Engine.Components;
using SpellSword.RPG.Items;
using SpellSword.Speech;

namespace SpellSword.Engine
{
    static class Create
    {
        public static IGameObject Corpse(Actor actor)
        {
            GameObject corpse = new GameObject(actor.Parent.Position, Layers.Floor, null, false);
            corpse.AddComponent(new GlyphComponent(actor.Parent.GetComponent<GlyphComponent>().Glyph.Character, Color.DimGray));
            corpse.AddComponent(new NameComponent(new Title("a", actor.Being.Name + " corpse")));
            return corpse;
        }

        public static IGameObject Item(Item item, Coord position)
        {
            GameObject itemObject = new GameObject(position, Layers.Floor, null, false);
            itemObject.AddComponent(new ItemComponent(item));
            itemObject.AddComponent(new GlyphComponent(item.Glyph));
            itemObject.AddComponent(new NameComponent(item.Title));

            return itemObject;
        }
    }
}
