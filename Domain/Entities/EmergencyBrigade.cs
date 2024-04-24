using FireBrigade.Domain.Entities.Base;
using FluentValidation;

namespace FireBrigade.Domain.Entities;

public enum Function
{
    CordenadorGeral,
    Líder,
    BrigadistaCombate,
    BrigadistaComunicacaoEvacuacao,
    BrigadistaCorteEnergiaEvacuacao,
    BrigadistaEvacuacao,
    BrigadistaPrimeirosSocorrosEvacuacao,
}

public class EmergencyBrigade : EntityBase
{
    public string Name { get; set; }
    public string Pole { get; set; }
    public string Address { get; set; }
    public Function Function { get; set; }
    public string Sector { get; set; }
    public string PhoneNumber { get; set; }
    public string ImageProfile { get; set; }

    public EmergencyBrigade(string name, string pole, string address, Function function, string sector, string phoneNumber, string imageProfile = "")
    {
        Name = name;
        Pole = pole;
        Address = address;
        Function = function;
        Sector = sector;
        PhoneNumber = phoneNumber;
        ImageProfile = imageProfile;
    }

    public bool IsValid() => new EmergencyBrigadeValidator().Validate(this).IsValid;
}

public class EmergencyBrigadeValidator : AbstractValidator<EmergencyBrigade>
{
    public EmergencyBrigadeValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome não pode estar vazio.")
            .MinimumLength(2).WithMessage("O nome deve ter no mínimo 2 caracteres.");
        RuleFor(x => x.Pole)
            .NotEmpty().WithMessage("O polo não pode estar vazio.")
            .MinimumLength(1).WithMessage("O polo deve ter no mínimo 1 caracter.");
        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("O endereço não pode estar vazio.")
            .MinimumLength(2).WithMessage("O endereço deve ter no mínimo 2 caracteres.");
        RuleFor(x => x.Function)
            .NotEmpty().WithMessage("A função não pode estar vazia.");
        RuleFor(x => x.Sector)
            .NotEmpty().WithMessage("O setor não pode estar vazio.")
            .MinimumLength(2).WithMessage("O setor deve ter no mínimo 2 caracteres.");
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("O número de telefone não pode estar vazio.")
            .MinimumLength(2).WithMessage("O número de telefone deve ter no mínimo 2 caracteres.");
    }
}
