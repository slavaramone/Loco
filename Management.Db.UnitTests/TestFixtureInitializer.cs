using AutoMapper;
using Management.Db;
using Management.Db.AutofacModules;
using NUnit.Framework;

[SetUpFixture]
public sealed class TestFixtureInitializer
{
    [OneTimeSetUp]
    public void Before_all()
    {
        TestInstances.Mapper = new MapperConfiguration(expression => AutoMapperModule.ApplyMapperConfiguration(expression, typeof(Program).Assembly)).CreateMapper();
    }

    [OneTimeTearDown]
    public void After_all() { }
}