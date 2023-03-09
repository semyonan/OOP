namespace Banks.Models;

public class InterestsCounter
{
    public InterestsCounter()
    {
        Interest = 0;
    }

    public double Interest { get; private set; }

    public void Increase(Percent percent, decimal currentFund)
    {
        Interest += (double)currentFund * percent.Value;
    }

    public void Reset()
    {
        Interest = 0;
    }
}