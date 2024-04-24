using FireBrigade.Domain.Entities.Base;
using FluentValidation;

namespace FireBrigade.Domain.Entities;

public class UserBrigade : EntityBase
{
    public string Email { get; set; }
    public string Password { get; set; }

    public UserBrigade(string email, string password)
    {
        Email = email;
        Password = password;
    }

    public bool IsValid() => new UserValidator().Validate(this).IsValid;
}

public class UserValidator : AbstractValidator<UserBrigade>
{
    public UserValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O email não pode estar vazio.")
            .EmailAddress().WithMessage("O email deve ser válido.");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("A senha não pode estar vazia.")
            .MinimumLength(6).WithMessage("A senha deve ter no mínimo 6 caracteres.");
    }
}