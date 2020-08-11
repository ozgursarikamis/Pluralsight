namespace TennisBookings.Web.Configuration
{

    public interface IContentConfiguration
    {
        bool CheckForProfanity { get; }
    }
    public class ContentConfiguration : IContentConfiguration
    {
        public bool CheckForProfanity { get; set; }
    }
}
