using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace SpaceInvaders
{
    class Bunker
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
        /// Determines Collision between the bunker and a missile
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
                 Vecteur2D[] v = new Vecteur2D[m.imageWidth*m.imageHeight];

                 for (int i = 0; i < m.imageWidth; i++)
                 {
                     for (int j = 0; j < m.imageHeight; j++)
                     {
                         v[(i*m.imageHeight)+j] = new Vecteur2D((i + m.Position.x) - Position.x, (j + m.Position.y) - Position.y);
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

                if (m.Vitesse.y < 0)
                {
                    for (int i = (int)currentV.y; i < 0; i--)
                    {
                        currentcolor = image.GetPixel((int)currentV.x, i);

                        if (currentcolor == Color.FromArgb(0, 255, 255, 255))
                        {
                            currentV.y = i + 1;
                            i = 0;
                        }
                    }
                }
                else
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
                }

                for (int i = 0; i < m.imageWidth; i++)
                {
                    if (currentV.y >= 0 && currentV.x > 0 && (i+currentV.x) < imageWidth)
                    {
                        currentcolor = image.GetPixel((int)(i + currentV.x), (int)currentV.y);

                        if (currentcolor == Color.FromArgb(255, 0, 0, 0))
                            image.SetPixel((int)(i + currentV.x), (int)currentV.y, Color.FromArgb(0, 255, 255, 255));

                        if (i == 2)
                        {
                            m.Lives--;
                            return true;
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
        /// <param name="position">start position x</param>
        public Bunker(Vecteur2D position)
        {
            this.image = space_invaders.Properties.Resources.bunker;
            this.imageWidth = image.Width;
            this.imageHeight = image.Height;

            Position = position;
        }
        #endregion
    }
}
