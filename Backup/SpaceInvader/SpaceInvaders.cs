using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace SpaceInvader
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SpaceInvaders : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D StarfieldImg;
        Texture2D InvaderImg;
        Texture2D AltInvaderImg;
        Texture2D RocketLauncherImg;
        Texture2D MissileImg;

        int RocketXPos;

        int AlienDirection;
        int AlienSpeed;

        Invader[] Invaders;

        double Ticks;

        Missile MissileFired;

        public SpaceInvaders()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
            graphics.ApplyChanges();

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            RocketXPos = 512;

            AlienDirection = -1;
            AlienSpeed = 16;

            Invaders = new Invader[11];

            int XPos = 512;
            for (int Count = 0; Count < 11; Count++)
            {
                Invaders[Count] = new Invader();
                Invaders[Count].SetXPos(XPos);
                Invaders[Count].SetYPos(100);

                XPos = XPos + 32;
            }

            Ticks = 0;

            MissileFired = null;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            StarfieldImg = Content.Load<Texture2D>("Starfield1024x768");
            InvaderImg = Content.Load<Texture2D>("inv1");
            AltInvaderImg = Content.Load<Texture2D>("inv12");
            RocketLauncherImg = Content.Load<Texture2D>("LaserBase");
            MissileImg = Content.Load<Texture2D>("bullet");

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // These statements check to see if there are any invaders remaining to shoot
            bool IsInvaderRemaining = false;
            for (int Count = 0; Count < 11; Count++)
            {
                if (Invaders[Count] != null)
                {
                    IsInvaderRemaining = true;
                    break;
                }
            }

            // If there are no invaders then move to end game state
            if (!IsInvaderRemaining)
            {
                this.Exit();
            }

            if (MissileFired != null)
            {
                MissileFired.Move();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                MissileFired = new Missile(RocketXPos, 650);
            }

            // TODO: Add your update logic here
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                RocketXPos = RocketXPos - 4;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                RocketXPos = RocketXPos + 4;
            }

            if (RocketXPos < 100)
            {
                RocketXPos = 100;
            }

            if (RocketXPos > 924)
            {
                RocketXPos = 924;
            }


            Ticks = Ticks + gameTime.ElapsedGameTime.TotalMilliseconds;

            if (Ticks > 500)
            {
                for (int Count = 0; Count < 11; Count++)
                {
                    if (Invaders[Count] != null)
                    {
                        Invaders[Count].MoveHorizontal(AlienSpeed * AlienDirection);
                    }
                }

                Invader LeftMostInvader = null;
                Invader RightMostInvader = null;

                for (int Count = 0; Count < 11; Count++)
                {
                    if (Invaders[Count] != null)
                    {
                        LeftMostInvader = Invaders[Count];
                        break;
                    }
                }

                for (int Count = 10; Count > 0; Count--)
                {
                    if (Invaders[Count] != null)
                    {
                        RightMostInvader = Invaders[Count];
                        break;
                    }
                }

                if (LeftMostInvader.GetXPos() < 96)
                {
                    AlienDirection = +1;

                    int XPos = 96;
                    for (int Count = 0; Count < 11; Count++)
                    {
                        if (Invaders[Count] != null)
                        {
                            Invaders[Count].MoveVertical(4);
                            Invaders[Count].SetXPos(XPos);
                        }

                        XPos = XPos + InvaderImg.Width;
                    }
                }

                if (RightMostInvader.GetXPos() > 924)
                {
                    AlienDirection = -1;

                    int XPos = 924 - InvaderImg.Width * 10;
                    for (int Count = 0; Count < 11; Count++)
                    {
                        if (Invaders[Count] != null)
                        {
                            Invaders[Count].MoveVertical(4);
                            Invaders[Count].SetXPos(XPos);
                        }

                        XPos = XPos + InvaderImg.Width;
                    }
                }

                Ticks = 0;
            }

            if (MissileFired != null)
            {
                Rectangle rectMissile = new Rectangle((int) MissileFired.GetPosition().X, (int) MissileFired.GetPosition().Y, MissileImg.Width, MissileImg.Height);

                for (int Count = 0; Count < 11; Count++)
                {
                    if (Invaders[Count] != null)
                    {
                        Rectangle rectInvader = new Rectangle(Invaders[Count].GetXPos(), Invaders[Count].GetYPos(), InvaderImg.Width, InvaderImg.Height);

                        if (rectMissile.Intersects(rectInvader))
                        {
                            Invaders[Count] = null;
                            MissileFired = null;

                            break;
                        }
                    }
                }
            }

            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(StarfieldImg, Vector2.Zero, Color.White);

            spriteBatch.Draw(RocketLauncherImg, new Vector2(RocketXPos, 650), Color.White);

            if (MissileFired != null)
            {
                Vector2 MissilePos = new Vector2(MissileFired.GetPosition().X, MissileFired.GetPosition().Y - MissileImg.Height);

                spriteBatch.Draw(MissileImg, MissilePos, Color.White);
            }

            for (int Count = 0; Count < 11; Count++)
            {
                if (Invaders[Count] != null)
                {
                    spriteBatch.Draw(InvaderImg, Invaders[Count].GetPos(), Color.White);
                }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
