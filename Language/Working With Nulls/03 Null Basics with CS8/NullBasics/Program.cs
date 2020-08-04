using System;

namespace NullBasics
{
    internal class Program
    {
        private static void Main()
        {
            var message = new Message
            {
                Text = "Hello there",
                From = null
            };
            Console.WriteLine(message.Text);
            Console.WriteLine(message.From);
            Console.WriteLine(message.ToUpperFrom());

            Console.ReadLine();
        }
    }

    public class Message
    {
        public string? From { get; set; } = "";
        public string Text { get; set; } = "";

        public string? ToUpperFrom()
        {
            return From is null ? "" : From?.ToUpperInvariant();
        }

    }
}
