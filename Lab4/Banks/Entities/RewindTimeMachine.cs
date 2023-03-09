using Banks.Exceptions;

namespace Banks.Entities;

public class RewindTimeMachine : ITimeMachine
{
    public RewindTimeMachine()
    {
        Start = DateTime.Now;
        CurDateTime = Start;
    }

    public DateTime Start { get; }
    public CentralBank? Observer { get; private set; }
    public DateTime CurDateTime { get; private set; }

    public void AddObserver(CentralBank centralBank)
    {
        Observer = centralBank;
    }

    public void RewindTime(int years, int months, int days)
    {
        if (Observer == null)
        {
            throw new TimeMachineExceptions("Observer must be set");
        }

        if ((days < 0)
            || (months < 0)
            || (years < 0))
        {
            throw new TimeMachineExceptions("Invalid arguments for rewind time operation");
        }

        var newDateTime = CurDateTime.AddYears(years).AddMonths(months).AddDays(days);

        var span = newDateTime - CurDateTime;
        for (int i = 0; i < span.Days; i++)
        {
            CurDateTime = CurDateTime.AddDays(1);
            if (CurDateTime.Day == Start.Day)
            {
                Observer.PayInterests();
            }

            Observer.CheckTermableAccounts(CurDateTime);

            Observer.RecountInterests();
        }
    }
}