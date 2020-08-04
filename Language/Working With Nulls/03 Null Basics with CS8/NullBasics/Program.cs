using System;
using System.Reflection;

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

            MessagePopulator.Populate(message);
            Console.WriteLine(message.From.Length);
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

    public class MessagePopulator
    {
        public static void Populate(Message message)
        {
            message.GetType().InvokeMember("From",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty,
                Type.DefaultBinder, message, new[] {"Jason (set using reflection) "}
            );
        }
    }
}
