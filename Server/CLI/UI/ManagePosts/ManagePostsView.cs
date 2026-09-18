namespace CLI.UI.ManagePosts;

using RepositoryContracts;

public class ManagePostsView
{
    private readonly IPostRepository postRepo;
    private readonly IUserRepository userRepo;
    private readonly ICommentRepository commentRepo;

    public ManagePostsView(IPostRepository postRepo, IUserRepository userRepo, ICommentRepository commentRepo)
    {
        this.postRepo = postRepo;
        this.userRepo = userRepo;
        this.commentRepo = commentRepo;
    }

    public async Task ShowAsync()
    {
        Console.WriteLine("--- Manage Posts ---");
        Console.WriteLine("1. Create Post");
        Console.WriteLine("2. Overview of Posts");
        Console.WriteLine("3. View Single Post & Comments");
        Console.Write("Choice: ");

        string? choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                await new CreatePostView(postRepo, userRepo).ShowAsync();
                break;
            case "2":
                await new ListPostsView(postRepo).ShowAsync();
                break;
            case "3":
                await new SinglePostView(postRepo, userRepo, commentRepo).ShowAsync();
                break;
        }
    }
}