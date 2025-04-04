using System.Collections.Generic;
using TechElite.Models;
using Microsoft.AspNetCore.Identity;
namespace TechElite.Models
{
    public record ForumViewModel
    (
        ICollection<ForumCategory> ForumCategories,
        ICollection<ForumThread> ForumThreads,
        ICollection<ForumReply> ForumReplies
    );
}
