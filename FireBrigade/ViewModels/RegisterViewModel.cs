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
        if (password != confirmPassword)
        {
            var currentPageErro = Shell.Current.CurrentPage;
            await currentPageErro.DisplayAlert("Erro",
                                              $"As senhas não conferem", "Ok");
            return;
        }

        var createUser = new UserBrigade(email.ToLower(), password.ToLower());
        if (!createUser.IsValid())
        {
            var currentPageErro = Shell.Current.CurrentPage;
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
