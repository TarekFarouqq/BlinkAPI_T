using Blink_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Blink_API.Repositories.ProductRepos
{
    public class ProductReviewRepo:GenericRepo<Review,int>
    {
        private readonly BlinkDbContext db;
        public ProductReviewRepo(BlinkDbContext _db) : base(_db)
        {
            db = _db;
        }
        public async Task AddUserReview(Review review)
        {
            var existingReview = await db.Reviews
                .FirstOrDefaultAsync(r => r.UserId == review.UserId && r.ProductId == review.ProductId);
            if(existingReview!= null)
            {
                existingReview.Rate = review.Rate;
                existingReview.CreationDate = DateTime.Now;
                db.Reviews.Update(existingReview);
            }
            else
            {
                review.CreationDate = DateTime.Now;
                db.Reviews.Add(review);
            }
           await SaveChanges();
        }
        public async Task AddReviewComment(int reviewId, ReviewComment reviewComment)
        {
            var review = await db.Reviews.FindAsync(reviewId);
            if (review != null)
            {
                reviewComment.CommentId = db.ReviewComments.Any() ? db.ReviewComments.Max(rc => rc.CommentId) + 1 : 1;
                db.ReviewComments.Add(reviewComment);
            }
            await SaveChanges();
        }
        public async Task UpdateReviewComment(int CommentId, ReviewComment reviewComment)
        {
            if(reviewComment != null)
            {
                var existingComment = await db.ReviewComments.FindAsync(CommentId);
                if (existingComment != null)
                {
                    existingComment.Content = reviewComment.Content;
                    db.ReviewComments.Update(existingComment);
                }
            }
            await SaveChanges();
        }
        public async Task DeleteReviewComment(int CommentId)
        {
            var existingComment = await db.ReviewComments.FindAsync(CommentId);
            if (existingComment != null)
            {
                existingComment.IsDeleted = true;
                db.ReviewComments.Update(existingComment);
            }
            await SaveChanges();
        }
        public async Task<List<Review>> GetAllReview()
        {
            var reviews = await db.Reviews
                .Include(r => r.User)
                .Include(r => r.ReviewComments)
                .Where(r => !r.IsDeleted)
                .ToListAsync();
            return reviews;
        }
        public async Task<Review?> GetReviewById(int reviewId)
        {
            var review = await db.Reviews
                .Include(r => r.User)
                .Include(r => r.ReviewComments)
                .FirstOrDefaultAsync(r => r.ReviewId == reviewId && !r.IsDeleted);
            return review;
        }
        public async Task<List<ReviewComment>> GetCommentByReviewId(int reviewId)
        {
            var comments = await db.ReviewComments
                .Where(rc => rc.ReviewId == reviewId && !rc.IsDeleted)
                .ToListAsync();
            return comments;
        }
        // New Methods
        public async Task<bool> AddReview(Review review)
        {
            if(review != null)
            {
                foreach(var comment in review.ReviewComments)
                {
                    comment.CommentId = db.ReviewComments.Any() ? db.ReviewComments.Max(rc => rc.CommentId) + 1 : 1;
                    comment.ReviewId = review.ReviewId; 
                    comment.IsDeleted = false;
                }
                await db.Reviews.AddAsync(review);
                await db.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
