using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpBestPractices.Async
{
    /// <summary>
    /// Limits the number of threads that can access a resource or pool of resources concurrently
    /// 
    /// The count on a semaphore is decremented each time a thread enters the semaphore, and incremented when a thread releases the semaphore.
    /// When the count is zero, subsequent requests block until other threads release the semaphore
    /// </summary>
    public class Semaphores
    {
        // A semaphore that simulates a limited resource pool.
        //
        private static Semaphore _pool;

        // A padding interval to make the output more orderly.
        private static int _padding;

        public static void SemaphoreExample()
        {
            // Create a semaphore that can satisfy up to three
            // concurrent requests. Use an initial count of zero,
            // so that the entire semaphore count is initially
            // owned by the main program thread.
            //
            _pool = new Semaphore(0, 3);

            // Create and start five numbered threads. 
            //
            for (int i = 1; i <= 5; i++)
            {
                Thread t = new Thread(new ParameterizedThreadStart(Worker));

                // Start the thread, passing the number.
                //
                t.Start(i);
            }

            // Wait for half a second, to allow all the
            // threads to start and to block on the semaphore.
            //
            Thread.Sleep(500);

            // The main thread starts out holding the entire
            // semaphore count. Calling Release(3) brings the 
            // semaphore count back to its maximum value, and
            // allows the waiting threads to enter the semaphore,
            // up to three at a time.
            //
            Console.WriteLine("Main thread calls Release(3).");
            _pool.Release(3);

            Console.WriteLine("Main thread exits.");
        }

        private static void Worker(object num)
        {
            // Each worker thread begins by requesting the
            // semaphore.
            Console.WriteLine("Thread {0} begins " +
                "and waits for the semaphore.", num);
            _pool.WaitOne(); // DECREASES SEMAPHORE COUNT - IF 0 THEN NEXT THREAD WILL WAIT

            // A padding interval to make the output more orderly.
            int padding = Interlocked.Add(ref _padding, 100);

            Console.WriteLine("Thread {0} enters the semaphore.", num);

            // The thread's "work" consists of sleeping for 
            // about a second. Each thread "works" a little 
            // longer, just to make the output more orderly.
            Thread.Sleep(1000 + padding);

            Console.WriteLine("Thread {0} releases the semaphore.", num);
            Console.WriteLine("Thread {0} previous semaphore count: {1}",
                num, _pool.Release()); // INCREASES SEMAPHORE COUNT
        }
    }
}
