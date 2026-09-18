using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class UserInMemoryRepository : IUserRepository
{
    private List<User> users;
    public UserInMemoryRepository()
    {
        users = new List<User>
        {
            new User { Id = 1, Name = "Alice", Password = "password123" },
            new User { Id = 2, Name = "Bob", Password = "password123" },
            new User { Id = 3, Name = "Charlie", Password = "password123" },
            new User { Id = 4, Name = "Diana", Password = "password123" }
        };
    }
    public Task<User> AddAsync(User user)
    {
        user.Id = users.Any()
            ? users.Max(p => p.Id) + 1
            : 1;
        users.Add(user);
        return Task.FromResult(user);
    }
    public Task UpdateAsync(User user)
    {
        User? existingUser = users.SingleOrDefault(p => p.Id == user.Id);
        if (existingUser is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{user.Id}' not found");
        }

        users.Remove(existingUser);
        users.Add(user);

        return Task.CompletedTask;
    }
    public Task DeleteAsync(int id)
    {
        User? userToRemove = users.SingleOrDefault(p => p.Id == id);
        if (userToRemove is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{id}' not found");
        }

        users.Remove(userToRemove);
        return Task.CompletedTask;
    }
    public Task<User> GetSingleAsync(int id)
    {
        User? userToFind = users.SingleOrDefault(p => p.Id == id);
        if (userToFind is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{id}' not found");
        }
        return Task.FromResult(userToFind);
    }
    public IQueryable<User> GetMany()
    {
        return users.AsQueryable();
    }
}