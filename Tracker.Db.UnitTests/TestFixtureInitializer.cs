using AutoMapper;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using Tracker.Db;
using Tracker.Db.AutofacModules;

[SetUpFixture]
public sealed class TestFixtureInitializer
{
    [OneTimeSetUp]
    public void Before_all()
    {
        TestInstances.Mapper = new MapperConfiguration(expression => AutoMapperModule.ApplyMapperConfiguration(expression, typeof(Program).Assembly)).CreateMapper();
    
        TestInstances.PeriodOptions = Options.Create(new PeriodOptions
        {
            ArchiveDataDelaySeconds = 90,
            TrackerDataRefreshSeconds = 30
        });
    }

    [OneTimeTearDown]
    public void After_all() { }
}