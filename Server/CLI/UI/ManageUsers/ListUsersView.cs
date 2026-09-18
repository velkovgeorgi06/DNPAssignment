namespace CLI.UI.ManageUsers;

using RepositoryContracts;

public class ListUsersView
{
    private readonly IUserRepository userRepo;

    public ListUsersView(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }

    public Task ShowAsync()
    {
        Console.WriteLine("\n--- Users List ---");
        var users = userRepo.GetMany().ToList();

        if (!users.Any())
        {
            Console.WriteLine("No users found.");
            return Task.CompletedTask;
        }

        foreach (var u in users)
        {
            Console.WriteLine($"ID: {u.Id} | Username: {u.Name}");
        }

        return Task.CompletedTask;
    }
}