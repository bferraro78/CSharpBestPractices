using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpBestPractices.MemoryManagement
{
    /// <summary>
    /// Span<T>
	/// New type for working with any kind of contiguous memory
	///	○ Arrays, Array Segments, strings, substrings
	///	○ Array like API - indexing
	///	○ Type safe, through the use of generics
	///	○ Array like performance
	///	○ Slicing - Create a new Span<T> with a sub section of a Span<T> WITH NO ALLOCATION
    ///            - If you want a substring or slice of array, without the allocation
    /// </summary>
    public class SpanContiguousMemory
    {

        private const string TestString = "YoYoYo Ma";
        private readonly int[] Numbers = new int[] { 0, 1, 2, 3, 4, 5, 6 };

        /// <summary>
        /// THIS METHOD IS ALLOCATION FREE - WE LOVE
        /// </summary>
        public void SpanStringExamples()
        {
            ReadOnlySpan<char> spanString = TestString.AsSpan();
            ReadOnlySpan<char> spanCharSlice = spanString.Slice(2,6);

            spanString.StartsWith("YoYo"); // Span Helpers
            spanString.Equals("YoYoYo Ma");
            var len = spanString.Length;
        }

        /// <summary>
        /// THIS METHOD IS ALLOCATION FREE - WE LOVE
        /// </summary>
        public void SpanArrayExamples()
        {
            Span<int> spanIntArray = Numbers;
            Span<int> sliceIntArray = spanIntArray.Slice(2, 6);

            spanIntArray[4] = 10; // Index
            sliceIntArray[1] = 10;

        }


    }
}
