using DataBaseAccess.Models;
using Service.Entities;

namespace Reports.Service.Entities;

public interface IMessageService
{
    public void Send(string title, string text, Guid receiverId, Guid? senderId = null, MessageSourceAccount? messageSourceAccount = null);
    public List<string> Receive(Guid? receiverId = null);
    public Tuple<string, string, string> GetWholeMessage(int index);

    public List<string> GetEmployees();

    public string GetWholeAccount(int index);

    public void CreateReport(
        List<string> commands,
        List<string> messengerTypes,
        DateTime? start = null,
        DateTime? endOf = null,
        Guid? subordinateId = null,
        Guid? id = null);

    public List<string> GetReports(string? supervisorId = null);
    public string GetWholeReport(int index);
}