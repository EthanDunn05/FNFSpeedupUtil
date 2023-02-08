using AutoFixture;
using AutoFixture.Xunit2;
using FNFSpeedupUtil.Tests.SpecimenBuilder;

namespace FNFSpeedupUtil.Tests.AutoData;

public class AutoJsonChartDataAttribute : AutoDataAttribute
{
    public AutoJsonChartDataAttribute() : base(FixtureFactory)
    {
    }

    private static IFixture FixtureFactory()
    {
        var fixture = new Fixture();
        fixture.Customizations.Add(new JsonChartSpecimenBuilder());
        return fixture;
    }
}