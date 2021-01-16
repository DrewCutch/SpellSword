using System;
using System.Collections.Generic;
using System.Text;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.Messaging;
using SpellSword.Actors.Pathing;
using SpellSword.Engine;
using SpellSword.MapGeneration;
using SpellSword.Time;

namespace SpellSword.Game
{
    class Floor
    {
        public int Index { get; }
        public Timeline Timeline { get; }
        public MapInfo MapInfo { get; }
        public Spawner Spawner { get; }
        public GoalMapStore GoalMapStore { get; }
        public MessageBus MessageBus { get; }
        public Coord Entrance { get; set; }
        public Coord Exit { get; set; }

        public Floor(int index, Timeline timeline, MapInfo mapInfo, Spawner spawner, GoalMapStore goalMapStore, MessageBus messageBus)
        {
            Index = index;
            Timeline = timeline;
            MapInfo = mapInfo;
            Spawner = spawner;
            GoalMapStore = goalMapStore;
            MessageBus = messageBus;
        }
    }
}
