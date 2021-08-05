namespace CSharpBestPractices.MemoryManagement
{
    /// <summary>
    /// 
    /// </summary>
    public class InOutParameters
    {
        public struct Point
        {
            public double X;
            public double Y;

            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }

            public void Translate(double dx, double dy)
            {
                X = X - dx;
                Y = Y - dy;
            }
        }

        public void InParameterExample()
        {
            Point p = new Point(10, 15);
            DoTranslate(p, 5, 5); // Since p is an "in" parameter - p x and y will NOT CHANGE

            DoTranslateWithRef(ref p, 5, 5); // "ref" must be used since the parameter accepts refs. The "p" object will change.
        }


        /// <summary>
        /// Pass p1 by as a reference
        /// In parameters - only passes reference in. WILL NOT MODIFY THE PARAMETER. Compiler creates defensive copy (since Point is a struct)
        /// </summary>
        /// <param name="p1"></param>
        /// <returns></returns>
        public void DoTranslate(in Point p, double dx, double dy)
        {
            p.Translate(dx, dy);
            //p.X = 6; - Compiler error as P.X is a readonly variable due to the "in"
        }

        /// <summary>
        /// using ref, will take in a reference of the struct, AND WILL NOT CREATE A COPY
        /// However since it is not readonly it will modify the actualy passed in property
        /// </summary>
        public void DoTranslateWithRef(ref Point p, double dx, double dy)
        {
            p.Translate(dx, dy);
            p.X = 3;
        }


    }

}
