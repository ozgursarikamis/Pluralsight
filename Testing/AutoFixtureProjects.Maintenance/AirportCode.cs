using System;

namespace AutoFixtureProjects.Maintenance
{
    public class AirportCode
    {
        private readonly string _code;
        public AirportCode(string code)
        {
            if (code is null)
            {
                throw new ArgumentNullException(nameof(code));
            }

            if (!IsValidAirportCode(code))
            {
                throw new ArgumentNullException(nameof(code));
            }

            _code = code;
        }

        public override string ToString()
        {
            return _code;
        }

        private static bool IsValidAirportCode(string code)
        {
            var isWrongLength = code.Length != 3;
            var isWrongCase = code != code.ToUpperInvariant();

            return !isWrongCase && !isWrongLength;
        }
    }
}