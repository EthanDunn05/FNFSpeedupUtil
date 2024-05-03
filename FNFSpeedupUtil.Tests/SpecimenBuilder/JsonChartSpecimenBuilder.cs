using System.Reflection;
using AutoFixture;
using AutoFixture.Kernel;
using FNFSpeedupUtil.JsonData.OgChartData;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FNFSpeedupUtil.Tests.SpecimenBuilder;

/// <summary>
/// Correctly builds a <see cref="OgJsonChart"/>
/// </summary>
public class JsonChartSpecimenBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        // Configure properties
        if (request is PropertyInfo propertyInfo)
        {
            // Extra Json Data can be empty
            var isJsonExtensionData = propertyInfo.CustomAttributes
                .Any(attr => attr.AttributeType == typeof(JsonExtensionDataAttribute));
            if (isJsonExtensionData)
                return new Dictionary<string, JToken>();
        }

        if (request is Type type)
        {
            if (type == typeof(OgJsonNote))
                return new OgJsonNote
                {
                    context.Create<double>(),
                    context.Create<int>(),
                    context.Create<int>()
                };

            if (type == typeof(OgJsonEvent))
                return new OgJsonEvent {context.Create<double>()};
        }

        return new NoSpecimen();
    }
}