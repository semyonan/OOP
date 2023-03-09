using DataBaseAccess.Models;
using Newtonsoft.Json;
using Reports.Service.Entities;
using Service.Entities;

namespace DataBaseAccess;

[Serializable]
public class DataBase
{
    private PhoneNumber? _lastPhoneNumber;
    public DataBase()
    {
        Employees = new List<Employee>();
        Accounts = new List<MessageSourceAccount>();
        PhoneNumbers = new List<PhoneNumber>();
        Messages = new List<Message>();
        Reports = new List<Report>();
        _lastPhoneNumber = new PhoneNumber(88003030100);
    }

    public List<Employee> Employees { get; set; }
    public List<MessageSourceAccount> Accounts { get; set; }
    public List<PhoneNumber> PhoneNumbers { get; set; }
    public List<Message> Messages { get; set; }
    public List<Report> Reports { get; set; }

    public static DataBase Deserialize()
    {
        if (!File.Exists(@"data_base.json")) return new DataBase();
        var json = File.ReadAllText(@"data_base.json");
        var data = JsonConvert.DeserializeObject<DataBase>(json, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Include });

        return data;
    }

    public PhoneNumber NewNumber()
    {
        if (_lastPhoneNumber == null)
        {
            throw new Exception();
        }

        _lastPhoneNumber = new PhoneNumber(_lastPhoneNumber.Number + 1);

        return _lastPhoneNumber;
    }

    public Employee GetEmployee(Guid id)
    {
        var employee = FindEmployee(id);
        if (employee == null)
        {
            throw new Exception();
        }

        return employee;
    }

    public Message GetMessage(string id)
    {
        var message = FindMessage(id);
        if (message == null)
        {
            throw new Exception();
        }

        return message;
    }

    public void ChangeState(string messageId, MessageState state)
    {
        var message = GetMessage(messageId);
        message.State = state;
    }

    public void Save()
    {
        var json = JsonConvert.SerializeObject(this);
        File.WriteAllText(@"data_base.json", json);
    }

    private Employee? FindEmployee(Guid id) => Employees.FirstOrDefault(x => Guid.Parse(x.Id.ToString()) == id);
    private Message? FindMessage(string id) => Messages.FirstOrDefault(x => x.Id == id);
}