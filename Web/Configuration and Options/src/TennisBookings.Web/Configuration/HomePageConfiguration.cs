﻿namespace TennisBookings.Web.Configuration
{
    public class HomePageConfiguration
    {
        public bool EnableGreeting { get; set; }
        public bool EnableWeatherForecast { get; set; }
        public string ForecastSectionTitle { get; set; }
    }

    public class GreetingConfiguration
    { 
        public string GreetingColour { get; set; }
    }
}