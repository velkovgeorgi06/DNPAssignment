namespace CLI.UI.ManagePosts;

using Entities;
using RepositoryContracts;

public class SinglePostView
{
    private readonly IPostRepository postRepo;
    private readonly IUserRepository userRepo;
    private readonly ICommentRepository commentRepo;

    public SinglePostView(IPostRepository postRepo, IUserRepository userRepo, ICommentRepository commentRepo)
    {
        this.postRepo = postRepo;
        this.userRepo = userRepo;
        this.commentRepo = commentRepo;
    }

    public async Task ShowAsync()
    {
        Console.Write("Enter Post ID: ");
        int postId = int.Parse(Console.ReadLine() ?? "0");

        Post post = await postRepo.GetSingleAsync(postId);
        User author = await userRepo.GetSingleAsync(post.UserId);
        var comments = commentRepo.GetMany().Where(c => c.PostId == postId).ToList();

        Console.WriteLine($"\n====================================");
        Console.WriteLine($"TITLE: {post.Title} (ID: {post.Id})");
        Console.WriteLine($"AUTHOR: {author.Name} (User ID: {author.Id})");
        Console.WriteLine($"------------------------------------");
        Console.WriteLine($"{post.Body}");
        Console.WriteLine($"====================================");
        Console.WriteLine($"COMMENTS ({comments.Count}):");

        foreach (var c in comments)
        {
            string commenterName = "Unknown";
            try { commenterName = (await userRepo.GetSingleAsync(c.UserId)).Name; } catch { }
            Console.WriteLine($" - [{commenterName}]: {c.Body} (Comment ID: {c.Id})");
        }
    }
}