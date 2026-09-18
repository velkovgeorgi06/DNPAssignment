namespace CLI.UI;

using CLI.UI.ManageComments;
using CLI.UI.ManagePosts;
using CLI.UI.ManageUsers;
using RepositoryContracts;

public class CliApp
{
    private readonly IUserRepository userRepo;
    private readonly ICommentRepository commentRepo;
    private readonly IPostRepository postRepo;

    public CliApp(IUserRepository userRepo, ICommentRepository commentRepo, IPostRepository postRepo)
    {
        this.userRepo = userRepo;
        this.commentRepo = commentRepo;
        this.postRepo = postRepo;
    }

    public async Task StartAsync()
    {
        bool running = true;

        while (running)
        {
            Console.Clear();
            Console.WriteLine("=== MAIN MENU ===");
            Console.WriteLine("1. Manage Users");
            Console.WriteLine("2. Manage Posts");
            Console.WriteLine("3. Manage Comments");
            Console.WriteLine("4. Exit");
            Console.Write("\nSelect an option: ");

            string? choice = Console.ReadLine();
            Console.Clear();

            try
            {
                switch (choice)
                {
                    case "1":
                        await new ManageUsersView(userRepo).ShowAsync();
                        break;
                    case "2":
                        await new ManagePostsView(postRepo, userRepo, commentRepo).ShowAsync();
                        break;
                    case "3":
                        await new ManageCommentsView(commentRepo, postRepo, userRepo).ShowAsync();
                        break;
                    case "4":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            if (running)
            {
                Console.WriteLine("\nPress ANY key to continue...");
                Console.ReadKey();
            }
        }
    }
}