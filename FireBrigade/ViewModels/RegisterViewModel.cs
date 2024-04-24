using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FireBrigade.Domain.Entities;
using FireBrigade.Domain.Interfaces;

namespace FireBrigade.ViewModels;

public partial class RegisterViewModel : ObservableObject
{
    [ObservableProperty]
    string? email;

    [ObservableProperty]
    string? password;

    [ObservableProperty]
    string? confirmPassword;

    private readonly IUserBrigadeRepository _userBrigadeRepository;
    public RegisterViewModel()
    {
        _userBrigadeRepository = App.Current.Handler.MauiContext.Services.GetService<IUserBrigadeRepository>();
    }

    [RelayCommand]
    public async Task Register()
    {
        var currentPageErro = Shell.Current.CurrentPage;
        if (password != confirmPassword)
        {
            await currentPageErro.DisplayAlert("Erro",
                                              $"As senhas não conferem", "Ok");
            return;
        }

        var createUser = new UserBrigade(
            email?.ToLower() ?? string.Empty,
            password?.ToLower() ?? string.Empty);

        var validationResult = new UserValidator().Validate(createUser);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("\n", validationResult.Errors.Select(e => e.ErrorMessage));
            await currentPageErro.DisplayAlert("Erro de Validação", errors, "Ok");
            return;
        }

        if (!createUser.IsValid())
        {
            await currentPageErro.DisplayAlert("Erro",
                               $"Preencha todos os campos", "Ok");

            return;
        }

        await _userBrigadeRepository.Create(createUser);

        var currentPage = Shell.Current.CurrentPage;
        bool isConfirmed = await currentPage.DisplayAlert("Sucesso",
            $"Usuário criado com sucesso", "Ok", "Fechar");

        if (isConfirmed)
            await Shell.Current.GoToAsync("//login");
    }

    [RelayCommand]
    public async Task GoToLogin()
    {
        await Shell.Current.GoToAsync("//login");
    }
}
