using Troschuetz.Random;

namespace SpellSword.MapGeneration.Sources
{
    public static class Source
    {
        public static Source<T> From<T>(T value)
        {
            return new ConstantSource<T>(value);
        }
    }

    public abstract class Source<T>
    {
        public bool Shared { get; }

        protected Source(bool shared)
        {
            Shared = shared;
        }

        public abstract SourceCursor<T> Pull(IGenerator rng);
        public abstract bool IsEmpty();

        public abstract Source<T> Clone();

        public static implicit operator Source<T>(T value)
        {
            return new ConstantSource<T>(value);
        }
    }
}

/*
    WeightedSource specialRoomsWeighted = new WeightedSource();
    specialRooms.Add(temple);
    specialRooms.Add(throneRoom);
    specialRooms.Add(jailRoom);

    LimitedSource specialRooms = new LimitedSource(specialRoomsWeighted, 1);

    WeightedSource roomNeighbors = new WeightedSource();
    roomNeighbors.Add(specialRooms, 1);
    roomNeighbors.Add(basicRoom, 10);
    
*/
