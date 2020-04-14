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
        public Vector2 SpriteIndex;
        public Rectangle Sprite;
        public Movement movement;
        public bool Accelerating;
        public bool Decelerating;

        public static System.Collections.Generic.List<Actor> Actors = new System.Collections.Generic.List<Actor>();


        public Actor(int maxHealth, int health, Vector2 spriteIndex, float acceleration, float deceleration, float maxVelocity)
        {
            MaxHealth = maxHealth;
            Health = health;
            SpriteIndex = spriteIndex;
            movement = new Movement(acceleration, deceleration, maxVelocity);
            Sprite = Game1.Sprites[(int)SpriteIndex.X, (int)SpriteIndex.Y];

            Actors.Add(this);
        }

        public void Update()
        {
            movement.Update();
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 scale, Texture2D spriteSheet)
        {
            if (this.OnScreen()) spriteBatch.Draw(spriteSheet, this.movement.Position, this.Sprite, Color.White, 0f, new Vector2(0, 0), scale, SpriteEffects.None, 0);
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
            for (int i = 0; i < Game1.tempMap.TileMap.GetLength(0); i++)
            {
                for (int j = 0; j < Game1.tempMap.TileMap.GetLength(1); j++)
                {
                    if (Game1.tempMap.TileMap[i, j].IsSurrounding(current))
                    {
                        surroundingTiles.Add(Game1.tempMap.TileMap[i, j]);
                    }
                }
            }
            return surroundingTiles;
        }
        public Tile GetCurrentTile()
        {
            return Game1.tempMap.TileMap[(int)System.Math.Floor(movement.Center.X / Game1.TileSize), (int)System.Math.Floor(movement.Center.Y / Game1.TileSize)];
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
