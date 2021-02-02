using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using SpellSword.RPG;

namespace SpellSword.Render.Panes
{
    class FillBarPane: Pane
    {
        public ResourceMeter ResourceMeter{ get; }
        public String Label { get; }

        private readonly Color _fillColor;
        private readonly Color _capacityColor;


        public FillBarPane(ResourceMeter meter, String label, Color fillColor, Color capacityColor)
        {
            ResourceMeter = meter;
            Label = label;
            _fillColor = fillColor;
            _capacityColor = capacityColor;
            Dirty = true;

            Height = 1;

            ResourceMeter.OnValueChanged += OnValueChanged;
            ResourceMeter.OnCapacityChanged += OnCapacityChanged;
        }

        private void OnCapacityChanged(int oldVal, int newVal)
        {
            Dirty = true;
        }

        private void OnValueChanged(int oldVal, int newVal)
        {
            Dirty = true;
        }

        public override void SuggestHeight(int height)
        {
            
        }

        public override bool Paint(Writeable writeContext)
        {
            if (!Dirty)
                return false;

            int width = writeContext.Width;
            int fill = width * ResourceMeter.CurrentValue / ResourceMeter.MaxCapacity;
            int cap = width * ResourceMeter.CurrentCapacity / ResourceMeter.MaxCapacity;

            writeContext.Clear();
            for (int i = 0; i < cap; i++)
                writeContext.SetGlyph(0, i, new Glyph(i < Label.Length ? (Characters) Label[i] : Characters.SPACE, _fillColor.Inverted(), i < fill ? _fillColor : _capacityColor));

            Dirty = false;
            return true;
        }
    }
}
