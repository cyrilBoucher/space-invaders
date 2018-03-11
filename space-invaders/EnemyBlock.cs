using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace SpaceInvaders
{
    class EnemyBlock
    {
        #region Fields
        /// <summary>
        /// Position
        /// </summary>
        public Vecteur2D Position;
        /// <summary>
        /// Enemy block size
        /// </summary>
        public Size Size;
        /// <summary>
        /// Probablity an enemy fires a rocket
        /// </summary>
        public double ShootProbability = 0.1;
        /// <summary>
        /// Horizontal speed
        /// </summary>
        private Vecteur2D SpeedX = new Vecteur2D(50,0);
        /// <summary>
        /// Vertical speed
        /// </summary>
        private Vecteur2D SpeedY = new Vecteur2D(0,10);

        #endregion

        #region Properties
        /// <summary>
        /// List of enemy ships
        /// </summary>
        public List<SpaceShip> Ships { get; set; }
        /// <summary>
        /// Is the block empty?
        /// </summary>
        public bool Alive { get { if (Ships.Count > 0) return true; 
                                    else return false; } }

        #endregion

        #region Constructor
        /// <summary>
        /// Simple constructor
        /// </summary>
        /// <param name="position">start position</param>
        public EnemyBlock(Vecteur2D position)
        {
            Position = position;
            Size = Size.Empty;

            Ships = new List<SpaceShip>();
        }
        #endregion
               
        #region Methods
        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="g">Graphics used to draw the item</param>
        public void Draw (Graphics g)
        {
            foreach (SpaceShip Ship in Ships)
                    g.DrawImage(Ship.image, new RectangleF((float)Ship.Position.x, (float)Ship.Position.y, Ship.imageWidth, Ship.imageHeight));

            //g.DrawRectangle(new Pen(Color.Red), (float) Position.x, (float) Position.y, (float) Size.Width, (float)Size.Height);
        }

        /// <summary>
        /// Add a row of enemies to the block
        /// </summary>
        /// <param name="width">Width of the row</param>
        /// <param name="nbShips">Number of ships</param>
        /// <param name="lives">Amount of lives ship spawn with</param>
        /// <param name="im">Image to illustrate the ship in the row </param>
        public void AddLine(int width, int nbShips, int lives, Bitmap im)
        {
            for (int i = 0; i < nbShips; i++)
                Ships.Add(new SpaceShip(im, new Vecteur2D(Position.x + i*(width / nbShips), Position.y + Size.Height), lives));

            Size.Height += im.Height + 10;
            Size.Width = width;

        }

        /// <summary>
        /// Move the object. Last call to move was deltaT seconds ago.
        /// </summary>
        /// <param name="deltaT">Time ellapsed since last move.</param>
        public void Move(double deltaT)
        {
            if ((Position.x >= 0 && (Position.x + Size.Width) < Game.game.gameSize.Width))
            {
                foreach (SpaceShip ship in Ships)
                    ship.Move(deltaT, SpeedX.x);

                Position.x += SpeedX.x * deltaT;

                if ((Position.x + Size.Width) >= Game.game.gameSize.Width)
                {
                    for (int i = 0; i < deltaT*2000; ++i)
                    {
                        foreach (SpaceShip Ship in Ships)
                            Ship.Position.y += SpeedY.y * deltaT;

                        Position.y += SpeedY.y * deltaT;
                    }
                    SpeedX = -SpeedX*1.1;
                }
            }

            else if (Position.x > 1)
            {
                foreach (SpaceShip Ship in Ships)
                    Ship.Move(deltaT, SpeedX.x);

                Position.x += SpeedX.x * deltaT;
            }

            else if (Position.x < 1)
            {                
                for (int i = 0; i < deltaT * 2000; ++i)
                {
                    foreach (SpaceShip Ship in Ships)
                        Ship.Position.y += SpeedY.y * deltaT;

                    Position.y += SpeedY.y * deltaT;
                }
                SpeedX = -SpeedX*1.1;

                foreach (SpaceShip Ship in Ships)
                    Ship.Position.x += SpeedX.x/10;

                Position.x += SpeedX.x / 10;
            }
        }

        /// <summary>
        /// Determines Collision between an enemy in the block and a missile
        /// </summary>
        /// <param name="m">Colliding missile</param>
        public bool Collision(Missile m)
        {
            if (m.Position.x > Position.x + Size.Width)
                return false;
            else if (m.Position.y > Position.y + Size.Height)
                return false;
            else if (Position.x > m.Position.x + m.imageWidth)
                return false;
            else if (Position.y > m.Position.y + m.imageHeight)
                return false;
            else
            {
                int index = -1;

                foreach (SpaceShip s in Ships)
                {
                     if (s.Collision(m))
                        index = Ships.IndexOf(s); 
                }

                if (index != -1)
                {
                    RemoveEnemy(index);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Determines Collision between an enemy in the block and a bunker
        /// </summary>
        /// <param name="b">Colliding bunler</param>
        public bool Collision(Bunker b)
        {
            
            if (b.Position.x > Position.x + Size.Width)
                return false;
            else if (b.Position.y > Position.y + Size.Height)
                return false;
            else if (Position.x > b.Position.x + b.imageWidth)
                return false;
            else if (Position.y > b.Position.y + b.imageHeight)
                return false;
            else
            {
                bool collision = false;
                foreach (SpaceShip s in Ships)
                {
                    if (b.Position.x > s.Position.x + s.imageWidth)
                        collision = false;
                    else if (b.Position.y > s.Position.y + s.imageHeight)
                        collision = false;
                    else if (s.Position.x > b.Position.x + b.imageWidth)
                        collision = false;
                    else if (s.Position.y > b.Position.y + b.imageHeight)
                        collision = false;
                    else
                        return true;
                }
                return collision;
            }               
        }

        /// <summary>
        /// Update the dimensions of the enemy block
        /// </summary>
        private void UpdateBBox()
        {
            Vecteur2D topLeftCorner = new Vecteur2D(Int32.MaxValue, Int32.MaxValue);
            Vecteur2D bottomRightCorner = new Vecteur2D(0,0);

            foreach (SpaceShip s in Ships)
            {
                if (s.Position.x < topLeftCorner.x)
                    topLeftCorner.x = s.Position.x;
                if (s.Position.y < topLeftCorner.y)
                    topLeftCorner.y = s.Position.y;

                if (s.Position.x > bottomRightCorner.x)
                    bottomRightCorner.x = s.Position.x + s.imageWidth;
                if (s.Position.y > bottomRightCorner.y)
                    bottomRightCorner.y = s.Position.y + s.imageHeight;

            }

            Position = new Vecteur2D (topLeftCorner.x, topLeftCorner.y);

            Size.Width = (int)bottomRightCorner.x - (int)Position.x;
            Size.Height = (int)bottomRightCorner.y - (int)Position.y;            
            
        }

        /// <summary>
        /// Makes an enemy fire a missile
        /// </summary>
        /// <param name="ship">Enemy ship</param>
        /// <param name="deltaT">Time ellapsed since last move.</param>
        public void RandomShoot(SpaceShip ship, double deltaT)
        {
            if (Game.prob.NextDouble() < deltaT * ShootProbability)
            {
                Missile shipMissile = new Missile(new Vecteur2D(200, -200),
                                                    new Vecteur2D(0, 0),
                                                    1);
                shipMissile.Position.x = ship.Position.x + ship.imageWidth / 2 - 1;
                shipMissile.Position.y = ship.Position.y + ship.imageHeight + shipMissile.imageHeight;

                Game.game.missiles.Add(shipMissile);
            }

        }

        /// <summary>
        /// Remove an enemy from the list of enemies
        /// </summary>
        /// <param name="index">index of the ship to remove</param>
        public void RemoveEnemy(int index)
        {            
            Game.game.RandomBonus(Ships[index].Position);
            Ships.RemoveAt(index);
            UpdateBBox();
            Game.game.playerScore.CalculateScore();
        }

        #endregion

        
    }
}
