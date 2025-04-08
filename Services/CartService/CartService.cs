using AutoMapper;
using Blink_API.DTOs.CartDTOs;
using Blink_API.DTOs.Category;
using Blink_API.Models;

namespace Blink_API.Services.CartService
{
    public class CartService
    {
        private readonly UnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public CartService(UnitOfWork _unitOfWork, IMapper _mapper)
        {
            unitOfWork = _unitOfWork;
            mapper = _mapper;
        }

        public async Task<List<ReadCartDTO>> GetAllCarts()
        {
            var carts = await unitOfWork.CartRepo.GetAll(); 
            var resultedMapping = mapper.Map<List<ReadCartDTO>>(carts);
            return resultedMapping;
        }

        public async Task<ReadCartDTO> GetByUserId(string id)
        {
            var cart = await unitOfWork.CartRepo.GetByUserId(id);
            var resultedMapping = mapper.Map<ReadCartDTO>(cart);
            return resultedMapping;
        }

        public async Task<ReadCartDTO> AddCart(string Userid, AddCartDetailsDTO cartDetail)
        {
            // Create or get the user's cart id by his user id 
            var cartId = await unitOfWork.CartRepo.AddCart(Userid);


        
                
                var exsistCartDetail = await unitOfWork.CartDetailsRepo.GetById(cartId.Value, cartDetail.ProductId);
                if (exsistCartDetail == null)
                {
                    CartDetail newCartDetail = new CartDetail()
                    {
                        CartId = cartId.Value,
                        ProductId = cartDetail.ProductId,
                        Quantity = cartDetail.Quantity,
                    };
                    unitOfWork.CartDetailsRepo.Add(newCartDetail);
                }
                else 
                {
                    exsistCartDetail.IsDeleted = false;
                    exsistCartDetail.Quantity += cartDetail.Quantity;
                    if (exsistCartDetail.Quantity == 0)
                    {
                        exsistCartDetail.IsDeleted = true;
                        unitOfWork.CartDetailsRepo.Update(exsistCartDetail);
                    }
                    else if (exsistCartDetail.Quantity > 0)
                    {
                        unitOfWork.CartDetailsRepo.Update(exsistCartDetail);
                    }
                

                }
            

            await unitOfWork.CartDetailsRepo.SaveChanges();

            // Fetch the updated cart after adding items
            var updatedCart = await unitOfWork.CartRepo.GetByUserId(Userid);
            return mapper.Map<ReadCartDTO>(updatedCart);
        }


    }
}
