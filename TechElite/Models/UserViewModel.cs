using System.ComponentModel.DataAnnotations;

namespace TechElite.Models
{
    public class UserViewModel : IValidatableObject
    {
        public string Id { get; set; }

        [Display(Name = "Användarnamn")]
        public string? UserName { get; set; }

        [Display(Name = "Roll")]
        public IList<string>? Roles { get; set; }

        [Display(Name = "E-post")]
        public string? Email { get; set; }

        [Display(Name = "Förnamn")]
        public string? FirstName { get; set; }

        [Display(Name = "Efternamn")]
        public string? LastName { get; set; }

        [Display(Name = "Lösenord")]
        public string? Password { get; set; }

        [Display(Name = "Bekräfta lösenord")]
        public string? PasswordConfirm { get; set; }

        public string CurrentPassword { get; set; }

        public bool ChangePassword { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ChangePassword)
            {
                if (string.IsNullOrEmpty(Password))
                {
                    yield return new ValidationResult("Lösenord krävs.", new[] { nameof(Password) });
                }

                if (string.IsNullOrEmpty(PasswordConfirm))
                {
                    yield return new ValidationResult("Du måste bekräfta lösenordet.", new[] { nameof(PasswordConfirm) });
                }

                if (!string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(PasswordConfirm) && Password != PasswordConfirm)
                {
                    yield return new ValidationResult("Lösenorden matchar inte.", new[] { nameof(PasswordConfirm) });
                }

                if (string.IsNullOrEmpty(CurrentPassword))
                {
                    yield return new ValidationResult("Nuvarande lösenord krävs för att ändra lösenord.", new[] { nameof(CurrentPassword) });
                }
            }
        }
    }
}