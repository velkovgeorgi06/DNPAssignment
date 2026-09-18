using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class CommentInMemoryRepository : ICommentRepository
{
    private List<Comment> comments;
    public CommentInMemoryRepository()
    {
        comments = new List<Comment>
        {
            new Comment { Id = 1, Body = "Great article, very clear!", UserId = 2, PostId = 1 },
            new Comment { Id = 2, Body = "Thanks for explaining this so simply.", UserId = 3, PostId = 1 },
            new Comment { Id = 3, Body = "Dependency injection saved my architecture.", UserId = 1, PostId = 2 },
            new Comment { Id = 4, Body = "LINQ is definitely my favorite C# feature.", UserId = 4, PostId = 3 }
        };
    }
    public Task<Comment> AddAsync(Comment comment)
    {
        comment.Id = comments.Any()
            ? comments.Max(p => p.Id) + 1
            : 1;
        comments.Add(comment);
        return Task.FromResult(comment);
    }
    public Task UpdateAsync(Comment comment)
    {
        Comment? existingComment = comments.SingleOrDefault(p => p.Id == comment.Id);
        if (existingComment is null)
        {
            throw new InvalidOperationException(
                $"Comment with ID '{comment.Id}' not found");
        }

        comments.Remove(existingComment);
        comments.Add(comment);

        return Task.CompletedTask;
    }
    public Task DeleteAsync(int id)
    {
        Comment? commentToRemove = comments.SingleOrDefault(p => p.Id == id);
        if (commentToRemove is null)
        {
            throw new InvalidOperationException(
                $"Comment with ID '{id}' not found");
        }

        comments.Remove(commentToRemove);
        return Task.CompletedTask;
    }
    public Task<Comment> GetSingleAsync(int id)
    {
        Comment? commentToFind = comments.SingleOrDefault(p => p.Id == id);
        if (commentToFind is null)
        {
            throw new InvalidOperationException(
                $"Comment with ID '{id}' not found");
        }
        return Task.FromResult(commentToFind);
    }
    public IQueryable<Comment> GetMany()
    {
        return comments.AsQueryable();
    }
}