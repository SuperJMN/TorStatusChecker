namespace TorStatusChecker;

public interface IUriBasedStringStore
{
    Task<string> Fetch(Uri uri);
}