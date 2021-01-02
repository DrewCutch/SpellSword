using System;
using System.Collections.Generic;
using System.Linq;

namespace SpellSword.Time
{
    class EventTimer
    {
        public enum EventTimerMoment
        {
            Begin,
            Action,
            End
        }

        public int Leadup { get; private set; }
        public int Cooldown { get; private set; }
        public int Progress { get; private set; }

        public int Duration => Leadup + Cooldown;

        public bool Active { get; private set; }

        public bool Looping { get; private set; }

        public IEnumerable<EventTimerMoment> PendingMoments => _pendingMoments;

        private readonly List<EventTimerMoment> _pendingMoments;

        public EventTimer(Timeline timeline = null)
        {
            if(timeline != null)
                timeline.OnAdvance += Advance;
            _pendingMoments = new List<EventTimerMoment>();
            SetUp(0, 0, false);
        }

        public EventTimer()
        {
            _pendingMoments = new List<EventTimerMoment>();
            SetUp(0, 0, false);
        }

        public EventTimer(int leadup, int cooldown, Timeline timeline = null, bool active = true, bool looping = false)
        {
            if (timeline != null)
                timeline.OnAdvance += Advance;
            _pendingMoments = new List<EventTimerMoment>();

            SetUp(leadup, cooldown, active, looping);
        }

        public EventTimer(EventTimerTiming timing, Timeline timeline = null, bool active = true, bool looping = false)
        {
            if(timeline != null)
                timeline.OnAdvance += Advance;
            _pendingMoments = new List<EventTimerMoment>();

            SetUp(timing, active, looping);
        }

        public void SetUp(EventTimerTiming timing, bool active = true, bool looping = false)
        {
            SetUp(timing.Leadup, timing.Cooldown, active, looping);
        }

        public void SetUp(int leadup, int cooldown, bool active = true, bool looping = false)
        {
            Leadup = leadup;
            Cooldown = cooldown;
            Active = active;
            Looping = looping;

            Progress = 0;
        }

        public void Advance(int ticks)
        {
            _pendingMoments.Clear();
            if (!Active)
                return;

            if (Progress == 0)
                _pendingMoments.AddRange(Enumerable.Repeat(EventTimerMoment.Begin, Looping ? ticks / Duration + 1 : 1));

            if (Progress < Leadup && Progress + ticks >= Leadup)
                _pendingMoments.AddRange(Enumerable.Repeat(EventTimerMoment.Action, Looping ? (ticks - Leadup)/ Duration + 1 : 1));

            if (Progress < Cooldown + Leadup && Progress + ticks >= Cooldown + Leadup)
            {

                _pendingMoments.AddRange(Enumerable.Repeat(EventTimerMoment.End, Looping ? (ticks - Duration) / Duration  + 1 : 1));
                Active = Looping; // If looping, then active stays true, otherwise it is set to false
            }

            Progress = (Progress + ticks) % Duration;
        }

        public void Start()
        {
            Active = true;
        }
    }
}
