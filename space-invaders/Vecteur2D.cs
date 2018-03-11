using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceInvaders
{
    class Vecteur2D
    {
        #region Fields
        /// <summary>
        /// Position
        /// </summary>
        public double x, y;

        #endregion

        #region Properties

        public double Norme
        {
            get
            {
                return (Math.Sqrt(x * x + y * y));
            }
        }

        #endregion

        #region Operators
        public static Vecteur2D operator +(Vecteur2D v1, Vecteur2D v2)
        {
            return new Vecteur2D(v1.x + v2.x, v1.y + v2.y);
        }

        public static Vecteur2D operator -(Vecteur2D v1, Vecteur2D v2)
        {
            return new Vecteur2D(v1.x - v2.x, v1.y - v2.y);
        }

        public static Vecteur2D operator -(Vecteur2D v1)
        {
            return new Vecteur2D(-v1.x, -v1.y);
        }

        public static Vecteur2D operator *(Vecteur2D v1, double factor)
        {
            return new Vecteur2D(v1.x * factor, v1.y * factor);
        }

        public static Vecteur2D operator *(double factor, Vecteur2D v1)
        {
            return new Vecteur2D(v1.x * factor, v1.y * factor);
        }

        public static Vecteur2D operator /(Vecteur2D v1, double factor)
        {
            return new Vecteur2D(v1.x / factor, v1.y / factor);
        }

        #endregion

        #region Constructor
        /// <summary>
        /// Simple constructor
        /// </summary>
        public Vecteur2D(double x = 0, double y = 0)
        {
            this.x = x;
            this.y = y;
        }
        #endregion

    }
}
