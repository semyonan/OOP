using Banks.Exceptions;
using Banks.Models;
using TransactionException = System.Transactions.TransactionException;

namespace Banks.Entities;

public class CentralBank
{
    private static CentralBank? _instance = null;
    private readonly List<Bank> _listOfBanks;
    private readonly List<Client> _listOfClients;
    private readonly List<ITransaction> _listOfTransactions;

    public CentralBank(ITimeMachine timeMachine)
    {
        _listOfBanks = new List<Bank>();
        _listOfTransactions = new List<ITransaction>();
        _listOfClients = new List<Client>();
        timeMachine.AddObserver(this);
        TimeMachine = timeMachine;
        _instance = this;
    }

    public IReadOnlyList<Bank> ListOfBanks => _listOfBanks.AsReadOnly();

    public IReadOnlyList<ITransaction> ListOfTransaction => _listOfTransactions.AsReadOnly();

    public IReadOnlyList<Client> ListOfClients => _listOfClients.AsReadOnly();

    public ITimeMachine TimeMachine { get; }
    public static CentralBank GetInstance()
    {
        if (_instance == null)
        {
            throw new BankException("Central bank must be created");
        }

        return _instance;
    }

    public Bank GetBank(string name) =>
        FindBank(name)
        ?? throw new BankException("Bank does't exists");
    public Client GetClient(Guid id) =>
        FindClient(id)
        ?? throw new BankException("Client does't exists");
    public IBankAccount GetAccount(Guid id) =>
        FindAccount(id)
        ?? throw new BankException("Account does't exists");

    public Bank AddBank(
        string name,
        BankConfiguration bankConfiguration)
    {
        if (BankExists(name))
        {
            throw new BankException("Bank already exists");
        }

        var bank = new Bank(name, bankConfiguration);

        _listOfBanks.Add(bank);

        return bank;
    }

    public Guid AddClient(
        string name,
        string serName,
        Address? address = null,
        RussianPassportNumber? passportNumber = null,
        IMessageHandler? messageHandler = null)
    {
        if (ClientExists(passportNumber))
        {
            throw new BankException("Client already exists");
        }

        var client = new ClientBuilder()
            .Name(name)
            .SerName(serName)
            .Address(address)
            .PassportDate(passportNumber)
            .MassageHandler(messageHandler)
            .Build();

        _listOfClients.Add(client);

        return client.Id;
    }

    public Guid AddDebitAccount(Guid clientId, string bankName)
    {
        var selectedClient = GetClient(clientId);
        var selectedBank = GetBank(bankName);

        var account = new DebitAccount(selectedClient, selectedBank);

        selectedBank.AddAccount(account);
        selectedClient.AddAccount(account);

        return account.Id;
    }

    public Guid AddDepositAccount(Guid clientId, string bankName, TimeSpan term)
    {
        var selectedClient = GetClient(clientId);
        var selectedBank = GetBank(bankName);

        var account = new DepositAccount(selectedClient, selectedBank, TimeMachine.CurDateTime, term);
        selectedBank.AddAccount(account);

        return account.Id;
    }

    public Guid AddCreditAccount(Guid clientId, string bankName)
    {
        var selectedClient = GetClient(clientId);
        var selectedBank = GetBank(bankName);

        var account = new CreditAccount(selectedClient, selectedBank, selectedBank.BankConfiguration.CreditAccountInfo.Limit);
        selectedBank.AddAccount(account);

        return account.Id;
    }

    public Guid WithdrawTransaction(Guid accountId, Money sum)
    {
        var selectedAccount = GetAccount(accountId);

        if (!AccountIsValid(selectedAccount) && sum > selectedAccount.Bank.BankConfiguration.LimitForPrecariousTransactions)
        {
            throw new TransactionException("Account is invalid");
        }

        var commission = new Money(0);

        if (selectedAccount is CreditAccount { Fund: < 0 } creditAccount)
        {
            commission += sum * selectedAccount.Bank.BankConfiguration.CreditAccountInfo.Commission.AsDecimal();
        }

        if (selectedAccount.Bank.BankConfiguration.WithdrawalCommission != null &&
            sum > selectedAccount.Bank.BankConfiguration.WithdrawalCommission.Item2)
        {
            commission += sum * selectedAccount.Bank.BankConfiguration.WithdrawalCommission.Item1.AsDecimal();
        }

        selectedAccount.Withdrawal(sum + commission);

        var transaction = new WithdrawTransaction(selectedAccount, sum);

        _listOfTransactions.Add(transaction);

        return transaction.Id;
    }

    public Guid ReplenishmentTransaction(Guid accountId, Money sum)
    {
        var selectedAccount = GetAccount(accountId);

        selectedAccount.Replenishment(sum);

        var transaction = new ReplenishmentTransaction(selectedAccount, sum);

        _listOfTransactions.Add(transaction);

        return transaction.Id;
    }

    public Guid? RemittanceTransaction(Guid senderAccountId, Guid receiverAccountId, Money sum)
    {
        var selectedReceiverAccount = GetAccount(receiverAccountId);
        var selectedSenderAccount = GetAccount(senderAccountId);

        if (!AccountIsValid(selectedSenderAccount) && sum > selectedSenderAccount.Bank.BankConfiguration.LimitForPrecariousTransactions)
        {
            throw new BankException("Account is not valid");
        }

        var commission = new Money(0);

        RemittanceTransaction transaction;

        if (selectedSenderAccount.Bank.BankConfiguration.AnotherBankRemittanceCommission != null
            && !selectedSenderAccount.Bank.Equals(selectedReceiverAccount.Bank))
        {
            commission = sum * selectedSenderAccount.Bank.BankConfiguration.AnotherBankRemittanceCommission.AsDecimal();
        }

        selectedSenderAccount.SendRemittance(sum + commission);

        try
        {
            selectedReceiverAccount.ReceiveRemittance(sum);
        }
        catch (System.Exception)
        {
            selectedSenderAccount.SpecificBankReplenishment(sum + commission);
            throw new TransactionException("Transaction canceled");
        }

        transaction = new RemittanceTransaction(selectedSenderAccount, selectedReceiverAccount, sum);

        _listOfTransactions.Add(transaction);

        return transaction.Id;
    }

    public void CancelTransaction(Guid transactionId)
    {
        var selectedTransaction = _listOfTransactions.FirstOrDefault(x => x.Id == transactionId);

        if (selectedTransaction == null)
        {
            throw new BankException("Transaction doesn't exists");
        }

        if (selectedTransaction is ReplenishmentTransaction replenishmentTransaction)
        {
            replenishmentTransaction.BankAccount.SpecificBankWithdrawal(replenishmentTransaction.Sum);
        }

        if (selectedTransaction is WithdrawTransaction withdrawTransaction)
        {
            withdrawTransaction.BankAccount.SpecificBankReplenishment(withdrawTransaction.Sum);
        }

        if (selectedTransaction is RemittanceTransaction remittanceTransaction)
        {
            remittanceTransaction.ReceiverAccount.SpecificBankWithdrawal(remittanceTransaction.Sum);
            remittanceTransaction.SenderAccount.SpecificBankReplenishment(remittanceTransaction.Sum);
        }
    }

    public void RecountInterests()
    {
        foreach (var bank in _listOfBanks)
        {
            bank.RecountInterests();
        }
    }

    public void PayInterests()
    {
        foreach (var bank in _listOfBanks)
        {
            bank.PayInterests();
        }
    }

    public void CheckTermableAccounts(DateTime curDateOnly)
    {
        foreach (var bank in _listOfBanks)
        {
            bank.CheckTermableAccounts(curDateOnly);
        }
    }

    private bool BankExists(string bankName) => _listOfBanks.Exists(x => x.Name == bankName);
    private Bank? FindBank(string bankName) => _listOfBanks.FirstOrDefault(x => x.Name == bankName);

    private bool ClientExists(RussianPassportNumber? passportNumber) => (passportNumber != null) &&
        _listOfClients.Exists(
            x => x.PassportNumber != null && x.PassportNumber.Equals(passportNumber));

    private bool ClientExists(Guid id) =>
        _listOfClients.Exists(
            x => x.Id == id);
    private Client? FindClient(Guid id) =>
        _listOfClients.FirstOrDefault(
            x => x.Id == id);

    private IBankAccount? FindAccount(Guid id) => _listOfBanks.SelectMany(x => x.ListOfBankAccounts)
        .FirstOrDefault(x => x.Id == id);

    private bool AccountIsValid(IBankAccount bankAccount)
    {
        return (bankAccount.Owner.PassportNumber != null) && (bankAccount.Owner.Address != null);
    }
}