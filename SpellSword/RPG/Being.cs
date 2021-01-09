using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Text;
using SpellSword.Engine.Components;
using SpellSword.RPG;
using SpellSword.RPG.Alignment;
using SpellSword.RPG.Attributes;
using SpellSword.RPG.Items;
using SpellSword.Time;

namespace SpellSword
{
    class Being: IDamagable
    {
        public AttributeSet Attributes
        {
            get;
            private set;
        }

        private readonly IAttributeSource _attributeSource;

        private int _level;
        public int Level
        {
            get => _level;
            private set
            {
                if (value == Level)
                    return;

                _level = value;
                Attributes = _attributeSource.GetAttributes(_level);
            }
        }

        private IOperator _levelFormula;

        private int _xp;
        private int Xp
        {
            get => _xp;
            set { 
                _xp = value;
                Level = _levelFormula.Calculate(_xp);
            }
        }

        public string Name { get; private set; }

        public Alignment Alignment { get; }

        public ResourceMeter Health { get; }
        public ResourceMeter Stamina { get; }
        public ResourceMeter Mana { get; }

        public EquipmentSlotSet Equipment { get; }

        public Inventory Inventory { get; }

        public event Action OnKilled;

        public Being(IAttributeSource attributeSource, Alignment alignment, EquipmentSlotSet equipment, int level, string name)
        {
            _attributeSource = attributeSource;
            Alignment = alignment;
            Level = level;
            Equipment = equipment;
            Name = name;

            Inventory = new Inventory();

            Health = new ResourceMeter(20, 5);
            Health.OnDeplete += OnZeroHealth;

            Stamina = new ResourceMeter(20, 0);
            Mana = new ResourceMeter(20, 0);
        }

        private void OnZeroHealth()
        {
            OnKilled?.Invoke();
        }

        public void AddXp(int xp)
        {
            Xp += xp;
        }

        public EventTimerTiming GetMovementTiming()
        {
            return new EventTimerTiming(500 / Attributes.Dexterity, 500 / Attributes.Dexterity);  
        }

        public void DoDamage(Damage damage)
        {
            Health.Expend(damage.Amount);

            if(Health.CurrentValue <= 0)
                Console.WriteLine("Killed!");
        }
    }
}
