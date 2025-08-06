using System.ComponentModel.DataAnnotations;

namespace AppRepository.Entities.Dtos
{
    public class CreateMemberDto
    {
        [Required(ErrorMessage ="Website ad alanı boş bırakılamaz")]
        [Display(Name ="Website adı")]
        public string WebName { get; set; }

        [Required(ErrorMessage ="Kullanıcı adı alanı boş bırakılamaz")]
        [Display(Name ="Kullanıcı adı")]
        public string Username { get; set; }

        [Required(ErrorMessage ="Şifre alanı boş bırakılamaz")]
        [Display(Name ="Şifre")]
        public string Password { get; set; }
    }
}
