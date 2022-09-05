// See https://aka.ms/new-console-template for more information

using DynamoDb.Linq.Extensions;
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Hello, World!");

var optionsBuilder = new DbContextOptionsBuilder<Context>()
    .UseDynamoDb("https://localhost:8000");
var context = new Context(optionsBuilder.Options);
context.Database.EnsureCreated();

public class Context : DbContext
{
    public Context(DbContextOptions<Context> optionsBuilder) : base(optionsBuilder)
    {
        
    }
    
    public DbSet<Entity> Entities => Set<Entity>();
}

public class Entity
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public int Age { get; set; }
}