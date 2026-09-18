namespace CLI.UI.ManageUsers;

using Entities;
using RepositoryContracts;

public class CreateUserView
{
    private readonly IUserRepository userRepo;

    public CreateUserView(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }

    public async Task ShowAsync()
    {
        Console.WriteLine("--- Create New User ---");
        Console.Write("Username: ");
        string name = Console.ReadLine() ?? "";
        Console.Write("Password: ");
        string password = Console.ReadLine() ?? "";
        User created = await userRepo.AddAsync(new User { Name = name, Password = password });
        Console.WriteLine($"User '{created.Name}' created with ID: {created.Id}");
    }
}