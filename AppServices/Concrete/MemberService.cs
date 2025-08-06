using AppRepository.Entities;
using AppRepository.Entities.Dtos;
using AppRepository.Repositories;
using AppServices.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AppServices.Concrete
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository<Member> memberRepository;
        private readonly IEncrypthonService encrypthonService;
        private readonly UserManager<AppUser> userManager;
        public MemberService(IMemberRepository<Member> memberRepository, IEncrypthonService encrypthonService, UserManager<AppUser> userManager)
        {
            this.memberRepository = memberRepository;
            this.encrypthonService = encrypthonService;
            this.userManager = userManager;
        }
        public async Task CreateAsync(CreateMemberDto createMemberDto, int appUserId)
        {
            var member = new Member
            {
                WebName = createMemberDto.WebName,
                Username = createMemberDto.Username,
                Password = encrypthonService.AesEncrypthon(createMemberDto.Password),
                AppUserId = appUserId 
            };
            await memberRepository.AddAsync(member);
        }

        public async Task<List<MemberDto>> GetAllAsync(int appUserId)
        {
            var allUser = await memberRepository.GetAll()
                .Where(m => m.AppUserId == appUserId)
                .ToListAsync();
            var result = allUser.Select(u => new MemberDto
            {
                Id = u.Id,
                WebName = u.WebName,
                Username = u.Username,
                Password = encrypthonService.AesDecrypthon(u.Password)
            }).ToList();
            return result;
        }
    

        public async Task DeleteAsync(int id, int appUserId)
        {
            var member = await memberRepository.GetAll()
                         .FirstOrDefaultAsync(m => m.Id == id && m.AppUserId == appUserId);
            if (member == null) return;
            await memberRepository.DeleteAsync(id);
        }

        public async Task<MemberDto?> GetByIdAsync(int id, int appUserId)
        {
            var entity = await memberRepository.GetAll()
                .Where(m => m.Id == id && m.AppUserId == appUserId)
                .FirstOrDefaultAsync();
            if (entity == null)
            {
                return null;
            }
            var memberDto = new MemberDto
            {
                Id = entity.Id,
                WebName = entity.WebName,
                Username = entity.Username,
                Password = encrypthonService.AesDecrypthon(entity.Password),
            };
            return memberDto;
        }

        public async Task UpdatesAsync(UpdateMemberDto updateMemberDto, int appUserId)
        {
            var existingMember = await memberRepository.GetByIdAsync(updateMemberDto.Id);
            if (existingMember != null && existingMember.AppUserId == appUserId)
            {
                existingMember.WebName = updateMemberDto.WebName;
                existingMember.Username = updateMemberDto.Username;
                existingMember.Password = encrypthonService.AesEncrypthon(updateMemberDto.Password);
         
                await memberRepository.UpdateAsync(existingMember);
            }
        }
    }
}