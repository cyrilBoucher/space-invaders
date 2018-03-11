using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.Media;
using WMPLib;

namespace SpaceInvaders
{
    class Game
    {
        #region enumeration
        public enum GameState{
            Intro,
            Menu,
            Play,
            Pause,
            Win,
            Lost
        };

        #endregion

        #region fields and properties

        #region gameplay elements
        /// <summary>
        /// Player ship
        /// </summary>
        public SpaceShip playerShip = null;
        /// <summary>
        /// Player missile
        /// </summary>
        public Missile playerMissile = null;
        /// <summary>
        /// Current game state
        /// </summary>
        public GameState currentState;
        /// <summary>
        /// List of bunkers
        /// </summary>
        public List<Bunker> bunkers;
        /// <summary>
        /// List of missiles
        /// </summary>
        public List<Missile> missiles = new List<Missile>();
        /// <summary>
        /// List of bonuses
        /// </summary>
        public List<Bonus> bonus = new List<Bonus>();
        /// <summary>
        /// Enemy block
        /// </summary>
        public EnemyBlock enemyBlock = null;
        /// <summary>
        /// Player's current bonus
        /// </summary>
        public Bonus playerBonus = null;
        /// <summary>
        /// Probabily of a bonus dropping
        /// </summary>
        public double bonusProbability = 0.5;
        /// <summary>
        /// Is the third bonus actived?
        /// </summary>
        public bool bonus3Activated = false;
        /// <summary>
        /// Counter for an activated bonus
        /// </summary>
        private static Stopwatch timeBonus = new Stopwatch();
        /// <summary>
        /// Time between waves
        /// </summary>
        private static Stopwatch timeBetweenWaves = new Stopwatch();
        /// <summary>
        /// Counter for the win screen music
        /// </summary>
        private static Stopwatch timeWinMusic = new Stopwatch();
        /// <summary>
        /// Counter for the lost screen music
        /// </summary>
        private static Stopwatch timeLostMusic = new Stopwatch();
        /// <summary>
        /// Player's speed
        /// </summary>
        private double playerSpeed = 300;

        /*delegate void BonusAction();
        public BonusAction action;*/
        #endregion

        #region game technical elements
        /// <summary>
        /// Size of the game area
        /// </summary>
        public Size gameSize;
        /// <summary>
        /// Win screen instance
        /// </summary>
        public Win winScreen = null;
        /// <summary>
        /// Lost screen instance
        /// </summary>
        public Lost lostScreen = null;
        /// <summary>
        /// Main menu instance
        /// </summary>
        public MainMenu mainMenu = new MainMenu();
        /// <summary>
        /// Player score
        /// </summary>
        public Score playerScore = null;
        /// <summary>
        /// Intro instance
        /// </summary>
        public Intro intro = new Intro();
        /// <summary>
        /// List of sound clips
        /// </summary>
        List<WMPLib.WindowsMediaPlayer> Sounds = new List<WMPLib.WindowsMediaPlayer>();
        /// <summary>
        /// Wether LostMusic was played
        /// </summary>
        bool lostMusicPlayed = false;
        /// <summary>
        /// Wether WinMusic was played
        /// </summary>
        bool winMusicPlayed = false;

        /// <summary>
        /// Keyboard state
        /// </summary>
        public bool KeySpacePressed { get; set; }
        public bool KeyLeftPressed { get; set; }
        public bool KeyRightPressed { get; set; }
        public bool KeyPPressed { get; set; }
        public bool KeyBPressed { get; set; }
        public bool KeyUpPressed { get; set; }
        public bool KeyDownPressed { get; set; }
        public bool KeyEnterPressed { get; set; }

        #endregion

        #region static fields (helpers)

        /// <summary>
        /// Singleton for easy access
        /// </summary>
        public static Game game { get; private set; }

        /// <summary>
        /// A shared black brush
        /// </summary>
        private static Brush blackBrush = new SolidBrush(Color.Black);

        /// <summary>
        /// A shared simple font
        /// </summary>
        private static Font defaultFont = new Font("Times New Roman", 24, FontStyle.Bold, GraphicsUnit.Pixel);

        /// <summary>
        /// A shared custom font
        /// </summary>
        public static Font ethnocentric15 = new Font(GameForm.pfc.Families[0], 15);

        /// <summary>
        /// Random number
        /// </summary>
        public static Random prob = new Random();
        #endregion

        #endregion

        #region constructors
        /// <summary>
        /// Singleton constructor
        /// </summary>
        /// <param name="gameSize">Size of the game area</param>
        /// 
        /// <returns></returns>
        public static Game CreateGame(Size gameSize)
        {
            if (game == null)
            {
                game = new Game(gameSize);
            }
            return game;
        }

        /// <summary>
        /// Private constructor
        /// </summary>
        /// <param name="gameSize">Size of the game area</param>
        private Game(Size gameSize)
        {
            this.gameSize = gameSize;
            playerShip = new SpaceShip(space_invaders.Properties.Resources.ship1,
                                    new Vecteur2D(gameSize.Width/2 - space_invaders.Properties.Resources.ship1.Width/2,
                                        gameSize.Height - space_invaders.Properties.Resources.ship1.Height),
                                    5);
            bunkers = new List<Bunker>();
            for (int i = gameSize.Width / 10 ; i < gameSize.Width; i += gameSize.Width / 3)
                bunkers.Add(new Bunker(new Vecteur2D(i, playerShip.Position.y - 100)));

            WindowsMediaPlayer Pew = new WindowsMediaPlayer();
            WindowsMediaPlayer DoublePoints = new WindowsMediaPlayer();
            WindowsMediaPlayer Boom = new WindowsMediaPlayer();
            WindowsMediaPlayer Suck = new WindowsMediaPlayer();
            WindowsMediaPlayer BigPew = new WindowsMediaPlayer();
            WindowsMediaPlayer Bop = new WindowsMediaPlayer();
            WindowsMediaPlayer Touched = new WindowsMediaPlayer();
            WindowsMediaPlayer Down = new WindowsMediaPlayer();
            WindowsMediaPlayer Up = new WindowsMediaPlayer();
            WindowsMediaPlayer Easy = new WindowsMediaPlayer();
            WindowsMediaPlayer Medium = new WindowsMediaPlayer();
            WindowsMediaPlayer Hard = new WindowsMediaPlayer();
            WindowsMediaPlayer EasyMusic = new WindowsMediaPlayer();
            WindowsMediaPlayer MediumMusic = new WindowsMediaPlayer();
            WindowsMediaPlayer HardMusic = new WindowsMediaPlayer();
            WindowsMediaPlayer WinMusic = new WindowsMediaPlayer();
            WindowsMediaPlayer LostMusic = new WindowsMediaPlayer();
            WindowsMediaPlayer IntroMusic = new WindowsMediaPlayer();

            /*Pew.URL = Environment.CurrentDirectory.Replace("bin\\Debug", "Resources\\Pew.wav");
            DoublePoints.URL = Environment.CurrentDirectory.Replace("bin\\Debug", "Resources\\DoublePoints.wav");
            Boom.URL = Environment.CurrentDirectory.Replace("bin\\Debug", "Resources\\Boom.wav");
            Suck.URL = Environment.CurrentDirectory.Replace("bin\\Debug", "Resources\\Suck.wav");
            BigPew.URL = Environment.CurrentDirectory.Replace("bin\\Debug", "Resources\\BigPew.wav");
            Bop.URL = Environment.CurrentDirectory.Replace("bin\\Debug", "Resources\\Bop.wav");
            Touched.URL = Environment.CurrentDirectory.Replace("bin\\Debug", "Resources\\Touched.wav");
            Down.URL = Environment.CurrentDirectory.Replace("bin\\Debug", "Resources\\Down.wav");
            Up.URL = Environment.CurrentDirectory.Replace("bin\\Debug", "Resources\\Up.wav");
            Easy.URL = Environment.CurrentDirectory.Replace("bin\\Debug", "Resources\\Easy.wav");
            Medium.URL = Environment.CurrentDirectory.Replace("bin\\Debug", "Resources\\Medium.wav");
            Hard.URL = Environment.CurrentDirectory.Replace("bin\\Debug", "Resources\\Hard.wav");
            EasyMusic.URL = Environment.CurrentDirectory.Replace("bin\\Debug", "Resources\\EasyMusic.wav");
            MediumMusic.URL = Environment.CurrentDirectory.Replace("bin\\Debug", "Resources\\MediumMusic.wav");
            HardMusic.URL = Environment.CurrentDirectory.Replace("bin\\Debug", "Resources\\HardMusic.wav");
            WinMusic.URL = Environment.CurrentDirectory.Replace("bin\\Debug", "Resources\\WinMusic.wav");
            LostMusic.URL = Environment.CurrentDirectory.Replace("bin\\Debug", "Resources\\LostMusic.wav");
            IntroMusic.URL = Environment.CurrentDirectory.Replace("bin\\Debug", "Resources\\IntroMusic.wav");*/

            Pew.URL = "Resources\\Pew.wav";
            DoublePoints.URL = "Resources\\DoublePoints.wav";
            Boom.URL = "Resources\\Boom.wav";
            Suck.URL = "Resources\\Suck.wav";
            BigPew.URL = "Resources\\BigPew.wav";
            Bop.URL = "Resources\\Bop.wav";
            Touched.URL = "Resources\\Touched.wav";
            Down.URL = "Resources\\Down.wav";
            Up.URL = "Resources\\Up.wav";
            Easy.URL = "Resources\\Easy.wav";
            Medium.URL = "Resources\\Medium.wav";
            Hard.URL = "Resources\\Hard.wav";
            EasyMusic.URL = "Resources\\EasyMusic.mp3";
            MediumMusic.URL = "Resources\\MediumMusic.mp3";
            HardMusic.URL = "Resources\\HardMusic.mp3";
            WinMusic.URL = "Resources\\WinMusic.wav";
            LostMusic.URL = "Resources\\LostMusic.wav";
            IntroMusic.URL = "Resources\\IntroMusic.mp3";

            Sounds.Add(Pew);
            Sounds.Add(DoublePoints);
            Sounds.Add(Boom);
            Sounds.Add(Suck);
            Sounds.Add(BigPew);
            Sounds.Add(Bop);
            Sounds.Add(Touched);
            Sounds.Add(Down);
            Sounds.Add(Up);
            Sounds.Add(Easy);
            Sounds.Add(Medium);
            Sounds.Add(Hard);
            Sounds.Add(EasyMusic);
            Sounds.Add(MediumMusic);
            Sounds.Add(HardMusic);
            Sounds.Add(WinMusic);
            Sounds.Add(LostMusic);
            Sounds.Add(IntroMusic);

            foreach (WindowsMediaPlayer s in Sounds)
            {
                s.settings.volume = 0;
                s.controls.stop();
            }   
        }

        #endregion

        #region methods
        /// <summary>
        /// Draw the whole game
        /// </summary>
        /// <param name="g">Graphics to draw in</param>
        public void Draw(Graphics g)
        {
            if (currentState == GameState.Intro)
            {
                intro.Draw(g);
            }
            else if (currentState == GameState.Menu)
            {
                mainMenu.Draw(g);
                playerScore = new Score();
            }
            else if (currentState == GameState.Play)
            {
                if (playerShip != null)
                {
                    playerShip.Draw(g);
  
                    playerScore.Draw(g);

                    g.DrawString("Lives : " + playerShip.Lives, ethnocentric15, blackBrush, 2, 2);

                    if (playerBonus != null)
                        playerBonus.Draw(g, gameSize.Height - 20, gameSize.Width - 20);
                }
                if (playerMissile != null)
                    playerMissile.Draw(g);

                if (bunkers != null)
                {
                    foreach (Bunker b in bunkers)
                        b.Draw(g);
                }

                if (enemyBlock != null)
                {
                    enemyBlock.Draw(g);

                    foreach (Missile m in missiles)
                        m.Draw(g);
                }

                if (bonus.Count != 0)
                {
                    foreach (Bonus b in bonus)
                        b.Draw(g);
                }
            }
            else if (currentState == GameState.Pause)
            {
                g.DrawImage(space_invaders.Properties.Resources.pause,
                            gameSize.Width / 2 - space_invaders.Properties.Resources.pause.Width / 2, 
                            gameSize.Height / 2 - space_invaders.Properties.Resources.pause.Height / 2);
            }
            else if (currentState == GameState.Lost)
            {
                if (lostScreen == null)
                {
                    lostScreen = new Lost();
                    lostScreen.Draw(g);
                }
                else
                {
                    lostScreen.Draw(g);

                    if ((lostScreen.Position.y + lostScreen.imageHeight / 2) > gameSize.Height / 2)
                    {
                        playerScore.DisplayScore(lostScreen, g);
                        lostScreen.Blink();
                        lostScreen.DisplayWinMenu(g);
                    }
                }
            }
            else if (currentState == GameState.Win)
            {
                if (winScreen == null)
                {
                    winScreen = new Win();
                    winScreen.Draw(g);
                }
                else
                {
                    winScreen.Draw(g);

                    if ((winScreen.Position.y + winScreen.imageHeight / 2) > Game.game.gameSize.Height / 2)
                    {
                        playerScore.DisplayScore(winScreen, g);
                        winScreen.Blink();
                        winScreen.DisplayWinMenu(g);
                    }
                }
            }
        }

        /// <summary>
        /// Update game
        /// </summary>
        public void Update(double deltaT)
        {
            if (currentState == GameState.Intro)
            {
                //Play "IntroMusic" sound
                Sounds[17].settings.volume = 200;
                Sounds[17].controls.play();
                if (Intro.timeIntro.ElapsedMilliseconds > 2000.0)
                {
                    if (Intro.timeIntro.ElapsedMilliseconds > 4400.0)
                        intro.Move(3000, deltaT);

                    if (Intro.timeIntro.ElapsedMilliseconds > 9200.0)
                    {
                        currentState = GameState.Menu;
                        Intro.timeIntro.Reset();
                    }
                }

            }
            else if (currentState == GameState.Menu)
            {
                mainMenu.Blink();
                if (KeyDownPressed && mainMenu.currentSelection < MainMenu.DifficultyState.Hard)
                {
                    mainMenu.currentSelection++;
                    KeyDownPressed = false;

                    //Play "Down" sound
                    Sounds[7].settings.volume = 100;
                    Sounds[7].controls.play();
                }
                if (KeyUpPressed && mainMenu.currentSelection > MainMenu.DifficultyState.Easy)
                {
                    mainMenu.currentSelection--;
                    KeyUpPressed = false;

                    //Play "Up" sound
                    Sounds[8].settings.volume = 100;
                    Sounds[8].controls.play();
                }
                if (KeyEnterPressed)
                {
                    mainMenu.LevelSelection();
                    if (mainMenu.currentSelection == MainMenu.DifficultyState.Easy)
                    { 
                        //Play "Easy" sound
                        Sounds[9].settings.volume = 100;
                        Sounds[9].controls.play();
                    }
                    if (mainMenu.currentSelection == MainMenu.DifficultyState.Medium)
                    { 
                        //Play "Medium" sound
                        Sounds[10].settings.volume = 100;
                        Sounds[10].controls.play();
                    }
                    if (mainMenu.currentSelection == MainMenu.DifficultyState.Hard)
                    { 
                        //Play "Hard" sound
                        Sounds[11].settings.volume = 100;
                        Sounds[11].controls.play();
                    }

                    
                    currentState = GameState.Play;
                    
                }
            }
            else if (currentState == GameState.Play)
            {
                Sounds[17].controls.stop();
                KeyEnterPressed = false;

                //Play the level music
                if (mainMenu.currentSelection == MainMenu.DifficultyState.Easy)
                {
                    Sounds[12].settings.volume = 100;
                    Sounds[12].controls.play();
                }
                else if (mainMenu.currentSelection == MainMenu.DifficultyState.Medium)
                {
                    Sounds[13].settings.volume = 100;
                    Sounds[13].controls.play();
                }
                else if (mainMenu.currentSelection == MainMenu.DifficultyState.Hard)
                {
                    Sounds[14].settings.volume = 100;
                    Sounds[14].controls.play();
                }

                //move left
                if (KeyLeftPressed)
                {
                    if (playerShip != null)
                    {
                        if (playerShip.Position.x <= 0)
                            playerShip.Position.x = 0;
                        else
                            playerShip.MoveLeft(deltaT, playerSpeed);
                    }
                }

                //move right
                if (KeyRightPressed)
                {
                    if (playerShip != null)
                    {
                        if (playerShip.Position.x >= game.gameSize.Width - playerShip.imageWidth)
                            playerShip.Position.x = game.gameSize.Width - playerShip.imageWidth;
                        else
                            playerShip.MoveRight(deltaT, playerSpeed);
                    }
                }

                //Draw missile
                if (KeySpacePressed)
                {
                    if (playerMissile == null)
                    {
                        playerMissile = new Missile(new Vecteur2D(200, 200),
                                                    new Vecteur2D(0, 0),
                                                    1);
                        playerMissile.Position.x = playerShip.Position.x + playerShip.imageWidth / 2 - 1;
                        playerMissile.Position.y = playerShip.Position.y - playerMissile.imageHeight;

                        //Play "pew" sound
                        Sounds[0].settings.volume = 100;
                        Sounds[0].controls.play();

                    }
                }

                //Move Player missile
                if (playerMissile != null)
                    playerMissile.Move(deltaT, playerMissile.Vitesse.y);


                //Test Player missile collision
                if (playerMissile != null)
                {
                    foreach (Bunker b in bunkers)
                        b.Collision(playerMissile);

                    if (enemyBlock.Collision(playerMissile))
                    {
                        //Play "Boom" sound
                        Sounds[2].settings.volume = 100;
                        Sounds[2].controls.play();
                    }

                    // maybe dead
                    if (!playerMissile.Alive)
                        playerMissile = null;
                }

                //Pause the game
                if (KeyPPressed)
                {
                    if (currentState == GameState.Play)
                    {
                        Sounds[12].settings.volume = 20;

                        Sounds[13].settings.volume = 20;

                        Sounds[14].settings.volume = 20;

                        currentState = GameState.Pause;
                        KeyPPressed = false;
                    }
                }

                //Random enemy shoots
                if (enemyBlock != null)
                {
                    foreach (SpaceShip s in enemyBlock.Ships)
                        enemyBlock.RandomShoot(s, deltaT);
                }

                //Move Enemy Block and enemy's missiles
                if (enemyBlock != null)
                {
                    enemyBlock.Move(deltaT);

                    List<Missile> index = new List<Missile>();
                    List<Bunker> indexBunker = new List<Bunker>(); ;

                    foreach (Missile m in missiles)
                    {
                        m.Move(deltaT, m.Vitesse.y);

                        foreach (Bunker b in bunkers)
                        {
                            if (b.Collision(m))
                                index.Add(m);
                        }

                        if (playerShip.Collision(m))
                        {
                            playerShip.Position = new Vecteur2D(gameSize.Width / 2 - space_invaders.Properties.Resources.ship1.Width / 2,
                                        gameSize.Height - space_invaders.Properties.Resources.ship1.Height);
                            index.Add(m);

                            //Play "Touched" sound
                            Sounds[6].settings.volume = 100;
                            Sounds[6].controls.play();

                        }
                    }

                    if (index.Count != 0)
                    {
                        foreach (Missile m in index)
                            missiles.Remove(m);
                    }

                    foreach (Bunker b in bunkers)
                    {
                        if (enemyBlock.Collision(b))
                            indexBunker.Add(b);
                    }

                    if (indexBunker.Count != 0)
                    {
                        foreach (Bunker b in indexBunker)
                            bunkers.Remove(b);
                    }

                }

                //Move Bonus
                if (bonus.Count != 0)
                {
                    foreach (Bonus b in bonus)
                        b.Move(deltaT);

                    int index = -1;

                    foreach (Bonus b in bonus)
                    {
                        if (b.Collision(playerShip))
                        {
                            playerBonus = b;
                            index = bonus.IndexOf(b);  

                            //Play "Suck" sound
                            Sounds[3].settings.volume = 100;
                            Sounds[3].controls.play();
                        }
                        else if (b.Position.y > gameSize.Height)
                            index = bonus.IndexOf(b);
                    }

                    if (index != -1)
                        bonus.RemoveAt(index);
                }

                //Use Bonus
                if (KeyBPressed)
                {
                    if (playerBonus != null)
                    {
                        if (playerMissile == null)
                        {
                            playerBonus.action();

                            if (playerBonus.bonusName == "DoublePoints")
                            {
                                //Play "DoublePoints" sound
                                Sounds[1].settings.volume = 100;
                                Sounds[1].controls.play();
                                bonus3Activated = true;
                            }

                            if (playerBonus.bonusName == "InstantKill")
                            {
                                //Play "Bop" sound
                                Sounds[5].settings.volume = 100;
                                Sounds[5].controls.play();
                            }

                            if (playerBonus.bonusName == "BigMissile")
                            {
                                //Play "BigPew" sound
                                Sounds[4].settings.volume = 100;
                                Sounds[4].controls.play();
                            }


                            playerBonus = null;
                            KeyBPressed = false;
                        }
                    }
                }

                //Increment bonus3's time
                if (bonus3Activated)
                {
                    timeBonus.Start();

                    if (timeBonus.ElapsedMilliseconds > 5000.0)
                    {
                        bonus3Activated = false;
                        timeBonus.Reset();
                        playerScore.BonusFactor = 1;
                    }
                }

                //Win the game
                if (!enemyBlock.Alive)
                    currentState = GameState.Win;

                //Lost the game
                if (!playerShip.Alive || (enemyBlock.Position.y + enemyBlock.Size.Height) >= playerShip.Position.y)
                    currentState = GameState.Lost;

                //Check player's lives
                if (!playerShip.Alive)
                    playerShip = null;
            }
            else if (currentState == GameState.Pause)
            {
                //Unpause game
                if (KeyPPressed)
                {
                    currentState = GameState.Play;
                    KeyPPressed = false;
                }
               
            }
            else if (currentState == GameState.Win)
            {
                //Stop game music
                Sounds[12].controls.stop();

                Sounds[13].controls.stop();

                Sounds[14].controls.stop();

                //Play "WinMusic" sound
                /*if (timeWinMusic.ElapsedMilliseconds < 9000.0)
                {
                    Sounds[15].settings.volume = 100;
                    Sounds[15].controls.play();
                }

                timeWinMusic.Start();

                if (timeWinMusic.ElapsedMilliseconds > 9500.0)
                {
                    Sounds[15].settings.volume = 0;
                    Sounds[15].controls.stop();
                }*/
                if (!winMusicPlayed)
                {
                    Sounds[15].settings.volume = 100;
                    Sounds[15].controls.play();
                    winMusicPlayed = true;
                }
                

                if ((winScreen.Position.y + winScreen.imageHeight / 2) < gameSize.Height / 2)
                    winScreen.Move(deltaT);

                // Choose after game end
                winScreen.Blink();
                if (KeyDownPressed && winScreen.currentSelection < Win.AfterState.Quit)
                {
                    winScreen.currentSelection++;
                    KeyDownPressed = false;

                    //Play "Down" sound
                    Sounds[7].settings.volume = 100;
                    Sounds[7].controls.play();
                }
                if (KeyUpPressed && winScreen.currentSelection > Win.AfterState.MainMenu)
                {
                    winScreen.currentSelection--;
                    KeyUpPressed = false;

                    //Play "Up" sound
                    Sounds[8].settings.volume = 100;
                    Sounds[8].controls.play();
                }

                if (KeyEnterPressed)
                {
                    winScreen.AfterSelection();                    
                    KeyEnterPressed = false;
                }
            }
            else if (currentState == GameState.Lost)
            {
                //Stop game music
                Sounds[12].controls.stop();

                Sounds[13].controls.stop();

                Sounds[14].controls.stop();

                //Play "LostMusic" sound
                /*if (timeLostMusic.ElapsedMilliseconds < 2650.0)
                {
                    Sounds[16].settings.volume = 100;
                    Sounds[16].controls.play();
                }

                timeLostMusic.Start();

                if (timeLostMusic.ElapsedMilliseconds > 2650.0)
                {
                    Sounds[16].settings.volume = 0;
                    Sounds[16].controls.stop();
                }*/

                if (!lostMusicPlayed)
                {
                    Sounds[16].settings.volume = 100;
                    Sounds[16].controls.play();
                    lostMusicPlayed = true;
                }

                if ((lostScreen.Position.y + lostScreen.imageHeight / 2) < gameSize.Height / 2)
                    lostScreen.Move(deltaT);

                // Choose after game end
                lostScreen.Blink();
                if (KeyDownPressed && lostScreen.currentSelection < Lost.AfterState.Quit)
                {
                    lostScreen.currentSelection++;
                    KeyDownPressed = false;

                    //Play "Down" sound
                    Sounds[7].settings.volume = 100;
                    Sounds[7].controls.play();
                }
                if (KeyUpPressed && lostScreen.currentSelection > Lost.AfterState.MainMenu)
                {
                    lostScreen.currentSelection--;
                    KeyUpPressed = false;

                    //Play "Up" sound
                    Sounds[8].settings.volume = 100;
                    Sounds[8].controls.play();
                }

                if (KeyEnterPressed)
                {
                    lostScreen.AfterSelection();
                    KeyEnterPressed = false;
                }
            }              
        }

        /// <summary>
        /// Drop a random bonus
        /// </summary>
        /// <param name="position">Postion of the destroyed enemy</param>
        public void RandomBonus(Vecteur2D position)
        {
            if (Game.prob.NextDouble() < bonusProbability)
            {
                Bonus b = new Bonus(position);
                bonus.Add(b);
            }
        }

        /// <summary>
        /// Load enemy block from a txt file
        /// </summary>
        /// <param name="filename">File name</param>
        public EnemyBlock InitFromFile(string filename) 
        {
            string[] lines = System.IO.File.ReadAllLines(@"Resources\" + filename);
            string[] rec = lines[0].Split(' ');

            if (rec.Length == 2)
                enemyBlock = new EnemyBlock (new Vecteur2D(Convert.ToDouble(rec[0]),Convert.ToDouble(rec[1])));
            else
                throw new Exception();

            for (int i = 1; i < lines.Length; i++)
            {
                rec = lines[i].Split(' ');
                if (rec.Length == 4)
                {
                    switch (rec[3])
                    {
                        case "ship1":
                            enemyBlock.AddLine(Convert.ToInt32(rec[0]),
                                                Convert.ToInt32(rec[1]),
                                                Convert.ToInt32(rec[2]),
                                                space_invaders.Properties.Resources.ship1);
                            break;

                        case "ship2":
                            enemyBlock.AddLine(Convert.ToInt32(rec[0]),
                                                Convert.ToInt32(rec[1]),
                                                Convert.ToInt32(rec[2]),
                                                space_invaders.Properties.Resources.ship2);
                            break;

                        case "ship3":
                            enemyBlock.AddLine(Convert.ToInt32(rec[0]),
                                                Convert.ToInt32(rec[1]),
                                                Convert.ToInt32(rec[2]),
                                                space_invaders.Properties.Resources.ship3);
                            break;

                        case "ship4":
                            enemyBlock.AddLine(Convert.ToInt32(rec[0]),
                                                Convert.ToInt32(rec[1]),
                                                Convert.ToInt32(rec[2]),
                                                space_invaders.Properties.Resources.ship4);
                            break;

                        case "ship5":
                            enemyBlock.AddLine(Convert.ToInt32(rec[0]),
                                                Convert.ToInt32(rec[1]),
                                                Convert.ToInt32(rec[2]),
                                                space_invaders.Properties.Resources.ship5);
                            break;

                        case "ship6":
                            enemyBlock.AddLine(Convert.ToInt32(rec[0]),
                                                Convert.ToInt32(rec[1]),
                                                Convert.ToInt32(rec[2]),
                                                space_invaders.Properties.Resources.ship6);
                            break;

                        case "ship7":
                            enemyBlock.AddLine(Convert.ToInt32(rec[0]),
                                                Convert.ToInt32(rec[1]),
                                                Convert.ToInt32(rec[2]),
                                                space_invaders.Properties.Resources.ship7);
                            break;

                        case "ship8":
                            enemyBlock.AddLine(Convert.ToInt32(rec[0]),
                                                Convert.ToInt32(rec[1]),
                                                Convert.ToInt32(rec[2]),
                                                space_invaders.Properties.Resources.ship8);
                            break;

                        case "ship9":
                            enemyBlock.AddLine(Convert.ToInt32(rec[0]),
                                                Convert.ToInt32(rec[1]),
                                                Convert.ToInt32(rec[2]),
                                                space_invaders.Properties.Resources.ship9);
                            break;
                    }
                }
                else
                    throw new Exception();

            }

            return enemyBlock;
        }
        #endregion


    }
}
