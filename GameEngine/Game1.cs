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

        Texture2D spriteSheet;    //the sprites
        public static Rectangle[,] Sprites;      //the positions of sprites on the spritesheet
        Tile[,] tileMap;         //the tile 
        const int TileOriginalSize=16; //original size in tilemap
		public static int TileSize=64; //scaled size in tilemap
        Vector2 scale;              // tileSize/tileOriginalSize
        Map tempMap;
        Player player;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            //graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            scale = new Vector2(TileSize / TileOriginalSize, TileSize / TileOriginalSize);
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            System.IO.FileStream fileStream = new System.IO.FileStream("C:\\Users\\Owen\\source\\repos\\GameEngine\\GameEngine\\Content\\sprites\\spritesheet.png", System.IO.FileMode.Open);
            spriteSheet = Texture2D.FromStream(GraphicsDevice, fileStream);
            fileStream.Dispose();


			Sprites = new Rectangle[spriteSheet.Width / TileOriginalSize, spriteSheet.Height / TileOriginalSize];
			for(int i = 0; i < spriteSheet.Height / TileOriginalSize; i++)
			{
				for(int j = 0; j < spriteSheet.Width / TileOriginalSize; j++)
				{
					Sprites[i, j] = new Rectangle(j * TileOriginalSize, i * TileOriginalSize, TileOriginalSize, TileOriginalSize);
				}
			}
            tempMap = new Map(10);
            player = new Player(100, 100, new Vector2(2, 0), 0.05f, -0.1f);
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
            tempMap.Draw(spriteBatch, scale, spriteSheet);
            player.Draw(spriteBatch, scale, spriteSheet);
			spriteBatch.End();

            base.Draw(gameTime);
        }
    }
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
            for(int i = 0; i < TileMap.GetLength(0); i++)
            {
                for(int j = 0; j < TileMap.GetLength(1); j++)
                {
                    TileMap[i, j] = new Tile(new Vector2(i * Game1.TileSize, j * Game1.TileSize), Game1.Sprites[Game1.Randomiser.Next(0, 2), Game1.Randomiser.Next(0, 2)], false, 0f);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 scale, Texture2D spriteSheet)
        {
            foreach (Tile tile in this.TileMap)
            {
                spriteBatch.Draw(spriteSheet, tile.position, tile.sprite, Color.White, 0f, new Vector2(0, 0), scale, SpriteEffects.None, 0);
            }
        }
    }
    public class Tile
    {
        public Vector2 position;
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
        }
    }

    
    public class Actor
    {
        public int MaxHealth;
        public int Health;
        public Vector2 SpriteIndex;
        public Rectangle Sprite;
        public Movement movement;
        public bool Accelerating;
        public bool Decelerating;
        

        public Actor(int maxHealth, int health, Vector2 spriteIndex, float acceleration, float deceleration)
        {
            MaxHealth = maxHealth;
            Health = health;
            SpriteIndex = spriteIndex;
            movement = new Movement(acceleration, deceleration);
            Sprite = Game1.Sprites[(int)SpriteIndex.X, (int)SpriteIndex.Y];
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 scale, Texture2D spriteSheet)
        {
            spriteBatch.Draw(spriteSheet, this.movement.Position, this.Sprite, Color.White, 0f, new Vector2(0, 0), scale, SpriteEffects.None, 0);
        }
    }

    public class Player : Actor
    {
        public Player(int maxHealth, int health, Vector2 spriteIndex, float acceleration, float deceleration) : base(maxHealth, health, spriteIndex, acceleration, deceleration)
        {

        }
    }

    public class Movement
    {
        public Vector2 Position;
        public readonly Vector2 MaxValocity = new Vector2(2f * Game1.TileSize, 2f * Game1.TileSize);
        public Vector2 Velocity = new Vector2(0, 0);
        public readonly float Acceleration;
        public readonly float Deceleration;
        public Directions direction;

        public Movement(float acceleration, float decelaration)
        {
            Acceleration = acceleration;
            Deceleration = decelaration;
        }

        public void Update()
        {
            GetDirection();
            //UpdateSpeed();
            UpdatePosition();
        }

        private void UpdateSpeed(bool accelerating, bool decelerating)
        {
            switch (direction)
            {
                case Directions.Up:
                    Velocity.Y += accelerating ? Acceleration : decelerating ? Deceleration : 0;
                    break;
                case Directions.Right:
                    Velocity.X += accelerating ? Acceleration : decelerating ? Deceleration : 0;
                    break;
                case Directions.Down:
                    Velocity.Y -= accelerating ? Acceleration : decelerating ? Deceleration : 0;
                    break;
                case Directions.Left:
                    Velocity.X -= accelerating ? Acceleration : decelerating ? Deceleration : 0;
                    break;
            }
        }

        private void UpdatePosition()
        {
            Position.X += Velocity.X;
            Position.Y += Velocity.Y;
        }

        private void GetDirection()
        {
            foreach (Keys key in Keyboard.GetState().GetPressedKeys())
            {
                switch (key)
                {
                    case Keys.W:
                        direction = Directions.Up;
                        break;
                    case Keys.D:
                        direction = Directions.Right;
                        break;
                    case Keys.S:
                        direction = Directions.Down;
                        break;
                    case Keys.A:
                        direction = Directions.Left;
                        break;
                }
            }
        }
    }

    public enum Directions
    {
        Up,
        Right,
        Down,
        Left
    }
}
