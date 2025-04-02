
namespace TechElite.Models
{
    public record ForumViewModel
    (
        ICollection<ForumCategory> ForumCategories,
        ICollection<ForumThread> ForumThreads,
        ICollection<Reply> Replies
    );
}
