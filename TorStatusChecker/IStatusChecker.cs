namespace TorStatusChecker;

public interface IStatusChecker
{
	IObservable<IList<Issue>> Issues { get; }
}