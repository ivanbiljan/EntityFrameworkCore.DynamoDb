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
context.Database.EnsureCreated();

public class Context : DbContext
{
    public Context(DbContextOptions<Context> optionsBuilder) : base(optionsBuilder)
    {
        
    }
    
    public DbSet<Entity> Entities => Set<Entity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Entity>().ToDynamoDbTable("entity").HasKey(e => e.PartitionKey);
        modelBuilder.Entity<Entity>().HasData(
            new Entity
            {
                PartitionKey = "pk1",
                SortKey = "sk2",
                Age = 11
            });
        
        base.OnModelCreating(modelBuilder);
    }
}

public class Entity
{
    public string PartitionKey { get; set; }

    public string SortKey { get; set; }

    public int Age { get; set; }
}