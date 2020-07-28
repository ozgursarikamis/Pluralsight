using System;

namespace ContactWriter
{
    [AttributeUsage(AttributeTargets.Property)] // only for properties
    public class DisplayAttribute : Attribute
    {
        public DisplayAttribute(string label, ConsoleColor color = ConsoleColor.White)
        {
            Label = label ?? throw new ArgumentNullException(nameof(label));
            Color = color;
        }

        public string Label { get; }
        public ConsoleColor Color { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class)] // only for properties
    public class DefaultColorAttribute : Attribute
    {
        public ConsoleColor ConsoleColor { get; set; } = ConsoleColor.Red;
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class IndentAttribute : Attribute
    {

    }

}
