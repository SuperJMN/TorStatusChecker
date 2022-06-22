namespace TorStatusChecker.Tests.Acceptance.Helpers;

public class Fetcher : IUriBasedStringStore
{
    private readonly IHttpClientFactory httpClientFactory;

    public Fetcher(IHttpClientFactory httpClientFactory)
    {
        this.httpClientFactory = httpClientFactory;
    }

    public async Task<string> Fetch(Uri uri)
    {
        using var client = httpClientFactory.CreateClient();
        return await client.GetStringAsync(uri);
    }
}