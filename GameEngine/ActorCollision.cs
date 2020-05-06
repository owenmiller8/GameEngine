using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class ActorCollision
    {
        public float Radius;
        public Actor Actor;
        public List<Vector2> CollisionPoints;

        const int CollisionPointCount = 16;

        public ActorCollision(Actor actor, int radius)
        {
            Actor = actor;
            Radius = radius;
            CollisionPoints = GenerateCollisionPoints(radius);
        }

        private List<Vector2> GenerateCollisionPoints(int radius)
        {
            var center = Actor.Center;
            var points = new List<Vector2>();
            double angle = 0;
            for(int i = 0; i < CollisionPointCount; i++)
            {
                angle = System.Math.PI*2 / CollisionPointCount * i;

                //System.Console.WriteLine(angle*(180/System.Math.PI));

                points.Add(new Vector2((float)(center.X + (radius * System.Math.Cos(angle))), (float)(center.Y + (radius * System.Math.Sin(angle)))));

            }
            return points;
        }
        public void UpdateCollisionPoints()
        {
            for(int i = 0; i < CollisionPointCount; i++)
            {
                CollisionPoints[i] = new Vector2(CollisionPoints[i].X + Actor.Movement.Velocity.X, CollisionPoints[i].Y + Actor.Movement.Velocity.Y);
            }
        }
        public bool CollidesWith(Tile tile)
        {
            if (!tile.collision) return false;
            bool collides = false;
            foreach (var collisionPoint in CollisionPoints)
            {
                if ((collisionPoint.X >= tile.position.X && collisionPoint.X <= tile.position.X + Game.TileSize) &&
                    (collisionPoint.Y >= tile.position.Y && collisionPoint.Y <= tile.position.Y + Game.TileSize))
                {
                    collides = true;
                    break;
                }
            }
            return collides;
        }
        public bool CollidesWith(List<Tile> tiles)
        {
            foreach(Tile tile in tiles)
            {
                if (CollidesWith(tile)) return true;
            }
            return false;
        }
        public bool CollidesWith(Actor actor)
        {
            return false;
        }
    }
}
