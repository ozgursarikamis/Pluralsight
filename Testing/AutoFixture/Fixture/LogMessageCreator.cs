using System;

namespace Fixture
{
    public static class LogMessageCreator
    {
        public static LogMessage Create(string message, DateTime when)
        {
            return new LogMessage
                   {
                       Year = when.Year,
                       Message = message
                   };            
        }
    }
}