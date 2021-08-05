using System;

namespace CSharpBestPractices.MemoryManagement
{
    /// <summary>
    /// Pass around Value types as if they were references to avoid runtime making copies
    /// - Allocatng reference type has COST, passing around is CHEAP
    /// - Allocating value type is CHEAP, passing around has COST (just cleaning the stack up, hard to pass around)
    /// 
    /// Solution: 
    /// 1. Allows value types to be used like reference types (pass by reference everywhere)
    /// 2. Use value types to reduce allocations on heap, reduce memory traffic
    /// 3. Pass by reference to avoid copies, enable modiyfing 
    /// 4. Minimize GC compacting
    /// </summary>
    public class RefReturnLocal
    {

        /// <summary>
        /// Struct are Value Types. When passed into a function, a copy is made...can be bad for memory performance if struct is large.
        /// </summary>
        public struct Point 
        {
            public int _coord; // State field - the value type we pass around as a reference in this example
            
            public Point(int value)
            {
                _coord = value;
            }

            public void SetPointCoord(int newCoord) // Self modifies state
            {
                _coord = newCoord;
            }

        }

        private Point _location = new Point(5);

        public Point GetLocationValue() => _location; // Get Value copy

        public ref Point GetLocationRef() => ref _location; // Get reference of the value type

        public ref readonly Point GetLocationReadonlyRef() => ref _location; // Get readonly reference of the value type


        /// <summary>
        /// ref Return/Local vs Copy
        /// </summary>
        public void ref_Return_Local()
        {
            var valueType = GetLocationValue()._coord; // Copy of global int 
            valueType += 5;
            Console.WriteLine("coord will be 5, since we retrieved location as a value. Value {0}", _location._coord);

            ref var refValueType = ref GetLocationRef()._coord; // refValueType is a ref local. Aka a reference of Global struct type
            refValueType += 5;
            Console.WriteLine("coord will be 10, since we gathered a reference of location and changed its state. Value {0}", _location._coord);
        }


        /// <summary>
        /// The above return/local ref will allow changes to state we may not want.
        /// ref return readonly will be needed
        /// </summary>
        public void ref_Readonly_Return()
        {

            ref var refValueType = ref GetLocationRef(); // Reference, will change since it is direct reference
            //refValueType += 5; // Compiler error

            refValueType.SetPointCoord(100);
            Console.WriteLine("coord will be 100, since we gathered a reference. Value {0}", _location._coord);

            _location = new Point(5);

            ref readonly var refReadonlyValueType = ref GetLocationReadonlyRef(); // Reference, but defensive copies made by compiler. Caller can not modify
            //refValueType += 5; // Compiler error

            refReadonlyValueType.SetPointCoord(100);
            Console.WriteLine("coord will be 5, since we gathered a defensive readonly copy of the reference. Value {0}", _location._coord);

        }



    }
}
