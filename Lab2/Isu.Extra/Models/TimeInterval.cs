using Isu.Extra.Exceptions;

namespace Isu.Extra.Models;

public class TimeInterval
{
    public TimeInterval(TimeOnly start, TimeOnly end)
    {
        if (start >= end)
        {
            throw new TimeException("Interval start can't be later interval end");
        }

        Start = start;
        End = end;
    }

    public TimeOnly Start { get; }
    public TimeOnly End { get; }
    public int CompareTo(object? obj)
    {
        if (obj is not TimeInterval timeInterval)
        {
            throw new TimeException("Invalid argument");
        }

        return Start.CompareTo(timeInterval.Start);
    }
}