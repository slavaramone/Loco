using System;
using AutoMapper;

static class TestInstances
{
    public static IMapper Mapper { get; set; }

    public static Random Random { get; } = new Random();
}
