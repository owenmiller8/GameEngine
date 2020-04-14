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
        List<Vector2> CollisionPoints;

        public ActorCollision(Actor actor, int radius)
        {
            Actor = actor;
            Radius = radius;
            CollisionPoints = GenerateCollisionPoints(radius);
        }

        private List<Vector2> GenerateCollisionPoints(int radius)
        {
            var center = Actor.GetCurrentTile().center;
            var points = new List<Vector2>();
            points.Add(new Vector2(center.X - 0, center.Y - radius));
            points.Add(new Vector2(center.X + radius, center.Y - 0));
            points.Add(new Vector2(center.X - 0, center.Y + radius));
            points.Add(new Vector2(center.X - radius, center.Y - 0));
            return points;
        }

        public bool CollidesWith(Tile tile)
        {
            foreach (var collisionPoint in CollisionPoints)
            {
                return ((collisionPoint.X >= tile.position.X && collisionPoint.X <= tile.position.X + (float)Game1.TileSize) &&
                    (collisionPoint.Y >= tile.position.Y && collisionPoint.Y <= tile.position.Y + (float)Game1.TileSize));
            }
            return false;
        }
        public bool CollidesWith(Actor actor)
        {
            return false;
        }
    }
}
