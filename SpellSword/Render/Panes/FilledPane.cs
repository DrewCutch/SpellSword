namespace SpellSword.Render.Panes
{
    class FilledPane: Pane
    {
        public readonly Glyph Fill;

        public FilledPane(Glyph fill)
        {
            Fill = fill;
        }

        public override bool Paint(IWriteable writeContext)
        {
            for (int i = 0; i < writeContext.Width; i++)
            {
                for (int j = 0; j < writeContext.Height; j++)
                {
                    writeContext.SetGlyph(j, i, Layer.Main, Fill);
                }
            }

            return true;
        }
    }
}
