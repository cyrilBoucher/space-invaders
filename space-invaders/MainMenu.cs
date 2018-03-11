using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace SpaceInvaders
{
    class MainMenu
    {
        #region enumeration
        /// <summary>
        /// Different difficulties
        /// </summary>
        public enum DifficultyState
        {
            Easy,
            Medium,
            Hard
        };

        #endregion
        
        #region Fields

        /// <summary>
        /// highlighted difficulty
        /// </summary>
        public DifficultyState currentSelection;
        /// <summary>
        /// Variable used to change alpha value
        /// </summary>
        public int light = -15;
        /// <summary>
        /// Color alpha property
        /// </summary>
        private static int alpha = 255;
        /// <summary>
        /// Brush to write text
        /// </summary>
        private static Brush blackBrush = new SolidBrush(Color.Black);
        /// <summary>
        /// Brush to write blinking text
        /// </summary>
        private static Brush blackBrushSelection = new SolidBrush(Color.FromArgb(alpha, 0, 0, 0));
        /// <summary>
        /// Font to use while writing
        /// </summary>
        public static Font ethnocentric35 = new Font(GameForm.pfc.Families[0], 35);
        #endregion

        #region Methods
        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="g">Graphics used to draw the item</param>
        public void Draw (Graphics g)
        {

            g.DrawString("Space", ethnocentric35, blackBrush, Game.game.gameSize.Width / 10, 100);
            g.DrawString("Invaders", ethnocentric35, blackBrush, Game.game.gameSize.Width / 4, 145);

            blackBrushSelection = new SolidBrush(Color.FromArgb(alpha, 0, 0, 0));

            if (currentSelection == DifficultyState.Easy)
            {
                g.DrawString("Easy", ethnocentric35, blackBrushSelection, Game.game.gameSize.Width / 5, 250);
                g.DrawString("Medium", ethnocentric35, blackBrush, Game.game.gameSize.Width / 6, 290);
                g.DrawString("Hard", ethnocentric35, blackBrush, Game.game.gameSize.Width / 6, 330);
            }

            if (currentSelection == DifficultyState.Medium)
            {
                g.DrawString("Easy", ethnocentric35, blackBrush, Game.game.gameSize.Width / 6, 250);
                g.DrawString("Medium", ethnocentric35, blackBrushSelection, Game.game.gameSize.Width / 5, 290);
                g.DrawString("Hard", ethnocentric35, blackBrush, Game.game.gameSize.Width / 6, 330);
            }

            if (currentSelection == DifficultyState.Hard)
            {
                g.DrawString("Easy", ethnocentric35, blackBrush, Game.game.gameSize.Width / 6, 250);
                g.DrawString("Medium", ethnocentric35, blackBrush, Game.game.gameSize.Width / 6, 290);
                g.DrawString("Hard", ethnocentric35, blackBrushSelection, Game.game.gameSize.Width / 5, 330);
            }

            
        }

        /// <summary>
        /// Change the alpha value of the drawing brush
        /// </summary>
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

        /// <summary>
        /// Select the level
        /// </summary>
        public void LevelSelection()
        {
            try
            {
                if (Game.game.mainMenu.currentSelection == MainMenu.DifficultyState.Easy)
                    Game.game.enemyBlock = Game.game.InitFromFile("level1Easy.txt");
                else if (Game.game.mainMenu.currentSelection == MainMenu.DifficultyState.Medium)
                    Game.game.enemyBlock = Game.game.InitFromFile("level1Medium.txt");
                else if (Game.game.mainMenu.currentSelection == MainMenu.DifficultyState.Hard)
                    Game.game.enemyBlock = Game.game.InitFromFile("level1Hard.txt");
            }
            catch (Exception e)
            {
                if (Game.game.mainMenu.currentSelection == MainMenu.DifficultyState.Easy)
                    Game.game.enemyBlock = Game.game.InitFromFile("level1EasyBackup.txt");
                else if (Game.game.mainMenu.currentSelection == MainMenu.DifficultyState.Medium)
                    Game.game.enemyBlock = Game.game.InitFromFile("level1MediumBackup.txt");
                else if (Game.game.mainMenu.currentSelection == MainMenu.DifficultyState.Hard)
                    Game.game.enemyBlock = Game.game.InitFromFile("level1HardBackup.txt");
            }

        }

        #endregion

        #region Constructor
        /// <summary>
        /// Simple constructor
        /// </summary>
        public MainMenu()
        {        

        }
        #endregion
    }
}
