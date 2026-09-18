using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class PostInMemoryRepository : IPostRepository
{
    private List<Post> posts;
    public PostInMemoryRepository()
    {
        posts = new List<Post>
        {
            new Post { Id = 1, Title = "Getting Started with C#", Body = "C# and .NET are powerful tools for building modern applications.", UserId = 1 },
            new Post { Id = 2, Title = "Dependency Injection Explained", Body = "Passing dependencies via constructors makes testing clean and modular.", UserId = 2 },
            new Post { Id = 3, Title = "LINQ Basics", Body = "LINQ lets you query collections effortlessly in C#.", UserId = 1 },
            new Post { Id = 4, Title = "Console Apps UI", Body = "Structuring CLI menus with single responsibility view classes.", UserId = 3 }
        };
    }
    public Task<Post> AddAsync(Post post)
    {
        post.Id = posts.Any()
            ? posts.Max(p => p.Id) + 1
            : 1;
        posts.Add(post);
        return Task.FromResult(post);
    }
    public Task UpdateAsync(Post post)
    {
        Post? existingPost = posts.SingleOrDefault(p => p.Id == post.Id);
        if (existingPost is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{post.Id}' not found");
        }

        posts.Remove(existingPost);
        posts.Add(post);

        return Task.CompletedTask;
    }
    public Task DeleteAsync(int id)
    {
        Post? postToRemove = posts.SingleOrDefault(p => p.Id == id);
        if (postToRemove is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{id}' not found");
        }

        posts.Remove(postToRemove);
        return Task.CompletedTask;
    }
    public Task<Post> GetSingleAsync(int id)
    {
        Post? postToFind = posts.SingleOrDefault(p => p.Id == id);
        if (postToFind is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{id}' not found");
        }
        return Task.FromResult(postToFind);
    }
    public IQueryable<Post> GetMany()
    {
        return posts.AsQueryable();
    }
}