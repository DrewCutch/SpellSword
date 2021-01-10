using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoRogue;
using GoRogue.Messaging;
using SpellSword.Time;

namespace SpellSword.Actors.Effects
{
    class EffectTarget
    {
        private static readonly Dictionary<Type, Type[]> TypeTreeCache = new Dictionary<Type, Type[]>();

        private readonly Dictionary<Type, List<object>> _targets;
        
        private readonly List<ActiveEffect> _activeEffects;

        public EffectTarget()
        {
            _targets = new Dictionary<Type, List<object>>();
            _activeEffects = new List<ActiveEffect>();
        }

        public void AddEffectReceiver<T>(T receiver)
        {
            Type receiverType = receiver.GetType();

            if (!TypeTreeCache.ContainsKey(receiverType))
                TypeTreeCache[receiverType] = Reflection.GetTypeTree(receiverType).ToArray();

            foreach (Type type in TypeTreeCache[receiverType])
            {
                if (!_targets.ContainsKey(type))
                {
                    _targets[type] = new List<object>();
                }

                _targets[type].Add(receiver);
            }
        }

        public void ApplyEffect<T>(IEffect<T> effect)
        {
            if (!_targets.TryGetValue(typeof(T), out List<object> effectTargets)) 
                return;

            foreach (T effectTarget in effectTargets)
            {
                effect.Apply(effectTarget);

                if (effect.Kind != EffectKind.OneShot)
                    _activeEffects.Add(new ActiveEffect<T>(effect, effectTarget, effect.Timing, effect.Kind == EffectKind.Repeating));
            }
        }

        public void UpdateActiveEffects(int ticks)
        {
            foreach (ActiveEffect activeEffect in _activeEffects)
            {
                activeEffect.Update(ticks);
            }

            _activeEffects.RemoveAll((effect) => effect.Complete);
        }

        private abstract class ActiveEffect
        {
            public bool Complete => !Timer.Active;

            private readonly EventTimer Timer;

            protected ActiveEffect(EventTimerTiming timing, bool loop)
            {
                Timer = new EventTimer(timing, looping: loop);
            }

            public void Update(int ticks)
            {
                Timer.Advance(ticks);

                foreach (EventTimer.EventTimerMoment pendingMoment in Timer.PendingMoments)
                {
                    if(pendingMoment == EventTimer.EventTimerMoment.Action)
                        OnEffectTrigger();
                    else if(pendingMoment == EventTimer.EventTimerMoment.End)
                        OnEffectEnd();
                }
            }

            protected abstract void OnEffectTrigger();

            protected abstract void OnEffectEnd();
        }

        private class ActiveEffect<T>: ActiveEffect
        {
            private readonly IEffect<T> _effect;
            private readonly T _target;

            public ActiveEffect(IEffect<T> effect, T target, EventTimerTiming timing, bool loop) : base(timing, loop)
            {
                _effect = effect;
                _target = target;
            }

            protected override void OnEffectTrigger()
            {
                _effect.Apply(_target);
            }

            protected override void OnEffectEnd()
            {
                if(_effect.Kind == EffectKind.Passive)
                    _effect.UnApply(_target);
            }
        }
    }
}
