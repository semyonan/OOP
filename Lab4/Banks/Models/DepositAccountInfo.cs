using Banks.Exceptions;

namespace Banks.Models;

public class DepositAccountInfo
{
    private readonly List<Tuple<Money, Percent>> _interestsPerDay;
    public DepositAccountInfo(List<Tuple<Money, Percent>> interests)
    {
        if (interests[0].Item1 != new Money(0))
        {
            throw new BankValidationException("Invalid deposit info arguments");
        }

        _interestsPerDay = new List<Tuple<Money, Percent>>();

        foreach (var interest in interests)
        {
            _interestsPerDay.Add(new Tuple<Money, Percent>(interest.Item1, interest.Item2 / 365));
        }

        _interestsPerDay.Sort((x, y) => x.Item1.CompareTo(y.Item1));
    }

    public IReadOnlyList<Tuple<Money, Percent>> InterestsPerDay => _interestsPerDay.AsReadOnly();

    public void ChangeInterests(List<Tuple<Money, Percent>> interests)
    {
        _interestsPerDay.Clear();
        _interestsPerDay.AddRange(interests);
    }
}