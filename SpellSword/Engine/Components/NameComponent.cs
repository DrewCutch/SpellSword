﻿using System;
using System.Collections.Generic;
using System.Text;
using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using SpellSword.Speech;

namespace SpellSword.Engine.Components
{
    class NameComponent: Component
    {
        public Title Title { get; }

        public NameComponent(Title title)
        {
            Title = title;
        }
    }
}
