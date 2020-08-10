using System;
using System.Reflection;
using AutoFixture.Kernel;

namespace Fixture.Tests.SpecimenBuilders
{
    public class AirportCodeStringPropertyGenerator : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            // see if we are trying to create a value for a property:
            var propertyInfo = request as PropertyInfo;
            if (propertyInfo is null)
            {
                // this specimen builder does not apply to current request

                // Null is a valid specimen so retun NoSpecimen
                return new NoSpecimen(); 
            }

            // now er know we're dealing with a property 
            // are we creating a value for an airport code?

            var isAirportCodeProperty = propertyInfo.Name.Contains("AirportCode");
            var isStringProperty = propertyInfo.PropertyType == typeof(string);

            if (isAirportCodeProperty && isStringProperty)
            {
                return RandomAirportCode();
            }

            return new NoSpecimen();
        }

        private static string RandomAirportCode() => 
            DateTime.Now.Ticks % 2 == 0 ? "LHR" : "PER";
    }
}
