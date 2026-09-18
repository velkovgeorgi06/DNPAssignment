namespace CLI.UI.ManageComments;

using RepositoryContracts;

public class ManageCommentsView
{
    private readonly ICommentRepository commentRepo;
    private readonly IPostRepository postRepo;
    private readonly IUserRepository userRepo;

    public ManageCommentsView(ICommentRepository commentRepo, IPostRepository postRepo, IUserRepository userRepo)
    {
        this.commentRepo = commentRepo;
        this.postRepo = postRepo;
        this.userRepo = userRepo;
    }

    public async Task ShowAsync()
    {
        Console.WriteLine("--- Manage Comments ---");
        Console.WriteLine("1. Create Comment");
        Console.WriteLine("2. View All Comments");
        Console.WriteLine("3. Delete Comment");
        Console.Write("Choice: ");

        string? choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                await new CreateCommentView(commentRepo, postRepo, userRepo).ShowAsync();
                break;
            case "2":
                await new ListCommentsView(commentRepo).ShowAsync();
                break;
            case "3":
                Console.Write("Enter Comment ID to delete: ");
                int deleteId = int.Parse(Console.ReadLine() ?? "0");
                await commentRepo.DeleteAsync(deleteId);
                Console.WriteLine("Comment deleted successfully!");
                break;
        }
    }
}