namespace TorStatusChecker.Tests.Acceptance.Helpers;

public class HttpClientFactory : IHttpClientFactory
{
    public HttpClient CreateClient(string name)
    {
        return new HttpClient();
    }
}