using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace SpaceInvaders
{
    class SpaceShip
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

        #endregion

        #region Properties
        /// <summary>
        /// Position
        /// </summary>
        public Vecteur2D Position { get; set; }

        /// <summary>
        /// Lives
        /// </summary>
        public int Lives { get; set; }

        /// <summary>
        /// Alive
        /// </summary>
        public bool Alive { get { if (Lives > 0) return true; else return false; } }

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
        /// Move the object right. Last call to move was deltaT seconds ago.
        /// </summary>
        /// <param name="deltaT">Tim ellapsed since last move.</param>
        /// <param name="playerSpeed">Speed of the player ship</param>
        public void MoveRight(double deltaT, double playerSpeed)
        {
            Position.x += playerSpeed * deltaT;
        }

        /// <summary>
        /// Move the object left. Last call to move was deltaT seconds ago.
        /// </summary>
        /// <param name="deltaT">Tim ellapsed since last move.</param>
        /// <param name="playerSpeed">Speed of the player ship</param>
        public void MoveLeft(double deltaT, double playerSpeed)
        {
            Position.x -= playerSpeed * deltaT;
        }

        /// <summary>
        /// Move the object right. Last call to move was deltaT seconds ago.
        /// </summary>
        /// <param name="deltaT">Tim ellapsed since last move.</param>
        /// <param name="playerSpeed">Speed of the enemy ship</param>
        public void Move(double deltaT, double enemySpeed)
        {
            Position.x += enemySpeed * deltaT;
        }

        /// <summary>
        /// Determines Collision between player ship and a missile
        /// </summary>
        /// <param name="m">Colliding missile</param>
        public bool Collision(Missile m)
        {
            if (m.Position.x > Position.x + imageWidth)
                return false;
            else if (m.Position.y > Position.y + imageHeight)
                return false;
            else if (Position.x > m.Position.x + m.imageWidth)
                return false;
            else if (Position.y > m.Position.y + m.imageHeight)
                return false;
            else
            {
                Vecteur2D[] v = new Vecteur2D[m.imageWidth * m.imageHeight];

                for (int i = 0; i < m.imageWidth; i++)
                {
                    for (int j = 0; j < m.imageHeight; j++)
                    {
                        v[(i * m.imageHeight) + j] = new Vecteur2D((i + m.Position.x) - Position.x, (j + m.Position.y) - Position.y);
                    }
                }

                Color currentcolor;

                Vecteur2D currentV = new Vecteur2D();

                for (int i = 0; i < v.Length; i++)
                {
                    if (v[i].y >= 0 && v[i].x > 0 && v[i].x < imageWidth && v[i].y < imageHeight)
                    {
                        currentcolor = image.GetPixel((int)v[i].x, (int)v[i].y);
                        if (currentcolor == Color.FromArgb(255, 0, 0, 0))
                        {
                            currentV.x = v[i].x;
                            currentV.y = v[i].y;
                            i = v.Length;
                        }
                    }
                }

                if (currentV.x != 0 || currentV.y != 0)
                {
                    for (int i = (int)currentV.y; i < imageHeight; i++)
                    {
                        currentcolor = image.GetPixel((int)currentV.x, i);

                        if (currentcolor == Color.FromArgb(0, 255, 255, 255))
                        {
                            currentV.y = i - 1;
                            i = imageHeight;
                        }
                    }

                    for (int i = 0; i < m.imageWidth; i++)
                    {
                        if (currentV.y > 0 && currentV.x > 0 && (i + currentV.x) < imageWidth)
                        {
                            currentcolor = image.GetPixel((int)(i + currentV.x), (int)currentV.y);

                            if (currentcolor == Color.FromArgb(255, 0, 0, 0))
                            {
                                Lives--;
                                m.Lives--;
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
        }

        #endregion

        #region Constructor
        /// <summary>
        /// Simple constructor
        /// </summary>
        /// <param name="image">Image used to illustrate the player ship</param>
        /// <param name="position">start position x</param>
        /// <param name="lives">Number of lives</param>
        public SpaceShip(Bitmap image, Vecteur2D position, int lives)
        {
            this.image = image;
            this.imageWidth = image.Width;
            this.imageHeight = image.Height;

            Position = position;
            
            Lives = lives;
        }
        #endregion
    }
}
