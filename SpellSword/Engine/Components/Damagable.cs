using System;
using System.Collections.Generic;
using System.Text;

namespace SpellSword.Engine.Components
{
    class Damagable: Component, IDamagable
    {
        public int CurrentHealth;
        public int Health;
        public Damagable(int health)
        {
            Health = health;
            CurrentHealth = health;
        }
        public void DoDamage(Damage damage)
        {
            CurrentHealth -= damage.Amount;

            if (CurrentHealth <= 0)
                Parent.CurrentMap.RemoveEntity(Parent);
        }
    }
}
