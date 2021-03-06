﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine
{

    public class Game : Microsoft.Xna.Framework.Game
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


        Rectangle rectangle = new Rectangle(0, 0, 1, 1);
        public static Texture2D rectTexture;

        public Game()
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
            System.IO.FileStream fileStream = new System.IO.FileStream(@"C:\Users\Pertt\Source\Repos\GameEngine\GameEngine\Content\sprites\spritesheet.png", System.IO.FileMode.Open);
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
            player = new Player(100, 10, new Vector2(0, 0),new Vector2(2, 0), 0.3f, 1.0f, 1.5f);


            Color[] data = new Color[rectangle.Width * rectangle.Height];
            rectTexture = new Texture2D(GraphicsDevice, rectangle.Width, rectangle.Height);

            for (int i = 0; i < data.Length; ++i)
                data[i] = Color.White;

            rectTexture.SetData(data);

        }
        protected override void UnloadContent()
        {
        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Actor.UpdateAll();
            //System.Console.WriteLine(player.GetCurrentTile());
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            tempMap.Draw(spriteBatch, scale, spriteSheet);
            Actor.DrawAll(spriteBatch, scale, spriteSheet);
			spriteBatch.End();

            base.Draw(gameTime);
        }
    }

    

    

    

    
}
