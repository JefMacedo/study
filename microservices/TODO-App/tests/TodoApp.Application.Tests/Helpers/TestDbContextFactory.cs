using System;
using Microsoft.EntityFrameworkCore;
using TodoApp.Infrastructure.Persistence;

namespace TodoApp.Application.Tests.Helpers;

public static class TestDbContextFactory
{
    public static AppDbContext CreateInMemoryContext(string dbName = null)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(dbName ?? Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }
}
