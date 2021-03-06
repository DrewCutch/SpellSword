﻿using System.Drawing;
using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using GoRogue.Messaging;
using SpellSword.Actors.Action;
using SpellSword.Engine;
using SpellSword.Engine.Components;
using SpellSword.Logging;
using SpellSword.Render;
using SpellSword.Render.Particles;
using SpellSword.RPG.Items;
using SpellSword.Speech;
using SpellSword.Update;

namespace SpellSword.Actors
{
    class Actor: Component, IUpdate, IDamagable, ILinkable
    { 
        public override IGameObject Parent { get; set; }

        private ActorAction currentAction;
        public Being Being { get; }

        public Agent Agent { get; }
        
        public MessageBus MainBus { get; }

        private bool _alive;

        public Actor(Being being, Agent agent, MessageBus mainBus)
        {
            Being = being;
            Agent = agent;
            MainBus = mainBus;

            currentAction = null;

            being.OnKilled += Kill;

            _alive = true;
        }

        private void Kill()
        {
            _alive = false;
            Parent.CurrentMap.RemoveEntity(Parent);

            IGameObject corpse = Create.Corpse(this);
            MainBus.Send(new SpawnEvent(corpse, true));

            foreach (Item item in Being.Equipment.Equipped.Values)
            {
                MainBus.Send(new SpawnEvent(Create.Item(item, corpse.Position), true));
            }

            foreach (Item item in Being.Inventory.Items.Keys)
            {
                MainBus.Send(new SpawnEvent(Create.Item(item, Being.Inventory.Items[item], corpse.Position), true));
            }

        }

        public bool ReadyToAct() => currentAction == null;

        public void Do(ActorAction action)
        {
            currentAction = action;
        }

        public void Update(int ticks)
        {
            if(!_alive)
                return;

            currentAction?.Timer.Advance(ticks);

            if (currentAction != null && currentAction.Resolve())
            {
                currentAction = null;
            }

            if(ReadyToAct()) 
                currentAction = Agent.GetNextAction(this);
        }

        public void DoDamage(Damage damage)
        {
            Parent.CurrentMap.GetTerrain(Parent.Position).GetComponent<IDecalable>()?.SetDecal(new Decal(new Title("in", "a", "pool of blood"), new Glyph(Color.FromArgb(100, Color.DarkRed))));
            Being.DoDamage(damage);
        }
        

        private IParticleEffect highlight = null;

        public void OnLinkHover()
        {
            highlight = new GlyphFlash(new Glyph(Color.FromArgb(100, Color.Aqua)), 10000, Parent.Position);
            MainBus.Send(new ParticleEvent(highlight));
        }

        public void OnLinkClick()
        { }

        public void OnLinkExit()
        {
            MainBus.Send(new ParticleEvent(highlight, false));
        }
    }
}
