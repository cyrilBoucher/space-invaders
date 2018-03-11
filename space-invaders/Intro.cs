using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;

namespace SpaceInvaders
{
    class Intro
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
        /// Brush to write text
        /// </summary>
        private static Brush blackBrush = new SolidBrush(Color.Black);
        /// <summary>
        /// Fonts to use while writing
        /// </summary>
        public static Font ethnocentric15 = new Font(GameForm.pfc.Families[0], 15);
        public static Font ethnocentric35 = new Font(GameForm.pfc.Families[0], 35);
        /// <summary>
        /// Intro timer
        /// </summary>
        public static Stopwatch timeIntro = new Stopwatch();

        #endregion

        #region Properties
        /// <summary>
        /// Position of the first line of text
        /// </summary>
        public Vecteur2D Position1 { get; set; }
        /// <summary>
        /// Position of the second line of text
        /// </summary>
        public Vecteur2D Position2 { get; set; }
        /// <summary>
        /// Position of the third line of text
        /// </summary>
        public Vecteur2D Position3 { get; set; }

        #endregion

        #region Methods
        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="g">Graphics used to draw the item</param>
        public void Draw (Graphics g)
        {
            if (timeIntro.ElapsedMilliseconds < 8500.0)
            {
                if (timeIntro.ElapsedMilliseconds < 4250.0)
                    g.DrawImage(image, new RectangleF(Game.game.gameSize.Width / 2 - imageWidth / 2, Game.game.gameSize.Height / 4, imageWidth, imageHeight));

                if (timeIntro.ElapsedMilliseconds > 4400.0)
                    g.DrawString("BookButcher Games", ethnocentric15, blackBrush, (int)Position1.x, (int)Position1.y);
                if (timeIntro.ElapsedMilliseconds > 5450.0)
                    g.DrawString("Presents", ethnocentric15, blackBrush, (int)Position2.x, (int)Position2.y);
                if (timeIntro.ElapsedMilliseconds > 6900.0)
                    g.DrawString("A C# project", ethnocentric15, blackBrush, (int)Position3.x, (int)Position3.y);

            }
            else
            {
                g.DrawString("Space", ethnocentric35, blackBrush, Game.game.gameSize.Width / 10, 100);
                g.DrawString("Invaders", ethnocentric35, blackBrush, Game.game.gameSize.Width / 4, 145);
            }
        }

        /// <summary>
        /// Move the object. Last call to move was deltaT seconds ago.
        /// </summary>
        /// <param name="deltaT">Time ellapsed since last move.</param>
        public void Move(int speed, double deltaT)
        {
            if (Position1.x > Game.game.gameSize.Width / 5)
                Position1.x -= speed/30;
            else if (Position1.x < Game.game.gameSize.Width / 5)
                Position1.x -= speed/100 * deltaT;

            if (Position2.x > Game.game.gameSize.Width / 5 && timeIntro.ElapsedMilliseconds > 5450.0)
                Position2.x -= speed / 30;
            else if (Position2.x < Game.game.gameSize.Width / 5)
                Position2.x -= speed / 100 * deltaT;

            if (Position3.x >= Game.game.gameSize.Width /4 && timeIntro.ElapsedMilliseconds > 6900.0)
                Position3.x -= speed / 30;
            else if (Position3.x < Game.game.gameSize.Width / 4)
                Position3.x -= speed / 100 * deltaT;
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Simple constructor
        /// </summary>
        public Intro()
        {
            this.image = space_invaders.Properties.Resources.esieelogo;
            this.imageWidth = image.Width;
            this.imageHeight = image.Height;
            timeIntro.Start();

            Position1 = new Vecteur2D(1000, 200.0);
            Position2 = new Vecteur2D(1000, 250.0);
            Position3 = new Vecteur2D(1000, 300.0);

        }
        #endregion
    }
}
