using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class Actor
    {
        public int MaxHealth;
        public int Health;
        public Vector2 Center;
        public Vector2 Position;
        public Vector2 SpriteIndex;
        public Rectangle Sprite;
        public ActorMovement Movement;
        public ActorCollision Collision;


        public static System.Collections.Generic.List<Actor> Actors = new System.Collections.Generic.List<Actor>();


        public Actor(int maxHealth, int health, Vector2 position, Vector2 spriteIndex, float acceleration, float deceleration, float maxVelocity)
        {
            MaxHealth = maxHealth;
            Health = health;
            SpriteIndex = spriteIndex;
            Position = position;
            Center = new Vector2(Position.X + Game.TileSize / 2, Position.Y + Game.TileSize / 2);
            Movement = new ActorMovement(this, acceleration, deceleration, maxVelocity);
            Sprite = Game.Sprites[(int)SpriteIndex.X, (int)SpriteIndex.Y];
            Collision = new ActorCollision(this, Game.TileSize / 3);
            Actors.Add(this);
        }

        public void Update()
        {
            Movement.Update();
            System.Console.WriteLine(Collision.CollidesWith(GetSurroundingTiles()));
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 scale, Texture2D spriteSheet)
        {
            if (this.OnScreen()) spriteBatch.Draw(spriteSheet, Position, this.Sprite, Color.White, 0f, new Vector2(0, 0), scale, SpriteEffects.None, 0);

            foreach(var collisionPoint in Collision.CollisionPoints)
            {
                spriteBatch.Draw(Game.rectTexture, collisionPoint, Color.Red);
            }
        }
        public static void DrawAll(SpriteBatch spriteBatch, Vector2 scale, Texture2D spriteSheet)
        {
            Actors.ForEach(a => a.Draw(spriteBatch, scale, spriteSheet));
        }
        public static void UpdateAll()
        {
            Actors.ForEach(a => a.Update());
        }
        public List<Tile> GetSurroundingTiles()
        {
            Tile current = GetCurrentTile();
            List<Tile> surroundingTiles = new List<Tile>();
            for (int i = 0; i < Game.tempMap.TileMap.GetLength(0); i++)
            {
                for (int j = 0; j < Game.tempMap.TileMap.GetLength(1); j++)
                {
                    if (Game.tempMap.TileMap[i, j].IsSurrounding(current))
                    {
                        surroundingTiles.Add(Game.tempMap.TileMap[i, j]);
                    }
                }
            }
            return surroundingTiles;
        }
        public Tile GetCurrentTile()
        {
            return Game.tempMap.TileMap[(int)System.Math.Floor(Center.X / Game.TileSize), (int)System.Math.Floor(Center.Y / Game.TileSize)];
        }
        public bool OnScreen()
        {
            return true;
        }
        public void Delete()
        {
            Actors.Remove(this);
        }
    }
}
