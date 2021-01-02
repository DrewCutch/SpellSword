using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Security.Cryptography;
using System.Text;

namespace SpellSword.RPG
{
    class ResourceMeter
    {
        public delegate void IntChange(int oldVal, int newVal);

        public int MaxCapacity { get; }
        public int CurrentCapacity { get; private set; }
        public int CurrentValue { get; private set; }

        public event Action OnDeplete;

        public event IntChange OnValueChanged; 
        public event IntChange OnCapacityChanged;

        private readonly int _decayRate;

        private int _decayCounter;

        public ResourceMeter(int maxCapacity, int decayRate = int.MaxValue)
        {
            MaxCapacity = maxCapacity;
            CurrentCapacity = maxCapacity;
            CurrentValue = maxCapacity;
            _decayRate = decayRate;
        }

        public void Expend(int value)
        {
            CurrentValue -= value;
            OnValueChanged?.Invoke(CurrentValue + value, CurrentValue);

            _decayCounter += value;

            CurrentCapacity -= _decayCounter / _decayRate;

            OnCapacityChanged?.Invoke(CurrentCapacity + _decayCounter / _decayRate, CurrentCapacity);

            _decayCounter %= _decayRate;

            if(CurrentValue <= 0)
                OnDeplete?.Invoke();
        }

        public void Exhaust(int value)
        {
            CurrentValue -= value;
            CurrentCapacity -= value;

            OnValueChanged?.Invoke(CurrentValue + value, CurrentValue);
            OnCapacityChanged?.Invoke(CurrentCapacity + value, CurrentCapacity);
        }

        public void Charge(int value)
        {
            CurrentValue = Math.Min(CurrentValue + value, CurrentCapacity);
            OnValueChanged?.Invoke(CurrentValue - value, CurrentValue);
        }
    }
}
