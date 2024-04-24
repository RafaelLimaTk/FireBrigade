using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FireBrigade.Domain.Entities;
using FireBrigade.Domain.Interfaces;
using FireBrigade.Infra.Data.Repositories;

namespace FireBrigade.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly IUserBrigadeRepository _userBrigadeRepository;

    [ObservableProperty]
    string? email;

    [ObservableProperty]
    string? password;

    public LoginViewModel()
    {
        _userBrigadeRepository = App.Current.Handler.MauiContext.Services.GetService<IUserBrigadeRepository>();
    } 

    [RelayCommand]
    public async Task Login()
    {
        var user = await _userBrigadeRepository.GetByUser(email, password);
        if (user is null) return;

        await Shell.Current.GoToAsync("home");
    }

    [RelayCommand]
    public async Task GoToRegister()
    {
        await Shell.Current.GoToAsync("register");
    }
}
