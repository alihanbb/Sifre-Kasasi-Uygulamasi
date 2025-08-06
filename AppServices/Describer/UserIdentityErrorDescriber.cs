using Microsoft.AspNetCore.Identity;

namespace AppServices.Describer
{
    public class UserIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateUserName),
                Description = "Bu kullanıcı adı zaten alınmış"
            };
        }
        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateEmail),
                Description = "Bu e-posta adresi zaten alınmış"
            };
        }

        public override IdentityError InvalidEmail(string email)
        {
            return new IdentityError
            {
                Code = nameof(InvalidEmail),
                Description = "E-posta adresi geçersiz"
            };
        }

        public override IdentityError PasswordRequiresDigit()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresDigit),
                Description = "Parola en az bir rakam içermelidir"
            };
        }

        public override IdentityError PasswordRequiresLower()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresLower),
                Description = "Parola en az bir küçük harf içermelidir"
            };
        }

        public override IdentityError PasswordRequiresUpper()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresUpper),
                Description = "Parola en az bir büyük harf içermelidir"
            };
        }

        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError
            {
                Code = nameof(PasswordTooShort),
                Description = $"Parola en az {length} karakter uzunluğunda olmalıdır"
            };
        }
        public override IdentityError InvalidUserName(string? firstName)
        {
            return new IdentityError
            {
                Code = nameof(InvalidUserName),
                Description = $"{firstName} alanında rakam veya karakter içeremez"
            };
        }
    }
}
