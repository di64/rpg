using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace aar
{
    public abstract class Entity
    {
        public abstract bool IsVisible(Graphics TargetSurface, Rectangle Viewport);

        public abstract void Draw(Graphics TargetSurface, Rectangle Viewport);

        public abstract bool Collide(Entity CollidedWith);

        public abstract bool Activate(Entity ActivatedBy);

        public abstract bool Attack(Entity AttackedBy);
    }
}
