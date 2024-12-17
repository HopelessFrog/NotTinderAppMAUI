using CommunityToolkit.Maui.Core;
using NotTinderAppMAUI.Helprers;
using NotTinderAppMAUI.Models;
using NotTinderAppMAUI.Models.DTOs;

namespace NotTinderAppMAUI.ViewModels;

public class UpdateStartupViewModel : BaseStartupViewModel
{
    private int _startupId;

    public UpdateStartupViewModel(ApiServiceProvider apiServiceProvider, IPopupService popupService) : base(
        apiServiceProvider, popupService)
    {
    }

    public async Task LoadStartupData(StartupShort startup)
    {
        _startupId = startup.Id;
        Name = startup.Name;
        Description = startup.Description;
        DonationTarget = startup.DonationTarget;
        Icon = new ImageItem { PathOrUrl = DevUsrlConvert(startup.Icon) };
        foreach (var image in startup.Images) Images.Add(new ImageItem { PathOrUrl = DevUsrlConvert(image) });
    }

    protected override async Task PutStartup()
    {
        await base.PutStartup();

        var files = new List<FileDto>();

        foreach (var image in Images)
            files.Add(new FileDto
            {
                AtributeName = ImagesAttribute, FileName = image.GetHashCode().ToString(),
                FileStream = await image.GetFileStreamAsync(), ContentType = "image/png"
            });

        files.Add(new FileDto
        {
            AtributeName = IconAttribute, FileName = Icon.GetHashCode().ToString(),
            FileStream = await Icon.GetFileStreamAsync(), ContentType = "image/png"
        });


        var response = await _apiServiceProvider.CallWebApi($"/api/Startups?id={_startupId}",
            new StartupRequest { Name = Name, Description = Description, DonationTarget = DonationTarget }, files,
            HttpMethod.Put);

        if (response.IsSuccess)
        {
            await ToastHelper.ShowToastAsync("Startup updated successfully");
            await _popupService.ClosePopupAsync(true);
        }
        else
        {
            await ToastHelper.ShowToastAsync(response.Message);
        }
    }

    private string DevUsrlConvert(string url)
    {
        var index = url.IndexOf('?');
        url = url.Substring(0, index);
        return url.Replace("localhost", "10.0.2.2");
    }
}