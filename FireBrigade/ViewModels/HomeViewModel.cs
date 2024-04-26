using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace FireBrigade.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    public HomeViewModel()
    {
    }

    [RelayCommand]
    public async Task NavigateToBrigadeEmergencyPage() => await Shell.Current.GoToAsync("brigadeemergency");

    [RelayCommand]
    public async Task NavigateToListBrigadeEmergencyPage() => await Shell.Current.GoToAsync("home/listbrigadeemergency");
}
