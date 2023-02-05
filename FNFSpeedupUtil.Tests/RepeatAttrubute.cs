using System.Reflection;
using Xunit.Sdk;

namespace FNFSpeedupUtil.Tests;

// Blatantly stolen from https://stackoverflow.com/a/55687930
public sealed class RepeatAttribute : DataAttribute
{
    private readonly int _count;

    public RepeatAttribute(int count)
    {
        if (count < 1)
        {
            throw new ArgumentOutOfRangeException(
                paramName: nameof(count),
                message: "Repeat count must be greater than 0."
            );
        }
        this._count = count;
    }

    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        foreach (var iterationNumber in Enumerable.Range(start: 1, count: _count))
        {
            yield return new object[] { iterationNumber };
        }
    }
}