using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Text;

namespace SpellSword.RPG.Alignment
{
    class Alignment
    {
        private readonly Dictionary<Alignment, AlignmentRelation> relations;
        public string Name { get; }
        private readonly AlignmentRelation _defaultRelation;
        private readonly bool _chaotic;

        public static Alignment ChaoticAlignment => new Alignment("chaos", AlignmentRelation.Enemy, true);

        public static Alignment PlayerAlignment => new Alignment("player", AlignmentRelation.Enemy);

        public Alignment(string name, AlignmentRelation defaultRelation, bool chaotic=false)
        {
            Name = name;
            _defaultRelation = defaultRelation;
            _chaotic = chaotic;
            relations = new Dictionary<Alignment, AlignmentRelation>();
        }

        public AlignmentRelation GetRelation(Alignment other)
        {
            if (other == this)
                return _chaotic ? AlignmentRelation.Enemy : AlignmentRelation.Ally;

            return relations.GetValueOrDefault(other, _defaultRelation);
        }
    }
}
