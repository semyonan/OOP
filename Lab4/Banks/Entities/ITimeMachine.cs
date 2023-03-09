namespace Banks.Entities;

public interface ITimeMachine
{
    public CentralBank? Observer { get; }
    public DateTime Start { get; }
    public DateTime CurDateTime { get; }

    public void AddObserver(CentralBank centralBank);
}