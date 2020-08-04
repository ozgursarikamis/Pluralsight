using System;

namespace NullBasics
{
    internal class Program
    {
        private static void Main()
        {
#nullable disable  // convert to a not-nullable reference type
            string message = null;
#nullable enable

            Console.WriteLine(message); // string is a reference type, so it can be null.
            Console.WriteLine("Press Enter to end!");
            Console.ReadLine();
        }
    }
}
