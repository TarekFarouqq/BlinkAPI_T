using AutoMapper;
using Blink_API.DTOs.Product;
using Blink_API.DTOs.ProductDTOs;
using Blink_API.Models;

namespace Blink_API.Services.ProductServices
{
    public class ProductReviewService
    {
        private readonly UnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public ProductReviewService(UnitOfWork _unitOfWork, IMapper _mapper)
        {
            unitOfWork = _unitOfWork;
            mapper = _mapper;
        }
        public async Task<bool> AddRevew(UserReviewCommentDTO userReviewCommentDTO)
        {
            List<ReviewComment> reviewComments=new List<ReviewComment>();
            reviewComments.Add(new ReviewComment
            {
                CommentId = 1,
                ReviewId = 1,
                Content = userReviewCommentDTO.Comment,
                IsDeleted = false
            });
            Review newReview = new Review()
            {
                Rate = userReviewCommentDTO.ReviewRate,
                CreationDate = DateTime.Now,
                UserId = userReviewCommentDTO.UserId,
                ProductId = userReviewCommentDTO.ProductId,
                ReviewComments = reviewComments
            };
            return await unitOfWork.ProductReviewRepo.AddReview(newReview); 
        }
    }
}
