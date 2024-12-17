using CommunityToolkit.Maui.Core;
using NotTinderAppMAUI.Helprers;
using NotTinderAppMAUI.Models;
using NotTinderAppMAUI.Models.DTOs;

namespace NotTinderAppMAUI.ViewModels;

public class AddStartupViewModel : BaseStartupViewModel
{
    public AddStartupViewModel(ApiServiceProvider apiServiceProvider, IPopupService popupService) : base(
        apiServiceProvider, popupService)
    {
    }

    protected override async Task PutStartup()
    {
        var files = new List<FileDto>();

        foreach (var image in Images)
            files.Add(new FileDto
            {
                AtributeName = ImagesAttribute, FileName = image.GetHashCode().ToString(),
                FileStream = File.OpenRead(image.PathOrUrl), ContentType = "image/png"
            });

        files.Add(new FileDto
        {
            AtributeName = IconAttribute, FileName = Icon.GetHashCode().ToString(),
            FileStream = File.OpenRead(Icon.PathOrUrl), ContentType = "image/png"
        });


        var response = await _apiServiceProvider.CallWebApi<StartupRequest, ResponseDto>("/api/Startups",
            new StartupRequest { Name = Name, Description = Description, DonationTarget = DonationTarget }, files);

        if (response.IsSuccess)
        {
            await ToastHelper.ShowToastAsync("Startup created successfully");
            await _popupService.ClosePopupAsync(true);
        }
        else
        {
            await ToastHelper.ShowToastAsync(response.Message);
        }
    }
}