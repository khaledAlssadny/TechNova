using ecommerce.Models;
using ecommerce.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ecommerce.Hubs
{
    public class CommentHub : Hub
    {
        protected readonly ICommentRepository commentRepository;
        protected readonly UserManager<ApplicationUser> userManager;

        // Team member names that are not allowed to appear in comments
        private static readonly string[] _bannedNames = new[]
        {
            "omar", "saeed", "abdelraheem", "hoda", "aswan"
        };

        public CommentHub(ICommentRepository commentRepository, UserManager<ApplicationUser> userManager)
        {
            this.commentRepository = commentRepository;
            this.userManager = userManager;
        }

        public async void SendComment(string userId, string Text, int ProductId)
        {
            // Check if the comment text contains any team member name (case-insensitive)
            string lowerText = Text?.ToLower() ?? string.Empty;
            bool containsBannedName = _bannedNames.Any(name => lowerText.Contains(name));

            if (containsBannedName)
            {
                // Notify only the sender that their comment was rejected
                await Clients.Caller.SendAsync(
                    "RejectedComment",
                    "Your comment was removed because it contains a restricted name."
                );
                return;
            }

            // Save comment to database
            Comment comment = new Comment() { UserId = userId, text = Text, ProductId = ProductId };
            commentRepository.Insert(comment);
            commentRepository.Save();

            Comment savedComment = commentRepository.Get(c => c.Id == comment.Id).ToList()[0];

            // Broadcast the comment to all connected clients on this product
            await Clients.All.SendAsync("ReciveComment", savedComment.User.UserName, Text, ProductId);
        }
    }
}

