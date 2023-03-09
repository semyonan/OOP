using Reports.Service.Entities;

namespace DataBaseAccess.Models;

public class Employee
{
    public Employee(string name, string serName, string id, PhoneNumber phoneNumber, string? higherSupervisor = null)
    {
        Name = name;
        HigherSupervisor = higherSupervisor;
        SerName = serName;
        Id = id;
        PhoneNumber = phoneNumber;
    }

    public PhoneNumber PhoneNumber { get; }
    public string? HigherSupervisor { get; }
    public string Name { get; }
    public string SerName { get; }
    public string Id { get; }
}