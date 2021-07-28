using System;
using CSharpBestPractices.MemoryManagement;
using CSharpBestPractices.Performance;

namespace CSharpBestPractices
{
    class Program
    {
        static void Main(string[] args)
        {
            new ValueTypesMemoryManagement().ref_Return_Local();
            new ValueTypesMemoryManagement().ref_Readonly_Return();

        }
    }
}
