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

        private bool _dirty;

        public FillBarPane(ResourceMeter meter, String label, Color fillColor, Color capacityColor)
        {
            ResourceMeter = meter;
            Label = label;
            _fillColor = fillColor;
            _capacityColor = capacityColor;
            _dirty = true;

            Height = 1;

            ResourceMeter.OnValueChanged += OnValueChanged;
            ResourceMeter.OnCapacityChanged += OnCapacityChanged;
        }

        private void OnCapacityChanged(int oldVal, int newVal)
        {
            _dirty = true;
        }

        private void OnValueChanged(int oldVal, int newVal)
        {
            _dirty = true;
        }

        public override void SuggestHeight(int height)
        {
            
        }

        public override bool Paint(IWriteable writeContext)
        {
            if (!_dirty)
                return false;

            int width = writeContext.Width;
            int fill = width * ResourceMeter.CurrentValue / ResourceMeter.MaxCapacity;
            int cap = width * ResourceMeter.CurrentCapacity / ResourceMeter.MaxCapacity;

            writeContext.Clear(Layer.Main);
            for (int i = 0; i < cap; i++)
                writeContext.SetGlyph(0, i, Layer.Main, new Glyph(i < Label.Length ? Label[i] : ' ', _fillColor.Inverted(), i <= fill ? _fillColor : _capacityColor));

            _dirty = false;
            return true;
        }
    }
}
