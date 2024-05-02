using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FireBrigade.Domain.Entities;
using FireBrigade.Domain.Interfaces;
using FireBrigade.Libraries.Messages;
using System.Windows.Input;

namespace FireBrigade.ViewModels;

public partial class ListBrigadeEmergencyViewModel : ObservableObject
{
    [ObservableProperty]
    private string textSearch = "";

    private List<EmergencyBrigade> emergencyBrigades;

    [ObservableProperty]
    public List<EmergencyBrigade> emergencyBrigadeFilter;

    private readonly IEmergencyBrigadeRepository _emergencyBrigadeRepository;

    public ListBrigadeEmergencyViewModel()
    {
        _emergencyBrigadeRepository = App.Current.Handler.MauiContext.Services.GetService<IEmergencyBrigadeRepository>();

        emergencyBrigades = _emergencyBrigadeRepository.GetAll().ToList();
        EmergencyBrigadeFilter = emergencyBrigades.ToList();

        WeakReferenceMessenger.Default.Register<EmergencyBrigadeMessage>(this, (r, m) =>
        {
            LoadEmergencyBrigade();
        });
    }

    private void LoadEmergencyBrigade()
    {
        emergencyBrigades = _emergencyBrigadeRepository.GetAll().ToList();
        EmergencyBrigadeFilter = emergencyBrigades.ToList();
    }

    public ICommand PerformSearch => new Command<string>((string query) =>
    {
        EmergencyBrigadeFilter = emergencyBrigades.Where(c => c.Name.ToLower().Contains(query.ToLower())).ToList();
    });

    public async Task EditEmergencyBrigade(EmergencyBrigade emergencyBrigade)
    {
        var emergencyBrigadeId = emergencyBrigade.Id;
        await Shell.Current.GoToAsync($"brigadeEmergency?Id={emergencyBrigadeId}");
    }
}
