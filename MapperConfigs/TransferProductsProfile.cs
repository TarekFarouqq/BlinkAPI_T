using AutoMapper;
using Blink_API.DTOs.TransferProductsDTOs;
using Blink_API.Models;

namespace Blink_API.MapperConfigs
{
    public class TransferProductsProfile:Profile
    {
        public TransferProductsProfile()
        {
            CreateMap<InventoryTransactionHeader, InsertInputTransferProductDTO>().ReverseMap();
            // ------------------------------------------------------------------------
            CreateMap<TransactionProduct, InsertInputTrasactionProductDTO>().ReverseMap();
            // ------------------------------------------------------------------------
            CreateMap<InventoryTransactionHeader, ReadInventoryTransactions>()
                .ForMember(dest => dest.DistInventoryName, option => option.MapFrom(src => src.TransactionDetail.DistInventory.InventoryName))
                .ForMember(dest => dest.SrcInventoryName, option => option.MapFrom(src => src.TransactionDetail.SrcInventory.InventoryName))
                .ForMember(dest=>dest.SrcInventoryId,option=>option.MapFrom(src=>src.TransactionDetail.SrcInventoryId))
                .ForMember(dest=>dest.DistInventoryId,option=>option.MapFrom(src=>src.TransactionDetail.DistInventoryId))
                .ReverseMap();
            // ------------------------------------------------------------------------
            CreateMap<TransactionProduct,ReadTrasactionProductsDTO>()
                .ForMember(dest=>dest.ProductName,option=>option.MapFrom(src=>src.Product.ProductName))
                .ReverseMap();
            // ------------------------------------------------------------------------
            CreateMap<InventoryTransactionHeader, InsertTransactionHistoryDTO>().ReverseMap();
            // ------------------------------------------------------------------------
            CreateMap<TransactionDetail, InsertTransactionDetailsDTO>().ReverseMap();
            // ------------------------------------------------------------------------
        }
    }
}
