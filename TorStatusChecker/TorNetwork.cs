using System.Reactive.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace TorStatusChecker;

public class TorNetwork : ITorNetwork
{
    private static readonly Uri IssuesPath = new("https://gitlab.torproject.org/api/v4/projects/786/repository/tree?path=content/issues");
    private static readonly Uri IssuesRoot = new("https://gitlab.torproject.org/tpo/tpa/status-site/-/raw/main/");

    private readonly IDeserializer deserializer;
    private readonly IUriBasedStringStore stringStore;

    public TorNetwork(IUriBasedStringStore stringStore)
    {
        this.stringStore = stringStore;

        Issues = GetIssueFilenames()
            .SelectMany(uris => uris.ToObservable()
                .SelectMany(GetIssueFromUri));

        deserializer = new DeserializerBuilder()
            .IgnoreUnmatchedProperties()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
    }

    public IObservable<Issue> Issues { get; }

    private static IObservable<Uri> ParseResponse(string responseText)
    {
        return JArray.Parse(responseText)
            .Select(d => d["path"])
            .Select(x => x.ToString())
            .Where(x => x.EndsWith(".md"))
            .Select(filename => new Uri(IssuesRoot, filename))
            .ToObservable();
    }

    private IObservable<IList<Uri>> GetIssueFilenames()
    {
        var input = Observable
            .FromAsync(() => stringStore.Fetch(IssuesPath))
            .SelectMany(s => ParseResponse(s).ToList());

        return input;
    }

    private IObservable<Issue> GetIssueFromUri(Uri path)
    {
        var observable = Observable
            .FromAsync(() => stringStore.Fetch(path))
            .Select(GetIssueFromContent);
        return observable;
    }

    private Issue GetIssueFromContent(string content)
    {
        var regex = @"---\s+(.*)\s+---(.*)";
        var matches = Regex.Match(content, regex, RegexOptions.Singleline);
        var yml = matches.Groups[1].Value;

        // Still unused
        var description = matches.Groups[2].Value;

        var issue = deserializer.Deserialize<Issue>(yml);
        return issue;
    }
}