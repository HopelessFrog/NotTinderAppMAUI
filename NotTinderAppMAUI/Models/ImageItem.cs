using NotTinderAppMAUI.Validation;

namespace NotTinderAppMAUI.Models;

public class ImageItem
{
    [NotEqual("question_icon.svg")] public string PathOrUrl { get; set; } = "question_icon.svg";

    public ImageSource Image => GetImageSource();

    private ImageSource GetImageSource()
    {
        if (Uri.IsWellFormedUriString(PathOrUrl, UriKind.Absolute))
            return ImageSource.FromUri(new Uri(PathOrUrl));
        return ImageSource.FromFile(PathOrUrl);
    }

    public async Task<Stream> GetFileStreamAsync()
    {
        if (Uri.IsWellFormedUriString(PathOrUrl, UriKind.Absolute))
        {
            var client = new HttpClient();
            return await client.GetStreamAsync(PathOrUrl);
        }

        return new FileStream(PathOrUrl, FileMode.Open, FileAccess.Read);
    }
}