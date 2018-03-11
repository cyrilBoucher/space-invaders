using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace SpaceInvaders
{
    class Bonus
    {

        #region Fields
        /// <summary>
        /// Image
        /// </summary>
        public Bitmap image;
        /// <summary>
        /// Image proportions
        /// </summary>
        public int imageWidth, imageHeight;
        /// <summary>
        /// Bonus name
        /// </summary>
        public string bonusName;
        /// <summary>
        /// BonusAction Delegate 
        /// </summary>
        public delegate void BonusAction();
        public BonusAction action;
        

        #endregion

        #region Properties
        /// <summary>
        /// Position
        /// </summary>
        public Vecteur2D Position { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="g">Graphics used to draw the item</param>
        public void Draw (Graphics g)
        {
            g.DrawImage(image, new RectangleF((float)Position.x, (float)Position.y, imageWidth, imageHeight));
        }

        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="g">Graphics used to draw the item</param>
        /// <param name="x">Drawing's x position</param>
        /// <param name="y">Drawing's y position</param>
        public void Draw(Graphics g, int x, int y)
        {
            g.DrawImage(image, new Rectangle(x, y, imageWidth, imageHeight));
        }

        /// <summary>
        /// Move the object. Last call to move was deltaT seconds ago.
        /// </summary>
        /// <param name="deltaT">Time ellapsed since last move.</param>
        public void Move(double deltaT)
        {
            Position.y += 100 * deltaT;
        }

        /// <summary>
        /// Determines Collision between the player's ship and the bonus
        /// </summary>
        /// <param name="s">player ship</param>
        public bool Collision(SpaceShip s)
        {
            if (s.Position.x > Position.x + imageWidth)
                return false;
            else if (s.Position.y > Position.y + imageHeight)
                return false;
            else if (Position.x > s.Position.x + s.imageWidth)
                return false;
            else if (Position.y > s.Position.y + s.imageHeight)
                return false;
            else
            {
                Vecteur2D[] vs = new Vecteur2D[s.imageWidth * s.imageHeight];
                Vecteur2D[] vb = new Vecteur2D[imageWidth * imageHeight];

                Color currentcolor;

                for (int i = 0; i < s.imageWidth; i++)
                {
                    for (int j = 0; j < s.imageHeight; j++)
                    {
                        currentcolor = s.image.GetPixel(i, j);
                        if (currentcolor == Color.FromArgb(255, 0, 0, 0))
                            vs[(i * s.imageHeight) + j] = new Vecteur2D((i + s.Position.x) - Position.x, 
                                                                        (j + s.Position.y) - Position.y);
                    }
                }

                for (int i = 0; i < image.Width; i++)
                {
                    for (int j = 0; j < image.Height; j++)
                    {
                        currentcolor = image.GetPixel(i,j);

                        if (currentcolor == Color.FromArgb(255, 0, 0, 0))
                            vb[(i * imageHeight) + j] = new Vecteur2D(i,j);
                        
                    }
                }

                for (int i = 0; i < vb.Length; i++)
                {
                    for (int j = 0; j < vs.Length; j++)
                    {
                        if (vs[j] != null && vb[i] != null)
                        {
                            if ((int)vs[j].x == (int)vb[i].x && (int)vs[j].y == (int)vb[i].y)
                                return true;
                        }
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Avtivates the first bonus ability
        /// </summary>
        public static void Bonus1()
        {
            Game.game.playerMissile = new Missile(new Vecteur2D(200, 200),
                                                    new Vecteur2D(0, 0),
                                                    999);
            Game.game.playerMissile.Position.x = Game.game.playerShip.Position.x + Game.game.playerShip.imageWidth / 2 - 1;
            Game.game.playerMissile.Position.y = Game.game.playerShip.Position.y - Game.game.playerMissile.imageHeight;
            Game.game.playerMissile.image = space_invaders.Properties.Resources.shoot4;
        }

        /// <summary>
        /// Avtivates the second bonus ability
        /// </summary>
        public static void Bonus2()
        {
            int closerSpaceshipIndex = 0;
            double minDistance = Double.MaxValue;
            double shipToShipDistance;

            foreach (SpaceShip s in Game.game.enemyBlock.Ships)
            {
                shipToShipDistance = Math.Sqrt(Math.Pow((Game.game.playerShip.Position.x - s.Position.x), 2) 
                                                + Math.Pow((Game.game.playerShip.Position.y - s.Position.y), 2));

                if (minDistance > shipToShipDistance) 
                {
                    minDistance = shipToShipDistance;
                    closerSpaceshipIndex = Game.game.enemyBlock.Ships.IndexOf(s);
                }
            }

            Game.game.enemyBlock.RemoveEnemy(closerSpaceshipIndex);

        }

        /// <summary>
        /// Avtivates the third bonus ability
        /// </summary>
        public static void Bonus3()
        {
            Game.game.playerScore.BonusFactor = 2;
        }

        #endregion

        #region Constructor
        /// <summary>
        /// Simple constructor
        /// </summary>
        /// <param name="position">start position x</param>
        public Bonus(Vecteur2D position)
        {
            Position = position;

            int choice = Game.prob.Next(1, 4);

            if (choice == 1)
            {
                image = space_invaders.Properties.Resources.bonus;
                imageWidth = image.Width;
                imageHeight = image.Height;
                bonusName = "BigMissile";
                action = new BonusAction(Bonus1);
            }
            else if (choice == 2)
            {
                image = space_invaders.Properties.Resources.bonus2;
                imageWidth = image.Width;
                imageHeight = image.Height;
                bonusName = "InstantKill";
                action = new BonusAction(Bonus2);
            }
            else if (choice == 3)
            {
                image = space_invaders.Properties.Resources.bonus3;
                imageWidth = image.Width;
                imageHeight = image.Height;
                bonusName = "DoublePoints";
                action = new BonusAction(Bonus3);
            }
            
        }
        #endregion
    }
}
