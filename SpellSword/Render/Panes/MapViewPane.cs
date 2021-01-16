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
        public GlyphRenderTranslationMap GlyphMapView { get; protected set; }

        private Map _map;
        public Map Map
        {
            get => _map;
            set
            {
                _map = value;
                _visibilityMap = UseFOV ? _map.FOV.BooleanFOV : new LambdaMapView<bool>(_map.Width, _map.Height, (pos) => true);
                _exploredMap = UseFOV ? (IMapView<bool>)_map.Explored : new LambdaMapView<bool>(_map.Width, _map.Height, (pos) => true);
                GlyphMapView = new GlyphRenderTranslationMap(_map);
                Dirty = true;
            }
        }

        private readonly MessageBus _controlBus;
        private LightMap _lightMap;
        public LightMap LightMap
        {
            get => _lightMap;
            set
            {
                _lightMap = value;
                Dirty = true;
            }
        }

        public bool UseFOV { get; }

        private IMapView<bool> _visibilityMap;
        private IMapView<bool> _exploredMap;

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

        public MapViewPane(MessageBus controlBus, JoystickConfig controls, InventoryDisplayPane playerInventoryDisplayPane, bool useFOV = true)
        {
            UseFOV = useFOV;

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
            if (!Dirty || _map == null)
                return false;

            writeContext.Clear();

            for (int x = 0; x < writeContext.Width && x + Offset.X < GlyphMapView.Width; x++)
                for (int y = 0; y < writeContext.Height && y + Offset.Y < GlyphMapView.Height; y++)
                {
                    if (!_exploredMap[x + Offset.X, y + Offset.Y])
                        continue;

                    foreach (GlyphRenderTranslationMap.MapGlyph glyph in GlyphMapView[x + Offset.X, y + Offset.Y])
                    {
                        if(_visibilityMap[x + Offset.X, y + Offset.Y])
                            writeContext.WriteGlyph(y, x, glyph.SelfLit ? glyph.Glyph : glyph.Glyph.MultipliedByColor(_lightMap[x + Offset.X, y + Offset.Y]));
                        else
                            writeContext.WriteGlyph(y, x, glyph.Glyph.MultipliedByColor(Color.FromArgb(50, 50, 50)));
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
