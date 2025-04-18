using AutoMapper;
using Blink_API.DTOs.BiDataDtos;
using Blink_API.Models;

namespace Blink_API.Services.BiDataService
{
    public class BiStockService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BiStockService(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        #region old
        //public async Task<List<stock_factDto>> GetAllStockFacts()
        //{
        //    var stockFacts = await _unitOfWork.BiDataRepos.GetAll();
        //    var stockFactsDto = _mapper.Map<List<stock_factDto>>(stockFacts);
        //    return stockFactsDto;

        //}
        #endregion
      

        public async IAsyncEnumerable<List<stock_factDto>> GetAllStockFactsInChunks()
        {
            var chunkSize = 5000;
            var chunk = new List<stock_factDto>();

            await foreach (var stockFact in _unitOfWork.BiDataRepos.GetAllAsStream())
            {
                var stockFactDto = _mapper.Map<stock_factDto>(stockFact);
                chunk.Add(stockFactDto);

                if (chunk.Count == chunkSize)
                {
                    yield return new List<stock_factDto>(chunk);
                    chunk.Clear();
                }
            }

            if (chunk.Count > 0)
            {
                yield return chunk;
            }
        }


        // get all from reviwe  :
        public async IAsyncEnumerable<List<Review_DimensionDto>> GetAllReviewDimensionsInChunks()
        {
            var chunkSize = 1000;
            var chunk = new List<Review_DimensionDto>();

            await foreach (var review in _unitOfWork.Review_DimensionRepos.GetAllAsStream())
            {
                var reviewDto = _mapper.Map<Review_DimensionDto>(review);
                chunk.Add(reviewDto);

                if (chunk.Count == chunkSize)
                {
                    yield return new List<Review_DimensionDto>(chunk);
                    chunk.Clear();
                }
            }

            if (chunk.Count > 0)
            {
                yield return chunk;
            }
        }

        #region old 
        //public async Task<List<Review_DimensionDto>> GetAllReviewDimensions()
        //{
        //    var reviewDimensions = await _unitOfWork.Review_DimensionRepos.GetAll();
        //    var reviewDimensionsDto = _mapper.Map<List<Review_DimensionDto>>(reviewDimensions);
        //    return reviewDimensionsDto;
        //}
        #endregion

        // get all from payment :

        //public async IAsyncEnumerable<List<Payment_DimensionDto>> GetAllPaymentsInChunks()
        //{
        //    var chunkSize = 1000;
        //    var chunk = new List<Payment_DimensionDto>();

        //    await foreach (var payment in _unitOfWork.Payment_DimensionRepos.GetAllAsStream())
        //    {
        //        var paymentDto = _mapper.Map<Payment_DimensionDto>(payment);
        //        chunk.Add(paymentDto);

        //        if (chunk.Count == chunkSize)
        //        {
        //            yield return new List<Payment_DimensionDto>(chunk);
        //            chunk.Clear();
        //        }
        //    }

        //    if (chunk.Count > 0)
        //    {
        //        yield return chunk;
        //    }
        //}

        #region old 
        public async Task<List<Payment_DimensionDto>> GetAllPayments()
        {
            var payments = await _unitOfWork.Payment_DimensionRepos.GetAll();
            var paymentsDto = _mapper.Map<List<Payment_DimensionDto>>(payments);
            return paymentsDto;
        }
        #endregion
        // get all from user role :

        public async IAsyncEnumerable<List<UserRoles_DimensionDto>> GetAllUserRolesInChunks()
        {
            var chunkSize = 1000;
            var chunk = new List<UserRoles_DimensionDto>();

            await foreach (var userRole in _unitOfWork.UserRoleRepo.GetAllAsStream())
            {
                var userRoleDto = _mapper.Map<UserRoles_DimensionDto>(userRole);
                chunk.Add(userRoleDto);

                if (chunk.Count == chunkSize)
                {
                    yield return new List<UserRoles_DimensionDto>(chunk);
                    chunk.Clear();
                }
            }

            if (chunk.Count > 0)
            {
                yield return chunk;
            }
        }

        #region old
        //public async Task<List<UserRoles_DimensionDto>> GetAllUserRoles()
        //{
        //    var userRoles = await _unitOfWork.UserRoleRepo.GetAll();
        //    var userRolesDto = _mapper.Map<List<UserRoles_DimensionDto>>(userRoles);
        //    return userRolesDto;
        //}
        #endregion
        // get all from roles :
        public async IAsyncEnumerable<List<Role_DiminsionDto>> GetAllRolesInChunks()
        {
            var chunkSize = 1000;
            var chunk = new List<Role_DiminsionDto>();

            await foreach (var role in _unitOfWork.IdentityRoleRepo.GetAllAsStream())
            {
                var roleDto = _mapper.Map<Role_DiminsionDto>(role);
                chunk.Add(roleDto);

                if (chunk.Count == chunkSize)
                {
                    yield return new List<Role_DiminsionDto>(chunk);
                    chunk.Clear();
                }
            }

            if (chunk.Count > 0)
            {
                yield return chunk;
            }
        }


        #region old 
        //public async Task<List<Role_DiminsionDto>> GetAllRoles()
        //{
        //    var roles = await _unitOfWork.IdentityRoleRepo.GetAll();
        //    var rolesDto = _mapper.Map<List<Role_DiminsionDto>>(roles);
        //    return rolesDto;
        //}
        #endregion
        // get all users :
        //public async Task<List<User_DimensionDto>> GetAllUsers()
        //{
        //    var users = await _unitOfWork.UserDiminsionRepos.GetAll();
        //    var usersDto = _mapper.Map<List<User_DimensionDto>>(users);
        //    return usersDto;
        //}

        // chunk get all users in db :
        public async IAsyncEnumerable<List<User_DimensionDto>> GetAllUsersInChunks()
        {
            var chunkSize = 1000;  
            var chunk = new List<User_DimensionDto>();

            await foreach (var user in _unitOfWork.UserDiminsionRepos.GetAllAsStream())
            {
                var userDto = _mapper.Map<User_DimensionDto>(user);
                chunk.Add(userDto);

                if (chunk.Count == chunkSize)
                {
                    yield return new List<User_DimensionDto>(chunk);
                    chunk.Clear();
                }
            }

            if (chunk.Count > 0)
            {
                yield return chunk;
            }
        }

        // get all product discount 
        public async IAsyncEnumerable<List<Product_DiscountDto>> GetAllProductDiscountsInChunks()
        {
            var chunkSize = 1000;
            var chunk = new List<Product_DiscountDto>();

            await foreach (var pd in _unitOfWork.ProductDiscountRepo.GetAllAsStream())
            {
                var pdDto = _mapper.Map<Product_DiscountDto>(pd);
                chunk.Add(pdDto);

                if (chunk.Count == chunkSize)
                {
                    yield return new List<Product_DiscountDto>(chunk);
                    chunk.Clear();
                }
            }

            if (chunk.Count > 0)
            {
                yield return chunk;
            }
        }

        #region old 
        //public async Task<List<Product_DiscountDto>> GetAllProductDiscounts()
        //{
        //    var productDiscounts = await _unitOfWork.ProductDiscountRepo.GetAll();
        //    var productDiscountsDto = _mapper.Map<List<Product_DiscountDto>>(productDiscounts);
        //    return productDiscountsDto;
        //}
        #endregion
        // get all inventory transaction:
        public async IAsyncEnumerable<List<Inventory_Transaction_Dto>> GetAllInventoryTransactionsInChunks()
        {
            var chunkSize = 1000;
            var chunk = new List<Inventory_Transaction_Dto>();

            await foreach (var inv in _unitOfWork.InventoryTransactionRepo.GetAllAsStream())
            {
                var invDto = _mapper.Map<Inventory_Transaction_Dto>(inv);
                chunk.Add(invDto);

                if (chunk.Count == chunkSize)
                {
                    yield return new List<Inventory_Transaction_Dto>(chunk);
                    chunk.Clear();
                }
            }

            if (chunk.Count > 0)
            {
                yield return chunk;
            }
        }

        #region old
        //public async Task<List<Inventory_Transaction_Dto>> GetAllInventoryTransactions()
        //{
        //    var inventoryTransactions = await _unitOfWork.InventoryTransactionRepo.GetAll();
        //    var inventoryTransactionsDto = _mapper.Map<List<Inventory_Transaction_Dto>>(inventoryTransactions);
        //    return inventoryTransactionsDto;
        //}
        #endregion
        // get all cart :

        public async IAsyncEnumerable<List<cart_DiminsionDto>> GetAllCartsInChunks()
        {
            var chunkSize = 1000;
            var chunk = new List<cart_DiminsionDto>();

            await foreach (var cart in _unitOfWork.CartDiminsionRepos.GetAllAsStream())
            {
                var cartDto = _mapper.Map<cart_DiminsionDto>(cart);
                chunk.Add(cartDto);

                if (chunk.Count == chunkSize)
                {
                    yield return new List<cart_DiminsionDto>(chunk);
                    chunk.Clear();
                }
            }

            if (chunk.Count > 0)
            {
                yield return chunk;
            }
        }

        #region old 
        //public async Task<List<cart_DiminsionDto>> GetAllCarts()
        //{
        //    // Correcting the property name to match the UnitOfWork type signature
        //    var carts = await _unitOfWork.CartDiminsionRepos.GetAll();
        //    var cartsDto = _mapper.Map<List<cart_DiminsionDto>>(carts);
        //    return cartsDto;
        //}
        #endregion
        // get all order facts :
        public async IAsyncEnumerable<List<order_FactDto>> GetAllOrderFactsInChunks()
        {
            var chunkSize = 1000;
            var chunk = new List<order_FactDto>();

            await foreach (var orderFact in _unitOfWork.OrderFactRepos.GetAllAsStream())
            {
                var orderFactDto = _mapper.Map<order_FactDto>(orderFact);
                chunk.Add(orderFactDto);

                if (chunk.Count == chunkSize)
                {
                    yield return new List<order_FactDto>(chunk);
                    chunk.Clear();
                }
            }

            if (chunk.Count > 0)
            {
                yield return chunk;
            }
        }

        #region old
        //public async Task<List<order_FactDto>> GetAllOrderFacts()
        //{
        //    var orderFacts = await _unitOfWork.OrderFactRepos.GetAll();
        //    var orderFactsDto = _mapper.Map<List<order_FactDto>>(orderFacts);
        //    return orderFactsDto;
        //}
        #endregion

        // get all discounts :
        public async IAsyncEnumerable<List<Discount_DimensionDto>> GetAllDiscountsInChunks()
        {
            var chunkSize = 1000;   
            var chunk = new List<Discount_DimensionDto>();

            await foreach (var discount in _unitOfWork.DiscountDiminsionRepo.GetAllAsStream())
            {
                var discountDto = _mapper.Map<Discount_DimensionDto>(discount);
                chunk.Add(discountDto);

                if (chunk.Count == chunkSize)
                {
                    yield return new List<Discount_DimensionDto>(chunk);
                    chunk.Clear();
                }
            }

            if (chunk.Count > 0)
            {
                yield return chunk;
            }
        }


        #region old
        //public async Task<List<Discount_DimensionDto>> GetAllDiscounts()
        //{
        //    var discounts = await _unitOfWork.DiscountDiminsionRepo.GetAll();
        //    var discountsDto = _mapper.Map<List<Discount_DimensionDto>>(discounts);
        //    return discountsDto;
        //}
        #endregion
        // get all inventory branch :
        public async IAsyncEnumerable<List<Branch_inventoryDto>> GetAllInventoryBranchesInChunks()
        {
            var chunkSize = 1000;   
            var chunk = new List<Branch_inventoryDto>();

            await foreach (var branch in _unitOfWork.BranchInventoryRepos.GetAllAsStream())
            {
                var branchDto = _mapper.Map<Branch_inventoryDto>(branch);
                chunk.Add(branchDto);

                if (chunk.Count == chunkSize)
                {
                    yield return new List<Branch_inventoryDto>(chunk);
                    chunk.Clear();
                }
            }

            if (chunk.Count > 0)
            {
                yield return chunk;
            }
        }

        #region old
        //public async Task<List<Branch_inventoryDto>> GetAllInventoryBranches()
        //{
        //    var inventoryBranches = await _unitOfWork.BranchInventoryRepos.GetAll();
        //    var inventoryBranchesDto = _mapper.Map<List<Branch_inventoryDto>>(inventoryBranches);
        //    return inventoryBranchesDto;
        //}
        #endregion
        // get all inventory transaction fact :
        public async IAsyncEnumerable<List<InventoryTransaction_FactDto>> GetAllInventoryTransactionFactsInChunks()
        {
            var chunkSize = 1000;   
            var chunk = new List<InventoryTransaction_FactDto>();

            await foreach (var transactionFact in _unitOfWork.InventoryTransactionFactRepos.GetAllAsStream())
            {
                var transactionFactDto = _mapper.Map<InventoryTransaction_FactDto>(transactionFact);
                chunk.Add(transactionFactDto);

                if (chunk.Count == chunkSize)
                {
                    yield return new List<InventoryTransaction_FactDto>(chunk);
                    chunk.Clear();
                }
            }

            if (chunk.Count > 0)
            {
                yield return chunk;
            }
        }

        #region old
        //public async Task<List<InventoryTransaction_FactDto>> GetAllInventoryTransactionFacts()
        //{
        //    var inventoryTransactionFacts = await _unitOfWork.InventoryTransactionFactRepos.GetAll();
        //    var inventoryTransactionFactsDto = _mapper.Map<List<InventoryTransaction_FactDto>>(inventoryTransactionFacts);
        //    return inventoryTransactionFactsDto;
        //}
        #endregion

        //get all product diminsion :
        public async IAsyncEnumerable<List<Product_DiminsionDto>> GetAllProductDimensionsInChunks()
        {
            var chunkSize = 25000;   
            var chunk = new List<Product_DiminsionDto>();

            await foreach (var product in _unitOfWork.ProductDiminsionRepos.GetAllProductsAsStream())
            {
                var productDto = _mapper.Map<Product_DiminsionDto>(product);
                chunk.Add(productDto);

                if (chunk.Count == chunkSize)
                {
                    yield return new List<Product_DiminsionDto>(chunk);
                    chunk.Clear();
                }
            }

            
            if (chunk.Count > 0)
            {
                yield return chunk;
            }
        }


        #region old
        //public async Task<List<Product_DiminsionDto>> GetAllProductDimensions()
        //{
        //    var productDimensions = await _unitOfWork.ProductDiminsionRepos.GetAll();
        //    var productDimensionsDto = _mapper.Map<List<Product_DiminsionDto>>(productDimensions);
        //    return productDimensionsDto;
        //}
        #endregion

    }
}
