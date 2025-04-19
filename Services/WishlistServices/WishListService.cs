using AutoMapper;
using Blink_API.DTOs.CartDTOs;
using Blink_API.DTOs.WishListDTOs;
using Blink_API.Models;

namespace Blink_API.Services.WishlistServices
{
    public class WishListService
    {

        private readonly UnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public WishListService(UnitOfWork _unitOfWork, IMapper _mapper)
        {
            unitOfWork = _unitOfWork;
            mapper = _mapper;
        }

        public async Task<List<ReadWishListDTO>> GetAllWishLists(int pgNumber, int pgSize)
        {
            var wishLists = await unitOfWork.WishListRepo.GetAll(pgNumber,pgSize);
            var resultedMapping = mapper.Map<List<ReadWishListDTO>>(wishLists);
            return resultedMapping;
        }

        public async Task<ReadWishListDTO> GetByUserId(string id)
        {
            var wishList = await unitOfWork.WishListRepo.GetByUserId(id);
            var resultedMapping = mapper.Map<ReadWishListDTO>(wishList);
            return resultedMapping;
        }

        public async Task DeleteWishList(int id)
        {
            await unitOfWork.WishListRepo.ClearWishList(id);
        }

        public async Task<ReadWishListDTO> AddWishList(string Userid, AddWishListDetailDTO wishListDetail)
        {
            // Create or get the user's wishlist id by his user id 
            var wishlistId = await unitOfWork.WishListRepo.AddWishList(Userid);

            var exsistWishListDetail = await unitOfWork.WishListDetailsRepo.GetById(wishlistId.Value, wishListDetail.ProductId);
            if (exsistWishListDetail == null)
            {
                WishListDetail newWishListDetail = new WishListDetail()
                {
                    WishListId = wishlistId.Value,
                    ProductId = wishListDetail.ProductId,
                };
                unitOfWork.WishListDetailsRepo.Add(newWishListDetail);
            }
            else if (exsistWishListDetail.IsDeleted)
            {
                exsistWishListDetail.IsDeleted = false;
            }
           


            await unitOfWork.CompleteAsync();

            // Fetch the updated wishList after adding items
            var updatedWishList = await unitOfWork.WishListRepo.GetByUserId(Userid);
            return mapper.Map<ReadWishListDTO>(updatedWishList);
        }
    }
}
