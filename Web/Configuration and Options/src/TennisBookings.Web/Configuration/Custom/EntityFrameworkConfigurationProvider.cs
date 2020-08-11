using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TennisBookings.Web.Data;

namespace TennisBookings.Web.Configuration.Custom
{
    public class EntityFrameworkConfigurationProvider : ConfigurationProvider
    {
        public Action<DbContextOptionsBuilder> OptionsAction { get; }

        public EntityFrameworkConfigurationProvider(Action<DbContextOptionsBuilder> optionsAction)
        {
            OptionsAction = optionsAction;
        }

        public override void Load()
        {
            var builder = new DbContextOptionsBuilder<TennisBookingDbContext>();
            OptionsAction(builder);

            using (var dbContext = new TennisBookingDbContext(builder.Options))
            {
                Data = dbContext.ConfigurationEntries.Any()
                    ? dbContext.ConfigurationEntries.ToDictionary(c => c.Key, c => c.Value)
                    : new Dictionary<string, string>();
            }
        }
    }

    public class EntityFrameworkConfigurationSource : IConfigurationSource
    {
        private readonly Action<DbContextOptionsBuilder> _optionsAction;

        public EntityFrameworkConfigurationSource(Action<DbContextOptionsBuilder> optionsAction)
        {
            _optionsAction = optionsAction;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new EntityFrameworkConfigurationProvider(_optionsAction);
        }
    }

    public static class EntityFrameworkExtensions
    {
        public static IConfigurationBuilder AddEFConfiguration(
            this IConfigurationBuilder builder,
            Action<DbContextOptionsBuilder> optionsAction)
        {
            return builder.Add(new EntityFrameworkConfigurationSource(optionsAction));
        }
    }
}
