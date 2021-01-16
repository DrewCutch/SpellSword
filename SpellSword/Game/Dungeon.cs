using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.Messaging;
using SpellSword.Actors.Pathing;
using SpellSword.Engine;
using SpellSword.Engine.Components;
using SpellSword.Logging;
using SpellSword.MapGeneration;
using SpellSword.MapGeneration.Sources;
using SpellSword.Render.Lighting;
using SpellSword.Render.Particles;
using SpellSword.Time;
using SpellSword.Util;
using Troschuetz.Random;

namespace SpellSword.Game
{
    class Dungeon: ISubscriber<FloorTransitionEvent>
    {
        public string Seed { get; }

        public MessageBus MainBus { get; }

        public IReadOnlyList<Floor> Floors => _floors;

        private List<Floor> _floors;

        private Source<IBiome> _biomes;
        private Rectangle _floorBounds;

        private ResettableRandom _rng;

        private int _focusFloor;
        public int FocusFloor
        {
            get => _focusFloor;
            protected set
            {
                if(value == _focusFloor)
                    return;

                int previous = _focusFloor;
                _focusFloor = value;

                OnFloorChange?.Invoke(previous > 0 ? _floors[previous] : null, _floors[_focusFloor]);
            }

        }

        public event Action<Floor, Floor> OnFloorChange;


        private List<FloorTransitionEvent> _pendingTransitions;

        public Dungeon(MessageBus gameBus, Rectangle floorBounds, Source<IBiome> biomes, string seed)
        {
            MainBus = gameBus;

            _floorBounds = floorBounds;
            _biomes = biomes.Clone();
            Seed = seed;

            _floors = new List<Floor>();
            _pendingTransitions = new List<FloorTransitionEvent>();

            _rng = new ResettableRandom(seed);

            GenerateNextFloor();
        }

        public void Handle(FloorTransitionEvent message)
        {
            _pendingTransitions.Add(message);
        }

        private void Transition(FloorTransitionEvent transition)
        {
            if (transition.ToFloor >= _floors.Count)
                GenerateNextFloor();

            FocusFloor = transition.ToFloor;

            Floor previousFloor = _floors[transition.FromFloor];
            Floor nextFloor = _floors[transition.ToFloor];

            IGameObject gameObject = transition.Target;

            nextFloor.MapInfo.Map.AddEntity(gameObject);

            gameObject.Position = nextFloor.Entrance;

            if (gameObject is UpdatingGameObject updatingGameObject)
            {
                updatingGameObject.Timeline = nextFloor.Timeline;
            }
        }

        private void GenerateNextFloor()
        {
            Map map = new Map(_floorBounds.Width, _floorBounds.Height, Layers.Effects + 1, Distance.MANHATTAN);

            LightMap lightMap = new LightMap(_floorBounds.Width, _floorBounds.Height, map.TransparencyView);

            MapInfo mapInfo = new MapInfo(map, lightMap);

            Spawner spawner = new Spawner(map);

            IBiome nextBiome = _biomes.Pull(_rng).Use();

            Timeline nextTimeline = new Timeline();
            MessageBus nextMessageBus = new MessageBus();

            nextMessageBus.RegisterSubscriber(spawner);

            GoalMapStore goalMapStore = new GoalMapStore(map);

            new TriggerRouter(map);

            Floor nextFloor = new Floor(_floors.Count, nextTimeline, mapInfo, spawner, goalMapStore, nextMessageBus);

            // Use last to go through all steps
            nextBiome.GenerateOn(nextFloor, _floorBounds, _rng).Last();

            MessageRouter router = new MessageRouter(this, _floors.Count);

            _floors.Add(nextFloor);

            router.Attach();
        }

        public void Tick()
        {
            if (_pendingTransitions.Count > 0)
            {
                foreach (FloorTransitionEvent transition in _pendingTransitions)
                {
                    Transition(transition);
                }

                _pendingTransitions.Clear();
            }
                

            int previous = FocusFloor - 1;
            int next = FocusFloor + 1;

            if(previous > 0)
                _floors[previous].Timeline.Advance(1);

            _floors[FocusFloor].Timeline.Advance(1);
            _floors[FocusFloor].GoalMapStore.UpdateMaps();

            if(next < _floors.Count)
                _floors[next].Timeline.Advance(1);
        }

        private class MessageRouter : ISubscriber<FloorTransitionEvent>, ISubscriber<ParticleEvent>, ISubscriber<WindowEvent>, ISubscriber<LogMessage>
        {
            private Dungeon _dungeon;
            private int _floor;

            public MessageRouter(Dungeon dungeon, int floor)
            {
                _dungeon = dungeon;
                _floor = floor;
            }

            public void Attach()
            {
                _dungeon._floors[_floor].MessageBus.RegisterSubscriber<FloorTransitionEvent>(this);
                _dungeon._floors[_floor].MessageBus.RegisterSubscriber<ParticleEvent>(this);
                _dungeon._floors[_floor].MessageBus.RegisterSubscriber<WindowEvent>(this);
                _dungeon._floors[_floor].MessageBus.RegisterSubscriber<LogMessage>(this);
            }

            public void Handle(FloorTransitionEvent message)
            {
                _dungeon.Handle(message);
            }

            public void Handle(ParticleEvent message)
            {
                SendIfFocused(message);
            }

            public void Handle(WindowEvent message)
            {
                SendIfFocused(message);
            }

            public void Handle(LogMessage message)
            {
                SendIfFocused(message);
            }


            private void SendIfFocused<T>(T message)
            {
                if (_dungeon.FocusFloor != _floor)
                    return;

                _dungeon.MainBus.Send(message);
            }
        }
    }
}
