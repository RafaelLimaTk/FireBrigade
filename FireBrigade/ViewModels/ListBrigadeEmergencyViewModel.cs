using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FireBrigade.Domain.Entities;
using FireBrigade.Domain.Interfaces;
using FireBrigade.Libraries.Messages;
using System.Xml.Linq;

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

    [RelayCommand]
    private void PerformSearch()
    {
        EmergencyBrigadeFilter = emergencyBrigades.Where(c => c.Name.ToLower().Contains(textSearch.ToLower())).ToList();
    }

    [RelayCommand]
    public async Task EditEmergencyBrigade(EmergencyBrigade emergencyBrigade)
    {
        var emergencyBrigadeId = emergencyBrigade.Id;
        await Shell.Current.GoToAsync($"../brigadeemergency?brigadeEmergencyId={emergencyBrigadeId}");
    }

    [RelayCommand]
    public async Task DeleteEmergencyBrigade(EmergencyBrigade emergencyBrigadeId)
    {

        var currentPage = Shell.Current.CurrentPage;
        bool isConfirmed = await currentPage.DisplayAlert("Confirmação de exclusão",
            $"Tem certeza que deseja excluir permanentemente o brigadista {emergencyBrigadeId.Name}? Esta ação não pode ser desfeita.", "Excluir", "Cancelar");
        if (isConfirmed)
        {
            var emergencyBrigade = await GetEmergencyBrigade(emergencyBrigadeId.Id);
            await _emergencyBrigadeRepository.Delete(emergencyBrigade);
            await Shell.Current.CurrentPage.DisplayAlert("Sucesso", "Brigada de emergência deletada com sucesso", "Ok");
            WeakReferenceMessenger.Default.Send(new EmergencyBrigadeMessage(emergencyBrigade));
        }
    }

    private async Task<EmergencyBrigade> GetEmergencyBrigade(Guid emergencyBrigadeId)
        => await _emergencyBrigadeRepository.GetById(emergencyBrigadeId);
}
