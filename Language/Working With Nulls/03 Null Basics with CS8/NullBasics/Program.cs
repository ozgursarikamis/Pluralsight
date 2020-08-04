using System;

namespace NullBasics
{
    internal class Program
    {
        private static void Main()
        {
            string? message = null;

            Console.WriteLine(message); // string is a reference type, so it can be null.
            Console.WriteLine("Press Enter to end!");
            Console.ReadLine();
        }
    }
}
