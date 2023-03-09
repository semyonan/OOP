using Service.Entities;

namespace Reports.Service.Entities;

public interface IAdministrationService
{
    public void AddEmployee(
        string subordinateName,
        string subordinateSerName,
        Guid? supervisorId = null);

    public void AddPhoneAccount(Guid employeeId);
    public List<string> GetEmployees();

    public void AddMessengerAccount(Guid employeeId);

    public void AddEmailAccount(Guid employeeId);

    public void Save();
}