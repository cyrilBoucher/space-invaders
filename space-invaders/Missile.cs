using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace SpaceInvaders
{
    class Missile
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
        /// Speed
        /// </summary>
        public Vecteur2D Vitesse { get; set; }

        /// <summary>
        /// Lives
        /// </summary>
        public int Lives { get; set; }

        /// <summary>
        /// Alive
        /// </summary>
        public bool Alive 
        { 
            get 
            {
                if (Lives <= 0)
                    return false;
                else if (Position.y <= 0)
                    return false;
                else
                return true; 
            } 
        }
            

        #endregion

        #region Constructor
        /// <summary>
        /// Simple constructor
        /// </summary>
        /// <param name="vitesse">Speed of the missile.</param>
        /// <param name="position">Position of the missile.</param>
        /// <param name="lives">Number of lives of the missile.</param>
        public Missile(Vecteur2D vitesse, Vecteur2D position, int lives)
        {
            this.image = space_invaders.Properties.Resources.shoot1;
            this.imageWidth = this.image.Width;
            this.imageHeight = this.image.Height;

            Vitesse = vitesse;

            Position = position;

            Lives = lives;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="g">Graphics used to draw the item</param>
        public void Draw(Graphics g)
        {
            g.DrawImage(image, new RectangleF((float)Position.x, (float)Position.y, imageWidth, imageHeight));
        }

        /// <summary>
        /// Move the object. Last call to move was deltaT seconds ago.
        /// </summary>
        /// <param name="deltaT">Tim ellapsed since last move.</param>
        /// <param name="vitesse">Speed of the missile</param>
        public void Move(double deltaT, double vitesse)
        {
            Position.y -= vitesse * deltaT;
        }
        #endregion
    }
}
