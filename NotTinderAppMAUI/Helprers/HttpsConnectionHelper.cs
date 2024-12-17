namespace NotTinderAppMAUI.Helprers;

public class HttpsConnectionHelper
{
    private readonly Lazy<HttpClient> LazyHttpClient;

    public HttpsConnectionHelper()
    {
        SslPort = 7777;
        DevServerRootUrl = FormattableString.Invariant($"https://{DevServerName}:{SslPort}");
        LazyHttpClient = new Lazy<HttpClient>(() => new HttpClient(new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
        }) { Timeout = TimeSpan.FromSeconds(10) });
    }

    public int SslPort { get; }

    public static string DevServerName { get; set; } = "10.0.2.2";


    public string DevServerRootUrl { get; }
    public HttpClient HttpClient => LazyHttpClient.Value;
}