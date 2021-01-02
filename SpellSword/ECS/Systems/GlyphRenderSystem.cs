using System;
using System.Collections.Generic;
using System.Text;
using Artemis;
using Artemis.Manager;
using Artemis.System;
using BearLib;
using SpellSword.ECS.Components;

namespace SpellSword.ECS.Systems
{
    [Artemis.Attributes.ArtemisEntitySystem(ExecutionType = ExecutionType.Synchronous, GameLoopType = GameLoopType.Draw, Layer = 1)]
    class GlyphRenderSystem: EntityComponentProcessingSystem<GameObjectComponent, GlyphComponent>
    {
        public override void Process(Entity entity, GameObjectComponent gameObjectComponent, GlyphComponent glyph)
        {
            Terminal.Color(glyph.Color);
            Terminal.Put(gameObjectComponent.GameObject.Position.X, gameObjectComponent.GameObject.Position.Y, glyph.Character);
        }
    }
}
