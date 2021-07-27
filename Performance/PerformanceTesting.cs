using System;
using System.Runtime.CompilerServices;

namespace CSharpBestPractices.Performance
{

    /// <summary>
    /// Code execution on a CPU is not always linear.
    /// When it has to evaluate a condition to then decide what next instruction to execute, it may try to guess the most probable branch to follow and execute it.
    /// When the condition is finally evaluated, if it took the correct path, time was saved.
    /// Otherwise, it has to go back and take the correct branch, loosing valuable time.
    /// The more it predicts correctly, the better is the performance.
    /// Performance can be maximized when this decision making can be avoided.
    /// </summary>
    public class PerformanceTesting
    {
        /// <summary>
        /// In this case, the CPU will try to guess if it should add the value or not, but it has no knowledge of the data in source or the logical condition in predicate.
        /// This makes it almost impossible for the CPU to infer an heuristic.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static int PredictingPredicatePaths_Sum(int[] source, Func<int, bool> predicate)
        {
            var sum = 0;
            foreach (var item in source)
            {
                if (predicate(item))
                    sum += item;
            }
            return sum;
        }

        /// <summary>
        /// Removing the logical condition is to convert the boolean to a number.
        /// Thus removing the logical condition.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static int PredictingPredicatePaths_SumOptimized(int[] source, Func<int, bool> predicate)
        {
            var sum = 0;
            foreach (var item in source)
                sum += predicate(item).AsByte() * item;
            return sum;
        }


    }

    static class BooleanExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte AsByte(this bool value)
            => Unsafe.As<bool, byte>(ref value);
    }
}
