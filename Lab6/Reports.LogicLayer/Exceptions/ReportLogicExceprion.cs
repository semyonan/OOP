namespace Reports.Service.Exceptions;

public class ReportLogicException : System.Exception
{
    public ReportLogicException() { }
    public ReportLogicException(string message)
        : base(message) { }
}