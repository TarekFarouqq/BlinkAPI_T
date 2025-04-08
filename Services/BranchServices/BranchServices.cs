using AutoMapper;
using Blink_API.DTOs.BranchDto;
using Blink_API.Errors;
using Blink_API.Models;
using Blink_API.Repositories.BranchRepos;
using Microsoft.AspNetCore.Http.HttpResults;

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
            var branches =await _unitOfWork.BranchRepos.GetAllAsync();
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
        public async Task<ApiResponse> UpdateBranch(int Id,AddBranchDTO updatedBranch)
        {
            if (updatedBranch == null)
            {
                return new ApiResponse(400, "Invalid branch data.");
            }
            var branch = await _unitOfWork.BranchRepos.GetBranchByIdAsync(Id);

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

        public async Task<ApiResponse> DeleteBranch(int branchId)
        {
            var branch = await _unitOfWork.BranchRepos.GetById(branchId);
            if (branch == null)
            {
                return new ApiResponse(404, "Branch not found.");
            }
          
            if (branch.IsDeleted==true)
            {
                return new ApiResponse(400, "Branch has already been deleted.");
            }
               _unitOfWork.BranchRepos.Delete(branch);
            await _unitOfWork.BranchRepos.SaveChanges();

            return new ApiResponse(200, "Branch deleted successfully.");
            

        }

    }
}
