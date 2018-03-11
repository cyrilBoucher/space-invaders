using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace SpaceInvaders
{
    class Lost
    {
        #region enumeration
        public enum AfterState
        {
            MainMenu,
            Quit
        };

        #endregion

        #region Fields
        /// <summary>
        /// Image
        /// </summary>
        public Bitmap image;
        /// <summary>
        /// Image proportions
        /// </summary>
        public int imageWidth, imageHeight;

        public int light = -15;
        private static int alpha = 255;
        private static Brush blackBrush = new SolidBrush(Color.Black);
        private static Brush blackBrushSelection = new SolidBrush(Color.FromArgb(alpha, 0, 0, 0));
        public static Font ethnocentric35 = new Font(GameForm.pfc.Families[0], 35);
        public AfterState currentSelection;

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
        /// Move the object. Last call to move was deltaT seconds ago.
        /// </summary>
        /// <param name="deltaT">Time ellapsed since last move.</param>
        public void Move(double deltaT)
        {
            Position.y += 120 * deltaT;
        }

        public void DisplayWinMenu(Graphics g)
        {
            blackBrushSelection = new SolidBrush(Color.FromArgb(alpha, 0, 0, 0));



            if (currentSelection == AfterState.MainMenu)
            {

                g.DrawString("Reset", ethnocentric35, blackBrushSelection, Game.game.gameSize.Width / 3, 400);
                g.DrawString("Quit", ethnocentric35, blackBrush, Game.game.gameSize.Width / 3, 440);
            }

            if (currentSelection == AfterState.Quit)
            {

                g.DrawString("Reset", ethnocentric35, blackBrush, Game.game.gameSize.Width / 3, 400);
                g.DrawString("Quit", ethnocentric35, blackBrushSelection, Game.game.gameSize.Width / 3, 440);
            }
        }

        public void Blink()
        {
            alpha += light;
            if (alpha >= 255 && light > 0)
            {
                light = -light;
                alpha = 255;
                return;
            }
            if (alpha <= 0 && light < 0)
            {
                light = -light;
                alpha = 0;
                return;
            }
        }

        public void AfterSelection()
        {
            if (currentSelection == AfterState.MainMenu)
                Application.Restart();

            if (currentSelection == AfterState.Quit)
                Application.Exit();
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Simple constructor
        /// </summary>
        /// <param name="x">start position</param>
        public Lost()
        {
            this.image = space_invaders.Properties.Resources.lost;
            this.imageWidth = image.Width;
            this.imageHeight = image.Height;

            Position = new Vecteur2D(Game.game.gameSize.Width / 2 - imageWidth / 2, - imageHeight);

        }
        #endregion
    }
}
