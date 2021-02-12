using System;
using System.Collections.Generic;
using System.Text;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using SpellSword.Update;

namespace SpellSword.Engine.Components
{
    class ProjectileComponent: Component, IUpdate
    {

        public event Action<IGameObject> OnCollide;

        public int XRate { get; }
        public int YRate { get; }

        public int XSign { get; }

        public int YSign { get; }

        private int _xTicks;
        private int _yTicks;

        private IDamagable _destroyOnCollide;

        public ProjectileComponent(int xRate, int yRate, IDamagable destroyOnCollide=null)
        {
            XSign = Math.Sign(xRate);
            YSign = Math.Sign(yRate);

            XRate = xRate == 0 ? int.MaxValue : Math.Abs(xRate);
            YRate = yRate == 0 ? int.MaxValue : Math.Abs(yRate);

            _destroyOnCollide = destroyOnCollide;
        }

        public void Update(int ticks)
        {
            _xTicks += ticks;
            _yTicks += ticks;


            while (_xTicks >= XRate || _yTicks >= YRate)
            {
                int dX = 0;
                int dY = 0;
                if (_xTicks >= XRate)
                {
                    dX = XSign;
                    _xTicks -= XRate;
                }

                if (_yTicks >= YRate)
                {
                    dY = YSign;
                    _yTicks -= YRate;
                }

                Coord newPos = Parent.Position + new Coord(dX, dY);
                if (Parent.CurrentMap?.GetEntity<IGameObject>(newPos, Parent.CurrentMap.LayerMasker.Mask(Parent.Layer)) is IGameObject collideWith)
                {
                    OnCollide?.Invoke(collideWith);
                    _destroyOnCollide?.DoDamage(new Damage(int.MaxValue));
                    break;
                }

                if(!Parent.CurrentMap?.WalkabilityView[newPos] ?? false)
                {
                    OnCollide?.Invoke(Parent.CurrentMap.GetObject(newPos));
                    _destroyOnCollide?.DoDamage(new Damage(int.MaxValue));
                    break;
                }

                Parent.Position = newPos;
            }
        }

        public override Component CloneTo(IGameObject gameObject)
        {
            ProjectileComponent clone = new ProjectileComponent(XRate, YRate);
            gameObject.AddComponent(clone);

            return clone;
        }
    }
}
