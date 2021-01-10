using System;
using System.Collections.Generic;
using System.Text;
using GoRogue.Messaging;
using SpellSword.Actors;
using SpellSword.Engine;
using SpellSword.Render.Panes;

namespace SpellSword.Input
{
    class GameControlConsumer: IKeyConsumer
    {
        public JoystickConfig JoystickConfig { get; }

        private MessageBus _mainBus;

        private readonly MapViewPane _mapViewPane;

        public GameControlConsumer(JoystickConfig joystickConfig, MessageBus mainBus, MapViewPane mapViewPane)
        {
            JoystickConfig = joystickConfig;
            _mainBus = mainBus;
            _mapViewPane = mapViewPane;
        }

        public bool Consume(int keyCode)
        {
            if (keyCode == JoystickConfig.Inventory)
            {
                _mapViewPane.ToggleInventory();
            }
            else
            {
                return false;
            }

            return true;
        }
    }
}
