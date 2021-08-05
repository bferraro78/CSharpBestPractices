using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpBestPractices.Performance
{
    public class LoweringCSharp
    {
        private string z = "global";

        public void LoweringExamples()
        {
            /* Common Compiler Lowering */
            var messageOne = "New York"; // -- > string message = "New York"
            decimal dec = 5M; // new System.Decimal(5);

            var count = 5;
            string messageTwo = "You" + "Me";
            string messageThree = $"You have {count}"; // Goes to string.Format("You have {0}", count);

        }

        public void IEnumerableLowering()
        {
            var values = new MyThing();

            /* The compiler just needs the IEnumerator implementation in order to compile */
            foreach (int m in values) // This will run as an invalid Cast Exception
            { }

            /* foreach vs for:
               - The compiler actually intiates an extra array when using foreach vs for(var i = 0)
            */
            int[] valuesArray = new int[6];
            for (int i = 0; i < valuesArray.Length; i++) // This will run as an invalid Cast Exception
            {
                int val = valuesArray[i];
            }
        }

        public void LambdaLowering()
        {
            string y = "closing on y variables";
            Action<string> act = x => Console.WriteLine(x + y + z);
            act("YoYoMa");
        }

        // Compiler Lowers Lamba to:
        /* Compiler wants to create the smallest possible class, and not store your class in memory
           - By including a global variable, the compiler has to use "_this" a whole reference to the class. NOT GOOD MEMORY MANAGEMENT
         */
        private class ActHelper
        {
            public LoweringCSharp _this;
            private string y;
            private void _act(string x)
            {
                Console.WriteLine(x + y + _this.z);
            }
        }


        public void YeildLowering()
        {
            foreach (int x in GetInts())
            {
                Console.WriteLine(x);
            }
        }

        // Returns a Generator object, that holds last "yielded" line in place
        private IEnumerable<int> GetInts()
        {
            yield return 1;
            yield return 2;
            yield return 3;
            yield return 4;
            yield return 5;
        }

        // Generator class Object created by compiler
        private class GetIntsHelper : IEnumerable<int>, IEnumerable, IEnumerator<int>, IDisposable, IEnumerator
        {
            private int _state;
            private int _current;
            private int _initalThread;

            int IEnumerator<int>.Current { get { return this._current; } }
            object IEnumerator.Current { get { return this._current; } }
            
            /* Good to know, if you are using an iterator on different threads, you COULD get lots of instances created */
            IEnumerator<int> IEnumerable<int>.GetEnumerator()
            {
                GetIntsHelper result;
                if (this._state == -2 && this._initalThread == Environment.CurrentManagedThreadId)
                {
                    this._state = 0;
                    result = this;
                }
                else
                {
                    result = new GetIntsHelper(0);
                }
                return result;
            }

            public GetIntsHelper(int initalState)
            {
                this._state = initalState;
                this._initalThread = Environment.CurrentManagedThreadId;
            }
            void IDisposable.Dispose() { }

            public IEnumerator GetEnumerator()
            {
                throw new NotImplementedException();
            }

            public bool MoveNext()
            {
                throw new NotImplementedException();
            }

            public void Reset()
            {
                throw new NotImplementedException();
            }

        }

    }

    public class MyThing
    {
        public MyEnumerator GetEnumerator()
        {
            return null;
        }
        
    }

    public class MyEnumerator
    { 
        public object Current { get; }
        public bool MoveNext() => false;

    }
}
