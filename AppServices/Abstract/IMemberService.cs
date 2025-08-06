using AppRepository.Entities;
using AppRepository.Entities.Dtos;

namespace AppServices.Abstract
{
    public interface IMemberService
    {
        Task UpdatesAsync(UpdateMemberDto updateMemberDto, int appUserId);
        Task<MemberDto?> GetByIdAsync(int id, int appUserId);
        Task CreateAsync(CreateMemberDto createMemberDto, int appUserId);
        Task<List<MemberDto>> GetAllAsync(int appUserId);
        Task DeleteAsync(int id, int appUserId);
    }
}
