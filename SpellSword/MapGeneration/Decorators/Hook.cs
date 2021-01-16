using System;
using System.Collections.Generic;
using System.Text;
using GoRogue;
using GoRogue.GameFramework;
using SpellSword.RPG.Items;

namespace SpellSword.MapGeneration.Decorators
{
    class Hook
    {
        public HookKind Kind { get; }
        public IGameObject Target { get; }
        public Item Item { get; }
    }
}
