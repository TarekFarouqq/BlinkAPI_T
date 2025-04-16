using AutoMapper;
using Blink_API.DTOs.CartDTOs;
using Blink_API.DTOs.PaymentCart;
using Blink_API.Models;
using Blink_API.Services.PaymentServices;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Text.Json;

namespace Blink_API.Repositories.CartRepos
{
    public class CartRepo : GenericRepo<Cart, int>
    {
        private readonly BlinkDbContext db;
      

        public CartRepo(BlinkDbContext _db ) : base(_db)
        {
            db = _db;
        }


        public override async Task<List<Cart>> GetAll()
        {
            return await db.Carts
                        .AsNoTracking()
                .Include(c => c.CartDetails.Where(cd => !cd.IsDeleted))
                    .ThenInclude(c => c.Product)
                        .ThenInclude(p => p.StockProductInventories)
                .Include(c => c.CartDetails.Where(cd => !cd.IsDeleted))
                    .ThenInclude(c => c.Product)
                        .ThenInclude(p => p.ProductImages)
                .Where(p => !p.IsDeleted)
                .ToListAsync();

        }

        public async Task<Cart?> GetByUserId(string id)
        {
            return await db.Carts
                 .AsNoTracking()
                .Include(c => c.CartDetails.Where(cd => !cd.IsDeleted))
                    .ThenInclude(c => c.Product)
                        .ThenInclude(p => p.StockProductInventories)
                .Include(c => c.CartDetails.Where(cd => !cd.IsDeleted))
                    .ThenInclude(c => c.Product)
                        .ThenInclude(p => p.ProductImages)
                .Where(p => !p.IsDeleted)
                .FirstOrDefaultAsync(p => p.UserId == id);
        }

        public async Task<int?> AddCart(string id)
        {

            var cart = await GetByUserId(id);

            if (cart == null || cart.IsDeleted == true )
            {
                cart = new Cart() { UserId = id };
                await db.Carts.AddAsync(cart);
                await db.SaveChangesAsync();
            }

            return cart.CartId;
        }

        #region Handle cart
        ///public async Task<bool> DeleteCartAsync(string basketId)
        ///{
        ///    return await _database.KeyDeleteAsync(basketId);
        ///}
        ///public async Task<CartPaymentDTO?> GetCartAsync(string basketId)
        ///{
        ///    var basket = await _database.StringGetAsync(basketId);
        ///    return basket.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CartPaymentDTO>(basket);
        ///}
        ///public async Task<CartPaymentDTO?> UpdateCartAsync(CartPaymentDTO cart)
        ///{
        ///    var createdOrUpdated = await _database.StringSetAsync(cart.CartId, JsonSerializer.Serialize(cart), TimeSpan.FromDays(30));
        ///    if (createdOrUpdated is false) return null;
        ///    return await GetCartAsync(cart.CartId);
        ///}

        public async Task UpdateCart(Cart cart)
        {
            try
            {
                db.Carts.Update(cart);
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                
                throw new Exception("Error updating cart: " + ex.Message);
            }
        }


        #endregion

    }
}
