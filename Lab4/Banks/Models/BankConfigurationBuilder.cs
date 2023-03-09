using Banks.Exceptions;

namespace Banks.Models;

public class BankConfigurationBuilder
{
    private CreditAccountInfo? _creditAccountInfo;
    private DepositAccountInfo? _depositAccountInfo;
    private DebitAccountInfo? _debitAccountInfo;
    private Money? _limitForPrecariousTransactions;
    private Percent? _anotherBankRemittanceCommission;
    private Tuple<Percent, Money>? _withdrawalCommission;

    public BankConfigurationBuilder()
    {
        _creditAccountInfo = null;
        _depositAccountInfo = null;
        _debitAccountInfo = null;
        _anotherBankRemittanceCommission = null;
        _withdrawalCommission = null;
        _limitForPrecariousTransactions = null;
    }

    public BankConfigurationBuilder CreditAccountInfo(CreditAccountInfo creditAccountInfo)
    {
        _creditAccountInfo = creditAccountInfo;

        return this;
    }

    public BankConfigurationBuilder DepositAccountInfo(DepositAccountInfo depositAccountInfo)
    {
        _depositAccountInfo = depositAccountInfo;

        return this;
    }

    public BankConfigurationBuilder DebitAccountInfo(DebitAccountInfo debitAccountInfo)
    {
        _debitAccountInfo = debitAccountInfo;

        return this;
    }

    public BankConfigurationBuilder LimitForPrecariousTransactions(Money limit)
    {
        _limitForPrecariousTransactions = limit;

        return this;
    }

    public BankConfigurationBuilder AnotherBankRemittanceCommission(Percent anotherBankRemittanceCommission)
    {
        _anotherBankRemittanceCommission = anotherBankRemittanceCommission;

        return this;
    }

    public BankConfigurationBuilder WithdrawalCommission(Percent withdrawalCommission, Money withdrawalCommissionLimit)
    {
        _withdrawalCommission = new Tuple<Percent, Money>(withdrawalCommission, withdrawalCommissionLimit);

        return this;
    }

    public BankConfiguration Build()
    {
        if (_creditAccountInfo == null
            || _debitAccountInfo == null
            || _depositAccountInfo == null)
        {
            throw new BankValidationException("Credit, Debit and Deposit info must be");
        }

        if (_limitForPrecariousTransactions == null)
        {
            _limitForPrecariousTransactions = new Money(0);
        }

        return new BankConfiguration(
            _depositAccountInfo,
            _debitAccountInfo,
            _creditAccountInfo,
            _limitForPrecariousTransactions,
            _anotherBankRemittanceCommission,
            _withdrawalCommission);
    }
}