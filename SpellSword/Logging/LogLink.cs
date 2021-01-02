using System.Drawing;
using GoRogue.GameFramework;
using SpellSword.ECS.Components;

namespace SpellSword.Logging
{
    public class LogLink
    {
        public string Name { get; }
        public Color Color { get; }
        public ILinkable Linkable { get; }

        public LogLink(string name, Color color, ILinkable linkable)
        {
            Name = name;
            Color = color;
            Linkable = linkable;
        }

        public override string ToString()
        {
            return $"[color]{Name}[color]";
        }
    }
}