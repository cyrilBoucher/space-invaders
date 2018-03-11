using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace SpaceInvaders
{
    class Score
    {
        #region Fields
        /// <summary>
        /// Score
        /// </summary>
        private int score;
        /// <summary>
        /// Bonus multiplier
        /// </summary>
        private int bonusFactor = 1;
        /// <summary>
        /// Size
        /// </summary>
        private SizeF size;
        /// <summary>
        /// Brush to write text
        /// </summary>
        private static Brush blackBrush = new SolidBrush(Color.Black);
        #endregion

        #region Properties
        /// <summary>
        /// Position
        /// </summary>
        public Vecteur2D Position { get; set; }

        /// <summary>
        /// Bonus multiplier
        /// </summary>
        public int BonusFactor { get { return bonusFactor; } set { bonusFactor = value;} }

        #endregion

        #region Methods
        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="g">Graphics used to draw the item</param>
        public void Draw (Graphics g)
        {
            size = g.MeasureString("Score : " + score, Game.ethnocentric15);

            g.DrawString("Score : " + score, Game.ethnocentric15, blackBrush,
                            new PointF((float)(Position.x-size.Width), (float)Position.y));
        }

        /// <summary>
        /// Display the player's score after a victory
        /// </summary>
        /// <param name="w">Instance of the win screen</param>
        /// <param name="g">Graphics used to draw the item</param>
        public void DisplayScore(Win w, Graphics g)
        {
            SizeF showOffScore = g.MeasureString("Your Score is " + score, Game.ethnocentric15);

            g.DrawString("Your Score is " + score, Game.ethnocentric15, blackBrush,
                            new PointF((float)(Game.game.gameSize.Width / 2 - showOffScore.Width / 2), (float)w.Position.y + w.imageHeight + 20));
        }

        /// <summary>
        /// Display the player's score after a defeat
        /// </summary>
        /// <param name="w">Instance of the lost screen</param>
        /// <param name="g">Graphics used to draw the item</param>
        public void DisplayScore(Lost l, Graphics g)
        {
            SizeF showOffScore = g.MeasureString("Your Score is " + score, Game.ethnocentric15);

            g.DrawString("Your Score is " + score, Game.ethnocentric15, blackBrush,
                            new PointF((float)(Game.game.gameSize.Width / 2 - showOffScore.Width / 2), (float)l.Position.y + l.imageHeight + 20));
        }

        /// <summary>
        /// Calculate player score
        /// </summary>
        public void CalculateScore()
        {
            score += 100 * bonusFactor;
        }

        
        #endregion

        #region Constructor
        /// <summary>
        /// Simple constructor
        /// </summary>
        /// <param name="x">start position x</param>
        /// <param name="y">start position y</param>
        public Score()
        {
            score = 0;
            Position = new Vecteur2D(Game.game.gameSize.Width, 2);
        }
        #endregion
    }
}
