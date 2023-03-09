using DataBaseAccess;
using DataBaseAccess.Models;

namespace Reports.Service.Entities;

public class AdministrationService : IAdministrationService
{
    private readonly DataBase _dataBase;
    public AdministrationService()
    {
        _dataBase = Context.GetInstance().DataBase;
    }

    public void AddEmployee(
        string subordinateName,
        string subordinateSerName,
        Guid? supervisorId = null)
    {
        string? id = null;
        if (supervisorId != null)
        {
            _dataBase.GetEmployee((Guid)supervisorId);
            id = supervisorId.Value.ToString();
        }

        _dataBase.Employees.Add(new Employee(
            subordinateName,
            subordinateSerName,
            Guid.NewGuid().ToString(),
            _dataBase.NewNumber(),
            id));
    }

    public List<string> GetEmployees()
    {
        return _dataBase.Employees.Select(x => x.Id.ToString()).ToList();
    }

    public void AddPhoneAccount(Guid employeeId)
    {
        var employee = _dataBase.GetEmployee(employeeId);
        _dataBase.Accounts.Add(new PhoneAccount(employee));
    }

    public void AddMessengerAccount(Guid employeeId)
    {
        var employee = _dataBase.GetEmployee(employeeId);
        _dataBase.Accounts.Add(new MessengerAccount(employee));
    }

    public void AddEmailAccount(Guid employeeId)
    {
        var employee = _dataBase.GetEmployee(employeeId);
        _dataBase.Accounts.Add(new EmailAccount(employee));
    }

    public void Save()
    {
        _dataBase.Save();
    }
}