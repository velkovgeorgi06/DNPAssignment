namespace CLI.UI.ManagePosts;

using RepositoryContracts;

public class ListPostsView
{
    private readonly IPostRepository postRepo;

    public ListPostsView(IPostRepository postRepo)
    {
        this.postRepo = postRepo;
    }

    public Task ShowAsync()
    {
        Console.WriteLine("--- Posts Overview ---");
        var posts = postRepo.GetMany().Select(p => new { p.Id, p.Title }).ToList();

        if (!posts.Any())
        {
            Console.WriteLine("No posts found.");
            return Task.CompletedTask;
        }

        foreach (var p in posts)
        {
            Console.WriteLine($"[ID: {p.Id}] Title: {p.Title}");
        }

        return Task.CompletedTask;
    }
}