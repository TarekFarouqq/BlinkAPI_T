using AutoMapper;

namespace Blink_API.Services.ProductServices
{
    public class ProductTransferService
    {
        private readonly UnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public ProductTransferService(UnitOfWork _unitOfWork, IMapper mapper)
        {
            unitOfWork = _unitOfWork;
            this.mapper = mapper;
        }

    }
}
