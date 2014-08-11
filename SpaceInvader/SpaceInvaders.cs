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


    //MAIN
    public class SpaceInvaders : Microsoft.Xna.Framework.Game
    {

        public SpaceInvaders()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
            graphics.ApplyChanges();

            Content.RootDirectory = "Content";
        }

        //Delclaring Game Attributes
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont GameFont;

        //Declaring All the Textures
        Texture2D StarfieldImg;
        Texture2D InvaderImg;
        Texture2D AltInvaderImg;
        Texture2D RocketLauncherImg;
        Texture2D MissileImg;
        Texture2D UFOimg;
       
        //Sound Effects
        SoundEffect InvaderSound;
        SoundEffectInstance InvaderSoundInstance;

        SoundEffect CollisionSound;
        SoundEffectInstance CollisionSoundInstance;

        SoundEffect ShootSound;
        SoundEffectInstance ShootSoundInstance;

        SoundEffect ThemeSound;
        SoundEffectInstance ThemeSoundInstance;
     
        //Create Game Variables
        int RocketXPos;

       

        int AlienDirection;
        int AlienSpeed;
        
        int PlayerScore;

        double ufoTime;

        double Ticks;

        int GameState;

        int GameLives;
        
        //Creating the random number for UFO apperance and invader shooting
        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        //Creating a UFO class, not refactored as only 2 variables
        class UFO
        {
            public int UFOxPos = 1;
            public bool ufoAlive = false;  
        }

        //Create Class Instances
        Missile MissileFired;
        Invader[] Invaders;
        UFO ufo;
        Missile invaderMissile;


        

        //INITIALISE

        //Game Start Screen
        protected override void Initialize()
        {
            InitialiseGameVariables();
            GameState = 1;
            base.Initialize();
        }


        protected  void InitialiseGameVariables()
        {
            
            //initialise variables.

            GameLives = 3;
            RocketXPos = 512;

            AlienDirection = -1;
            AlienSpeed = 16;

            Invaders = new Invader[55];
            
            int XPos = 512;
            int posY = 100;
            int posX = 512;

            //Generating the invaders. 5 blocks of code for each row
                for (int Count = 0; Count < 11; Count++)
                    {
                        Invaders[Count] = new Invader();
                        Invaders[Count].SetXPos(posX);
                        Invaders[Count].SetYPos(posY);

                        posX = posX + 32;
                        XPos = XPos + 32;
                    }
            //Resetting the X position
                posX = 512;

                for (int Count = 11; Count < 22; Count++)
                    {
                        posY = 150;
                        Invaders[Count] = new Invader();
                        Invaders[Count].SetXPos(posX);
                        Invaders[Count].SetYPos(posY);

                        posX = posX + 32;
                        XPos = XPos + 32;
                    }

                posX = 512;

                for (int Count = 22; Count < 33; Count++)
                    {
                        posY = 200;
                        Invaders[Count] = new Invader();
                        Invaders[Count].SetXPos(posX);
                        Invaders[Count].SetYPos(posY);

                        posX = posX + 32;
                        XPos = XPos + 32;
                    }
                posX = 512;

                for (int Count = 33; Count < 44; Count++)
                    {
                        posY = 250;
                        Invaders[Count] = new Invader();
                        Invaders[Count].SetXPos(posX);
                        Invaders[Count].SetYPos(posY);

                        posX = posX + 32;
                        XPos = XPos + 32;
                    }
                posX = 512;

                for (int Count = 44; Count < 55; Count++)
                    {
                        posY = 300;
                        Invaders[Count] = new Invader();
                        Invaders[Count].SetXPos(posX);
                        Invaders[Count].SetYPos(posY);

                        posX = posX + 32;
                        XPos = XPos + 32;
                    }

            
            //Setting More Game Variables & Instances
            Ticks = 0;
            MissileFired = null;
            PlayerScore = 0;
            ufo = new UFO();
            base.Initialize();
            invaderMissile = null;
            

        }

        //LOAD CONTENT
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load All The Game Images
            StarfieldImg = Content.Load<Texture2D>("Starfield1024x768");
            InvaderImg = Content.Load<Texture2D>("inv1");
            AltInvaderImg = Content.Load<Texture2D>("inv12");
            RocketLauncherImg = Content.Load<Texture2D>("LaserBase");
            MissileImg = Content.Load<Texture2D>("bullet");
            UFOimg = Content.Load<Texture2D>("ufo");

            //Load The Game Font
            GameFont = Content.Load<SpriteFont>("GameFont");

            //Load All Sounds
            InvaderSound = Content.Load<SoundEffect>("invadermove");
            InvaderSoundInstance = InvaderSound.CreateInstance();

            ShootSound = Content.Load<SoundEffect>("shoot");
            ShootSoundInstance = ShootSound.CreateInstance();

            CollisionSound = Content.Load<SoundEffect>("invaderkilled");
            CollisionSoundInstance = CollisionSound.CreateInstance();

            ThemeSound = Content.Load<SoundEffect>("theme");
            ThemeSoundInstance = ThemeSound.CreateInstance();
            //loop the theme sound
            ThemeSoundInstance.IsLooped = true;

        }

        //UNLOAD CONTENT
        protected override void UnloadContent()
        {
            //BLANK
        }


        //GAME UPDATE
        
        //Screen 1
        public void UpdateStarted(GameTime currentTime)
        {
            //if S is pressed, start game, i.e. state 2
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                GameState = 2;
            }
            //play theme song (on loop)
            ThemeSoundInstance.Play();
        }


        // Main Update
        public void UpdatePlaying(GameTime currentTime)
        {
            //stop the theme song during the gameplay
            ThemeSoundInstance.Stop();

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // These statements check to see if there are any invaders remaining to shoot
            bool IsInvaderRemaining = false;
            for (int Count = 0; Count < 55; Count++)
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

                GameState = 4;

                return;
            }
            //move missle if active
            if (MissileFired != null)
            {

                MissileFired.Move();
            }

            //if space is pressed, initiate missile
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                //i found it irritating that pressing space would remove the existing missile
                //now, a missile will only shoot if there is no existing missile
                if (MissileFired == null)
                {
                    //at location of rocket
                    MissileFired = new Missile(RocketXPos, 650);

                    //play shooting sound
                    ShootSoundInstance.Play();
                }


            }

            //with the added code for pressing space,
            //we need to kill the missile if it goes off the screen.
            //otherwise if the missile misses an invader, it will continue forever you you will be unable to shoot another
            //if there is a missile

            if (MissileFired != null)
            {
                //above 1 Y co-ordinate
                if (MissileFired.GetPosition().Y < 1)
                {
                    //kill
                    MissileFired = null;
                }
            }

            // Move the rocket left and right
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                RocketXPos = RocketXPos - 4;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                RocketXPos = RocketXPos + 4;
            }

            //Keep the rocket on the screen
            if (RocketXPos < 100)
            {
                RocketXPos = 100;
            }

            if (RocketXPos > 924)
            {
                RocketXPos = 924;
            }


            //Moving The Invaders
            Ticks = Ticks + currentTime.ElapsedGameTime.TotalMilliseconds;

            //variable for changing speed
            int speedInt = 500;

            //vairable for choosing invader
            int invaderChoose = 0;

            //find the first invader which isnt dead
            do
            {
                invaderChoose = invaderChoose + 1;
            }
            while (Invaders[invaderChoose] == null);

            //assign speed depending on y position of selected invader

            if (Invaders[invaderChoose].GetYPos() < 200)
            {
                speedInt = 500;
            }
            else if (Invaders[invaderChoose].GetYPos() > 200)
            {
                speedInt = 300;
            }
            else if (Invaders[invaderChoose].GetYPos() > 400)
            {
                speedInt = 100;
            }

            // move invaders
            if (Ticks > speedInt)
            {
                //Play sound each time they move
                InvaderSoundInstance.Play();
                //setting initial speed and direction
                for (int Count = 0; Count < 55; Count++)
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
                            Invaders[Count].MoveVertical(8);
                            Invaders[Count].SetXPos(XPos);
                        }

                        XPos = XPos + InvaderImg.Width;
                    }

                    XPos = 96;
                    for (int Count = 11; Count < 22; Count++)
                    {

                        if (Invaders[Count] != null)
                        {
                            Invaders[Count].MoveVertical(8);
                            Invaders[Count].SetXPos(XPos);
                        }

                        XPos = XPos + InvaderImg.Width;
                    }

                    XPos = 96;
                    for (int Count = 22; Count < 33; Count++)
                    {

                        if (Invaders[Count] != null)
                        {
                            Invaders[Count].MoveVertical(8);
                            Invaders[Count].SetXPos(XPos);
                        }

                        XPos = XPos + InvaderImg.Width;
                    }

                    XPos = 96;
                    for (int Count = 33; Count < 44; Count++)
                    {

                        if (Invaders[Count] != null)
                        {
                            Invaders[Count].MoveVertical(8);
                            Invaders[Count].SetXPos(XPos);
                        }

                        XPos = XPos + InvaderImg.Width;
                    }
                    XPos = 96;
                    for (int Count = 44; Count < 55; Count++)
                    {

                        if (Invaders[Count] != null)
                        {
                            Invaders[Count].MoveVertical(8);
                            Invaders[Count].SetXPos(XPos);
                        }

                        XPos = XPos + InvaderImg.Width;
                    }
                }


                // Keeping launcher in constraints of invaders
                if (RocketXPos < LeftMostInvader.GetXPos())
                {
                    RocketXPos = LeftMostInvader.GetXPos();
                }
                if (RocketXPos > RightMostInvader.GetXPos())
                {
                    RocketXPos = RightMostInvader.GetXPos();
                }

                // Moving Invaders left and right
                if (RightMostInvader.GetXPos() > 924)
                {
                    AlienDirection = -1;

                    int XPos = 924 - InvaderImg.Width * 11;
                    for (int Count = 0; Count < 11; Count++)
                    {
                        if (Invaders[Count] != null)
                        {
                            Invaders[Count].MoveVertical(8);
                            Invaders[Count].SetXPos(XPos);
                        }

                        XPos = XPos + InvaderImg.Width;
                    }

                    XPos = 924 - InvaderImg.Width * 11;
                    for (int Count = 11; Count < 22; Count++)
                    {
                        if (Invaders[Count] != null)
                        {
                            Invaders[Count].MoveVertical(8);
                            Invaders[Count].SetXPos(XPos);
                        }

                        XPos = XPos + InvaderImg.Width;
                    }

                    XPos = 924 - InvaderImg.Width * 11;
                    for (int Count = 22; Count < 33; Count++)
                    {
                        if (Invaders[Count] != null)
                        {
                            Invaders[Count].MoveVertical(8);
                            Invaders[Count].SetXPos(XPos);
                        }

                        XPos = XPos + InvaderImg.Width;
                    }

                    XPos = 924 - InvaderImg.Width * 11;
                    for (int Count = 33; Count < 44; Count++)
                    {
                        if (Invaders[Count] != null)
                        {
                            Invaders[Count].MoveVertical(8);
                            Invaders[Count].SetXPos(XPos);
                        }

                        XPos = XPos + InvaderImg.Width;
                    }

                    XPos = 924 - InvaderImg.Width * 11;
                    for (int Count = 44; Count < 55; Count++)
                    {
                        if (Invaders[Count] != null)
                        {
                            Invaders[Count].MoveVertical(8);
                            Invaders[Count].SetXPos(XPos);
                        }

                        XPos = XPos + InvaderImg.Width;
                    }
                }
                //Resetting millisecond count for moving invaders.
                Ticks = 0;
            }

            // UFO

            //Generate Random Value Between 1 and 500
            int ReturnValue = RandomNumber(1, 500);

            //If There is no active UFO and The Random Number Is 1, Then Set ufoalive To True
            if (ufo.ufoAlive == false && ReturnValue == 1)
            {
                ufo.ufoAlive = true;
            }

            //Count milliseconds
            ufoTime = ufoTime + currentTime.ElapsedGameTime.TotalMilliseconds;

            // If Ufoalive is true, i.e. there is an active ufo, increment  X position by 10 pixels every 100 milliseconds
            // then reset ufotime variable to zero
            if (ufo.ufoAlive == true)
            {
                if (ufoTime > 100)
                {
                    ufo.UFOxPos = ufo.UFOxPos + 10;
                    ufoTime = 0;
                }

            }

            // if ufo position goes further than 1000 pixels, set ufoalive to false, i.e. kill the ufo. so a new one can be generated.
            if (ufo.UFOxPos > 1000)
            {
                ufo.ufoAlive = false;
                ufo.UFOxPos = 10;
            }

            // if a missile is fired
            if (MissileFired != null)
            {
                //rectangle for missile
                Rectangle rectMissile = new Rectangle((int)MissileFired.GetPosition().X, (int)MissileFired.GetPosition().Y, MissileImg.Width, MissileImg.Height);
                //for each invader
                for (int Count = 0; Count < 55; Count++)
                {
                    if (Invaders[Count] != null)
                    {
                        Rectangle rectInvader = new Rectangle(Invaders[Count].GetXPos(), Invaders[Count].GetYPos(), InvaderImg.Width, InvaderImg.Height);
                        //Check for invader collision with the missile
                        if (rectMissile.Intersects(rectInvader))
                        {
                            //set invader and missile to null
                            Invaders[Count] = null;
                            MissileFired = null;
                            //increase score and play collision sound
                            PlayerScore = PlayerScore + 100;
                            CollisionSoundInstance.Play();

                            break;
                        }
                    }
                }

                //checking for collison with ufo
                Rectangle rectUfo = new Rectangle(ufo.UFOxPos, 10, UFOimg.Width, UFOimg.Height);
                if (rectMissile.Intersects(rectUfo))
                {
                    //add score, jump UFO to end position, Play Collision Sound 
                    PlayerScore = PlayerScore + 1000;
                    ufo.UFOxPos = 1000;

                    CollisionSoundInstance.Play();
                }
            }

            //check for collision between invader and rocker launcher
            for (int Count2 = 0; Count2 < 55; Count2++)
            {
                if (Invaders[Count2] != null)
                {
                    Rectangle rectInvader2 = new Rectangle(Invaders[Count2].GetXPos(), Invaders[Count2].GetYPos(), InvaderImg.Width, InvaderImg.Height);
                    Rectangle rectRocket = new Rectangle(RocketXPos, 650, RocketLauncherImg.Width, RocketLauncherImg.Height);

                    if (rectInvader2.Intersects(rectRocket))
                    {
                        //if collision occurs, remove a life
                        GameLives = GameLives - 1;
                    }
                }
            }


            //Makeing the invaders fire back
            //using a new missile class which goes the opposite direction to the existing missile class
            // i felt it was easier to seperate the two classes completely than create another instance of the existing missile class and creating a reverse move.

            //if an invader isnt currently firing

            if (invaderMissile == null)
            {
                //generate random number for possibility of fire
                int RandomInvaderMissileFired = RandomNumber(1, 500);

                //if random number is equal to 1, fire a missle
                if (RandomInvaderMissileFired == 1)
                {
                    //generate second random number for choosing which invader

                    int ChooseInvader = RandomNumber(1, 55);
                    //if the invader is not dead
                    if (Invaders[ChooseInvader] != null)
                    {
                        //create new missile
                        invaderMissile = new Missile(Invaders[ChooseInvader].GetXPos(), Invaders[ChooseInvader].GetYPos());
                    }
                }
            }

            //if missile is existant
            if (invaderMissile != null)
            {
                //check for collisions
                Rectangle rectRocket = new Rectangle(RocketXPos, 650, RocketLauncherImg.Width, RocketLauncherImg.Height);
                Rectangle rectInvaderMissile = new Rectangle((int)invaderMissile.GetPosition().X, (int)invaderMissile.GetPosition().Y, MissileImg.Width, MissileImg.Height);

                if (rectInvaderMissile.Intersects(rectRocket))
                {
                    //if collision, remove life
                    GameLives = GameLives - 1;
                    invaderMissile = null;
                }
                //kill if no longer visible
                else if (invaderMissile.GetPosition().Y > 700)
                {
                    invaderMissile = null;
                }
                // else move the missile
                else
                {
                    invaderMissile.MoveReverse();
                }
            }

            //check remaining lives
            if (GameLives < 0)
            {
                //end game to end screen if none left
                GameState = 3;
                return;
            }



        }
            
            
            
        

        // end screen - failure
        public void UpdateEnded(GameTime currentTime)
        {
            //display two options
            if (Keyboard.GetState().IsKeyDown(Keys.X))
            {
                this.Exit();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                InitialiseGameVariables();
                GameState = 1;
            }

        }
        //end screen - won
        public void UpdateWon(GameTime currentTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.X))
            {
                this.Exit();
            }
        }

        protected override void Update(GameTime gameTime)
        {
            switch (GameState)
            {
                case 1: UpdateStarted(gameTime);
                    break;

                case 2: UpdatePlaying(gameTime);
                    break;

                case 3: UpdateEnded(gameTime);
                    break;
                case 4: UpdateWon(gameTime);
                    break;
            }

            base.Update(gameTime);
        }
        
        
        
        
        
        //DRAW


        //Draw Start Screen
        public void DrawStarted(GameTime currentTime)
        {
            spriteBatch.Begin();

            //draw required text
            // create strings and use vector positioning to centre the text
            //use draw method to assign colour and position.
            spriteBatch.Draw(StarfieldImg, Vector2.Zero, Color.White);

            Vector2 StringDimensions = GameFont.MeasureString("S P A C E   I N V A D E R S!");

            int XPos = (1024 - (int)StringDimensions.X) / 2;

            spriteBatch.DrawString(GameFont, "S P A C E   I N V A D E R S!", new Vector2(XPos, 200), Color.LightGreen);

            StringDimensions = GameFont.MeasureString("P R E S S   'S'   T O    S T A R T");

            XPos = (1024 - (int)StringDimensions.X) / 2;

            spriteBatch.DrawString(GameFont, "P R E S S   'S'   T O    S T A R T", new Vector2(XPos, 300), Color.LightGreen);

            StringDimensions = GameFont.MeasureString("I N S T R U C T I O N S :");

            XPos = (1024 - (int)StringDimensions.X) / 2;

            spriteBatch.DrawString(GameFont, "I N S T R U C T I O N S :", new Vector2(XPos, 450), Color.LightGreen);


            StringDimensions = GameFont.MeasureString("U S E   L E F T   A N D   R I G H T  A R R O W S   T O   M O V E");

            XPos = (1024 - (int)StringDimensions.X) / 2;

            spriteBatch.DrawString(GameFont, "U S E   L E F T   A N D   R I G H T  A R R O W S   T O   M O V E", new Vector2(XPos, 500), Color.LightGreen);

            StringDimensions = GameFont.MeasureString("P R E S S   S P A C E   T O   S H O O T");

            XPos = (1024 - (int)StringDimensions.X) / 2;

            spriteBatch.DrawString(GameFont, "P R E S S   S P A C E   T O   S H O O T", new Vector2(XPos, 550), Color.LightGreen);



            spriteBatch.End();

        }


        public void DrawPlaying(GameTime currentTime)
        {
            spriteBatch.Begin();

            //Draw Images, BackGround And Rocket
            spriteBatch.Draw(StarfieldImg, Vector2.Zero, Color.White);
            spriteBatch.Draw(RocketLauncherImg, new Vector2(RocketXPos, 650), Color.White);

            //Draw Missiles If Fired
            if (MissileFired != null)
            {
                Vector2 MissilePos = new Vector2(MissileFired.GetPosition().X, MissileFired.GetPosition().Y - MissileImg.Height);
                spriteBatch.Draw(MissileImg, MissilePos, Color.White);
            }

            //Draw invader missile if fired
            if (invaderMissile != null)
            {
                Vector2 InvaderMissilePos = new Vector2(invaderMissile.GetPosition().X, invaderMissile.GetPosition().Y);
                spriteBatch.Draw(MissileImg, InvaderMissilePos, Color.White);
            }

            //Draw Invaders If Not Null
            for (int Count = 0; Count < 55; Count++)
            {
                if (Invaders[Count] != null)
                {
                    spriteBatch.Draw(InvaderImg, Invaders[Count].GetPos(), Color.White);
                }
            }

            //Draw Scoreing
            //Create String (Writeline Statement)
            string ScoreText = String.Format("Score = {0}", PlayerScore);
            //Draw String, Assign Location, font etc.
            spriteBatch.DrawString(GameFont, ScoreText, new Vector2(10, 10), Color.White);

            //Draw Lives
            //Create String
            string LivesText = String.Format("Lives = {0}", GameLives);
            //Draw String
            spriteBatch.DrawString(GameFont, LivesText, new Vector2(10, 40), Color.White);

           

            //if the ufo is alive, i.e ufoalive is true, then draw the ufo
            if (ufo.ufoAlive == true)
            {
                spriteBatch.Draw(UFOimg, new Vector2(ufo.UFOxPos, 20), Color.White);
            }

            spriteBatch.End();
        }

        //Draw Fail Screen
        public void DrawEnded(GameTime currentTime)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(StarfieldImg, Vector2.Zero, Color.White);


            string YouLost = String.Format("Y O U   L O S T");

            Vector2 StringDimensions = GameFont.MeasureString(YouLost);

            int XPos = (1024 - (int)StringDimensions.X) / 2;  

            spriteBatch.DrawString(GameFont, "Y O U   L O S T", new Vector2(XPos, 100), Color.LightGreen);


            string FinalScoreString = String.Format("Final score = {0}", PlayerScore);

            StringDimensions = GameFont.MeasureString(FinalScoreString);
            
            XPos = (1024 - (int)StringDimensions.X) / 2;

            spriteBatch.DrawString(GameFont, FinalScoreString, new Vector2(XPos, 200), Color.LightGreen);


            StringDimensions = GameFont.MeasureString("P R E S S   'R'   T O    R E S T A R T   G A M E");

            XPos = (1024 - (int)StringDimensions.X) / 2;

            spriteBatch.DrawString(GameFont, "P R E S S   'R'   T O    R E S T A R T   G A M E", new Vector2(XPos, 300), Color.LightGreen);


            StringDimensions = GameFont.MeasureString("P R E S S   'X'   T O    E X I T   G A M E");

            XPos = (1024 - (int)StringDimensions.X) / 2;

            spriteBatch.DrawString(GameFont, "P R E S S   'X'   T O    E X I T   G A M E", new Vector2(XPos, 400), Color.LightGreen);


            spriteBatch.End();
        }

        //Draw Win Screen
        public void DrawWon(GameTime currentTime)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(StarfieldImg, Vector2.Zero, Color.White);

            string FinalScoreString = String.Format("F I N A L   S C O R E = {0}", PlayerScore);

            Vector2 StringDimensions = GameFont.MeasureString(FinalScoreString);

            int XPos = (1024 - (int)StringDimensions.X) / 2;

            spriteBatch.DrawString(GameFont, FinalScoreString, new Vector2(XPos, 200), Color.LightGreen);

            StringDimensions = GameFont.MeasureString("Y O U   W O N   T H E   G A M E");

            XPos = (1024 - (int)StringDimensions.X) / 2;

            spriteBatch.DrawString(GameFont, "Y O U   W O N   T H E   G A M E", new Vector2(XPos, 300), Color.LightGreen);

            StringDimensions = GameFont.MeasureString("P R E S S   'X'   T O    E X I T   G A M E");

            XPos = (1024 - (int)StringDimensions.X) / 2;

            spriteBatch.DrawString(GameFont, "P R E S S   'X'   T O    E X I T   G A M E", new Vector2(XPos, 400), Color.LightGreen);

            spriteBatch.End();

        }
     


        protected override void Draw(GameTime gameTime)
        {
            switch (GameState)
            {
                case 1: DrawStarted(gameTime);
                    break;

                case 2: DrawPlaying(gameTime);
                    break;

                case 3: DrawEnded(gameTime);
                    break;
                case 4: DrawWon(gameTime);
                    break;
            }

            base.Draw(gameTime);
        }
    }
}
