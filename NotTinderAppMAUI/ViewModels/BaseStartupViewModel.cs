using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NotTinderAppMAUI.Models;
using NotTinderAppMAUI.Validation;

namespace NotTinderAppMAUI.ViewModels;

public partial class BaseStartupViewModel : ObservableValidator
{
    protected const string ImagesAttribute = "images";
    protected const string IconAttribute = "icon";
    protected readonly ApiServiceProvider _apiServiceProvider;

    protected readonly IPopupService _popupService;

    [Required] [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(PutStartupCommand))]
    private string _description;

    [Required]
    [Range(1, int.MaxValue)]
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyCanExecuteChangedFor(nameof(PutStartupCommand))]
    private int _donationTarget;

    [ObservableProperty] [Required] [NotifyDataErrorInfo] [NotifyCanExecuteChangedFor(nameof(PutStartupCommand))]
    private ImageItem _icon = new();

    [Required]
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotEmpty(ErrorMessage = "Add at least 1 picture!")]
    [NotifyCanExecuteChangedFor(nameof(PutStartupCommand))]
    private ObservableCollection<ImageItem> _images = new();

    [Required] [ObservableProperty] [NotifyDataErrorInfo] [NotifyCanExecuteChangedFor(nameof(PutStartupCommand))]
    private string _name;

    public BaseStartupViewModel(ApiServiceProvider apiServiceProvider, IPopupService popupService)
    {
        _popupService = popupService;
        _apiServiceProvider = apiServiceProvider;
        _images.CollectionChanged += (sender, args) => OnPropertyChanged(nameof(Images));
        _images.CollectionChanged += (sender, args) => OnPropertyChanged(nameof(IsImagesNotAdded));
        _images.CollectionChanged += (sender, args) => ValidateAllProperties();

        ValidateAllProperties();
    }

    public bool IsImagesNotAdded => Images.Count == 0;

    [RelayCommand]
    private async Task DeleteImage(ImageItem image)
    {
        Images.Remove(image);
    }

    [RelayCommand]
    private async Task AddImage()
    {
        try
        {
            var result = await FilePicker.Default.PickMultipleAsync(new PickOptions
            {
                FileTypes = FilePickerFileType.Images,
                PickerTitle = "Add image"
            });

            if (result != null)
                foreach (var file in result)
                    Images.Add(new ImageItem { PathOrUrl = file.FullPath });
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Ошибка", $"Не удалось загрузить изображение: {ex.Message}",
                "OK");
        }
    }

    [RelayCommand]
    private async Task PickIcon()
    {
        try
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                FileTypes = FilePickerFileType.Images,
                PickerTitle = "Pick icon"
            });

            if (result != null) Icon = new ImageItem { PathOrUrl = result.FullPath };
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Ошибка", $"Не удалось загрузить изображение: {ex.Message}",
                "OK");
        }
    }

    [RelayCommand(CanExecute = nameof(CanPut))]
    protected virtual async Task PutStartup()
    {
    }

    protected bool CanPut()
    {
        ValidateAllProperties();

        return !HasErrors;
    }
}