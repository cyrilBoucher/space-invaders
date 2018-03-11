using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace SpaceInvaders
{
    public partial class GameForm : Form
    {

        #region fields
        /// <summary>
        /// Instance of the game
        /// </summary>
        private Game game;

        public static PrivateFontCollection pfc = new PrivateFontCollection();

        #region time management
        /// <summary>
        /// Game watch
        /// </summary>
        Stopwatch watch = new Stopwatch();

        /// <summary>
        /// Last update time
        /// </summary>
        long lastTime = 0;
        #endregion
        
        
        #endregion

        #region constructor
        /// <summary>
        /// Create form, create game
        /// </summary>
        public GameForm()
        {
            InitializeComponent();
            byte[] fontData = SpaceInvaders.Properties.Resources.ethnocentric_rg;

            IntPtr fontPtr = Marshal.AllocCoTaskMem(fontData.Length);

            Marshal.Copy(fontData, 0, fontPtr, fontData.Length);

            pfc.AddMemoryFont(fontPtr, fontData.Length);

            Marshal.FreeCoTaskMem(fontPtr);

            game = Game.CreateGame(new Size(this.Width - 18, this.Height - 33)); // border size...
            watch.Start();
            WorldClock.Start();

        }
        #endregion

        #region events
        /// <summary>
        /// Paint event of the form, see msdn for help => paint game with double buffering
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameForm_Paint(object sender, PaintEventArgs e)
        {
            BufferedGraphics bg = BufferedGraphicsManager.Current.Allocate(e.Graphics, e.ClipRectangle);
            Graphics g = bg.Graphics;
            g.Clear(Color.White);

            game.Draw(g);

            bg.Render();
            bg.Dispose();

        }

        /// <summary>
        /// Tick event => update game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WorldClock_Tick(object sender, EventArgs e)
        {
            if (game.currentState == Game.GameState.Pause)
                watch.Stop();
            else if (game.currentState == Game.GameState.Play)
                watch.Start();
            // get time with millisecond precision
            long nt = watch.ElapsedMilliseconds;
            // compute ellapsed time since last call to update
            double deltaT = ((nt - lastTime) / 1000.0);
            game.Update(deltaT);
            // remember the time of this update
            lastTime = nt;

            Invalidate();
        }

        /// <summary>
        /// Key down event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Space:
                    game.KeySpacePressed = true;
                    break;
                case Keys.Left:
                    game.KeyLeftPressed = true;
                    break;
                case Keys.Right:
                    game.KeyRightPressed = true;
                    break;
                case Keys.P:
                    game.KeyPPressed = true;
                    break;
                case Keys.B:
                    game.KeyBPressed = true;
                    break;
                case Keys.Up:
                    game.KeyUpPressed = true;
                    break;
                case Keys.Down:
                    game.KeyDownPressed = true;
                    break;
                case Keys.Enter:
                    game.KeyEnterPressed = true;
                    break;
            }

        }

        /// <summary>
        /// Key up event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameForm_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Space:
                    game.KeySpacePressed = false;
                    break;
                case Keys.Left:
                    game.KeyLeftPressed = false;
                    break;
                case Keys.Right:
                    game.KeyRightPressed = false;
                    break;
                case Keys.P:
                    game.KeyPPressed = false;
                    break;
                case Keys.B:
                    game.KeyBPressed = false;
                    break;
                case Keys.Up:
                    game.KeyUpPressed = false;
                    break;
                case Keys.Down:
                    game.KeyDownPressed = false;
                    break;
                case Keys.Enter:
                    game.KeyEnterPressed = true;
                    break;
            }
        }
        #endregion

        private void GameForm_Load(object sender, EventArgs e)
        {

        }
    }
}
