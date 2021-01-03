using System;
using System.Collections.Generic;
using System.Text;

namespace SpellSword.RPG
{
    sealed class PronounSet
    {
        public string Subject { get; }
        public string Object { get; }

        public string Possessive { get; }

        public static PronounSet SecondPerson = new PronounSet("you", "you", "your");
        public static PronounSet ThirdPerson = new PronounSet("they", "them", "their");

        private PronounSet(string subject, string @object, string possessive)
        {
            Subject = subject;
            Object = @object;
            Possessive = possessive;
        }
    }
}
