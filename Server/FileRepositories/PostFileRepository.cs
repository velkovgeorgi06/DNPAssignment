namespace FileRepositories;

using System.Text.Json;
using Entities;
using RepositoryContracts;

public class PostFileRepository : IPostRepository
{
    private readonly string filePath = "posts.json";

    public PostFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }

    private async Task<List<Post>> LoadDataAsync()
    {
        string json = await File.ReadAllTextAsync(filePath);
        return JsonSerializer.Deserialize<List<Post>>(json) ?? new List<Post>();
    }

    private async Task SaveDataAsync(List<Post> posts)
    {
        string json = JsonSerializer.Serialize(posts, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(filePath, json);
    }

    public async Task<Post> AddAsync(Post post)
    {
        List<Post> posts = await LoadDataAsync();
        int maxId = posts.Count > 0 ? posts.Max(p => p.Id) : 0;
        post.Id = maxId + 1;
        posts.Add(post);
        await SaveDataAsync(posts);
        return post;
    }

    public async Task UpdateAsync(Post post)
    {
        List<Post> posts = await LoadDataAsync();
        Post? existing = posts.FirstOrDefault(p => p.Id == post.Id);
        if (existing is null)
        {
            throw new InvalidOperationException($"Post with ID '{post.Id}' not found.");
        }

        posts.Remove(existing);
        posts.Add(post);
        await SaveDataAsync(posts);
    }

    public async Task DeleteAsync(int id)
    {
        List<Post> posts = await LoadDataAsync();
        Post? existing = posts.FirstOrDefault(p => p.Id == id);
        if (existing is null)
        {
            throw new InvalidOperationException($"Post with ID '{id}' not found.");
        }

        posts.Remove(existing);
        await SaveDataAsync(posts);
    }

    public async Task<Post> GetSingleAsync(int id)
    {
        List<Post> posts = await LoadDataAsync();
        Post? existing = posts.FirstOrDefault(p => p.Id == id);
        if (existing is null)
        {
            throw new InvalidOperationException($"Post with ID '{id}' not found.");
        }

        return existing;
    }

    public IQueryable<Post> GetMany()
    {
        string json = File.ReadAllTextAsync(filePath).Result;
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(json) ?? new List<Post>();
        return posts.AsQueryable();
    }
}