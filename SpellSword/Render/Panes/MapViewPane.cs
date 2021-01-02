using System;
using System.Drawing;
using BearLib;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapViews;
using GoRogue.Messaging;
using SpellSword.Actors;
using SpellSword.Input;
using SpellSword.Render.Lighting;

namespace SpellSword.Render.Panes
{
    class MapViewPane: Pane
    {
        public IMapView<Glyph[]> GlyphMapView { get; }

        private bool _dirty;

        private readonly MessageBus _controlBus;
        private readonly LightMap _lightMap;
        private readonly IMapView<bool> _visibilityMap;
        private readonly IMapView<bool> _exploredMap;


        private readonly JoystickConfig _joystickConfig;


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

        public MapViewPane(Map map, LightMap lightMap, MessageBus controlBus, JoystickConfig controls)
        {
            GlyphMapView = new GlyphRenderTranslationMap(map);
            _visibilityMap = map.FOV.BooleanFOV;
            _exploredMap = map.Explored;
            _lightMap = lightMap;
            _controlBus = controlBus;
            _joystickConfig = controls;

            _dirty = true;
        }

        public void SetDirty()
        {
            _dirty = true;
        }

        public override bool Paint(IWriteable writeContext)
        {
            if (!_dirty)
                return false;

            writeContext.Clear(Layer.Main);
            writeContext.Clear(Layer.Low);

            for (int x = 0; x < writeContext.Width && x + Offset.X < GlyphMapView.Width; x++)
                for (int y = 0; y < writeContext.Height && y + Offset.Y < GlyphMapView.Height; y++)
                {
                    if (!_exploredMap[x + Offset.X, y + Offset.Y])
                        continue;


                    writeContext.SetGlyph(y, x, Layer.Main, GlyphMapView[x + Offset.X, y + Offset.Y][Layer.Main.MapNum].MultipliedByColor(_lightMap[x + Offset.X, y + Offset.Y]));
                    writeContext.SetGlyph(y, x, Layer.Low, GlyphMapView[x + Offset.X, y + Offset.Y][Layer.Low.MapNum].MultipliedByColor(_lightMap[x + Offset.X, y + Offset.Y]));
                }

            _dirty = false;

            return true;
        }

        public override void OnMouseMove(Coord last, Coord current)
        {
            _controlBus.Send(new MouseMoveEvent(last, current));
        }

        public override void OnKeyDown(int keyCode)
        {
            if (keyCode == _joystickConfig.MapUp)
            {
                Offset += new Coord(0, -1);
                SetDirty();
            }
            if (keyCode == _joystickConfig.MapDown)
            {
                Offset += new Coord(0, 1);
                SetDirty();
            }
            if (keyCode == _joystickConfig.MapLeft)
            {
                Offset += new Coord(-1, 0);
                SetDirty();
            }
            if (keyCode == _joystickConfig.MapRight)
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
