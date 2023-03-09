namespace Banks.Models;

public interface ITermableAccount
{
    public DateTime Start { get; }
    public TimeSpan Term { get; }
    public bool Opened { get; }

    public void Open();
}