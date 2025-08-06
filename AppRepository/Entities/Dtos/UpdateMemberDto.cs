using System.ComponentModel.DataAnnotations;

namespace AppRepository.Entities.Dtos
{
    public class UpdateMemberDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Web sitesi ismi gereklidir.")]
        public string WebName { get; set; }

        [Required(ErrorMessage ="Kullanıcı adı gereklidir.")]
        public string Username { get; set; }

        [Required(ErrorMessage ="Şifre gereklidir.")]
        public string Password { get; set; }
    }
}
