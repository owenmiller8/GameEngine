using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameEngine
{

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static System.Random Randomiser = new System.Random();
        //public static randomInt = Randomiser.Next

        Texture2D spriteSheet;    //the sprites
        public static Rectangle[,] sprites;      //the positions of sprites on the spritesheet
        Tile[,] tileMap;         //the tile 
        const int tileOriginalSize=16; //original size in tilemap
		public static int tileSize=64; //scaled size in tilemap
        Vector2 scale;              // tileSize/tileOriginalSize
        Map tempMap;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            //graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            scale=new Vector2(tileSize / tileOriginalSize, tileSize / tileOriginalSize);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            System.IO.FileStream fileStream = new System.IO.FileStream("C:\\Users\\Pertt\\Source\\Repos\\GameEngine\\GameEngine\\Content\\sprites\\spritesheet.png", System.IO.FileMode.Open);
            spriteSheet = Texture2D.FromStream(GraphicsDevice, fileStream);
            fileStream.Dispose();


			sprites = new Rectangle[spriteSheet.Width / tileOriginalSize, spriteSheet.Height / tileOriginalSize];
			for(int i = 0; i < spriteSheet.Height / tileOriginalSize; i++)
			{
				for(int j = 0; j < spriteSheet.Width / tileOriginalSize; j++)
				{
					sprites[i, j] = new Rectangle(j * tileOriginalSize, i * tileOriginalSize, tileOriginalSize, tileOriginalSize);
				}
			}
            tempMap = new Map(10);
        }
        protected override void UnloadContent()
        {
        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            foreach(Tile tile in tempMap.tileMap)
            {
                spriteBatch.Draw(spriteSheet, tile.position, tile.sprite, Color.White, 0f, new Vector2(0,0), scale, SpriteEffects.None, 0);
            }
            
			spriteBatch.End();

            base.Draw(gameTime);
        }
    }
    public class Map
    {
        public int mapSize;
        public Tile[,] tileMap;

        public Map(int size)
        {
            mapSize = size;
            tileMap = new Tile[mapSize, mapSize];
            GenerateLevel();
        }
        private void GenerateLevel()
        {
            for(int i = 0; i < tileMap.GetLength(0); i++)
            {
                for(int j = 0; j < tileMap.GetLength(1); j++)
                {
                    tileMap[i, j] = new Tile(new Vector2(i * Game1.tileSize, j * Game1.tileSize), Game1.sprites[Game1.Randomiser.Next(0, 2), Game1.Randomiser.Next(0, 2)], false);
                }
            }
        }
    }
    public class Tile
    {
        public Vector2 position;
        public Rectangle? sprite;
        public bool collision;

        public bool seen;

        public Tile(Vector2 pos, Rectangle spr, bool coll)
        {
            position = pos;
            sprite = spr;
            collision = coll;
        }
    }
}
