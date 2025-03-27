namespace TechElite.Models
{
    public class ForumViewModel
    {
        public ICollection<ForumCategory>? forumCategories { get; set; }
        public ICollection<Thread>? Threads { get; set; }
        public ICollection<Reply>? Replys { get; set; }

    }
}
