using System;

namespace RGeos.Geometries
{
    /*sh 
     *th ch Arth Arcth sech csch cth Arsh Arch*/
    public class MathEx
    {
        public static double sh(double x)
        {
            //sh(x)=(exp(x)-exp(-x))/2    
            double sh = (Math.Pow(Math.E, x) - Math.Pow(Math.E, -x)) / 2;
            return sh;
        }
        public static double ch(double x)
        {
            // ch(x)=(exp(x)+exp(-x))/2
            double ch = (Math.Exp(x) + Math.Exp(-x)) / 2;
            return ch;
        }
        public static double arsh(double x)
        {
            //arsh=ln（x+根号（x²+1））
            return System.Math.Log(x + Math.Sqrt(x * x + 1), Math.E);
        }
        public static double arch(double x)
        {
            //arch=ln（x+根号（x²-1））
            return System.Math.Log(x + Math.Sqrt(x * x - 1), Math.E);
        }
    }
}
