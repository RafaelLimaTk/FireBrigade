using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FireBrigade.Domain.Entities;
using FireBrigade.Domain.Interfaces;
using System.Diagnostics;

namespace FireBrigade.ViewModels;

public partial class BrigadeEmergencyViewModel : ObservableObject
{
    [ObservableProperty]
    string pole;

    [ObservableProperty]
    string address;

    [ObservableProperty]
    string name;

    [ObservableProperty]
    string sector;

    [ObservableProperty]
    string phoneNumber;

    [ObservableProperty]
    string imagePath;

    [ObservableProperty]
    string selectedFunction;

    [ObservableProperty]
    List<string> functions;

    private readonly IEmergencyBrigadeRepository _emergencyBrigadeRepository;

    public BrigadeEmergencyViewModel()
    {
        _emergencyBrigadeRepository = App.Current.Handler.MauiContext.Services.GetService<IEmergencyBrigadeRepository>();
        Functions = Enum.GetNames(typeof(Function)).ToList();

        SelectedFunction = Functions.FirstOrDefault();
    }

    [RelayCommand]
    public async Task SaveEmergencyBrigade()
    {
        Function function = Enum.Parse<Function>(SelectedFunction);
        var emergencyBrigadeCreate = new EmergencyBrigade(name, pole, address, function, sector, phoneNumber, imagePath);
        var validationResult = new EmergencyBrigadeValidator().Validate(emergencyBrigadeCreate);
        if (!emergencyBrigadeCreate.IsValid())
        {
            var errors = string.Join("\n", validationResult.Errors.Select(e => e.ErrorMessage));
            await Shell.Current.CurrentPage.DisplayAlert("Erro", errors, "Ok");
            return;
        }

        await _emergencyBrigadeRepository.Create(emergencyBrigadeCreate);
        await Shell.Current.CurrentPage.DisplayAlert("Sucesso", "Brigada de emergência criada com sucesso", "Ok");
    }

    [RelayCommand]
    public async Task DeleteEmergencyBrigade(EmergencyBrigade emergencyBrigade)
    {
        await _emergencyBrigadeRepository.Delete(emergencyBrigade);
        await Shell.Current.CurrentPage.DisplayAlert("Sucesso", "Brigada de emergência deletada com sucesso", "Ok");
    }

    [RelayCommand]
    public async Task UpdateEmergencyBrigade(EmergencyBrigade emergencyBrigade)
    {
        await _emergencyBrigadeRepository.Update(emergencyBrigade);
        await Shell.Current.CurrentPage.DisplayAlert("Sucesso", "Brigada de emergência atualizada com sucesso", "Ok");
    }

    [RelayCommand]
    public async Task SelectImage()
    {
        try
        {
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Por favor selecione uma foto"
            });

            if (result != null)
            {
                var stream = await result.OpenReadAsync();
                var folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Images");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                var filePath = Path.Combine(folderPath, result.FileName);
                using (var fileStream = File.OpenWrite(filePath))
                {
                    await stream.CopyToAsync(fileStream);
                }

                ImagePath = filePath;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
    }
}
