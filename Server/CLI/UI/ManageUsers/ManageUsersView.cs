namespace CLI.UI.ManageUsers;

using Entities;
using RepositoryContracts;

public class ManageUsersView
{
    private readonly IUserRepository userRepo;

    public ManageUsersView(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }

    public async Task ShowAsync()
    {
        Console.WriteLine("--- Manage Users ---");
        Console.WriteLine("1. Create User");
        Console.WriteLine("2. View All Users");
        Console.WriteLine("3. Update User");
        Console.WriteLine("4. Delete User");
        Console.Write("Choice: ");

        string? choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                await new CreateUserView(userRepo).ShowAsync();
                break;
            case "2":
                await new ListUsersView(userRepo).ShowAsync();
                break;
            case "3":
                Console.Write("Enter User ID to update: ");
                int updateId = int.Parse(Console.ReadLine() ?? "0");
                User userToUpdate = await userRepo.GetSingleAsync(updateId);

                Console.Write($"New Name (current: {userToUpdate.Name}): ");
                userToUpdate.Name = Console.ReadLine() ?? userToUpdate.Name;
                Console.Write("New Password: ");
                userToUpdate.Password = Console.ReadLine() ?? userToUpdate.Password;

                await userRepo.UpdateAsync(userToUpdate);
                Console.WriteLine("User updated successfully!");
                break;
            case "4":
                Console.Write("Enter User ID to delete: ");
                int deleteId = int.Parse(Console.ReadLine() ?? "0");
                await userRepo.DeleteAsync(deleteId);
                Console.WriteLine("User deleted successfully!");
                break;
        }
    }
}