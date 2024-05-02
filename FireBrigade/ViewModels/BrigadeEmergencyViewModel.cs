using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FireBrigade.Domain.Entities;
using FireBrigade.Domain.Interfaces;
using System.ComponentModel.Design;
using System.Diagnostics;

namespace FireBrigade.ViewModels;

[QueryProperty(nameof(BrigadeEmergencyId), "brigadeEmergencyId")]
public partial class BrigadeEmergencyViewModel : ObservableObject
{
    [ObservableProperty]
    string brigadeEmergencyId;

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

    private bool IsEditMode => !string.IsNullOrEmpty(BrigadeEmergencyId);

    private readonly IEmergencyBrigadeRepository _emergencyBrigadeRepository;

    public BrigadeEmergencyViewModel()
    {
        _emergencyBrigadeRepository = App.Current.Handler.MauiContext.Services.GetService<IEmergencyBrigadeRepository>();
        Functions = Enum.GetNames(typeof(Function)).ToList();

        SelectedFunction = Functions.FirstOrDefault();
    }

    [RelayCommand]
    public async Task NavigateToListBrigadeEmergencyPage() => await Shell.Current.GoToAsync("home/listbrigadeemergency");

    partial void OnBrigadeEmergencyIdChanged(string value)
    {
        if (IsEditMode)
            LoadEmergencyBrigadeEdit(BrigadeEmergencyId);
    }

    private async Task LoadEmergencyBrigadeEdit(string brigadeEmergencyId)
    {
        var emergencyId = Guid.Parse(brigadeEmergencyId);
        var emergencyBrigade = await _emergencyBrigadeRepository.GetById(emergencyId);

        if (emergencyBrigade is not null)
        {
            PopulateEmergencyBrigade(emergencyBrigade);
        }
    }
    
    private void PopulateEmergencyBrigade(EmergencyBrigade emergencyBrigade)
    {
        Name = emergencyBrigade.Name;
        Pole = emergencyBrigade.Pole;
        Address = emergencyBrigade.Address;
        Sector = emergencyBrigade.Sector;
        PhoneNumber = emergencyBrigade.PhoneNumber;
        ImagePath = emergencyBrigade.ImageProfile;
        SelectedFunction = emergencyBrigade.Function.ToString();
    }

    [RelayCommand]
    public async Task SaveAndEdit()
    {
        if (IsEditMode)
            await UpdateEmergencyBrigade(Guid.Parse(BrigadeEmergencyId));
        else
            await SaveEmergencyBrigade();
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
    public async Task DeleteEmergencyBrigade(Guid emergencyBrigadeId)
    {

        var currentPage = Shell.Current.CurrentPage;
        bool isConfirmed = await currentPage.DisplayAlert("Confirmação de exclusão",
            $"Tem certeza que deseja excluir permanentemente o brigadista {name}? Esta ação não pode ser desfeita.", "Excluir", "Cancelar");
        if (isConfirmed)
        {
            var emergencyBrigade = await GetEmergencyBrigade(emergencyBrigadeId);
            await _emergencyBrigadeRepository.Delete(emergencyBrigade);
            await Shell.Current.CurrentPage.DisplayAlert("Sucesso", "Brigada de emergência deletada com sucesso", "Ok");
        }
    }

    [RelayCommand]
    public async Task UpdateEmergencyBrigade(Guid emergencyBrigadeId)
    {
        var emergencyBrigade = await GetEmergencyBrigade(emergencyBrigadeId);
        emergencyBrigade = MapperEmergencyBrigade(emergencyBrigade);

        await _emergencyBrigadeRepository.Update(emergencyBrigade);
        await Shell.Current.CurrentPage.DisplayAlert("Sucesso", "Brigada de emergência atualizada com sucesso", "Ok");
        await NavigateToListBrigadeEmergencyPage();
    }

    private EmergencyBrigade MapperEmergencyBrigade(EmergencyBrigade emergencyBrigade)
    {
        emergencyBrigade.Name = Name;
        emergencyBrigade.Pole = Pole;
        emergencyBrigade.Address = Address;
        emergencyBrigade.Sector = Sector;
        emergencyBrigade.PhoneNumber = PhoneNumber;
        emergencyBrigade.ImageProfile = ImagePath;
        emergencyBrigade.Function = Enum.Parse<Function>(SelectedFunction);

        return emergencyBrigade;
    }

    private async Task<EmergencyBrigade> GetEmergencyBrigade(Guid emergencyBrigadeId)
        => await _emergencyBrigadeRepository.GetById(emergencyBrigadeId);

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
