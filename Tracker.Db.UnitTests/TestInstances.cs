using AutoMapper;
using Microsoft.Extensions.Options;
using System;
using Tracker.Db;

static class TestInstances
{
    public static IMapper Mapper { get; set; }

    public static IOptions<PeriodOptions> PeriodOptions { get; set; }

    public static Random Random { get; } = new Random();
}
