using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;


namespace CSharpBestPractices.MemoryManagement
{
    public class PoolingBuffers
    {
        /// <summary>
        /// Anything over 85k bytes is considered a Large Object, and is stored on the LOH (vs the SOH)
        /// Garbage collection is done in 3 generation stages. Anything on LOH is automatically stored on LOH, which is collected not often.
        /// </summary>
        public void PoolingBuffersExampleOne()
        {
            /* BufferManager essentially allocates a large chunk of contiguous memory and sets it aside for later use.
               Every time you need to use a buffer, you take one from the pool, use it, and return it to the pool when done.
               This process is much faster than creating and destroying a buffer every time you need to use one. */
            var maxPoolSize = 1000L;
            var maxBufferSize = 100;
            var bm = BufferManager.CreateBufferManager(maxPoolSize, maxBufferSize); // 2nd arg is max size you are allowed to take at one time

            var buffer = bm.TakeBuffer(50); // Grabs buffer memory from pool

            // MemoryStream can use less then the provider buffer automatically. 
            // This saves Large Object and Memory Allocation Steps
            using (var ms = new MemoryStream(buffer))
            {
                // Do work.
            }

            bm.ReturnBuffer(buffer); //  return buffer taken to be reused for later

            /* Once you return a buffer to a pool, you still have a reference to that memory, 
               so it is advisable to set your buffer to null immediately afterwards to avoid accidentally using it.*/
            buffer = null;
        }

        /// <summary>
        /// Using PoolingBuffers to get a ByteArray (ToArray()), w/o possibly creating another LOH.
        /// Solution: Read the data out of the MemoryStreaminto your own array, obtained from the BufferManager
        /// </summary>
        public void PoolingBufferToArray(object o)
        {
            var maxPoolSize = 1000L;
            var maxBufferSize = 100;
            var bm = BufferManager.CreateBufferManager(maxPoolSize, maxBufferSize);
            var buffer = bm.TakeBuffer(50);
            byte[] output;

            using (var ms = new MemoryStream(buffer))
            {
                // Using Newtonsoft.Json or something
                //var bytesWritten = MySerializer.Serialize(ms, o); 
                var bytesWritten = 0;

                // Do more work.

                output = bm.TakeBuffer(bytesWritten);
                ms.Read(output, 0, bytesWritten);
            }

            bm.Clear(); // Does not always zero-out the buffer when you take or return one. Zero it out so buffer returns properly
            bm.ReturnBuffer(buffer);
            buffer = null;

            // Use 'output' for something.

            bm.Clear(); // 
            bm.ReturnBuffer(output);
            output = null;

        }

        /// <summary>
        /// BAD WAY TO DO THIS.
        /// ToArray() - will create a new object on the Large Object Heap (LOH)
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public byte[] BadSerialize(object o)
        {
            using (var stream = new MemoryStream())
            {
                // Using Newtonsoft.Json Serlize or Something
                //MySerializer.Serialize(stream, o); 
                return stream.ToArray();
            }
        }
    }
}
