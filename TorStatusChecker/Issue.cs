namespace TorStatusChecker;

public class Issue
{
    public string Title { get; set; }
    public DateTimeOffset Date { get; set; }
    public bool Resolved { get; set; }
    public string Severity { get; set; }
    public IList<string> Affected { get; set; }
}