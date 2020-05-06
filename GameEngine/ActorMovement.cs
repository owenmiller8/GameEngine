using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class ActorMovement
    {
        public Vector2 Velocity = new Vector2(0, 0);
        public Actor Actor;
        public readonly float MaxVelocity;
        public readonly float Acceleration;
        public readonly float Deceleration;

        public ActorMovement(Actor actor, float acceleration, float decelaration, float maxVelocity)
        {
            Acceleration = acceleration;
            Deceleration = decelaration;
            MaxVelocity = maxVelocity;
            Actor = actor;
            
        }

        public virtual void Update()
        {
            UpdatePosition();
            Actor.Collision.UpdateCollisionPoints();
        }

        private void UpdatePosition()
        {
            Actor.Position.X += Velocity.X;
            Actor.Position.Y += Velocity.Y;
            Actor.Center.X = Actor.Position.X + Game.TileSize / 2;
            Actor.Center.Y = Actor.Position.Y + Game.TileSize / 2;
        }
    }
}
