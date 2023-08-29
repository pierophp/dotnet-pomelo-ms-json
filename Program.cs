using DotnetPomeloMsJson;

using var db = new BloggingContext();

var post = new Post()
{
    Title = "Test",
    Tags = new List<Tag>
    {
        new Tag() { Text = "A" },
        new Tag() { Text = "B" }
    }
};

Console.WriteLine("Inserting a new post");
db.Add(post);
db.SaveChanges();

// Read
Console.WriteLine("Querying for a post");

var post2 = db.Posts.OrderBy(b => b.Title).First();

Console.WriteLine(post2);
