using System.Collections.Generic;
using TechElite.Models;
using Microsoft.AspNetCore.Identity;
using TechElite.Areas.Identity.Data;
namespace TechElite.Models
{
    public record ForumViewModel
    (
        ICollection<ForumCategory> ForumCategories,
        ICollection<ForumThread> ForumThreads,
        ICollection<ForumReply> ForumReplies,
        ApplicationUser? CurrentUser
    );
}
