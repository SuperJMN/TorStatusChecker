namespace TorStatusChecker;

public interface ITorNetwork
{
    public IObservable<Issue> Issues { get; }
}