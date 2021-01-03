using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using BearLib;
using GoRogue;
using GoRogue.Messaging;
using SpellSword.Actors.Action;
using SpellSword.Input;
using SpellSword.Render;
using SpellSword.Render.Particles;
using SpellSword.Time;

namespace SpellSword.Actors
{
    class UserControlAgent: Agent, ISubscriber<KeyEvent>, ISubscriber<MouseMoveEvent>
    {
        public bool WaitingForInput { get; private set; }

        private readonly JoystickConfig _joystick;

        private KeyEvent _input;

        private MouseMoveEvent _mouseMove;

        private IAimVisualization _aimVisualization;

        public UserControlAgent(JoystickConfig joystick)
        {
            _joystick = joystick;
            WaitingForInput = true;
        }

        public override ActorAction GetNextAction(Actor actor)
        {
            if (!actor.ReadyToAct())
                return null;

            WaitingForInput = true;

            if (_input == null)
                return null;

            Direction dir = null;

            int key = _input.KeyCode;

            if (key == _joystick.UpCode)
            {
                dir = Direction.UP;
            }
            else if (key == _joystick.RightCode)
            {
                dir = Direction.RIGHT;
            }
            else if (key == _joystick.DownCode)
            {
                dir = Direction.DOWN;
            }
            else if (key == _joystick.LeftCode)
            {
                dir = Direction.LEFT;
            }

            if (dir == null)
            {
                return null;
            }

            WaitingForInput = false;
            _input = null;
            _aimVisualization?.Stop();
            _aimVisualization = null;

            if (actor.Parent.CurrentMap.GetObject(actor.Parent.Position + dir) != null && 
                actor.Being.Equipment.GetMain().CanUse(actor, actor.Parent.Position + dir))
                return new UseAction(actor, actor.Parent.Position + dir, actor.Being.Equipment.GetMain());

            if (actor.Parent.CurrentMap.GetObject(actor.Parent.Position + dir) != null && !actor.Parent.CurrentMap.GetObject(actor.Parent.Position + dir).IsWalkable)
                return null;

            return new MoveAction(actor, dir);
        }

        public void VisualizeAim(Actor actor)
        {
            if (!WaitingForInput)
                return;

            if (_mouseMove != null && _aimVisualization == null)
            {
                _aimVisualization = actor.Being.Equipment.GetMain().GetVisualization(actor, _mouseMove.NewPos);
                actor.MainBus.Send(new ParticleEvent(_aimVisualization));
            }

            if (_mouseMove != null)
            {
                _aimVisualization.UpdateTarget(_mouseMove.NewPos);
            }
        }

        public void Handle(KeyEvent key)
        {
            if (WaitingForInput && 
                (key.KeyCode == _joystick.UpCode || key.KeyCode == _joystick.DownCode || key.KeyCode == _joystick.LeftCode || key.KeyCode == _joystick.RightCode))
            {
                _input = key;
                WaitingForInput = false;
            }
        }

        public void Handle(MouseMoveEvent message)
        {
            if (!WaitingForInput)
                return;

            _mouseMove = message;
        }
    }
}
