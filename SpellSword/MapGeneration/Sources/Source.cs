using Troschuetz.Random;

namespace SpellSword.MapGeneration.Sources
{
    public static class Source
    {
        public static Source<TContext, TValue> From<TContext, TValue>(TValue value)
        {
            return new ConstantSource<TContext, TValue>(value);
        }
    }

    public abstract class Source<TContext, TValue>
    {
        public bool Shared { get; }

        protected Source(bool shared)
        {
            Shared = shared;
        }

        public abstract SourceCursor<TValue> Pull(TContext context);
        public abstract bool IsEmpty();

        public abstract Source<TContext, TValue> Clone();

        public static implicit operator Source<TContext, TValue>(TValue value)
        {
            return new ConstantSource<TContext, TValue>(value);
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
