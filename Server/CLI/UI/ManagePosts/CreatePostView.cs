namespace CLI.UI.ManagePosts;

using Entities;
using RepositoryContracts;

public class CreatePostView
{
    private readonly IPostRepository postRepo;
    private readonly IUserRepository userRepo;

    public CreatePostView(IPostRepository postRepo, IUserRepository userRepo)
    {
        this.postRepo = postRepo;
        this.userRepo = userRepo;
    }

    public async Task ShowAsync()
    {
        Console.WriteLine("--- Create New Post ---");
        Console.Write("User ID: ");
        int userId = int.Parse(Console.ReadLine() ?? "0");
        await userRepo.GetSingleAsync(userId);
        Console.Write("Title: ");
        string title = Console.ReadLine() ?? "";
        Console.Write("Body: ");
        string body = Console.ReadLine() ?? "";
        Post post = await postRepo.AddAsync(new Post { Title = title, Body = body, UserId = userId });
        Console.WriteLine($"Post created successfully with ID: {post.Id}");
    }
}