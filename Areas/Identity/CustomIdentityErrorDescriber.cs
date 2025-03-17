using Microsoft.AspNetCore.Identity;

// Skapar egen class CustomIdentityErrorDescriber för att sätta egna felmeddelanden vid registrering och login

namespace LUXLocks_projekt.Areas.Identity
{
    public class CustomIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError PasswordRequiresDigit()
        {
            return new IdentityError { Code = nameof(PasswordRequiresDigit), Description = "Lösenordet måste innehålla minst en siffra (0-9)." };
        }

        public override IdentityError PasswordRequiresLower()
        {
            return new IdentityError { Code = nameof(PasswordRequiresLower), Description = "Lösenordet måste innehålla minst en liten bokstav (a-z)." };
        }

        public override IdentityError PasswordRequiresUpper()
        {
            return new IdentityError { Code = nameof(PasswordRequiresUpper), Description = "Lösenordet måste innehålla minst en stor bokstav (A-Z)." };
        }

        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new IdentityError { Code = nameof(PasswordRequiresNonAlphanumeric), Description = "Lösenordet måste innehålla minst ett specialtecken (t.ex. @, #, !)." };
        }

        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError { Code = nameof(PasswordTooShort), Description = $"Lösenordet måste vara minst {length} tecken långt." };
        }

        public override IdentityError InvalidEmail(string? email)
        {
            return new IdentityError { Code = nameof(InvalidEmail), Description = "Ange en giltig e-postadress." };
        }

        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError { Code = nameof(DuplicateUserName), Description = "Denna e-postadress är redan registrerad." };
        }
    }
}