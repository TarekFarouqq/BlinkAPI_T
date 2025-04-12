using Blink_API.Models;
using Blink_API.Repositories.Order;

namespace Blink_API.Services.OrderServicees
{
    public class orderServices
    {
        private readonly UnitOfWork _unitOfWork;

        public orderServices(UnitOfWork unitOfWork)
        {
           _unitOfWork = unitOfWork;
        }

        public async Task<List<OrderHeader>> GetAllOrders()
        {
            return await _unitOfWork.OrderRepo.GetOrdersWithDetails();
        }
        public async Task<OrderHeader?> GetOrderById(int id)
        {
            return await _unitOfWork.OrderRepo.GetOrderByIdWithDetails(id);
        }

        public async Task AddOrder(OrderHeader order)
        {
             _unitOfWork.OrderRepo.Add(order);
           await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteOrder(int id)
        {
            await _unitOfWork.OrderRepo.Delete(id);
            await _unitOfWork.CompleteAsync();
        }


        public async Task UpdateOrder(OrderHeader order)
        {
            _unitOfWork.OrderRepo.Update(order);
            await _unitOfWork.CompleteAsync();

        }
    }
}
