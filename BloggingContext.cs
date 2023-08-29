using System.Diagnostics.Tracing;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DotnetPomeloMsJson;

public class BloggingContext : DbContext
{
    public DbSet<Post> Posts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = "server=localhost;user=root;password=root;database=ef;port=33062";

        var serverVersion = new MySqlServerVersion(new Version(8, 0, 31));

        optionsBuilder
            .UseMySql(connectionString, serverVersion, options => options.UseMicrosoftJson())
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql/issues/1752
        modelBuilder.Entity<Post>().Property(p => p.Tags).HasColumnType("json")
        // .OwnsOne(
        //     author => author.Tags,
        //     ownedNavigationBuilder =>
        //     {
        //         ownedNavigationBuilder.HasColumnType("json")
        //         ownedNavigationBuilder.ToJson();
        //     }
        // )
        ;
    }
}

public class Tag
{
    public string Text { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Code { get; set; }
}

public class Post
{
    public int Id { get; private set; }
    public string Title { get; set; }
    public List<Tag> Tags { get; set; } = new();
}
