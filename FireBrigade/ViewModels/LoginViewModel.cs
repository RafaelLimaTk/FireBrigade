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
    public async void Save()
    {
        var createUser = new UserBrigade(email, password);
        if (!createUser.IsValid()) return;

        //await _userBrigadeRepository.Create(createUser);
        GetUser(email, password);
    }

    public async void GetUser(string email, string password)
    {
        var teste = await _userBrigadeRepository.GetByUser(email, password);
    }
}
