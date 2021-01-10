using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using BearLib;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapViews;
using GoRogue.Messaging;
using SpellSword.Actors;
using SpellSword.Engine;
using SpellSword.Input;
using SpellSword.Render.Lighting;

namespace SpellSword.Render.Panes
{
    class MapViewPane: Pane
    {
        public IMapView<IEnumerable<Glyph>> GlyphMapView { get; }

        private readonly MessageBus _controlBus;
        private readonly LightMap _lightMap;
        private readonly IMapView<bool> _visibilityMap;
        private readonly IMapView<bool> _exploredMap;

        private readonly InventoryDisplayPane _playerInventoryDisplayPane;


        private readonly JoystickConfig _joystickConfig;

        private bool _inventoryOpen;


        private Coord _offset;
        public Coord Offset
        {
            get => _offset;
            private set
            {
                if (value.X >= 0 && value.Y >= 0)
                    _offset = value;
            }
        }

        public MapViewPane(Map map, LightMap lightMap, MessageBus controlBus, JoystickConfig controls, InventoryDisplayPane playerInventoryDisplayPane)
        {
            GlyphMapView = new GlyphRenderTranslationMap(map);
            _visibilityMap = map.FOV.BooleanFOV;
            _exploredMap = map.Explored;
            _lightMap = lightMap;
            _controlBus = controlBus;
            _joystickConfig = controls;
            _playerInventoryDisplayPane = playerInventoryDisplayPane;

            _inventoryOpen = false;

            Dirty = true;
        }

        public void SetDirty()
        {
            Dirty = true;
        }

        public override bool Paint(IWriteable writeContext)
        {
            if (!Dirty)
                return false;

            writeContext.Clear();

            for (int x = 0; x < writeContext.Width && x + Offset.X < GlyphMapView.Width; x++)
                for (int y = 0; y < writeContext.Height && y + Offset.Y < GlyphMapView.Height; y++)
                {
                    if (!_exploredMap[x + Offset.X, y + Offset.Y])
                        continue;

                    foreach (Glyph glyph in GlyphMapView[x + Offset.X, y + Offset.Y])
                    {
                        if(_visibilityMap[x + Offset.X, y + Offset.Y])
                            writeContext.WriteGlyph(y, x, glyph.MultipliedByColor(_lightMap[x + Offset.X, y + Offset.Y]));
                        else
                            writeContext.WriteGlyph(y, x, glyph.BlackAndWhite());
                    }
                }

            Dirty = false;

            return true;
        }

        public override void OnMouseMove(Coord last, Coord current)
        {
            _controlBus.Send(new MouseMoveEvent(last, current));
        }

        public void ToggleInventory()
        {
            if (!_inventoryOpen)
            {
                _controlBus.Send(WindowEvent.Open(_playerInventoryDisplayPane, new GoRogue.Rectangle(new Coord(Width / 2, Height / 2), Width / 4, Height / 4), Color.DarkGray));
                _inventoryOpen = true;
            }
            else
            {
                _controlBus.Send(WindowEvent.Close(_playerInventoryDisplayPane));
                _inventoryOpen = false;
            }
        }

        public override void OnKeyDown(int keyCode)
        {
            if (keyCode == _joystickConfig.MapUp)
            {
                Offset += new Coord(0, -1);
                SetDirty();
            }
            else if (keyCode == _joystickConfig.MapDown)
            {
                Offset += new Coord(0, 1);
                SetDirty();
            }
            else if (keyCode == _joystickConfig.MapLeft)
            {
                Offset += new Coord(-1, 0);
                SetDirty();
            }
            else if (keyCode == _joystickConfig.MapRight)
            {
                Offset += new Coord(1, 0);
                SetDirty();
            }
            else
            {
                _controlBus.Send(new KeyEvent(keyCode));
            }
        }
    }
}
