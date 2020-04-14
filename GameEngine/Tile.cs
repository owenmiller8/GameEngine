using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class Tile
    {
        public Vector2 position;
        public Vector2 center;
        public Rectangle sprite;
        public bool collision;
        public float Friction;
        public bool seen;

        public Tile(Vector2 pos, Rectangle spr, bool coll, float friction)
        {
            position = pos;
            sprite = spr;
            collision = coll;
            Friction = friction;
            center = new Vector2(position.X + Game1.TileSize / 2, position.Y + Game1.TileSize / 2);
        }
        public bool IsSurrounding(Tile tile)
        {
            return System.Math.Abs(this.position.X) - System.Math.Abs(tile.position.X) == Game1.TileSize && System.Math.Abs(this.position.Y) - System.Math.Abs(tile.position.Y) == Game1.TileSize;
        }
        public override string ToString()
        {
            return position.ToString();
        }
    }
}
