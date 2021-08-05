using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpBestPractices.Performance
{
    public class ForEachCasting
    {


        public void CastingForEach()
        {

            Person[] persons = new Person[] { new Customer(1), new Employee(2), new Customer(3), new Customer(4) };

            foreach (Customer c in persons) // Breaks with Invalid Case Exception due to Employee not getting the cast
            {
                Console.WriteLine(c._age);
            }

            foreach (Customer c in persons)
            {
                Console.WriteLine(c._age);
            }

        }

    }

    public class Person
    {
        public int _age;
        public Person(int age)
        {
            _age = age;
        }
    }


    public class Customer : Person
    {
        public Customer(int age) : base(age) { }
    }

    public class Employee : Person
    {
        public Employee(int age) : base(age) { }

    }

}
