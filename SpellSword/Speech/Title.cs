using System;
using System.Collections.Generic;
using System.Text;

namespace SpellSword.Speech
{
    class Title
    {
        public string Preposition { get; }
        public string Article { get; }
        public string Name { get; }

        public Title(string preposition, string article, string name)
        {
            Preposition = preposition;
            Article = article;
            Name = name;
        }

        public Title(string article, string name)
        {
            Article = article;
            Name = name;

            Preposition = "on";
        }

        public override string ToString()
        {
            return Article + " " + Name;
        }
    }
}
