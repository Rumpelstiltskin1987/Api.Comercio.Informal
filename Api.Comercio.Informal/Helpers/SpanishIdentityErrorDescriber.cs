using Microsoft.AspNetCore.Identity;

namespace Api.Comercio.Informal.Helpers
{
    public class SpanishIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DefaultError() => new IdentityError { Code = nameof(DefaultError), Description = "Ha ocurrido un error desconocido." };
        public override IdentityError ConcurrencyFailure() => new IdentityError { Code = nameof(ConcurrencyFailure), Description = "Error de concurrencia optimista, el objeto ha sido modificado." };
        public override IdentityError PasswordMismatch() => new IdentityError { Code = nameof(PasswordMismatch), Description = "La contraseña es incorrecta." };
        public override IdentityError InvalidToken() => new IdentityError { Code = nameof(InvalidToken), Description = "El token no es válido." };
        public override IdentityError LoginAlreadyAssociated() => new IdentityError { Code = nameof(LoginAlreadyAssociated), Description = "Un usuario con este inicio de sesión ya existe." };
        public override IdentityError InvalidUserName(string userName) => new IdentityError { Code = nameof(InvalidUserName), Description = $"El nombre de usuario '{userName}' no es válido." };
        public override IdentityError InvalidEmail(string email) => new IdentityError { Code = nameof(InvalidEmail), Description = $"La dirección de correo electrónico '{email}' no es válida." };
        public override IdentityError DuplicateUserName(string userName) => new IdentityError { Code = nameof(DuplicateUserName), Description = $"El nombre de usuario '{userName}' ya está en uso." };
        public override IdentityError DuplicateEmail(string email) => new IdentityError { Code = nameof(DuplicateEmail), Description = $"El correo electrónico '{email}' ya está en uso." };
        public override IdentityError InvalidRoleName(string role) => new IdentityError { Code = nameof(InvalidRoleName), Description = $"El nombre de rol '{role}' no es válido." };
        public override IdentityError DuplicateRoleName(string role) => new IdentityError { Code = nameof(DuplicateRoleName), Description = $"El nombre de rol '{role}' ya está en uso." };
        public override IdentityError UserAlreadyInRole(string role) => new IdentityError { Code = nameof(UserAlreadyInRole), Description = "El usuario ya tiene asignado este rol." };
        public override IdentityError UserNotInRole(string role) => new IdentityError { Code = nameof(UserNotInRole), Description = "El usuario no tiene asignado este rol." };
        public override IdentityError PasswordTooShort(int length) => new IdentityError { Code = nameof(PasswordTooShort), Description = $"La contraseña debe tener al menos {length} caracteres." };
        public override IdentityError PasswordRequiresNonAlphanumeric() => new IdentityError { Code = nameof(PasswordRequiresNonAlphanumeric), Description = "La contraseña debe tener al menos un carácter no alfanumérico (ejemplo: . , ! #)." };
        public override IdentityError PasswordRequiresDigit() => new IdentityError { Code = nameof(PasswordRequiresDigit), Description = "La contraseña debe tener al menos un dígito ('0'-'9')." };
        public override IdentityError PasswordRequiresLower() => new IdentityError { Code = nameof(PasswordRequiresLower), Description = "La contraseña debe tener al menos una letra minúscula ('a'-'z')." };
        public override IdentityError PasswordRequiresUpper() => new IdentityError { Code = nameof(PasswordRequiresUpper), Description = "La contraseña debe tener al menos una letra mayúscula ('A'-'Z')." };
    }
}
