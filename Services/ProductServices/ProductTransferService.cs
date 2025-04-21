using AutoMapper;
using Blink_API.DTOs.TransferProductsDTOs;
using Blink_API.Models;

namespace Blink_API.Services.ProductServices
{
    public class ProductTransferService
    {
        // 1= Product In
        // 2= Product Out
        // 3= Transfer Product
        // 4= Depricated Product
        
        private readonly UnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public ProductTransferService(UnitOfWork _unitOfWork, IMapper mapper)
        {
            unitOfWork = _unitOfWork;
            this.mapper = mapper;
        }
        public async Task<List<ReadInventoryTransactions>> GetAllTransactionHeader()
        {
            var result =  await unitOfWork.ProductTransferRepo.GetAllTransactionHeader();
            var mappedResult = mapper.Map<List<ReadInventoryTransactions>>(result);
            return mappedResult;
        }
        public async Task<ReadInventoryTransactions> GetTransactionHeaderById(int id)
        {
            var result = await unitOfWork.ProductTransferRepo.GetTransactionHeaderById(id);
            var mappedResult = mapper.Map<ReadInventoryTransactions>(result);
            return mappedResult;
        }
        public async Task AddInputInventory(InsertInputTransferProductDTO insertInputTransferProductDTO)
        {
            var mappedTransactionHeader=mapper.Map<InventoryTransactionHeader>(insertInputTransferProductDTO);
            var mappedTransactionProduct=mapper.Map<ICollection<TransactionProduct>>(insertInputTransferProductDTO.TransactionProducts);
            foreach(var transactionProduct in mappedTransactionProduct)
            {
                transactionProduct.InventoryTransactionHeader = mappedTransactionHeader;
            }
            mappedTransactionHeader.TransactionProducts = mappedTransactionProduct;
            await unitOfWork.ProductTransferRepo.AddInputInventory(mappedTransactionHeader);
        }
        public async Task CreateTransaction(InsertTransactionHistoryDTO insertTransactionHistoryDTO)
        {
            var mappedTransactionHeader=mapper.Map<InventoryTransactionHeader>(insertTransactionHistoryDTO);
            var mappedTransactionDetail = mapper.Map<TransactionDetail>(insertTransactionHistoryDTO.TransactionDetail);
            var mappedTransactionProducts = mapper.Map<ICollection<TransactionProduct>>(insertTransactionHistoryDTO.TransactionProducts);
            mappedTransactionHeader.TransactionDetail= mappedTransactionDetail;
            mappedTransactionHeader.TransactionProducts=mappedTransactionProducts;
            int srcId = mappedTransactionDetail.SrcInventoryId;
            int destId = mappedTransactionDetail.DistInventoryId;
            foreach(var product in mappedTransactionProducts)
            {
                int prdId = product.ProductId;
                int stock = product.TransactionQuantity;
                var price =  await unitOfWork.ProductTransferRepo.DecreaseStock(prdId, srcId, stock);
                await unitOfWork.ProductTransferRepo.IncreaseStock(prdId, destId, stock, price);
            }
            await unitOfWork.ProductTransferRepo.AddInputInventory(mappedTransactionHeader);
        }
        public async Task UpdateTransaction(int id, InsertTransactionHistoryDTO insertTransactionHistoryDTO)
        {
            var mappedTransactionHeader = mapper.Map<InventoryTransactionHeader>(insertTransactionHistoryDTO);
            var mappedTransactionDetail = mapper.Map<TransactionDetail>(insertTransactionHistoryDTO.TransactionDetail);
            var mappedTransactionProducts = mapper.Map<ICollection<TransactionProduct>>(insertTransactionHistoryDTO.TransactionProducts);
            mappedTransactionHeader.TransactionDetail = mappedTransactionDetail;
            mappedTransactionHeader.TransactionProducts = mappedTransactionProducts;
            mappedTransactionHeader.InventoryTransactionHeaderId = id;
            mappedTransactionDetail.InventoryTransactionHeaderId = id;
            foreach (var product in mappedTransactionProducts)
            {
                product.InventoryTransactionId = id;
            }
            int srcId = mappedTransactionDetail.SrcInventoryId;
            int destId = mappedTransactionDetail.DistInventoryId;
            // Return All Products First 
            var oldTransactionProducts = await unitOfWork.ProductTransferRepo.GetOldTransactionProductsByTransactionId(id);
            foreach (var product in oldTransactionProducts)
            {
                int prdId = product.ProductId;
                int stock = product.TransactionQuantity;
                var price = await unitOfWork.ProductTransferRepo.DecreaseStock(prdId, destId, stock);
                await unitOfWork.ProductTransferRepo.IncreaseStock(prdId, srcId, stock, price);
            }
            await unitOfWork.ProductTransferRepo.DeleteOldTransactionProducts(id);
            // Complete Update Transaction
            foreach (var product in mappedTransactionProducts)
            {
                int prdId = product.ProductId;
                int stock = product.TransactionQuantity;
                var price = await unitOfWork.ProductTransferRepo.DecreaseStock(prdId, srcId, stock);
                await unitOfWork.ProductTransferRepo.IncreaseStock(prdId, destId, stock, price);
            }
            await unitOfWork.ProductTransferRepo.UpdateTransaction(mappedTransactionHeader);
        }
        public async Task<int> GetTotalPages(int pgSize)
        {
            return await unitOfWork.ProductTransferRepo.GetTotalPages(pgSize);
        }  
        public async Task<List<ReadInventoryTransactions>> GetDataWithPagination(int pgNumber,int pgSize)
        {
            var result = await unitOfWork.ProductTransferRepo.GetAllTransactionHeader(pgNumber,pgSize);
            var mappedResult = mapper.Map<List<ReadInventoryTransactions>>(result);
            return mappedResult;
        }
    }
}
