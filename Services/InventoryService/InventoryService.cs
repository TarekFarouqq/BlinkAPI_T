using AutoMapper;
using Blink_API.DTOs.BranchDto;
using Blink_API.DTOs.InventoryDTOS;
using Blink_API.Errors;
using Blink_API.Models;

namespace Blink_API.Services.InventoryService
{
    public class InventoryService
    {
        private readonly UnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public InventoryService(UnitOfWork _unitOfWork, IMapper _mapper)
        {
            unitOfWork = _unitOfWork;
            mapper = _mapper;
        }

        public async Task<List<ReadInventoryDTO>> GetAllInventories()
        {
            var inventories = await unitOfWork.InventoryRepo.GetAll();
            var inventoriesDto = mapper.Map<List<ReadInventoryDTO>>(inventories);
            return inventoriesDto;

        }

        public async Task<ReadInventoryDTO?> GetInventoryById(int id)
        {
            var inventory = await unitOfWork.InventoryRepo.GetById(id);
            if (inventory == null) return null;
            var inventoryDto = mapper.Map<ReadInventoryDTO>(inventory);
            return inventoryDto;

        }

        public async Task<ApiResponse> AddInventory(AddInventoryDTO newInventory)
        {
            if (newInventory == null)
            {
                return new ApiResponse(400, "Invalid Inventory data.");
            }
            var inventory = mapper.Map<Inventory>(newInventory);

            unitOfWork.InventoryRepo.Add(inventory);
            await unitOfWork.InventoryRepo.SaveChanges();
            return new ApiResponse(201, "Inventory added successfully.");


        }
        public async Task<ApiResponse> UpdateInventory(int Id, AddInventoryDTO updatedInventory)
        {
            if (updatedInventory == null)
            {
                return new ApiResponse(400, "Invalid Inventory data.");
            }
            var inventory = await unitOfWork.InventoryRepo.GetById(Id);

            if (inventory == null)
            {
                return new ApiResponse(404, "Inventory not found.");
            }
            mapper.Map(updatedInventory, inventory);
            unitOfWork.InventoryRepo.Update(inventory);
            await unitOfWork.InventoryRepo.SaveChanges();
            return new ApiResponse(200, "Inventory updated successfully.");
        }

        public async Task<ApiResponse> DeleteInventory(int id)
        {
            var inventory = await unitOfWork.InventoryRepo.GetById(id);
            if (inventory == null || inventory.IsDeleted)
            {
                return new ApiResponse(404, "Inventory not found.");
            }

            await unitOfWork.InventoryRepo.Delete(id);
            await unitOfWork.InventoryRepo.SaveChanges();

            return new ApiResponse(200, "Inventory deleted successfully.");
        }

    }
}
