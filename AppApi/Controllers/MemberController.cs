using AppRepository.Entities.Dtos;
using AppServices.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IMemberService _memberService;

        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        [HttpGet("user/{appUserId}")]
        [Authorize(Roles = "manager,member")]
        public async Task<IActionResult> GetAllMembers(int appUserId)
        {
            try
            {
                // Debug için kullanıcı bilgisini loglayalım
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                var userName = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
                var roles = User.FindAll(System.Security.Claims.ClaimTypes.Role).Select(c => c.Value);
                
                Console.WriteLine($"API: User ID: {userId}, Username: {userName}, Roles: {string.Join(",", roles)}");
                Console.WriteLine($"API: Requested AppUserId: {appUserId}");

                var members = await _memberService.GetAllAsync(appUserId);
                return Ok(members);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"API Error: {ex.Message}");
                return StatusCode(500, $"Sunucu hatası: {ex.Message}");
            }
        }

        [HttpGet("{id}/user/{appUserId}")]
        [Authorize(Roles = "manager,member")]
        public async Task<IActionResult> GetMemberById(int id, int appUserId)
        {
            try
            {
                var member = await _memberService.GetByIdAsync(id, appUserId);
                if (member == null)
                    return NotFound("Kullanıcı bilgisi bulunamadı");
                return Ok(member);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Sunucu hatası: {ex.Message}");
            }
        }

        [HttpPost("user/{appUserId}")]
        [Authorize(Roles = "manager,member")]
        public async Task<IActionResult> CreateMember(int appUserId, [FromBody] CreateMemberDto createMemberDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                
                await _memberService.CreateAsync(createMemberDto, appUserId);
                return StatusCode(201, "Kullanıcı kaydı başarıyla gerçekleşti");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Sunucu hatası: {ex.Message}");
            }
        }

        [HttpPut("{id}/user/{appUserId}")]
        [Authorize(Roles = "manager,member")]
        public async Task<IActionResult> UpdateMember(int id, int appUserId, [FromBody] UpdateMemberDto updateMemberDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
              
                await _memberService.UpdatesAsync(updateMemberDto, appUserId);
                return Ok("Kullanıcı bilgileri başarıyla güncellendi.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Sunucu hatası: {ex.Message}");
            }
        }

        [HttpDelete("{id}/user/{appUserId}")]
        [Authorize(Roles = "manager,member")]
        public async Task<IActionResult> DeleteMember(int id, int appUserId)
        {
            try
            {
                await _memberService.DeleteAsync(id, appUserId);
                return Ok("İlgili kullanıcı bilgisi silindi");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Sunucu hatası: {ex.Message}");
            }
        }
    }
}
