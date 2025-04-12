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

        public async Task<List<stock_factDto>> GetAllStockFacts()
        {
            var stockFacts = await _unitOfWork.BiDataRepos.GetAll();
            var stockFactsDto = _mapper.Map<List<stock_factDto>>(stockFacts);
            return stockFactsDto;

        }

        // get all from reviwe  :
        public async Task<List<Review_DimensionDto>> GetAllReviewDimensions()
        {
            var reviewDimensions = await _unitOfWork.Review_DimensionRepos.GetAll();
            var reviewDimensionsDto = _mapper.Map<List<Review_DimensionDto>>(reviewDimensions);
            return reviewDimensionsDto;
        }

        // get all from payment :
        public async Task<List<Payment_DimensionDto>> GetAllPayments()
        {
            var payments = await _unitOfWork.Payment_DimensionRepos.GetAll();
            var paymentsDto = _mapper.Map<List<Payment_DimensionDto>>(payments);
            return paymentsDto;
        }

        // get all from user role :
        public async Task<List<UserRoles_DimensionDto>> GetAllUserRoles()
        {
            var userRoles = await _unitOfWork.UserRoleRepo.GetAll();
            var userRolesDto = _mapper.Map<List<UserRoles_DimensionDto>>(userRoles);
            return userRolesDto;
        }
        // get all from roles :
        public async Task<List<Role_DiminsionDto>> GetAllRoles()
        {
            var roles = await _unitOfWork.IdentityRoleRepo.GetAll();
            var rolesDto = _mapper.Map<List<Role_DiminsionDto>>(roles);
            return rolesDto;
        }

        // get all users :
        public async Task<List<User_DimensionDto>> GetAllUsers()
        {
            var users = await _unitOfWork.UserDiminsionRepos.GetAll();
            var usersDto = _mapper.Map<List<User_DimensionDto>>(users);
            return usersDto;
        }

        // get all product discount 
        public async Task<List<Product_DiscountDto>> GetAllProductDiscounts()
        {
            var productDiscounts = await _unitOfWork.ProductDiscountRepo.GetAll();
            var productDiscountsDto = _mapper.Map<List<Product_DiscountDto>>(productDiscounts);
            return productDiscountsDto;
        }
        // get all inventory transaction:
        public async Task<List<Inventory_Transaction_Dto>> GetAllInventoryTransactions()
        {
            var inventoryTransactions = await _unitOfWork.InventoryTransactionRepo.GetAll();
            var inventoryTransactionsDto = _mapper.Map<List<Inventory_Transaction_Dto>>(inventoryTransactions);
            return inventoryTransactionsDto;
        }
        // get all cart :
        public async Task<List<cart_DiminsionDto>> GetAllCarts()
        {
            // Correcting the property name to match the UnitOfWork type signature
            var carts = await _unitOfWork.CartDiminsionRepos.GetAll();
            var cartsDto = _mapper.Map<List<cart_DiminsionDto>>(carts);
            return cartsDto;
        }

        // get all order facts :
        public async Task<List<order_FactDto>> GetAllOrderFacts()
        {
            var orderFacts = await _unitOfWork.OrderFactRepos.GetAll();
            var orderFactsDto = _mapper.Map<List<order_FactDto>>(orderFacts);
            return orderFactsDto;
        }

        // get all discounts :
        public async Task<List<Discount_DimensionDto>> GetAllDiscounts()
        {
            var discounts = await _unitOfWork.DiscountDiminsionRepo.GetAll();
            var discountsDto = _mapper.Map<List<Discount_DimensionDto>>(discounts);
            return discountsDto;
        }

        // get all inventory branch :
        public async Task<List<Branch_inventoryDto>> GetAllInventoryBranches()
        {
            var inventoryBranches = await _unitOfWork.BranchInventoryRepos.GetAll();
            var inventoryBranchesDto = _mapper.Map<List<Branch_inventoryDto>>(inventoryBranches);
            return inventoryBranchesDto;
        }

        // get all inventory transaction fact :
        public async Task<List<InventoryTransaction_FactDto>> GetAllInventoryTransactionFacts()
        {
            var inventoryTransactionFacts = await _unitOfWork.InventoryTransactionFactRepos.GetAll();
            var inventoryTransactionFactsDto = _mapper.Map<List<InventoryTransaction_FactDto>>(inventoryTransactionFacts);
            return inventoryTransactionFactsDto;
        }

        //get all product diminsion :
        public async Task<List<Product_DiminsionDto>> GetAllProductDimensions()
        {
            var productDimensions = await _unitOfWork.ProductDiminsionRepos.GetAll();
            var productDimensionsDto = _mapper.Map<List<Product_DiminsionDto>>(productDimensions);
            return productDimensionsDto;
        }

    }
}
