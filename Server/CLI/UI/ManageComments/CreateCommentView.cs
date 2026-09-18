namespace CLI.UI.ManageComments;

using Entities;
using RepositoryContracts;

public class CreateCommentView
{
    private readonly ICommentRepository commentRepo;
    private readonly IPostRepository postRepo;
    private readonly IUserRepository userRepo;

    public CreateCommentView(ICommentRepository commentRepo, IPostRepository postRepo, IUserRepository userRepo)
    {
        this.commentRepo = commentRepo;
        this.postRepo = postRepo;
        this.userRepo = userRepo;
    }

    public async Task ShowAsync()
    {
        Console.WriteLine("--- Add Comment ---");
        
        Console.Write("Post ID: ");
        int postId = int.Parse(Console.ReadLine() ?? "0");
        await postRepo.GetSingleAsync(postId); // Validate post exists

        Console.Write("User ID: ");
        int userId = int.Parse(Console.ReadLine() ?? "0");
        await userRepo.GetSingleAsync(userId); // Validate user exists

        Console.Write("Comment Body: ");
        string body = Console.ReadLine() ?? "";

        Comment comment = await commentRepo.AddAsync(new Comment 
        { 
            PostId = postId, 
            UserId = userId, 
            Body = body 
        });

        Console.WriteLine($"Comment created successfully with ID: {comment.Id}");
    }
}