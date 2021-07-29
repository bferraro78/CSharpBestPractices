namespace CSharpBestPractices.MemoryManagement
{
    public class ReadonlyStruct
    {

        /// <summary>
        /// A readonly struct will AVOID DEFENSIVE COPIES all together as the compiler reconize a change of state is not allowed
        /// </summary>
        readonly public struct Point
        {
            public int _coord { get; } // State field - the value type we pass around as a reference in this example

            public Point(int value)
            {
                _coord = value;
            }

            // Can not change the state of the struct- COMPILER ERROR
            //public void SetPointCoord(int newCoord)
            //{
            //    _coord = newCoord;
            //}

        }

    }
}
