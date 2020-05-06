using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine
{
    public class Map
    {
        public int MapSize;
        public Tile[,] TileMap;

        public Map(int size)
        {
            MapSize = size;
            TileMap = new Tile[MapSize, MapSize];
            GenerateLevel();
        }
        private void GenerateLevel()
        {
            for (int i = 0; i < TileMap.GetLength(0); i++)
            {
                for (int j = 0; j < TileMap.GetLength(1); j++)
                {
                    TileMap[i, j] = new Tile(new Vector2(i * Game.TileSize, j * Game.TileSize), Game.Sprites[Game.Randomiser.Next(0, 2), Game.Randomiser.Next(0, 2)], System.Convert.ToBoolean(Game.Randomiser.Next(0, 2)), 0f); //Generate random map
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 scale, Texture2D spriteSheet)
        {
            foreach (Tile tile in this.TileMap)
            {
                spriteBatch.Draw(spriteSheet, tile.position, tile.sprite, Color.White, 0f, new Vector2(0, 0), scale, SpriteEffects.None, 0);
                if(tile.collision) spriteBatch.Draw(Game.rectTexture, tile.center, Color.Cyan);
            }
        }
    }
}
