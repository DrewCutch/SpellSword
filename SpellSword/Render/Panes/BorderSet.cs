namespace SpellSword.Render.Panes
{
    class BorderSet
    {
        public static BorderSet ThinBorder = new BorderSet('─', ' ', '│', '┘', '─', '└', '│', '┍');

        public readonly char North;
        public readonly char NorthEast;
        public readonly char East;
        public readonly char SouthEast;
        public readonly char South;
        public readonly char SouthWest;
        public readonly char West;
        public readonly char NorthWest;

        public BorderSet(char north, char northEast, char east, char southEast, char south, char southWest, char west, char northWest)
        {
            North = north;
            NorthEast = northEast;
            East = east;
            SouthEast = southEast;
            South = south;
            SouthWest = southWest;
            West = west;
            NorthWest = northWest;
        }
    }
}
