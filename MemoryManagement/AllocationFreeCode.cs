using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CSharpBestPractices.MemoryManagement
{
    /// <summary>
    ///
    /// </summary>
    public class AllocationFreeCode
    {
        /// <summary>
        /// Things that come up to reduce amount of memory used easily
        /// </summary>
        public void LowHangingFruite()
        {
            // 1. Object reuse. Use existing array rather than allocating a new one, pooling, etc

            // 2. String concatination. Use and reuse StringBuilder. Pre allocatge length if possible
            StringBuilder builder = new StringBuilder(); // instead of concat using +

            // 3. Params arguments
            MyParamsMethod("hello", "world");
            MyParamsMethod(); // Compiler does this => MyParamsMethod(Array.Empty<string>); The Compiler has automatically removes that allocation amount
            // Solution: Create method overloads if you know your method will take in N arguments, cutting out the allocation of N param arrays

            // 4. Avoid Boxing - passing a value type (int, long, date, time span) as a reference to a function
            // Boxing will take the value object and create a new object on the Heap!
            int valueType = 0;
            Console.WriteLine("Example of boxing, the string format wants an object reference {0}", valueType); // Will make a copy of the value as an object on the heap
            // Solution: Introduce generic overloads. Create multiple overloads for a Logging class that include values types or generics, making the runtime do the work for you


            // 5. Closures - Compiler rewrites to capture local variables as fields in new class (allocating a new class object). 
            //               Moving the lambda to the new class where it will continue to live.
            // Soltuion: Avoid in Critical Paths. Pass state (model object, etc) as argument to lamda. 
            //           Investigate with local functions.

            // 6. LINQ - Uses closures which come with extra allocations as mentioned above
            // Soltuion: Avoid in Critical Paths. Use for or foreach / if statements. 

            // 7. Iterators - Code is reqritten to a State Machine. Which is an extra allocation.
            IteratorExample();

            // 8. async-await
            // Solution: Investigate ValueTask (better than async await but might not work)

        }


        private void MyParamsMethod(params string[] args) { }

        // Local Function vs Lambda (Uses Closure) Example
        private static string GetText(string path, string filename)
        {
            var reader = File.OpenText($"{AppendPathSeparator(path)}{filename}");
            var text = reader.ReadToEnd();
            return text;

            // Closure using Lambda Expression
            Func<string, string> AppendPathSeparatorLambda = filepath => filepath.EndsWith(@"\") ? filepath : filepath + @"\";

            // Local function
            string AppendPathSeparator(string filepath)
            {
                return filepath.EndsWith(@"\") ? filepath : filepath + @"\";
            }

        }

        private void IteratorExample()
        {
            foreach (var str in GetMessages()) { }
        }

        private IEnumerable<string> GetMessages()
        {
            yield return "YoYo";
            yield return "Ma";
        }

    }
}
