using System;
using CSharpBestPractices.MemoryManagement;
using CSharpBestPractices.Performance;

namespace CSharpBestPractices
{
    class Program
    {
        static void Main(string[] args)
        {
            var refReturnLocal = new RefReturnLocal();
            refReturnLocal.ref_Return_Local();
            Console.WriteLine();
            refReturnLocal.ref_Readonly_Return();
        }
    }
}
