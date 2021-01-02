namespace SpellSword
{
    public class AttributeSet
    {
        public int Strength { get; }
        public int Dexterity { get; }
        public int Resolve { get; }
        public int Intelligence { get; }
        public int Magic { get; }

        public AttributeSet(int strength, int dexterity, int resolve, int intelligence, int magic)
        {
            Strength = strength;
            Dexterity = dexterity;
            Resolve = resolve;
            Intelligence = intelligence;
            Magic = magic;
        }
    }
}