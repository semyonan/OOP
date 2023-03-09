namespace Banks.Models;

public class BankConfiguration
{
    public BankConfiguration(
        DepositAccountInfo depositAccountInfo,
        DebitAccountInfo debitAccountInfo,
        CreditAccountInfo creditAccountInfo,
        Money limitForPrecariousTransactions,
        Percent? anotherBankRemittanceCommission = null,
        Tuple<Percent, Money>? withdrawalCommission = null)
    {
        DepositAccountInfo = depositAccountInfo;
        DebitAccountInfo = debitAccountInfo;
        CreditAccountInfo = creditAccountInfo;
        LimitForPrecariousTransactions = limitForPrecariousTransactions;
        AnotherBankRemittanceCommission = anotherBankRemittanceCommission;
        WithdrawalCommission = withdrawalCommission;
    }

    public CreditAccountInfo CreditAccountInfo { get; private set; }
    public DepositAccountInfo DepositAccountInfo { get; private set; }
    public DebitAccountInfo DebitAccountInfo { get; private set; }
    public Money LimitForPrecariousTransactions { get; private set; }
    public Percent? AnotherBankRemittanceCommission { get; private set; }
    public Tuple<Percent, Money>? WithdrawalCommission { get; private set; }

    public void ChangeCreditAccountInfo(CreditAccountInfo creditAccountInfo)
    {
        CreditAccountInfo = creditAccountInfo;
    }

    public void ChangeDebitAccountInfo(DebitAccountInfo debitAccountInfo)
    {
        DebitAccountInfo = debitAccountInfo;
    }

    public void ChangeDepositAccountInfo(DepositAccountInfo depositAccountInfo)
    {
        DepositAccountInfo = depositAccountInfo;
    }

    public void ChangeLimitForPrecariousTransactions(Money limit)
    {
        LimitForPrecariousTransactions = limit;
    }

    public void ChangeAnotherBankRemittanceCommission(Percent anotherBankRemittanceCommission)
    {
        AnotherBankRemittanceCommission = anotherBankRemittanceCommission;
    }

    public void ChangeWithdrawalCommission(Percent withdrawalCommission, Money withdrawalCommissionLimit)
    {
        WithdrawalCommission = new Tuple<Percent, Money>(withdrawalCommission, withdrawalCommissionLimit);
    }
}