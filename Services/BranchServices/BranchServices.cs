using AutoMapper;
using Blink_API.DTOs.BranchDto;
using Blink_API.Errors;
using Blink_API.Models;
using Blink_API.Repositories.BranchRepos;

namespace Blink_API.Services.BranchServices
{
    public class BranchServices
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BranchServices(UnitOfWork unitOfWork,IMapper mapper)
        {
           _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<List<ReadBranchDTO>> GetAllBranches()
        {
            var branches = await _unitOfWork.BranchRepos.GetAll();
            var branchesDto = _mapper.Map<List<ReadBranchDTO>>(branches);
            return branchesDto;

        }

        public async Task<ReadBranchDTO?> GetBranchById(int id)
        {
            var branch = await _unitOfWork.BranchRepos.GetById(id);
            if (branch == null) return null;
            var branchDto = _mapper.Map<ReadBranchDTO>(branch);
            return branchDto;

        }

        public async Task<ApiResponse> AddBranch(AddBranchDTO newbranch)
        {
            if (newbranch == null)
            {
                return new ApiResponse(400, "Invalid branch data.");
            }
            var branch = _mapper.Map<Branch>(newbranch);
            var existingBranch = await _unitOfWork.BranchRepos.GetFirstOrDefaultAsync(b => b.BranchName == branch.BranchName);
            if (existingBranch != null)
            {
                return new ApiResponse(400, "Branch with the same name already exists.");
            }
            _unitOfWork.BranchRepos.Add(branch);
            await _unitOfWork.BranchRepos.SaveChanges();
            return new ApiResponse(201, "Branch added successfully.");


        }
        public async Task<ApiResponse> UpdateBranch(int Id, AddBranchDTO updatedBranch)
        {
            if (updatedBranch == null)
            {
                return new ApiResponse(400, "Invalid branch data.");
            }
            var branch = await _unitOfWork.BranchRepos.GetById(Id);

            if (branch == null)
            {
                return new ApiResponse(404, "Branch not found.");
            }
            branch.BranchName = updatedBranch.BranchName;
            branch.BranchAddress = updatedBranch.BranchAddress;
            branch.Phone = updatedBranch.Phone;
            _unitOfWork.BranchRepos.Update(branch);
            await _unitOfWork.BranchRepos.SaveChanges();
            return new ApiResponse(200, "Branch updated successfully.");
        }

        public async Task<ApiResponse> DeleteBranch(int id)
        {
            var branch = await _unitOfWork.BranchRepos.GetById(id);
            if (branch == null || branch.IsDeleted)
            {
                return new ApiResponse(404, "Branch not found.");
            }
            var inventoriesCount = await _unitOfWork.InventoryRepo.CountInventoriesForBranch(id);

            if (inventoriesCount > 0)
            {
                return new ApiResponse(400, "Cannot delete branch with existing inventories,please delete the inventories first !");
            }
            await _unitOfWork.BranchRepos.Delete(id);
            await _unitOfWork.BranchRepos.SaveChanges();

            return new ApiResponse(200, "Branch deleted successfully.");
        }

    }
}
