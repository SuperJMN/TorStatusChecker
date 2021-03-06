namespace TorStatusChecker;

public class Issue
{
    public string Title { get; set; }
    public DateTimeOffset Date { get; set; }
    public bool Resolved { get; set; }
    public string Severity { get; set; }
    public IList<string> Affected { get; set; }

    public override string ToString()
    {
        return $"{nameof(Title)}: {Title}, {nameof(Date)}: {Date}, {nameof(Resolved)}: {Resolved}, {nameof(Severity)}: {Severity}, {nameof(Affected)}: {{{string.Join(", ", Affected)}}}";
    }
}