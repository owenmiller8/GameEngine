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
        const int TileOriginalSize=16; //original size in tilemap
		public static int TileSize=64; //scaled size in tilemap
        Vector2 scale;              // tileSize/tileOriginalSize
        public static Map tempMap;
        Player player;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            //this.graphics.SynchronizeWithVerticalRetrace = false;
            //base.IsFixedTimeStep = false;
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
            System.IO.FileStream fileStream = new System.IO.FileStream("C:\\Users\\Pertt\\Source\\Repos\\GameEngine\\GameEngine\\Content\\sprites\\spritesheet.png", System.IO.FileMode.Open);
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
            player = new Player(100, 10, new Vector2(2, 0), 0.15f, 0.4f, 2.5f);
        }
        protected override void UnloadContent()
        {
        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Actor.UpdateAll();
            System.Console.WriteLine(player.GetCurrentTile());
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            tempMap.Draw(spriteBatch, scale, spriteSheet);
            Actor.DrawAll(spriteBatch, scale, spriteSheet);
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
                    TileMap[i, j] = new Tile(new Vector2(i * Game1.TileSize, j * Game1.TileSize), Game1.Sprites[Game1.Randomiser.Next(0, 2), Game1.Randomiser.Next(0, 2)], false, 0f); //Generate random map
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
            if(this.OnScreen()) spriteBatch.Draw(spriteSheet, this.movement.Position, this.Sprite, Color.White, 0f, new Vector2(0, 0), scale, SpriteEffects.None, 0);
        }
        public static void DrawAll(SpriteBatch spriteBatch, Vector2 scale, Texture2D spriteSheet)
        {
            Actors.ForEach(a => a.Draw(spriteBatch, scale, spriteSheet));
        }
        public static void UpdateAll()
        {
            Actors.ForEach(a => a.Update());
        }
        private Tile[] GetSurroundingTiles()
        {
            Tile current = GetCurrentTile();
            for(int i = 0; i < Game1.tempMap.TileMap.GetLength(0); i++)
            {
                for(int j=0;j<Game1.tempMap.TileMap.GetLength(1); j++)
                {
                    if(Game1.tempMap.TileMap[i,j].IsSurrounding(current))
                }
            }
            return null;
        }
        public Tile GetCurrentTile()
        {
            return Game1.tempMap.TileMap[(int)System.Math.Floor(movement.Center.X/Game1.TileSize), (int)System.Math.Floor(movement.Center.Y / Game1.TileSize)];
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

    public class Player : Actor
    {
        public Player(int maxHealth, int health, Vector2 spriteIndex, float acceleration, float deceleration, float maxVelocity) : base(maxHealth, health, spriteIndex, acceleration, deceleration, maxVelocity)
        {
            movement = new PlayerMovement(acceleration, deceleration, maxVelocity);
        }
    }

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

    public class ActorCollision
    {
        public float Radius;
        public Actor Actor;

        public ActorCollision(Actor actor, int radius)
        {
            Actor = actor;
            Radius = radius;
        }

        public bool CollidesWith(Tile tile)
        {
            return false;
        }
        public bool CollidesWith(Actor actor)
        {
            return false;
        }
    }

    public class PlayerMovement : Movement
    {

        public PlayerMovement(float acceleration, float decelaration, float maxVelocity) : base(acceleration, decelaration, maxVelocity)
        {
        }
        public override void Update()
        {
            HandleFriction();
            HandleKeyInput();
            base.Update();
        }
        private void UpdateSpeed(Directions direction)
        {
            switch (direction)
            {
                case Directions.Up:
                    Velocity.Y -= Velocity.Y <= 0 ? Acceleration : Deceleration;
                    if (-Velocity.Y > MaxVelocity) Velocity.Y = -MaxVelocity;
                    break;
                case Directions.Right:
                    Velocity.X += Velocity.X >= 0 ? Acceleration : Deceleration;
                    if (Velocity.X > MaxVelocity) Velocity.X = MaxVelocity;
                    break;
                case Directions.Down:
                    Velocity.Y += Velocity.Y >= 0 ? Acceleration : Deceleration;
                    if (Velocity.Y > MaxVelocity) Velocity.Y = MaxVelocity;
                    break;
                case Directions.Left:
                    Velocity.X -= Velocity.X <= 0 ? Acceleration : Deceleration;
                    if (-Velocity.X > MaxVelocity) Velocity.X= -MaxVelocity;
                    break;
            }
            
            
        }
        private float GetFriction()
        {
            return 0.03f;
        }

        private void HandleFriction()
        {
            float friction = GetFriction();
            
            if (Velocity.Y != 0) Velocity.Y += Velocity.Y > 0 ? -friction : friction;
            if (Velocity.X != 0) Velocity.X += Velocity.X > 0 ? -friction : friction;
            if (System.Math.Abs(Velocity.Y) < friction) Velocity.Y = 0;
            if (System.Math.Abs(Velocity.X) < friction) Velocity.X = 0;
        }
        private void HandleKeyInput()
        {
            foreach (Keys key in Keyboard.GetState().GetPressedKeys())
            { 
                switch (key)
                {
                    case Keys.W:
                        UpdateSpeed(Directions.Up);
                        break;
                    case Keys.D:
                        UpdateSpeed(Directions.Right);
                        break;
                    case Keys.S:
                        UpdateSpeed(Directions.Down);
                        break;
                    case Keys.A:
                        UpdateSpeed(Directions.Left);
                        break;
                }
            }
        }
        enum Directions
        {
            Up,
            Right,
            Down,
            Left
        }
    }
}
