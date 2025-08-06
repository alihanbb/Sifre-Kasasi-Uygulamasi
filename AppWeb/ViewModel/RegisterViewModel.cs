using System.ComponentModel.DataAnnotations;

namespace AppWeb.ViewModel
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="İsim alanı boş bırakılamaz")]
        [Display(Name ="Ad:")]
        public string FirstName { get; set; }

        [Required(ErrorMessage ="Soyad alanı boş bırakılamaz")]
        [Display(Name = "Soyad:")]
        public string LastName { get; set; }

        [Required(ErrorMessage ="Email alanı boş bırakılamaz")]
        [Display(Name = "Soyad:")]
        [EmailAddress(ErrorMessage ="Email formatı yanlıştır")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Kullanıcı adı alanı boş bırakılamaz")]
        [Display(Name = "Kullanıcı Adı:")]
        public string UserName { get; set; }

        [Required(ErrorMessage ="Şifre alanı boş bırakılamaz")]
        [Display(Name = "Şifre:")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Şifreniz en az 6 karakter olabilir")]
        public string Password { get; set; }

        [Required(ErrorMessage ="Şifre tekrar alanı boş bırakılamaz")]
        [Display(Name = "Şifre Tekrar:")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Şifre aynı değildir.")]
        [MinLength(6, ErrorMessage = "Şifreniz en az 6 karakter olabilir")]
        public string PasswordConfirm { get; set; }

    }
}
