using System;
using System.Collections.Generic;
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

        public bool Expend(int value)
        {
            int expended = Math.Min(value, CurrentValue);

            if (expended == 0 )
                return CurrentValue == 0;

            CurrentValue -= expended;

            OnValueChanged?.Invoke(CurrentValue + expended, CurrentValue);

            _decayCounter += expended;

            int decay = _decayRate == 0 ? 0 : _decayCounter / _decayRate;
            CurrentCapacity -= decay;

            OnCapacityChanged?.Invoke(CurrentCapacity + decay, CurrentCapacity);

            if(_decayRate != 0) 
                _decayCounter %= _decayRate;

            if(CurrentValue <= 0)
                OnDeplete?.Invoke();

            return expended == value;
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
            if (CurrentValue == CurrentCapacity)
                return;

            CurrentValue = Math.Min(CurrentValue + value, CurrentCapacity);
            OnValueChanged?.Invoke(CurrentValue - value, CurrentValue);
        }
    }
}
