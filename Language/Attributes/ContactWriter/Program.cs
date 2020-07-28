using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using static System.Console;

namespace ContactWriter
{
    internal class Program
    {
        private static void Main()
        {
            var sarah = new Contact
            {
                FirstName = "Sarah", AgeInYears = 42
            };
            var sarahWriter = new ContactConsoleWriter(sarah);
            sarahWriter.Write();
            WriteLine("\n\n Hit enter to exit...");
            ReadLine();
        }
    }

    [DefaultColor(ConsoleColor = ConsoleColor.Magenta)]
    [DebuggerDisplay("First Name={FirstName} and Age In Years={AgeInYears}")]
    [DebuggerTypeProxy(typeof(ContactDebugDisplay))]
    public class Contact
    {
        [Indent]
        [Display("First Name: ", ConsoleColor.Cyan)]
        public string FirstName { get; set; }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int AgeInYears { get; set; }
    }

    internal class ContactDebugDisplay
    {
        private readonly Contact _contact;

        public ContactDebugDisplay(Contact contact)
        {
            _contact = contact;
        }

        public string UpperName => _contact.FirstName.ToUpperInvariant();
        public string AgeInHex => _contact.AgeInYears.ToString("X");
    }

    public class ContactConsoleWriter
    {
        private ConsoleColor _color;
        private static Contact _contact;
        public ContactConsoleWriter(Contact contact)
        {
            _contact = contact;
        }

        // [Obsolete("This method will be removed in next version", true)]
        public void Write()
        {
            UseDefaultColor();
            WriteFirstName();
            WriteAge();
        }

        private void UseDefaultColor()
        {
            DefaultColorAttribute defaultColorAttribute =
                (DefaultColorAttribute)Attribute.GetCustomAttribute(typeof(Contact), typeof(DisplayAttribute));
            if (defaultColorAttribute != null)
            {
                ForegroundColor = defaultColorAttribute.ConsoleColor;
            }
        }

        private void WriteAge()
        {
            WriteLine(_contact.AgeInYears);
        }

        private void WriteFirstName()
        {
            PropertyInfo firstNameProperty = 
                typeof(Contact).GetProperty(nameof(Contact.FirstName));

            DisplayAttribute firstNameDisplayAttribute =
                (DisplayAttribute)Attribute.GetCustomAttribute(firstNameProperty, typeof(DisplayAttribute));

            IndentAttribute[] indentAttributes =
                (IndentAttribute[])Attribute.GetCustomAttributes(firstNameProperty, typeof(IndentAttribute));

            PreserveForegroundColor();
            var sb = new StringBuilder();

            if (indentAttributes != null)
            {
                foreach (var indentAttribute in indentAttributes)
                {
                    sb.Append(new string(' ', 4));
                }
            }
            if (firstNameDisplayAttribute != null)
            {
                ForegroundColor = firstNameDisplayAttribute.Color;
                sb.Append(firstNameDisplayAttribute.Label);
            }

            if (_contact.FirstName != null)
            {
                sb.Append(_contact.FirstName);
            }

            WriteLine(sb);
            RestoreForegroundColor();
        }
        [Conditional("DEBUG")]
        private static void OutputDebugInfo()
        {
            WriteLine("*** DEBUG MODE ***");
        }
        [Conditional("EXTRA")]
        private void OutputExtraInfo()
        {
            WriteLine("*** EXTRA MODE ***");
        }

        private void PreserveForegroundColor()
        {
            _color = ForegroundColor;
        }

        private void RestoreForegroundColor()
        {
            ForegroundColor = _color;
        }
    }
}
