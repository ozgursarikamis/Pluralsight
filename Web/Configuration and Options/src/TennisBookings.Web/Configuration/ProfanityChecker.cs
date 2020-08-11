namespace TennisBookings.Web.Configuration
{
    public interface IProfanityChecker
    {
        bool ContainsProfanity(string input);
    }
    public class ProfanityChecker : IProfanityChecker
    {
        public bool ContainsProfanity(string input)
        {
            return !string.IsNullOrEmpty(input) && input.ToLowerInvariant().Contains("darn");
        }
    }
}
