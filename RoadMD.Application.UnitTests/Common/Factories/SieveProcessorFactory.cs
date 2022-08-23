using Microsoft.Extensions.Options;
using Moq;
using RoadMD.Application.Common.Sieve;
using Sieve.Models;
using Sieve.Services;


namespace RoadMD.Application.UnitTests.Common.Factories;

public static class SieveProcessorFactory
{
    public static ISieveProcessor Create()
    {
        return new ApplicationSieveProcessor(
            new SieveOptionsAccessor(),
            new SieveCustomSortMethods(),
            new SieveCustomFilterMethods());
    }
}

public class SieveOptionsAccessor : IOptions<SieveOptions>
{
    public SieveOptions Value { get; }

    public SieveOptionsAccessor()
    {
        Value = new SieveOptions
        {
            ThrowExceptions = true
        };
    }
}