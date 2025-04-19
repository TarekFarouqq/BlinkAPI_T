using Blink_API.DTOs.CartDTOs;
using Blink_API.DTOs.WishListDTOs;
using Blink_API.Services.CartService;
using Blink_API.Services.WishlistServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blink_API.Controllers.WishList
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishListController : ControllerBase
    {
        private readonly WishListService wishListService;

        public WishListController(WishListService _wishListService)
        {
            wishListService = _wishListService;
        }

        [HttpGet("{pgNumber}/{pgSize}")]
        public async Task<ActionResult> GetAllWishLists(int pgNumber,int pgSize)
        {
            var wishLists = await wishListService.GetAllWishLists(pgNumber,pgSize);
            if (wishLists == null)
                return NotFound();
            string baseUrl = $"{Request.Scheme}://{Request.Host}/";
            foreach (var wish in wishLists)
            {
                foreach (var detail in wish.WishListDetails)
                {
                    detail.ProductImageUrl = $"{baseUrl}{detail.ProductImageUrl.Replace("wwwroot/", "")}";
                }
            }
            return Ok(wishLists);
        }

        [HttpGet("GetByUserId/{id}")]
        public async Task<ActionResult> GetByUserId(string id)
        {
            var wishList = await wishListService.GetByUserId(id);
            if (wishList == null)
                return NotFound();
            string baseUrl = $"{Request.Scheme}://{Request.Host}/";
            foreach (var detail in wishList.WishListDetails)
            {
                detail.ProductImageUrl = $"{baseUrl}{detail.ProductImageUrl.Replace("wwwroot/", "")}";
            }
            return Ok(wishList);
        }

        [HttpPost("AddWishList/{userId}")]
        public async Task<ActionResult<ReadCartDTO>> AddWishList(string userId, [FromBody] AddWishListDetailDTO wishListDetail)
        {
            if (string.IsNullOrEmpty(userId) || wishListDetail == null)
            {
                return BadRequest("Invalid cart data.");
            }

            var addedWishList = await wishListService.AddWishList(userId, wishListDetail);
            return Ok(addedWishList);
        }

        [HttpDelete("ClearWishList/{wishListId}")]
        public async Task<ActionResult> DeleteCart(int wishListId)
        {
            if (wishListId <= 0) // Fix: Added a valid condition to check for invalid id
            {
                return BadRequest("Invalid wishlist Id.");
            }

            await wishListService.DeleteWishList(wishListId);
            return NoContent();
        }

    }
}
