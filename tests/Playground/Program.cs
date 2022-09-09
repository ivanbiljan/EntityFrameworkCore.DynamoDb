// See https://aka.ms/new-console-template for more information

using DynamoDb.Linq.Extensions;
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Hello, World!");

var optionsBuilder = new DbContextOptionsBuilder<Context>()
    .UseDynamoDb("9qbibn", "od7t6m",
        options =>
        {
            options.WithServiceUrl("http://localhost:8000");
        });

var context = new Context(optionsBuilder.Options);
await context.Database.EnsureCreatedAsync();

public class Context : DbContext
{
    public Context(DbContextOptions<Context> optionsBuilder) : base(optionsBuilder)
    {
        
    }
    
    public DbSet<Entity> Entities => Set<Entity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Entity>().ToDynamoDbTable("entity");
        
        base.OnModelCreating(modelBuilder);
    }
}

public class Entity
{
    public string PartitionKey { get; set; }

    public string SortKey { get; set; }

    public int Age { get; set; }
}