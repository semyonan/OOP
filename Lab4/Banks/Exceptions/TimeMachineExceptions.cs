namespace Banks.Exceptions;

public class TimeMachineExceptions : System.Exception
{
    public TimeMachineExceptions() { }

    public TimeMachineExceptions(string message)
        : base(message)
    {
    }
}