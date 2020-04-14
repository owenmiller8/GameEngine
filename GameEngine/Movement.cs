using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class Movement
    {
        public Vector2 Position;
        public Vector2 Velocity = new Vector2(0, 0);
        public Vector2 Center;
        public readonly float MaxVelocity;
        public readonly float Acceleration;
        public readonly float Deceleration;

        public Movement(float acceleration, float decelaration, float maxVelocity)
        {
            Acceleration = acceleration;
            Deceleration = decelaration;
            MaxVelocity = maxVelocity;
            Center = new Vector2(Position.X + Game1.TileSize / 2, Position.Y + Game1.TileSize / 2);
        }

        public virtual void Update()
        {
            Center.X = Position.X + Game1.TileSize / 2;
            Center.Y = Position.Y + Game1.TileSize / 2;
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            Position.X += Velocity.X;
            Position.Y += Velocity.Y;
        }
    }
}
