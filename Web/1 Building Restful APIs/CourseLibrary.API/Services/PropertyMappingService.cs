using System;
using System.Collections.Generic;
using System.Linq;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Models;

namespace CourseLibrary.API.Services
{
    public interface IPropertyMapping { }
    public interface IPropertyMappingService
    {
        Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();
    }

    public class PropertyMapping<TSource, TDestination> : IPropertyMapping
    {
        public Dictionary<string, PropertyMappingValue> _mappingDictionary { get; }

        public PropertyMapping(Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            _mappingDictionary = mappingDictionary ??
                                 throw new ArgumentNullException(nameof(mappingDictionary));
        }
    }


    public class PropertyMappingService : IPropertyMappingService
    {
        private readonly Dictionary<string, PropertyMappingValue> _authorPropertyMapping
            = new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                { "Id", new PropertyMappingValue(new List<string>{ "Id" }) },
                { "MainCategory", new PropertyMappingValue(new List<string>{ "MainCategory" }) },
                { "Age", new PropertyMappingValue(new List<string>{ "DateOfBirth" }, true) },
                { "Name", new PropertyMappingValue(new List<string>{ "FirstName", "LastName" }) }
            };

        private readonly IList<IPropertyMapping> _propertyMappings 
            = new List<IPropertyMapping>();

        public PropertyMappingService()
        {
            _propertyMappings.Add(new PropertyMapping<AuthorDto, Author>(_authorPropertyMapping));
            
        }

        public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>()
        {
            // get matching mapping:
            var matchingMapping = _propertyMappings
                .OfType<PropertyMapping<TSource, TDestination>>();

            var propertyMappings = matchingMapping as PropertyMapping<TSource, TDestination>[] ?? matchingMapping.ToArray();
            if (propertyMappings.Length == 1)
            {
                return propertyMappings.First()._mappingDictionary;
            }

            throw new Exception("Cannot find exact property mapping instance " +
                                $"for <{typeof(TSource)}, {typeof(TDestination)}");
        }
    }

    public class PropertyMappingValue
    {
        public IEnumerable<string> DestinationProperties { get; set; }
        public bool Revert { get; set; }

        public PropertyMappingValue(IEnumerable<string> destinationProperties,
            bool revert = false)
        {
            DestinationProperties = destinationProperties
                                    ?? throw new ArgumentNullException(nameof(destinationProperties));
            Revert = revert;
        }
    }
}