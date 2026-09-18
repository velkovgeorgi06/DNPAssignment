namespace CLI.UI.ManageComments;

using Entities;
using RepositoryContracts;

public class ListCommentsView
{
    private readonly ICommentRepository commentRepo;

    public ListCommentsView(ICommentRepository commentRepo)
    {
        this.commentRepo = commentRepo;
    }

    public Task ShowAsync()
    {
        Console.WriteLine("--- Comments Overview ---");
        var comments = commentRepo.GetMany().ToList();

        if (!comments.Any())
        {
            Console.WriteLine("No comments found.");
            return Task.CompletedTask;
        }

        foreach (var c in comments)
        {
            Console.WriteLine($"[ID: {c.Id}] Post ID: {c.PostId} | User ID: {c.UserId} | Body: \"{c.Body}\"");
        }

        return Task.CompletedTask;
    }
}